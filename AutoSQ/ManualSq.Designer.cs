namespace AutoSQ
{
  partial class ManualSq
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqForm));
      this.UTextBox = new MTextBox();
      this.ITextBox = new MTextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.sqLabel = new System.Windows.Forms.Label();
      this.cursor = new System.Windows.Forms.PictureBox();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.cursor)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.SuspendLayout();
      // 
      // UTextBox
      // 
      this.UTextBox.Location = new System.Drawing.Point(158, 12);
      this.UTextBox.Name = "UTextBox";
      this.UTextBox.Size = new System.Drawing.Size(100, 20);
      this.UTextBox.TabIndex = 0;
      this.UTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UTextBox_KeyDown);
      // 
      // ITextBox
      // 
      this.ITextBox.Location = new System.Drawing.Point(158, 38);
      this.ITextBox.Name = "ITextBox";
      this.ITextBox.Size = new System.Drawing.Size(100, 20);
      this.ITextBox.TabIndex = 1;
      this.ITextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ITextBox_KeyDown);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 15);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(140, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Average unspent resources:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 41);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(125, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Resource collection rate:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 67);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(25, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "SQ:";
      // 
      // sqLabel
      // 
      this.sqLabel.AutoSize = true;
      this.sqLabel.Location = new System.Drawing.Point(43, 67);
      this.sqLabel.Name = "sqLabel";
      this.sqLabel.Size = new System.Drawing.Size(13, 13);
      this.sqLabel.TabIndex = 5;
      this.sqLabel.Text = "0";
      // 
      // cursor
      // 
      this.cursor.Image = global::AutoSQ.Properties.Resources.Cursor;
      this.cursor.Location = new System.Drawing.Point(309, 197);
      this.cursor.Name = "cursor";
      this.cursor.Size = new System.Drawing.Size(9, 9);
      this.cursor.TabIndex = 7;
      this.cursor.TabStop = false;
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = global::AutoSQ.Properties.Resources.macro;
      this.pictureBox1.InitialImage = global::AutoSQ.Properties.Resources.macro;
      this.pictureBox1.Location = new System.Drawing.Point(15, 97);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(318, 150);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBox1.TabIndex = 6;
      this.pictureBox1.TabStop = false;
      // 
      // SqForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(347, 264);
      this.Controls.Add(this.cursor);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.sqLabel);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.ITextBox);
      this.Controls.Add(this.UTextBox);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MaximumSize = new System.Drawing.Size(363, 300);
      this.MinimumSize = new System.Drawing.Size(363, 300);
      this.Name = "SqForm";
      this.Text = "Spending Quotient Calculator";
      ((System.ComponentModel.ISupportInitialize)(this.cursor)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private MTextBox UTextBox;
    private MTextBox ITextBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label sqLabel;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.PictureBox cursor;
  }
}

