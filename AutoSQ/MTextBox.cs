using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutoSQ
{
  public partial class MTextBox : TextBox
  {
    private const int SendKey = 0x0100;

    [DllImport("user32.dll")]
    public static extern IntPtr PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    public void SendToTextBox(KeyEventArgs e)
    {
      PostMessage(Handle, SendKey, ((IntPtr)e.KeyCode), IntPtr.Zero);
    }
  }
}
