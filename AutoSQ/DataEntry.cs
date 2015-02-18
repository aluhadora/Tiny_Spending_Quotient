using System;
using System.Linq;
using System.Windows.Forms;

namespace AutoSQ
{
  public partial class DataEntry : Form
  {
    private readonly GlobalKeyboardHook _gkh;

    public DataEntry(int length)
    {
      InitializeComponent();
      TopMost = true;

      textBox.MaxLength = length;
      Width = Math.Max(20, length * 6 + 6) + 24;
      textBox.Width = Math.Max(20, length * 6 + 6);
      textBox.GotFocus += TextBoxGotFocus;
      textBox.Enter += TextBoxGotFocus;

      _gkh = new GlobalKeyboardHook
        {
          HookedKeys = Enum.GetValues(typeof(Keys)).OfType<Keys>().ToList()
        };
    }

    private void TextBoxGotFocus(object sender, EventArgs e)
    {
      textBoxSteal.Focus();
    }

    public string Output { get; private set; }

    private void DataEntry_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
      {
        e.Handled = true;
        e.SuppressKeyPress = true;
        Close();
      }
      else if (e.KeyCode == Keys.Enter)
      {
        e.Handled = true;
        e.SuppressKeyPress = true;
        Output = textBox.Text;
        Close();
      }
      else
      {
        textBox.SendToTextBox(e);
        if (textBox.MaxLength == textBox.Text.Length)
        {
          Output = textBox.Text;
          Close();
        }
      }
    }

    private void DataEntry_Load(object sender, EventArgs e)
    {
      _gkh.KeyDown += DataEntry_KeyDown;
    }
  }
}
