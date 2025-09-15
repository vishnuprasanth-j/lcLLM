using lcLLM.Helpers;

namespace lcLLM.Commands
{
    [Command(PackageGuids.lcLLMCommandString, PackageIds.cmdDocumentCode)]
    internal sealed class DocumentCodeCommand : BaseCommand<DocumentCodeCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await CommandHelpers.PerformRefactoringSuggestionAsync(Enums.CodeType.Documentation);
        }
    }
}
