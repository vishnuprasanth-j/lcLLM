using lcLLM.Helpers;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.InteropServices;

namespace lcLLM.Windows
{
    [Guid("22dcf2f8-c8c1-4cf4-b1aa-cde7897a63a8")]
    public class RefactorSuggestionWindow : ToolWindowPane, IVsWindowFrameNotify3
    {
        public RefactorSuggestionWindow() : base(null)
        {
            this.Caption = "Code suggestion";
            this.Content = new RefactorSuggestionWindowControl();
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
