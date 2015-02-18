using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using tesseract;

namespace AutoSQ
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new SqForm());
      return;
      var process = new Process(true);
      var namePrint = Process.GetNamePrint();
      var bmp = process.Bmp;
      process.Go(Dummy);
      var testingBmp = process.TestingBmp;

      Clipboard.SetImage(namePrint);
      Clipboard.SetImage(bmp);
      Clipboard.SetImage(testingBmp);
    }

    public static void Dummy(string[] t)
    {
      
    }
  }
}
