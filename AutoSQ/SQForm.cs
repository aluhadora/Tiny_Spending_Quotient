using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace AutoSQ
{
  public partial class SqForm : Form
  {
    public readonly GlobalKeyboardHook Gkh;
    private Process _process;
    private const string Format = "###";

    public SqForm()
    {
      InitializeComponent();

      Gkh = new GlobalKeyboardHook();
      Gkh.HookedKeys.Add(Keys.Escape);
      Gkh.HookedKeys.Add(Keys.F1);
      Gkh.HookedKeys.Add(Keys.F2);
      Gkh.HookedKeys.Add(Keys.F12);

      Left = 1615;
      Top = 369;
      Width = 126;

      BackgroundImage = Background(Height);
    }

    private void FillDialog(string[] lines)
    {
      var count = lines.Length;
      var labels = new[] { sqLabel1, sqLabel2, sqLabel3, sqLabel4, sqLabel5, sqLabel6, sqLabel7, sqLabel8 };

      for (int i = 0; i < count; i++)
      {
        var sq = Compute(lines[i]);
        if (!sq.HasValue)
        {
          SetLabelTextEvent(labels[i], lines[i] == "..." ? "..." : "Err");
          return;
        }
        SetLabelTextEvent(labels[i], sq.Value.ToString(Format));
        SetHeightEvent(labels[i].Bottom + 30);
      }

      for (int i = count; i < 8; i++)
      {
        SetLabelTextEvent(labels[i], string.Empty);
      }

    }

    public delegate void SetLabelText(Label label, string value);
    private void SetLabelTextEvent(Label label, string value)
    {
      if (label.InvokeRequired)
      {
        SetLabelText d = SetLabelTextEvent;
        Invoke(d, new object[] { label, value });
      }
      else
      {
        label.Text = value;
      }
    }

    public delegate void SetHeight(int value);
    private void SetHeightEvent(int value)
    {
      if (InvokeRequired)
      {
        SetHeight d = SetHeightEvent;
        Invoke(d, new object[] { value });
      }
      else
      {
        if (Height == value) return;

        var bmp = Background(value);
        BackgroundImage = bmp;
        Height = value;
      }
    }

    private static double? Compute(string or)
    {
      try
      {
        var words = or.Split(' ');
        var u = Extract(words[0].Replace("\n", ""));
        var i = Extract(words[1].Replace("\n", ""));
        var sq = Compute(u, i);

        return sq;
      }
      catch
      {
        return null;
      }
    }

    private static int Extract(string word)
    {
      int j = -1;
      var word2 = word;
      for (int i = word.Length - 1; i >= 0; i--)
      {
        j++;
        if (j != 3) continue;
        word2 = word2.Remove(i, 1);
      }

      try
      {
        return Convert.ToInt32(word2);
      }
      catch (Exception)
      {
        return 0;
      }
    }

    private static double Compute(double u, double i)
    {
      u = u > 0 ? u : 1;
      return 35 * (0.00137 * i - Math.Log(u)) + 240;
    }

    private void SqForm_Load(object sender, EventArgs e)
    {
      Left = 1615;
      Top = 369;
      Width = 126;

      BackgroundImage = Background(Height);
      Gkh.KeyDown += SqForm_KeyDown;

      Go();
    }

    private void Go()
    {
      var thread = new Thread(StartProcess);
      thread.Start();
    }

    private void StartProcess()
    {
      _process = new Process();
      _process.Go(FillDialog);
    }

    private static Image Background(int y)
    {
      var top = Properties.Resources.TopBackGround;
      var bottom = Properties.Resources.BottomBackGround;
      var mid = Properties.Resources.MidBackGround;

      var bmp = new Bitmap(top.Width, y);
      var graphics = Graphics.FromImage(bmp);
      
      var original = mid;
      var scaled = new Bitmap(original.Width, y - top.Height + 2);
      var scaledMid = Graphics.FromImage(scaled);

      scaledMid.DrawImage(original, new Rectangle(0, 0, scaled.Width, scaled.Height));

      graphics.DrawImage(top, 0, 0);
      graphics.DrawImage(scaled, 0, top.Height);
      graphics.DrawImage(bottom, 0, y - bottom.Height);

      return bmp;
    }

    private void SqForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
      {
        e.Handled = true;
        e.SuppressKeyPress = true;
        Close();
      }
      else if (e.KeyCode == Keys.Enter)
      {
        Gkh.Unhook();
        (new ManualSq()).ShowDialog(this);
        Gkh.Hook();
      }
      else if (e.KeyCode == Keys.F1)
      {
        e.Handled = true;
        e.SuppressKeyPress = true;
        Go();
      }
      else if (e.KeyCode == Keys.F2)
      {
        e.Handled = true;
        e.SuppressKeyPress = true;
      }
      else if (e.KeyCode == Keys.F6)
      {
        e.Handled = true;
        e.SuppressKeyPress = true;
        Clipboard.Clear();
      }
      else if (e.KeyCode == Keys.F12)
      {
        e.Handled = true;
        e.SuppressKeyPress = true;
        Gkh.Unhook();
        LaunchFunForm();
        Gkh.Hook();
      }
    }

    private void LaunchFunForm()
    {
      var bmp = new Bitmap(_process.NamePrint.Width + _process.Bmp.Width, _process.Bmp.Height);
      Left = 1920 + bmp.Width;
      var graphics = Graphics.FromImage(bmp);
      graphics.DrawImage(_process.NamePrint, 0, 0);
      graphics.DrawImage(_process.Bmp, _process.NamePrint.Width, 0);
      var form = new FunForm(bmp);
      form.ShowDialog(this);
      Left = 1615;
    }

    private void Label_Click(object sender, EventArgs e)
    {
      var control = (Control)sender;
      Gkh.Unhook();
      _process.Go(control.Top, control.Height);
      Gkh.Hook();
    }
  }
}
