﻿namespace ControlSystemDesign
{
  partial class Form1
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
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.richTextBox1 = new System.Windows.Forms.RichTextBox();
      this.button1 = new System.Windows.Forms.Button();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.button2 = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(72, 48);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(636, 35);
      this.textBox1.TabIndex = 0;
      this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
      // 
      // richTextBox1
      // 
      this.richTextBox1.Location = new System.Drawing.Point(81, 139);
      this.richTextBox1.Name = "richTextBox1";
      this.richTextBox1.Size = new System.Drawing.Size(1028, 448);
      this.richTextBox1.TabIndex = 1;
      this.richTextBox1.Text = "";
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(738, 42);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(371, 46);
      this.button1.TabIndex = 2;
      this.button1.Text = "Read Input File";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // textBox2
      // 
      this.textBox2.Location = new System.Drawing.Point(81, 633);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(627, 35);
      this.textBox2.TabIndex = 3;
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(738, 627);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(371, 46);
      this.button2.TabIndex = 4;
      this.button2.Text = "Plant Frequency Response";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(2349, 1239);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.textBox2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.richTextBox1);
      this.Controls.Add(this.textBox1);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.RichTextBox richTextBox1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.Button button2;
  }
}

