using System;
using System.Drawing;
using System.Windows.Forms;

namespace AutoSQ
{
  public partial class FunForm : Form
  {
    private readonly GlobalKeyboardHook _gkh;
    private readonly Bitmap _bmp;

    public FunForm(Bitmap bmp)
    {
      InitializeComponent();
      _bmp = bmp;
      _gkh = new GlobalKeyboardHook();
      _gkh.HookedKeys.Add(Keys.Escape);
    }

    private void FunForm_Load(object sender, EventArgs e)
    {
      TopMost = true;
      Left = 1920;
      Top = 467;
      Width = _bmp.Width;
      Height = _bmp.Height;

      BackgroundImage = _bmp;
      _gkh.KeyDown += FunForm_KeyDown;
    }

    private void FunForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape) return;
      e.Handled = true;
      e.SuppressKeyPress = true;
      Close();
    }
  }
}
