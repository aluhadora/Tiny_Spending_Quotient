using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TinySpendingQuotient
{
  public static class FocusManager
  {
    // Sets the window to be foreground
    [DllImport("User32")]
    private static extern int SetForegroundWindow(IntPtr hwnd);


    // Activate or minimize a window
    [DllImport("User32")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    public const int Show = 5;
    public const int Minimize = 6;
    public const int Restore = 9;

    public static void ActivateApplication(string briefAppName)
    {
      Process[] procList = Process.GetProcessesByName(briefAppName);

      if (procList.Length > 0)
      {
        ShowWindow(procList[0].MainWindowHandle, Restore);
        SetForegroundWindow(procList[0].MainWindowHandle);
      }
    }
    
    public static bool ShowWindowp(IntPtr hWnd, int nCmdShow)
    {
      return ShowWindow(hWnd, nCmdShow);
    }

    public static int SetForegroundWindowp(IntPtr hwnd)
    {
      return SetForegroundWindow(hwnd);
    }
  }
}
