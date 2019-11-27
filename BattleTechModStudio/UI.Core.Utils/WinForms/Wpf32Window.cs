using System;
using System.Windows;
using System.Windows.Interop;

namespace UI.Core.Utils.WinForms
{
    using IWin32Window = System.Windows.Forms.IWin32Window;

    public class Wpf32Window : IWin32Window
    {
        public Wpf32Window(Window wpfWindow)
        {
            this.Handle = new WindowInteropHelper(wpfWindow).Handle;
        }

        public IntPtr Handle { get; }
    }
}