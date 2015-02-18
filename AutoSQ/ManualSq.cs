using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AutoSQ
{
  public partial class ManualSq : Form
  {
    public ManualSq()
    {
      InitializeComponent();

      TopMost = true;
    }


    private void UTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      HandleText(sender, e);
    }

    private void ITextBox_KeyDown(object sender, KeyEventArgs e)
    {
      HandleText(sender, e);
    }

    private void HandleText(object sender, KeyEventArgs e)
    {
      MTextBox next = (sender == ITextBox ? UTextBox : ITextBox);

      if (e.KeyCode == Keys.Escape || e.KeyCode == (Keys)Keys.KeyCode.GetHashCode())
      {
        Close();
        return;
      }
      if (e.KeyCode == Keys.Back)
      {
        return;
      }
      if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
      {
        e.Handled = true;
        e.SuppressKeyPress = true;
        next.Focus();
        next.SelectAll();
        Compute();
        return;
      }

      var numericKeys = new List<Keys>
        {
          Keys.NumPad0,
          Keys.NumPad1,
          Keys.NumPad2,
          Keys.NumPad3,
          Keys.NumPad4,
          Keys.NumPad5,
          Keys.NumPad6,
          Keys.NumPad7,
          Keys.NumPad8,
          Keys.NumPad9,
          Keys.D0,
          Keys.D1,
          Keys.D2,
          Keys.D3,
          Keys.D4,
          Keys.D5,
          Keys.D6,
          Keys.D7,
          Keys.D8,
          Keys.D9,
        };

      var numericHashCodes = numericKeys.Select(x => x.GetHashCode()).ToList();

      if (!numericKeys.Contains(e.KeyCode) && !numericHashCodes.Contains(e.KeyCode.GetHashCode()))
      {
        e.SuppressKeyPress = true;
        e.Handled = true;
        return;
      }
    }

    private void Compute()
    {
      double sq = 0;

      if (UTextBox.Text != string.Empty && ITextBox.Text != string.Empty)
      {
        var u = Convert.ToDouble(UTextBox.Text);
        var i = Convert.ToDouble(ITextBox.Text);

        u = u > 0 ? u : 1;
        sq = 35 * (0.00137 * i - Math.Log(u)) + 240;
      }

      var intSq = (int)sq;
      MoveCursor(intSq);
      sqLabel.Text = intSq.ToString();
    }

    private void MoveCursor(int sq)
    {
      var x = -2.72 * sq + 309;

      int groups = sq / 25;

      x += groups * 4;

      cursor.Left = (int)x;
    }
  }
}
