global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;
using lcLLM.Windows;
using System.Runtime.InteropServices;
using System.Threading;

namespace lcLLM
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.guidlcLLMPackageString)]
    [ProvideOptionPage(typeof(GeneralOptions), "lcLLM", "Configuration", 0, 0, true)]
    [ProvideOptionPage(typeof(ModelsOptions), "lcLLM", "Models", 0, 0, true)]
    [ProvideToolWindow(typeof(RefactorSuggestionWindow))]
    [ProvideToolWindow(typeof(MarkdownViewerWindow))]
    public sealed class lcLLMPackage : ToolkitPackage
    {
        public static lcLLMPackage Instance { get; private set; }

        protected override async Task InitializeAsync(
            CancellationToken cancellationToken,
            IProgress<ServiceProgressData> progress)
        {
            Instance = this;
            await this.RegisterCommandsAsync();
        }
    }
}