using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ControlSystemDesign
{
  public partial class ControlSystemDesign : Form
  {

    // set up global arrays
    Array w = Array.CreateInstance(typeof(double), 1000);
    Array svminP = Array.CreateInstance(typeof(double), 1000);
    Array svmaxP = Array.CreateInstance(typeof(double), 1000);
    Array svminCLh2wz = Array.CreateInstance(typeof(double), 1000);
    Array svmaxCLh2wz = Array.CreateInstance(typeof(double), 1000);
    Array svminCLh2wy = Array.CreateInstance(typeof(double), 1000);
    Array svmaxCLh2wy = Array.CreateInstance(typeof(double), 1000);
    Array svminCLh2wu = Array.CreateInstance(typeof(double), 1000);
    Array svmaxCLh2wu = Array.CreateInstance(typeof(double), 1000);
    Array svminCLhinfwz = Array.CreateInstance(typeof(double), 1000);
    Array svmaxCLhinfwz = Array.CreateInstance(typeof(double), 1000);
    Array svminCLhinfwy = Array.CreateInstance(typeof(double), 1000);
    Array svmaxCLhinfwy = Array.CreateInstance(typeof(double), 1000);
    Array svminCLhinfwu = Array.CreateInstance(typeof(double), 1000);
    Array svmaxCLhinfwu = Array.CreateInstance(typeof(double), 1000);
    string ControlSystemParentFolder = @"U:\\John\Documents\\Software Projects";
    string WorkingDataParentFolder = "Data";
    string DataFolder = "Example1";
    string FreqRspFileName = "freqrsp.dat";
    string ResultsFileName = "compensator.dat";

    private void unwinddata(ref int m)
    {
      // open input stream
      string PlotFileName = ControlSystemParentFolder + "\\" + WorkingDataParentFolder + "\\" + DataFolder + "\\" + FreqRspFileName;
      string[] records = File.ReadAllLines(PlotFileName);
      int n = records.Length;

      // the first part of file has the open-loop plant frequency response
      int startIndex = records.Length + 1;
      int endIndex = startIndex + 1;
      for (int i = 0; i < n; i++)
      {
        if (records[i].Contains("//") && (startIndex > records.Length) && (endIndex > records.Length))
        {
          // do nothing
        }
        else if (!records[i].Contains("//") & (startIndex > records.Length) && (endIndex > records.Length))
        {
          startIndex = i + 1;
        }
        else if (records[i].Contains("//") && (startIndex < records.Length) && (endIndex > records.Length))
        {
          endIndex = i;
        }
      }

      // now that the data has been isolated between startIndex and endIndex, grab the data and put it into arrays
      m = endIndex - startIndex + 1;

      // grab the data
      for (int i = startIndex; i < endIndex; i++)
      {
        char delimiter = ' ';
        String[] substrings = records[i].Split(delimiter);
        bool wfound = false;
        bool svminPfound = false;
        bool svmaxPfound = false;
        foreach (var substring in substrings)
        {
          if (!wfound && !substring.Equals(""))
          {
            wfound = true;
            w.SetValue(Convert.ToDouble(substring), i - startIndex);
          }

          else if (wfound && !svminPfound && !substring.Equals(""))
          {
            svminPfound = true;
            svminP.SetValue(Convert.ToDouble(substring), i - startIndex);
          }

          else if (wfound && svminPfound && !svmaxPfound && !substring.Equals(""))
          {
            svmaxPfound = true;
            svmaxP.SetValue(Convert.ToDouble(substring), i - startIndex);
          }
        }
      }

      // the second part of file has the closed-loop frequency response under an h2-compensator
      m = endIndex - startIndex + 1;
      int startSearch = endIndex + 1;

      // reset startIndex and endIndex for next part of search
      startIndex = records.Length + 1;
      endIndex = startIndex + 1;
      for (int i = startSearch; i < n; i++)
      {
        // find the top of the closed-loop h2-compensator frequency response
        if (records[i].Contains("//**") && (startIndex > records.Length) && (endIndex > records.Length))
        {
          // do nothing
        }
        // find records with comments
        else if (records[i].Contains("//") && (startIndex > records.Length) && (endIndex > records.Length))
        {
          // do nothing
        }
        // find records with data
        else if (!records[i].Contains("//") & (startIndex > records.Length) && (endIndex > records.Length))
        {
          startIndex = i + 1;
        }
        // find next data group
        else if (records[i].Contains("//**") && (startIndex < records.Length) && (endIndex > records.Length))
        {
          endIndex = i;
        }
      }

      // now that the data has been isolated between startIndex and endIndex, grab the data and put it into arrays

      // grab the data
      // there are three data sets of interest:  
      //    1.  transfer from w to z (z/w)
      //    2.  transfer from w to y (y/w)
      //    3.  transfer from w to u (u/w)
      // the strucuture of the data is:  [m data lines; 2 comments; m data lines; 2 comments; m data lines]
      for (int k = 0; k < 3; k++)
      {

        for (int i = startIndex + k* (m + 2); i < startIndex + m + k*(m + 2); i++)
        {
          char delimiter = ' ';
          String[] substrings = records[i].Split(delimiter);
          bool wfound = false;
          bool svminPfound = false;
          bool svmaxPfound = false;
          foreach (var substring in substrings)
          {
            if (records[i].Contains("//"))
            {
              // do nothing
            }
            else if (!wfound && !substring.Equals(""))
            {
              wfound = true;
              w.SetValue(Convert.ToDouble(substring), i - startIndex - k* (m + 2));
            }

            else if (wfound && !svminPfound && !substring.Equals(""))
            {
              svminPfound = true;
              if (k == 0)
                svminCLh2wz.SetValue(Convert.ToDouble(substring), i - startIndex - k* (m + 2));
              if (k == 1)
                svminCLh2wy.SetValue(Convert.ToDouble(substring), i - startIndex - k* (m + 2));
              if (k == 2)
                svminCLh2wu.SetValue(Convert.ToDouble(substring), i - startIndex - k* (m + 2));
            }

            else if (wfound && svminPfound && !svmaxPfound && !substring.Equals(""))
            {
              svmaxPfound = true;
              if (k == 0)
                svmaxCLh2wz.SetValue(Convert.ToDouble(substring), i - startIndex- k* (m + 2));
              if (k == 1)
                svmaxCLh2wy.SetValue(Convert.ToDouble(substring), i - startIndex- k* (m + 2));
              if (k == 2)
                svmaxCLh2wu.SetValue(Convert.ToDouble(substring), i - startIndex- k* (m + 2));
            }
          }
        }
      }

      // the third part of file has the closed-loop frequency response under an hinf-compensator
      startSearch = endIndex + 1;

      // reset startIndex and endIndex for next part of search
      startIndex = records.Length + 1;
      endIndex = startIndex + 1;
      for (int i = startSearch; i < n; i++)
      {
        // find the top of the closed-loop h2-compensator frequency response
        if (records[i].Contains("//**") && (startIndex > records.Length) && (endIndex > records.Length))
        {
          // do nothing
        }
        // find records with comments
        else if (records[i].Contains("//") && (startIndex > records.Length) && (endIndex > records.Length))
        {
          // do nothing
        }
        // find records with data
        else if (!records[i].Contains("//") & (startIndex > records.Length) && (endIndex > records.Length))
        {
          startIndex = i + 1;
        }
        // find next data group
        else if (records[i].Contains("//**") && (startIndex < records.Length) && (endIndex > records.Length))
        {
          endIndex = i;
        }
      }

      // now that the data has been isolated between startIndex and endIndex, grab the data and put it into arrays

      // grab the data
      // there are three data sets of interest:  
      //    1.  transfer from w to z (z/w)
      //    2.  transfer from w to y (y/w)
      //    3.  transfer from w to u (u/w)
      // the strucuture of the data is:  [m data lines; 2 comments; m data lines; 2 comments; m data lines]
      for (int k = 0; k < 3; k++)
      {

        for (int i = startIndex + k* (m + 2); i < Math.Min(startIndex + m + k* (m + 2), records.Length); i++)
        {
          char delimiter = ' ';
          String[] substrings = records[i].Split(delimiter);
          bool wfound = false;
          bool svminPfound = false;
          bool svmaxPfound = false;
          foreach (var substring in substrings)
          {
            if (records[i].Contains("//"))
            {
              // do nothing
            }
            else if (!wfound && !substring.Equals(""))
            {
              wfound = true;
              w.SetValue(Convert.ToDouble(substring), i - startIndex - k* (m + 2));
            }

            else if (wfound && !svminPfound && !substring.Equals(""))
            {
              svminPfound = true;
              if (k == 0)
                svminCLhinfwz.SetValue(Convert.ToDouble(substring), i - startIndex - k* (m + 2));
              if (k == 1)
                svminCLhinfwy.SetValue(Convert.ToDouble(substring), i - startIndex - k* (m + 2));
              if (k == 2)
                svminCLhinfwu.SetValue(Convert.ToDouble(substring), i - startIndex - k* (m + 2));
            }

            else if (wfound && svminPfound && !svmaxPfound && !substring.Equals(""))
            {
              svmaxPfound = true;
              if (k == 0)
                svmaxCLhinfwz.SetValue(Convert.ToDouble(substring), i - startIndex- k* (m + 2));
              if (k == 1)
                svmaxCLhinfwy.SetValue(Convert.ToDouble(substring), i - startIndex- k* (m + 2));
              if (k == 2)
                svmaxCLhinfwu.SetValue(Convert.ToDouble(substring), i - startIndex- k* (m + 2));
            }
          }
        }
      }
    }

    public ControlSystemDesign()
    {
      InitializeComponent();
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {

    }

    private void PlotPlant_Click(object sender, EventArgs e)
    {
      // invoke unwinddata
      int m = 0;
      unwinddata(ref m);

      // compute the min/max values for the vertical axis
      double minY, maxY;
      minY = Math.Pow(2, 52);
      maxY = Math.Pow(2, -52);
      for (int i = 0; i < (m - 1); i++)
      {
        // set minimum value for Y axis
        minY = Math.Min(minY, Convert.ToDouble(svminP.GetValue(i)));
        // set maximum value for Y axis
        maxY = Math.Max(maxY, Convert.ToDouble(svmaxP.GetValue(i)));
      }
      // find closest decades to minY and maxY
      int decminY = (int)Math.Log10(minY) -  1;
      int decmaxY = (int)Math.Log10(maxY) +  1;

      // load data and display on the chart
      ChartPlant.Series.Clear();
      ChartPlant.Titles.Clear();
      ChartPlant.Titles.Add("Open-Loop Plant");
      ChartPlant.ChartAreas[0].Name = "PlantFreqRsp";
      ChartPlant.ChartAreas[0].AxisX.IsLogarithmic = true;
      ChartPlant.ChartAreas[0].AxisY.IsLogarithmic = true;
      ChartPlant.ChartAreas[0].AxisX.Title = "Frequency [r/s]";
      ChartPlant.ChartAreas[0].AxisY.Title = "Magnitude";
      ChartPlant.ChartAreas[0].AxisY.Minimum = Math.Pow(10, decminY);
      ChartPlant.ChartAreas[0].AxisY.Maximum = Math.Pow(10, decmaxY);

      ChartPlant.Series.Add("minimum singular value");
      ChartPlant.Series["minimum singular value"].ChartType = SeriesChartType.Line;
      ChartPlant.Series["minimum singular value"].ChartArea = "PlantFreqRsp";

      ChartPlant.Series.Add("maximum singular value");
      ChartPlant.Series["maximum singular value"].ChartType = SeriesChartType.Line;
      ChartPlant.Series["maximum singular value"].ChartArea = "PlantFreqRsp";
      for (int i = 0; i < (m - 1); i++)
      {
        ChartPlant.Series["minimum singular value"].Points.AddXY(w.GetValue(i), svminP.GetValue(i));
        ChartPlant.Series["maximum singular value"].Points.AddXY(w.GetValue(i), svmaxP.GetValue(i));
      }
    }

    private void textBox1_TextChanged_1(object sender, EventArgs e)
    {

    }

    private void ResultsText_TextChanged(object sender, EventArgs e)
    {

    }

    private void Ploth2_Click(object sender, EventArgs e)
    {
      // invoke unwinddata
      int m = 0;
      unwinddata(ref m);

      // compute the min/max values for the vertical axis
      double minY, maxY;
      minY = Math.Pow(2, 52);
      maxY = Math.Pow(2, -52);
      for (int i = 0; i < (m - 1); i++)
      {
        // set minimum value for Y axis
        minY = Math.Min(minY, Convert.ToDouble(svminCLh2wz.GetValue(i)));
        minY = Math.Min(minY, Convert.ToDouble(svminCLh2wy.GetValue(i)));
        minY = Math.Min(minY, Convert.ToDouble(svminCLh2wu.GetValue(i)));
        // set maximum value for Y axis
        maxY = Math.Max(maxY, Convert.ToDouble(svmaxCLh2wz.GetValue(i)));
        maxY = Math.Max(maxY, Convert.ToDouble(svmaxCLh2wy.GetValue(i)));
        maxY = Math.Max(maxY, Convert.ToDouble(svmaxCLh2wu.GetValue(i)));
        // set maximum value for Y axis
        if (Convert.ToDouble(svmaxCLh2wz.GetValue(i)) > maxY)
          maxY = Convert.ToDouble(svmaxCLh2wz.GetValue(i));
        if (Convert.ToDouble(svmaxCLh2wy.GetValue(i)) > maxY)
          maxY = Convert.ToDouble(svmaxCLh2wy.GetValue(i));
        if (Convert.ToDouble(svmaxCLh2wu.GetValue(i)) > maxY)
          maxY = Convert.ToDouble(svmaxCLh2wu.GetValue(i));
      }
      // find closest decades to minY and maxY
      int decminY = (int)Math.Log10(minY) -  1;
      int decmaxY = (int)Math.Log10(maxY) +  1;

      // load data and display on the chart
      Charth2.Series.Clear();
      Charth2.Titles.Clear();
      Charth2.Titles.Add("Closed-Loop h2 Design");
      Charth2.ChartAreas[0].Name = "h2FreqRsp";
      Charth2.ChartAreas[0].AxisX.IsLogarithmic = true;
      Charth2.ChartAreas[0].AxisY.IsLogarithmic = true;
      Charth2.ChartAreas[0].AxisX.Title = "Frequency [r/s]";
      Charth2.ChartAreas[0].AxisY.Title = "Magnitude";
      Charth2.ChartAreas[0].AxisY.Minimum = Math.Pow(10, decminY);
      Charth2.ChartAreas[0].AxisY.Maximum = Math.Pow(10, decmaxY);

      Charth2.Series.Add("minimum singular value (z/w)");
      Charth2.Series["minimum singular value (z/w)"].ChartType = SeriesChartType.Line;
      Charth2.Series["minimum singular value (z/w)"].ChartArea = "h2FreqRsp";
      Charth2.Series["minimum singular value (z/w)"].Color = Color.Red;
      Charth2.Series["minimum singular value (z/w)"].BorderDashStyle = ChartDashStyle.Dash;

      Charth2.Series.Add("maximum singular value (z/w)");
      Charth2.Series["maximum singular value (z/w)"].ChartType = SeriesChartType.Line;
      Charth2.Series["maximum singular value (z/w)"].ChartArea = "h2FreqRsp";
      Charth2.Series["maximum singular value (z/w)"].Color = Color.Red;
      Charth2.Series["maximum singular value (z/w)"].BorderDashStyle = ChartDashStyle.Solid;

      Charth2.Series.Add("minimum singular value (y/w)");
      Charth2.Series["minimum singular value (y/w)"].ChartType = SeriesChartType.Line;
      Charth2.Series["minimum singular value (y/w)"].ChartArea = "h2FreqRsp";
      Charth2.Series["minimum singular value (y/w)"].Color = Color.Blue;
      Charth2.Series["minimum singular value (y/w)"].BorderDashStyle = ChartDashStyle.Dash;

      Charth2.Series.Add("maximum singular value (y/w)");
      Charth2.Series["maximum singular value (y/w)"].ChartType = SeriesChartType.Line;
      Charth2.Series["maximum singular value (y/w)"].ChartArea = "h2FreqRsp";
      Charth2.Series["maximum singular value (y/w)"].Color = Color.Blue;
      Charth2.Series["maximum singular value (y/w)"].BorderDashStyle = ChartDashStyle.Solid;

      Charth2.Series.Add("minimum singular value (u/w)");
      Charth2.Series["minimum singular value (u/w)"].ChartType = SeriesChartType.Line;
      Charth2.Series["minimum singular value (u/w)"].ChartArea = "h2FreqRsp";
      Charth2.Series["minimum singular value (u/w)"].Color = Color.Cyan;
      Charth2.Series["minimum singular value (u/w)"].BorderDashStyle = ChartDashStyle.Dash;

      Charth2.Series.Add("maximum singular value (u/w)");
      Charth2.Series["maximum singular value (u/w)"].ChartType = SeriesChartType.Line;
      Charth2.Series["maximum singular value (u/w)"].ChartArea = "h2FreqRsp";
      Charth2.Series["maximum singular value (u/w)"].Color = Color.Cyan;
      Charth2.Series["maximum singular value (u/w)"].BorderDashStyle = ChartDashStyle.Solid;

      for (int i = 0; i < (m - 1); i++)
      {
        Charth2.Series["minimum singular value (z/w)"].Points.AddXY(w.GetValue(i), svminCLh2wz.GetValue(i));
        Charth2.Series["maximum singular value (z/w)"].Points.AddXY(w.GetValue(i), svmaxCLh2wz.GetValue(i));
        Charth2.Series["minimum singular value (y/w)"].Points.AddXY(w.GetValue(i), svminCLh2wy.GetValue(i));
        Charth2.Series["maximum singular value (y/w)"].Points.AddXY(w.GetValue(i), svmaxCLh2wy.GetValue(i));
        Charth2.Series["minimum singular value (u/w)"].Points.AddXY(w.GetValue(i), svminCLh2wu.GetValue(i));
        Charth2.Series["maximum singular value (u/w)"].Points.AddXY(w.GetValue(i), svmaxCLh2wu.GetValue(i));
      }
    }

    private void Plothinf_Click(object sender, EventArgs e)
    {
      // invoke unwinddata
      int m = 0;
      unwinddata(ref m);

      // compute the min/max values for the vertical axis
      double minY, maxY;
      minY = Math.Pow(2, 52);
      maxY = Math.Pow(2, -52);
      for (int i = 0; i < (m - 1); i++)
      {
        // set minimum value for Y axis
        minY = Math.Min(minY, Convert.ToDouble(svminCLhinfwz.GetValue(i)));
        minY = Math.Min(minY, Convert.ToDouble(svminCLhinfwy.GetValue(i)));
        minY = Math.Min(minY, Convert.ToDouble(svminCLhinfwu.GetValue(i)));
        // set maximum value for Y axis
        maxY = Math.Max(maxY, Convert.ToDouble(svmaxCLhinfwz.GetValue(i)));
        maxY = Math.Max(maxY, Convert.ToDouble(svmaxCLhinfwy.GetValue(i)));
        maxY = Math.Max(maxY, Convert.ToDouble(svmaxCLhinfwu.GetValue(i)));
      }
      // find closest decades to minY and maxY
      int decminY = (int)Math.Log10(minY) -  1;
      int decmaxY = (int)Math.Log10(maxY) +  1;

      // load data and display on the chart
      Charthinf.Series.Clear();
      Charthinf.Titles.Clear();
      Charthinf.Titles.Add("Closed-Loop h-infinity Design");
      Charthinf.ChartAreas[0].Name = "hinfFreqRsp";
      Charthinf.ChartAreas[0].AxisX.IsLogarithmic = true;
      Charthinf.ChartAreas[0].AxisY.IsLogarithmic = true;
      Charthinf.ChartAreas[0].AxisX.Title = "Frequency [r/s]";
      Charthinf.ChartAreas[0].AxisY.Title = "Magnitude";
      Charthinf.ChartAreas[0].AxisY.Minimum = Math.Pow(10, decminY);
      Charthinf.ChartAreas[0].AxisY.Maximum = Math.Pow(10, decmaxY);

      Charthinf.Series.Add("minimum singular value (z/w)");
      Charthinf.Series["minimum singular value (z/w)"].ChartType = SeriesChartType.Line;
      Charthinf.Series["minimum singular value (z/w)"].ChartArea = "hinfFreqRsp";
      Charthinf.Series["minimum singular value (z/w)"].Color = Color.Red;
      Charthinf.Series["minimum singular value (z/w)"].BorderDashStyle = ChartDashStyle.Dash;

      Charthinf.Series.Add("maximum singular value (z/w)");
      Charthinf.Series["maximum singular value (z/w)"].ChartType = SeriesChartType.Line;
      Charthinf.Series["maximum singular value (z/w)"].ChartArea = "hinfFreqRsp";
      Charthinf.Series["maximum singular value (z/w)"].Color = Color.Red;
      Charthinf.Series["maximum singular value (z/w)"].BorderDashStyle = ChartDashStyle.Solid;

      Charthinf.Series.Add("minimum singular value (y/w)");
      Charthinf.Series["minimum singular value (y/w)"].ChartType = SeriesChartType.Line;
      Charthinf.Series["minimum singular value (y/w)"].ChartArea = "hinfFreqRsp";
      Charthinf.Series["minimum singular value (y/w)"].Color = Color.Blue;
      Charthinf.Series["minimum singular value (y/w)"].BorderDashStyle = ChartDashStyle.Dash;

      Charthinf.Series.Add("maximum singular value (y/w)");
      Charthinf.Series["maximum singular value (y/w)"].ChartType = SeriesChartType.Line;
      Charthinf.Series["maximum singular value (y/w)"].ChartArea = "hinfFreqRsp";
      Charthinf.Series["maximum singular value (y/w)"].Color = Color.Blue;
      Charthinf.Series["maximum singular value (y/w)"].BorderDashStyle = ChartDashStyle.Solid;

      Charthinf.Series.Add("minimum singular value (u/w)");
      Charthinf.Series["minimum singular value (u/w)"].ChartType = SeriesChartType.Line;
      Charthinf.Series["minimum singular value (u/w)"].ChartArea = "hinfFreqRsp";
      Charthinf.Series["minimum singular value (u/w)"].Color = Color.Cyan;
      Charthinf.Series["minimum singular value (u/w)"].BorderDashStyle = ChartDashStyle.Dash;

      Charthinf.Series.Add("maximum singular value (u/w)");
      Charthinf.Series["maximum singular value (u/w)"].ChartType = SeriesChartType.Line;
      Charthinf.Series["maximum singular value (u/w)"].ChartArea = "hinfFreqRsp";
      Charthinf.Series["maximum singular value (u/w)"].Color = Color.Cyan;
      Charthinf.Series["maximum singular value (u/w)"].BorderDashStyle = ChartDashStyle.Solid;

      for (int i = 0; i < (m - 1); i++)
      {
        Charthinf.Series["minimum singular value (z/w)"].Points.AddXY(w.GetValue(i), svminCLhinfwz.GetValue(i));
        Charthinf.Series["maximum singular value (z/w)"].Points.AddXY(w.GetValue(i), svmaxCLhinfwz.GetValue(i));
        Charthinf.Series["minimum singular value (y/w)"].Points.AddXY(w.GetValue(i), svminCLhinfwy.GetValue(i));
        Charthinf.Series["maximum singular value (y/w)"].Points.AddXY(w.GetValue(i), svmaxCLhinfwy.GetValue(i));
        Charthinf.Series["minimum singular value (u/w)"].Points.AddXY(w.GetValue(i), svminCLhinfwu.GetValue(i));
        Charthinf.Series["maximum singular value (u/w)"].Points.AddXY(w.GetValue(i), svmaxCLhinfwu.GetValue(i));
      }
    }

    private void ControlSystemDesign_Load(object sender, EventArgs e)
    {
      

    }

    private void LoadResults_Click(object sender, EventArgs e)
    {
      string TextFileName = ControlSystemParentFolder + "\\" + WorkingDataParentFolder + "\\" + DataFolder + "\\" + ResultsFileName;
      ResultsText.Text = File.ReadAllText(TextFileName);
    }

    private void Run_Click(object sender, EventArgs e)
    {
    }

    private void Run_Click_1(object sender, EventArgs e)
    {
      Process p = new Process();
      p.StartInfo.UseShellExecute = false;
      p.StartInfo.FileName = "U:\\John\\Documents\\Software Projects\\MatrixSolutions\\x64\\Release\\ControlSystemClients.exe";
      p.StartInfo.CreateNoWindow = true;
      p.Start();
      p.WaitForExit();
    }

    private void Quit_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }
  }
}
