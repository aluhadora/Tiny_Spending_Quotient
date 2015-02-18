using System.Windows.Forms;

namespace TinySpendingQuotient
{
  public partial class MTextBox : TextBox
  {
    public MTextBox()
    {
      InitializeComponent();
    }

    public void PressKey(KeyEventArgs e)
    {
      Focus();
      OnKeyDown(e);
    }
  }
}
