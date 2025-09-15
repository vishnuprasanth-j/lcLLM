using lcLLM.Helpers;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.InteropServices;

namespace lcLLM.Windows
{
    [Guid("3d2451a9-4f2b-42b8-92f0-6c9a5d2c8e1c")]
    public class MarkdownViewerWindow : ToolWindowPane, IVsWindowFrameNotify3
    {
        public MarkdownViewerWindow() : base(null)
        {
            this.Caption = "Markdown viewer";
            this.Content = new MarkdownViewer();
        }
        int IVsWindowFrameNotify3.OnShow(int fShow)
        {
            return VSConstants.S_OK;
        }

        int IVsWindowFrameNotify3.OnMove(int x, int y, int w, int h)
        {
            return VSConstants.S_OK;
        }

        int IVsWindowFrameNotify3.OnSize(int x, int y, int w, int h)
        {
            return VSConstants.S_OK;
        }

        int IVsWindowFrameNotify3.OnDockableChange(int fDockable, int x, int y, int w, int h)
        {
            return VSConstants.S_OK;
        }

        int IVsWindowFrameNotify3.OnClose(ref uint pgrfSaveOptions)
        {
            RefactorSuggestionHelper.Reset();
            return VSConstants.S_OK;
        }
    }
}
