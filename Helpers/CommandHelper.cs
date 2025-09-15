using EnvDTE;
using System.Linq;
using System.Windows.Controls;
using CodeType = lcLLM.Enums.CodeType;

namespace lcLLM.Helpers
{
    internal class CommandHelpers
    {
        public static string ActiveDocumentPath { get; private set; }
        public static TextBox ManualPromptTextBox { get; set; }

        public static string GetCurrentMethodCode()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (Package.GetGlobalService(typeof(DTE)) is not DTE dte)
            {
                return string.Empty;
            }

            var activeDocument = dte.ActiveDocument;
            if (activeDocument == null) return string.Empty;

            ActiveDocumentPath = dte.ActiveDocument.FullName;

            if (activeDocument.Selection is not TextSelection textSelection) return string.Empty;

            if (string.IsNullOrWhiteSpace(textSelection.Text))
            {
                textSelection.SelectLine();
                return textSelection.Text.Trim();
            }

            var selectedLines = textSelection.Text
                .Split('\r', '\n')
                .Select(l => l.Trim())
                .Where(l => !string.IsNullOrEmpty(l));

            return string.Join("\r\n", selectedLines);
        }
        public static async Task PerformRefactoringSuggestionAsync(CodeType codeType, string manualPrompt = "")
        {
            var message = "Waiting for LLM response (task requested: " + Enum.GetName(typeof(CodeType), codeType) + ") ...";

            var progressBarHelper = new ProgressBarHelper(ServiceProvider.GlobalProvider);
            progressBarHelper.StartIndeterminateDialog(message);

            var methodCode = GetCurrentMethodCode();

            if (string.IsNullOrEmpty(methodCode) && string.IsNullOrEmpty(manualPrompt))
            {
                progressBarHelper.StopDialog();
                WindowHelper.WarningBox("It is necessary to select the source code to be processed from the editor");
                return;
            }

            var refactoringHelper = RefactorSuggestionHelper.Instance;
            await refactoringHelper.RequestCodeSuggestionsAsync(methodCode, ActiveDocumentPath, codeType, manualPrompt);

            progressBarHelper.StopDialog();
        }
    }
}