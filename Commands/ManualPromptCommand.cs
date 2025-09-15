using lcLLM.Helpers;


namespace lcLLM.Commands
{
    [Command(PackageGuids.lcLLMCommandString, PackageIds.cmdManualPrompt)]
    internal sealed class ManualPromptCommand : BaseCommand<ManualPromptCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            var prompt = CommandHelpers.ManualPromptTextBox?.Text ?? "";
            await RefactoringHelper.RunManualPromptAsync(prompt);
        }
    }
}
