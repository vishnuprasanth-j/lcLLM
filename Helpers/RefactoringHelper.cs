using lcLLM.Clients;
using lcLLM.Enums;
using lcLLM.Models;
using lcLLM.Windows;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace lcLLM.Helpers
{
    internal class RefactoringHelper
    {
        private readonly GeneralOptions _generalOptions;
        private readonly ModelsOptions _modelsOptions;
        private readonly AsyncPackage _package;

        private LlmRequest _lastPrompt = new();
        public LlmRequest LastPrompt => _lastPrompt;



        private LlmRequest? _lastPrompt;
        public LlmRequest? LastPrompt => _lastPrompt;

        public RefactoringHelper()
        {
            var package = lcLLMPackage.Instance;
            _package = package;
            _generalOptions = package.GetDialogPage(typeof(GeneralOptions)) as GeneralOptions;
            _modelsOptions = package.GetDialogPage(typeof(ModelsOptions)) as ModelsOptions;
        }
        private bool IsMarkdown(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return false;

            return content.Contains("#") ||
                   content.Contains("```") ||
                   content.Contains("- ") ||
                   content.Contains("*") ||
                   content.Contains("_");
        }


        public async Task RequestCodeSuggestionsAsync(
            string methodCode,
            string activeDocumentPath,
            CodeType codeType,
            string manualPrompt = "")
        {
            var suggestion = await GetCodeSuggestionsAsync(methodCode, codeType, manualPrompt);
            LlmMessage toStore = LlmMessage.CreateAssistantMessage(suggestion.Code);
            _promptHistory[_promptHistory.Count - 1].Messages.Add(toStore);

            if (suggestion.Type == CodeType.Undefined)
            {
                WindowHelper.ErrorBox("There was an error during LLM query. Please check if LLM APIs are reachable");
                return;
            }

            if (suggestion.Type != CodeType.Manual)
            {
                switch (suggestion.Type)
                {
                    case CodeType.Documentation:
                    case CodeType.Review:
                        await ShowMarkdownWindowAsync(suggestion.Code);
                        break;
                    default:
                        await ShowSuggestionWindowAsync(suggestion.Code, activeDocumentPath);
                        break;
                }
            }
            else
            {
                if (IsMarkdown(suggestion.Code))
                {
                    await ShowMarkdownWindowAsync(suggestion.Code);
                }
                else
                {
                    await ShowSuggestionWindowAsync(suggestion.Code, activeDocumentPath);
                }
            }
        }


        private async Task<CodeSuggestionResponse> GetCodeSuggestionsAsync(string methodCode, CodeType codeType, string manualPrompt)
        {
            var promptHelper = new PromptHelper(_generalOptions.Language);
            var prompt = "";
            if (codeType == CodeType.Manual)
            {
                LlmMessage toStore = LlmMessage.CreateUserMessage(manualPrompt);
                _promptHistory[_promptHistory.Count - 1].Messages.Add(toStore);
                prompt = JsonConvert.SerializeObject(_promptHistory[_promptHistory.Count - 1], Formatting.Indented, new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            else
            {
                var promptSwitch = codeType switch
                {
                    CodeType.Manual => promptHelper.CreateForManualRequest(_modelsOptions.LlmFollowUp, methodCode, manualPrompt),
                    CodeType.Refactor => promptHelper.CreateForRefactor(_modelsOptions.LlmRefactor, methodCode),
                    CodeType.Test => promptHelper.CreateForTests(_modelsOptions.LlmUnitTests, methodCode),
                    CodeType.Documentation => promptHelper.CreateForDocumentation(_modelsOptions.LlmDocumentation, methodCode),
                    CodeType.Review => promptHelper.CreateForReview(_modelsOptions.LlmReview, methodCode),
                    _ => throw new ArgumentException("Invalid requested code type"),
                };
                prompt = promptSwitch;
                LlmRequest promptToStore = JsonConvert.DeserializeObject<LlmRequest>(promptSwitch);
                _promptHistory.Add(promptToStore);
            }
            using var llmClient = new LlmClient(_generalOptions.LlmServer);
            return await llmClient.GetCodeSuggestionsAsync(codeType, prompt);
        }

        private async Task ShowSuggestionWindowAsync(string suggestion, string activeDocumentPath)
        {
            ToolWindowPane window = await WindowHelper.ShowToolWindowAsync<RefactorSuggestionWindow>(_package);
            var control = (RefactorSuggestionWindowControl)window.Content;
            control.DisplaySuggestion(suggestion, activeDocumentPath);
        }

        private async Task ShowMarkdownWindowAsync(string suggestion)
        {
            ToolWindowPane window = await WindowHelper.ShowToolWindowAsync<MarkdownViewerWindow>(_package);
            var control = (MarkdownViewer)window.Content;
            control.DisplaySuggestion(suggestion);
        }

        public static async Task RunManualPromptAsync(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                WindowHelper.MsgBox(0, "A follow-up prompt must be set to proceed");
                return;
            }

            await CommandHelpers.PerformRefactoringSuggestionAsync(Enums.CodeType.Manual, prompt);
        }

    }
}
