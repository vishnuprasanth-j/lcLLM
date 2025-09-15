using lcLLM.Helpers;


namespace lcLLM.Commands
{
    [Command(PackageGuids.lcLLMCommandString, PackageIds.cmdCodeReview)]
    internal sealed class CodeReviewCommand : BaseCommand<CodeReviewCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await CommandHelpers.PerformRefactoringSuggestionAsync(Enums.CodeType.Review);
        }
    }
}
