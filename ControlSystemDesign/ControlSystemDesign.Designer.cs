namespace ControlSystemDesign
{
  partial class ControlSystemDesign
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
      System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
      System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
      System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
      System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
      System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
      System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
      this.Run = new System.Windows.Forms.Button();
      this.Quit = new System.Windows.Forms.Button();
      this.LoadResults = new System.Windows.Forms.Button();
      this.PlotPlant = new System.Windows.Forms.Button();
      this.Ploth2 = new System.Windows.Forms.Button();
      this.Plothinf = new System.Windows.Forms.Button();
      this.ChartPlant = new System.Windows.Forms.DataVisualization.Charting.Chart();
      this.Charth2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
      this.Charthinf = new System.Windows.Forms.DataVisualization.Charting.Chart();
      this.SelectInputFile = new System.Windows.Forms.Button();
      this.SelectOutputFolder = new System.Windows.Forms.Button();
      this.ResultsText = new System.Windows.Forms.TextBox();
      this.SaveInputFile = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.ChartPlant)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.Charth2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.Charthinf)).BeginInit();
      this.SuspendLayout();
      // 
      // Run
      // 
      this.Run.Location = new System.Drawing.Point(105, 350);
      this.Run.Name = "Run";
      this.Run.Size = new System.Drawing.Size(125, 60);
      this.Run.TabIndex = 10;
      this.Run.Text = "Run";
      this.Run.UseVisualStyleBackColor = true;
      this.Run.Click += new System.EventHandler(this.Run_Click_1);
      // 
      // Quit
      // 
      this.Quit.Location = new System.Drawing.Point(105, 450);
      this.Quit.Name = "Quit";
      this.Quit.Size = new System.Drawing.Size(125, 60);
      this.Quit.TabIndex = 11;
      this.Quit.Text = "Quit";
      this.Quit.UseVisualStyleBackColor = true;
      this.Quit.Click += new System.EventHandler(this.Quit_Click);
      // 
      // LoadResults
      // 
      this.LoadResults.Location = new System.Drawing.Point(825, 1975);
      this.LoadResults.Name = "LoadResults";
      this.LoadResults.Size = new System.Drawing.Size(400, 50);
      this.LoadResults.TabIndex = 9;
      this.LoadResults.Text = "Load Results";
      this.LoadResults.UseVisualStyleBackColor = true;
      this.LoadResults.Click += new System.EventHandler(this.LoadResults_Click);
      // 
      // PlotPlant
      // 
      this.PlotPlant.Location = new System.Drawing.Point(2532, 625);
      this.PlotPlant.Name = "PlotPlant";
      this.PlotPlant.Size = new System.Drawing.Size(400, 50);
      this.PlotPlant.TabIndex = 2;
      this.PlotPlant.Text = "Plant Frequency Response";
      this.PlotPlant.UseVisualStyleBackColor = true;
      this.PlotPlant.Click += new System.EventHandler(this.PlotPlant_Click);
      // 
      // Ploth2
      // 
      this.Ploth2.Location = new System.Drawing.Point(2532, 1300);
      this.Ploth2.Name = "Ploth2";
      this.Ploth2.Size = new System.Drawing.Size(400, 50);
      this.Ploth2.TabIndex = 4;
      this.Ploth2.Text = "h2-Design";
      this.Ploth2.UseVisualStyleBackColor = true;
      this.Ploth2.Click += new System.EventHandler(this.Ploth2_Click);
      // 
      // Plothinf
      // 
      this.Plothinf.Location = new System.Drawing.Point(2532, 1975);
      this.Plothinf.Name = "Plothinf";
      this.Plothinf.Size = new System.Drawing.Size(400, 50);
      this.Plothinf.TabIndex = 5;
      this.Plothinf.Text = "hinf-Design";
      this.Plothinf.UseVisualStyleBackColor = true;
      this.Plothinf.Click += new System.EventHandler(this.Plothinf_Click);
      // 
      // ChartPlant
      // 
      chartArea1.Name = "PlantFreqRsp";
      this.ChartPlant.ChartAreas.Add(chartArea1);
      legend1.Name = "Legend1";
      this.ChartPlant.Legends.Add(legend1);
      this.ChartPlant.Location = new System.Drawing.Point(1750, 50);
      this.ChartPlant.Name = "ChartPlant";
      this.ChartPlant.Size = new System.Drawing.Size(2000, 550);
      this.ChartPlant.TabIndex = 3;
      this.ChartPlant.Text = "Plant Frequency Response";
      // 
      // Charth2
      // 
      chartArea2.Name = "h2FreqRsp";
      this.Charth2.ChartAreas.Add(chartArea2);
      legend2.Name = "Legend1";
      this.Charth2.Legends.Add(legend2);
      this.Charth2.Location = new System.Drawing.Point(1750, 725);
      this.Charth2.Name = "Charth2";
      this.Charth2.Size = new System.Drawing.Size(2000, 550);
      this.Charth2.TabIndex = 6;
      this.Charth2.Text = "h2 Frequency Response";
      // 
      // Charthinf
      // 
      chartArea3.Name = "hinfFreqRsp";
      this.Charthinf.ChartAreas.Add(chartArea3);
      legend3.Name = "Legend1";
      this.Charthinf.Legends.Add(legend3);
      this.Charthinf.Location = new System.Drawing.Point(1750, 1400);
      this.Charthinf.Name = "Charthinf";
      this.Charthinf.Size = new System.Drawing.Size(2000, 550);
      this.Charthinf.TabIndex = 7;
      this.Charthinf.Text = "hinf Frequency Response";
      // 
      // SelectInputFile
      // 
      this.SelectInputFile.Location = new System.Drawing.Point(30, 50);
      this.SelectInputFile.Name = "SelectInputFile";
      this.SelectInputFile.Size = new System.Drawing.Size(280, 60);
      this.SelectInputFile.TabIndex = 12;
      this.SelectInputFile.Text = "Select Input File";
      this.SelectInputFile.UseVisualStyleBackColor = true;
      this.SelectInputFile.Click += new System.EventHandler(this.SelectInputFile_Click);
      // 
      // SelectOutputFolder
      // 
      this.SelectOutputFolder.Location = new System.Drawing.Point(30, 250);
      this.SelectOutputFolder.Name = "SelectOutputFolder";
      this.SelectOutputFolder.Size = new System.Drawing.Size(280, 60);
      this.SelectOutputFolder.TabIndex = 13;
      this.SelectOutputFolder.Text = "Select Output Folder";
      this.SelectOutputFolder.UseVisualStyleBackColor = true;
      this.SelectOutputFolder.Click += new System.EventHandler(this.SelectOutputFolder_Click);
      // 
      // ResultsText
      // 
      this.ResultsText.Location = new System.Drawing.Point(375, 50);
      this.ResultsText.Multiline = true;
      this.ResultsText.Name = "ResultsText";
      this.ResultsText.Size = new System.Drawing.Size(1300, 1900);
      this.ResultsText.TabIndex = 15;
      // 
      // SaveInputFile
      // 
      this.SaveInputFile.Location = new System.Drawing.Point(30, 150);
      this.SaveInputFile.Name = "SaveInputFile";
      this.SaveInputFile.Size = new System.Drawing.Size(280, 60);
      this.SaveInputFile.TabIndex = 16;
      this.SaveInputFile.Text = "Save Input File";
      this.SaveInputFile.UseVisualStyleBackColor = true;
      this.SaveInputFile.Click += new System.EventHandler(this.EditInputFile_Click);
      // 
      // ControlSystemDesign
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.ClientSize = new System.Drawing.Size(3828, 2097);
      this.Controls.Add(this.SaveInputFile);
      this.Controls.Add(this.ResultsText);
      this.Controls.Add(this.SelectOutputFolder);
      this.Controls.Add(this.SelectInputFile);
      this.Controls.Add(this.Quit);
      this.Controls.Add(this.Run);
      this.Controls.Add(this.LoadResults);
      this.Controls.Add(this.Charthinf);
      this.Controls.Add(this.Charth2);
      this.Controls.Add(this.Plothinf);
      this.Controls.Add(this.Ploth2);
      this.Controls.Add(this.ChartPlant);
      this.Controls.Add(this.PlotPlant);
      this.Name = "ControlSystemDesign";
      this.Text = "Control System Design";
      ((System.ComponentModel.ISupportInitialize)(this.ChartPlant)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.Charth2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.Charthinf)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.Button Run;
    private System.Windows.Forms.Button Quit;
    private System.Windows.Forms.Button LoadResults;
    private System.Windows.Forms.Button PlotPlant;
    private System.Windows.Forms.Button Ploth2;
    private System.Windows.Forms.Button Plothinf;
    private System.Windows.Forms.DataVisualization.Charting.Chart ChartPlant;
    private System.Windows.Forms.DataVisualization.Charting.Chart Charth2;
    private System.Windows.Forms.DataVisualization.Charting.Chart Charthinf;
    private System.Windows.Forms.Button SelectInputFile;
    private System.Windows.Forms.Button SelectOutputFolder;
    private System.Windows.Forms.TextBox ResultsText;
    private System.Windows.Forms.Button SaveInputFile;
  }
}

