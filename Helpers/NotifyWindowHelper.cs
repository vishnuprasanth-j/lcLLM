using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lcLLM.Helpers
{
    public abstract class NotifyWindowHelper : IVsWindowFrameNotify3
    {
        int IVsWindowFrameNotify3.OnShow(int fShow)
        {
            throw new NotImplementedException();
        }

        int IVsWindowFrameNotify3.OnMove(int x, int y, int w, int h)
        {
            throw new NotImplementedException();
        }

        int IVsWindowFrameNotify3.OnSize(int x, int y, int w, int h)
        {
            throw new NotImplementedException();
        }

        int IVsWindowFrameNotify3.OnDockableChange(int fDockable, int x, int y, int w, int h)
        {
            throw new NotImplementedException();
        }

        int IVsWindowFrameNotify3.OnClose(ref uint pgrfSaveOptions)
        {
            throw new NotImplementedException();
        }
    }
}
