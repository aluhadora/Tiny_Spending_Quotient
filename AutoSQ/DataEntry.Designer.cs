namespace AutoSQ
{
  partial class DataEntry
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.textBox = new AutoSQ.MTextBox();
      this.textBoxSteal = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // textBox
      // 
      this.textBox.Location = new System.Drawing.Point(12, 12);
      this.textBox.MaxLength = 1;
      this.textBox.Name = "textBox";
      this.textBox.Size = new System.Drawing.Size(20, 20);
      this.textBox.TabIndex = 0;
      // 
      // textBoxSteal
      // 
      this.textBoxSteal.Location = new System.Drawing.Point(2, 51);
      this.textBoxSteal.Name = "textBoxSteal";
      this.textBoxSteal.Size = new System.Drawing.Size(100, 20);
      this.textBoxSteal.TabIndex = 1;
      // 
      // DataEntry
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Cyan;
      this.ClientSize = new System.Drawing.Size(44, 44);
      this.Controls.Add(this.textBoxSteal);
      this.Controls.Add(this.textBox);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "DataEntry";
      this.Text = "DataEntry";
      this.TransparencyKey = System.Drawing.Color.Cyan;
      this.Load += new System.EventHandler(this.DataEntry_Load);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DataEntry_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private MTextBox textBox;
    private System.Windows.Forms.TextBox textBoxSteal;
  }
}