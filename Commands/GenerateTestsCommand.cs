using lcLLM.Helpers;

namespace lcLLM.Commands
{
    [Command(PackageGuids.lcLLMCommandString, PackageIds.cmdGenerateUnitTests)]
    internal sealed class GenerateTests : BaseCommand<GenerateTests>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await CommandHelpers.PerformRefactoringSuggestionAsync(Enums.CodeType.Test);
        }
    }
}
