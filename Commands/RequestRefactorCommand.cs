using lcLLM.Helpers;


namespace lcLLM.Commands
{
    [Command(PackageGuids.lcLLMCommandString, PackageIds.cmdRequestRefactor)]
    internal sealed class RequestRefactorCommand : BaseCommand<RequestRefactorCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await CommandHelpers.PerformRefactoringSuggestionAsync(Enums.CodeType.Refactor);
        }
    }
}
