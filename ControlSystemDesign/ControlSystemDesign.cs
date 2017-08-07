//=======================================================================================
// module 'ControlSystemDesign'
//
// description:
//  This module creates the actions in response to the interrupts from the module form.
//
// J R Dowdle
// 06-Aug-2017
//=======================================================================================

// included namespaces
using Microsoft.Win32;
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

// program namespace
namespace ControlSystemDesign
{
  // beginning of module 'ControlSystemDesign'

  //=====================================================================================
  // class 'ControlSystemDesign:Form'
  //
  // description:
  //  This class contains the data and methods for responding to the interrupts from
  //  the form.
  //
  // J R Dowdle
  // 06-Aug-2017
  //=====================================================================================

  // beginning of class 'ControlSystemDesign:Form'

  public partial class ControlSystemDesign:Form
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

    // set paths and file names
    string FreqRspFileName = "freqrsp.dat";
    string ResultsFileName = "compensator.dat";
    string InputDataFileName = "U:\\John\\Documents\\Software Projects\\Data\\Example1\\input.dat";
    string InputDataFilePath = "U:\\John\\Documents\\Software Projects\\Dat\\Example1";
    string OutputDataPath = "U:\\John\\Documents\\Software Projects\\Data\\Example1";

    //=====================================================================================
    // method 'unwinddata'
    //
    // description:
    //  This method reads data from a file and loads it into arrays for plotting.
    //
    // J R Dowdle
    // 06-Aug-2017
    //=====================================================================================

    // beginning of method 'unwinddata'

    private void unwinddata(ref int m)
    {
      // executable code

      // open input stream
      string PlotFileName = OutputDataPath + "\\" + FreqRspFileName;
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

    //=====================================================================================
    // end of method 'unwinddata'
    //=====================================================================================

    //=====================================================================================
    // method 'ControlSystemDesign
    //
    // description:
    //  This method initializes the form.
    //
    // J R Dowdle
    // 06-Aug-2017
    //=====================================================================================

    // beginning of method 'ControlSystemDesign'

    public ControlSystemDesign()
    {
      // executable code

      InitializeComponent();
    }

    //=====================================================================================
    // end of method 'ControlSystemDesign'
    //=====================================================================================

    //=====================================================================================
    // method 'PlotPlant_Click'
    //
    // description:
    //  This method plots the plant open-loop frequency response in response to a user
    //  request.
    //
    // J R Dowdle
    // 06-Aug-2017
    //=====================================================================================

    // beginning of method 'PlotPlant_Click'

    private void PlotPlant_Click(object sender, EventArgs e)
    {
      // executable code

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

      ChartPlant.Series.Add("minimum response");
      ChartPlant.Series["minimum response"].ChartType = SeriesChartType.Line;
      ChartPlant.Series["minimum response"].ChartArea = "PlantFreqRsp";

      ChartPlant.Series.Add("maximum response");
      ChartPlant.Series["maximum response"].ChartType = SeriesChartType.Line;
      ChartPlant.Series["maximum response"].ChartArea = "PlantFreqRsp";
      for (int i = 0; i < (m - 1); i++)
      {
        ChartPlant.Series["minimum response"].Points.AddXY(w.GetValue(i), svminP.GetValue(i));
        ChartPlant.Series["maximum response"].Points.AddXY(w.GetValue(i), svmaxP.GetValue(i));
      }
    }

    //=====================================================================================
    // end of method 'PlotPlant_Click'
    //=====================================================================================

    //=====================================================================================
    // method 'Ploth2_Click'
    //
    // description:
    //  This method plots the h2-optimal closed-loop frequency response in response to a 
    //  user request.
    //
    // J R Dowdle
    // 06-Aug-2017
    //=====================================================================================

    // beginning of method 'Ploth2_Click'

    private void Ploth2_Click(object sender, EventArgs e)
    {
      // executable code

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

      Charth2.Series.Add("minimum response (z/w)");
      Charth2.Series["minimum response (z/w)"].ChartType = SeriesChartType.Line;
      Charth2.Series["minimum response (z/w)"].ChartArea = "h2FreqRsp";
      Charth2.Series["minimum response (z/w)"].Color = Color.Red;
      Charth2.Series["minimum response (z/w)"].BorderDashStyle = ChartDashStyle.Dash;

      Charth2.Series.Add("maximum response (z/w)");
      Charth2.Series["maximum response (z/w)"].ChartType = SeriesChartType.Line;
      Charth2.Series["maximum response (z/w)"].ChartArea = "h2FreqRsp";
      Charth2.Series["maximum response (z/w)"].Color = Color.Red;
      Charth2.Series["maximum response (z/w)"].BorderDashStyle = ChartDashStyle.Solid;

      Charth2.Series.Add("minimum response (y/w)");
      Charth2.Series["minimum response (y/w)"].ChartType = SeriesChartType.Line;
      Charth2.Series["minimum response (y/w)"].ChartArea = "h2FreqRsp";
      Charth2.Series["minimum response (y/w)"].Color = Color.Blue;
      Charth2.Series["minimum response (y/w)"].BorderDashStyle = ChartDashStyle.Dash;

      Charth2.Series.Add("maximum response (y/w)");
      Charth2.Series["maximum response (y/w)"].ChartType = SeriesChartType.Line;
      Charth2.Series["maximum response (y/w)"].ChartArea = "h2FreqRsp";
      Charth2.Series["maximum response (y/w)"].Color = Color.Blue;
      Charth2.Series["maximum response (y/w)"].BorderDashStyle = ChartDashStyle.Solid;

      Charth2.Series.Add("minimum response (u/w)");
      Charth2.Series["minimum response (u/w)"].ChartType = SeriesChartType.Line;
      Charth2.Series["minimum response (u/w)"].ChartArea = "h2FreqRsp";
      Charth2.Series["minimum response (u/w)"].Color = Color.Cyan;
      Charth2.Series["minimum response (u/w)"].BorderDashStyle = ChartDashStyle.Dash;

      Charth2.Series.Add("maximum response (u/w)");
      Charth2.Series["maximum response (u/w)"].ChartType = SeriesChartType.Line;
      Charth2.Series["maximum response (u/w)"].ChartArea = "h2FreqRsp";
      Charth2.Series["maximum response (u/w)"].Color = Color.Cyan;
      Charth2.Series["maximum response (u/w)"].BorderDashStyle = ChartDashStyle.Solid;

      for (int i = 0; i < (m - 1); i++)
      {
        Charth2.Series["minimum response (z/w)"].Points.AddXY(w.GetValue(i), svminCLh2wz.GetValue(i));
        Charth2.Series["maximum response (z/w)"].Points.AddXY(w.GetValue(i), svmaxCLh2wz.GetValue(i));
        Charth2.Series["minimum response (y/w)"].Points.AddXY(w.GetValue(i), svminCLh2wy.GetValue(i));
        Charth2.Series["maximum response (y/w)"].Points.AddXY(w.GetValue(i), svmaxCLh2wy.GetValue(i));
        Charth2.Series["minimum response (u/w)"].Points.AddXY(w.GetValue(i), svminCLh2wu.GetValue(i));
        Charth2.Series["maximum response (u/w)"].Points.AddXY(w.GetValue(i), svmaxCLh2wu.GetValue(i));
      }
    }

    //=====================================================================================
    // end of method 'Ploth2_Click'
    //=====================================================================================

    //=====================================================================================
    // method 'Plothinf_Click'
    //
    // description:
    //  This method plots the h-infinity closed-loop frequency response in response to a 
    //  user request.
    //
    // J R Dowdle
    // 06-Aug-2017
    //=====================================================================================

    // beginning of method 'Plothinf_Click'

    private void Plothinf_Click(object sender, EventArgs e)
    {
      // executable code

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

      Charthinf.Series.Add("minimum response (z/w)");
      Charthinf.Series["minimum response (z/w)"].ChartType = SeriesChartType.Line;
      Charthinf.Series["minimum response (z/w)"].ChartArea = "hinfFreqRsp";
      Charthinf.Series["minimum response (z/w)"].Color = Color.Red;
      Charthinf.Series["minimum response (z/w)"].BorderDashStyle = ChartDashStyle.Dash;

      Charthinf.Series.Add("maximum response (z/w)");
      Charthinf.Series["maximum response (z/w)"].ChartType = SeriesChartType.Line;
      Charthinf.Series["maximum response (z/w)"].ChartArea = "hinfFreqRsp";
      Charthinf.Series["maximum response (z/w)"].Color = Color.Red;
      Charthinf.Series["maximum response (z/w)"].BorderDashStyle = ChartDashStyle.Solid;

      Charthinf.Series.Add("minimum response (y/w)");
      Charthinf.Series["minimum response (y/w)"].ChartType = SeriesChartType.Line;
      Charthinf.Series["minimum response (y/w)"].ChartArea = "hinfFreqRsp";
      Charthinf.Series["minimum response (y/w)"].Color = Color.Blue;
      Charthinf.Series["minimum response (y/w)"].BorderDashStyle = ChartDashStyle.Dash;

      Charthinf.Series.Add("maximum response (y/w)");
      Charthinf.Series["maximum response (y/w)"].ChartType = SeriesChartType.Line;
      Charthinf.Series["maximum response (y/w)"].ChartArea = "hinfFreqRsp";
      Charthinf.Series["maximum response (y/w)"].Color = Color.Blue;
      Charthinf.Series["maximum response (y/w)"].BorderDashStyle = ChartDashStyle.Solid;

      Charthinf.Series.Add("minimum response (u/w)");
      Charthinf.Series["minimum response (u/w)"].ChartType = SeriesChartType.Line;
      Charthinf.Series["minimum response (u/w)"].ChartArea = "hinfFreqRsp";
      Charthinf.Series["minimum response (u/w)"].Color = Color.Cyan;
      Charthinf.Series["minimum response (u/w)"].BorderDashStyle = ChartDashStyle.Dash;

      Charthinf.Series.Add("maximum response (u/w)");
      Charthinf.Series["maximum response (u/w)"].ChartType = SeriesChartType.Line;
      Charthinf.Series["maximum response (u/w)"].ChartArea = "hinfFreqRsp";
      Charthinf.Series["maximum response (u/w)"].Color = Color.Cyan;
      Charthinf.Series["maximum response (u/w)"].BorderDashStyle = ChartDashStyle.Solid;

      for (int i = 0; i < (m - 1); i++)
      {
        Charthinf.Series["minimum response (z/w)"].Points.AddXY(w.GetValue(i), svminCLhinfwz.GetValue(i));
        Charthinf.Series["maximum response (z/w)"].Points.AddXY(w.GetValue(i), svmaxCLhinfwz.GetValue(i));
        Charthinf.Series["minimum response (y/w)"].Points.AddXY(w.GetValue(i), svminCLhinfwy.GetValue(i));
        Charthinf.Series["maximum response (y/w)"].Points.AddXY(w.GetValue(i), svmaxCLhinfwy.GetValue(i));
        Charthinf.Series["minimum response (u/w)"].Points.AddXY(w.GetValue(i), svminCLhinfwu.GetValue(i));
        Charthinf.Series["maximum response (u/w)"].Points.AddXY(w.GetValue(i), svmaxCLhinfwu.GetValue(i));
      }
    }

    //=====================================================================================
    // end of method 'Plothinf_Click'
    //=====================================================================================

    //=====================================================================================
    // method 'LoadResults_Click'
    //
    // description:
    //  This method loads the compensator results data file into the results text window
    //  in response to a user request.
    //
    // J R Dowdle
    // 06-Aug-2017
    //=====================================================================================

    // beginning of method 'LoadResults_Click'

    private void LoadResults_Click(object sender, EventArgs e)
    {
      // executable code

      // load the data into the results window
      string TextFileName = OutputDataPath + "\\" + ResultsFileName;
      ResultsText.Text = File.ReadAllText(TextFileName);
    }

    //=====================================================================================
    // end of method 'LoadResults_Click'
    //=====================================================================================

    //=====================================================================================
    // method 'Run_Click_1'
    //
    // description:
    //  This method runs the control system design algorithms in response to a user 
    //  request.
    //
    // J R Dowdle
    // 06-Aug-2017
    //=====================================================================================

    // beginning of method 'Run_Click_1'

    private void Run_Click_1(object sender, EventArgs e)
    {
      // executable code

      // spawn a process to run the control system design code
      Process p = new Process();
      p.StartInfo.UseShellExecute = false;
      p.StartInfo.FileName = "U:\\John\\Documents\\Software Projects\\MatrixSolutions\\x64\\Release\\ControlSystemClients.exe";
      p.StartInfo.Arguments = "\"" + InputDataFileName + "\"" + " " + "\"" + OutputDataPath + "\"";
      p.StartInfo.CreateNoWindow = false;
      p.Start();
      p.WaitForExit();

      // load the data into the results window
      string TextFileName = OutputDataPath + "\\" + ResultsFileName;
      ResultsText.Text = File.ReadAllText(TextFileName);
    }

    //=====================================================================================
    // end of method 'Run_Click_1'
    //=====================================================================================

    //=====================================================================================
    // method 'Quit_Click'
    //
    // description:
    //  This method exits the application in response to a user request.
    //
    // J R Dowdle
    // 06-Aug-2017
    //=====================================================================================

    // beginning of method 'Quit_Click'

    private void Quit_Click(object sender, EventArgs e)
    {
      // executable code

      // exit application
      Application.Exit();
    }

    //=====================================================================================
    // end of method 'Quit_Click'
    //=====================================================================================

    //=====================================================================================
    // method 'SelectInputFile_Click'
    //
    // description:
    //  This method ...
    //
    // J R Dowdle
    // 07-Aug-2017
    //=====================================================================================

    // beginning of method 'SelectInputFile_Click'

    private void SelectInputFile_Click(object sender, EventArgs e)
    {
      // executable code

      // open file dialog box
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Title = "Find input data file";
      ofd.InitialDirectory = @"C:\";
      ofd.RestoreDirectory = true;

      // if everything worked ok, get file name and load file into text box
      if (ofd.ShowDialog() == DialogResult.OK)
      {
        // load the data into the results window
        FileInfo file = new FileInfo(ofd.FileName);
        InputDataFileName = file.FullName;
        InputDataFilePath = Path.GetDirectoryName(InputDataFileName);
        ResultsText.Text = File.ReadAllText(InputDataFileName);
      }
    }

    //=====================================================================================
    // end of method 'SelectInputFile_Click'
    //=====================================================================================

    private void SelectOutputFolder_Click(object sender, EventArgs e)
    {
      // executable code

      // open file dialog box
      FolderBrowserDialog fbd = new FolderBrowserDialog();

      // if everything worked ok, get folder path name
      if (fbd.ShowDialog() == DialogResult.OK)
      {
        // get folder path name
        OutputDataPath = fbd.SelectedPath;
      }
    }
  }

  //=====================================================================================
  // end of class 'ControlSystemDesign:Form'
  //=====================================================================================
}

//=======================================================================================
// end of module 'ControlSystemDesign'
//=======================================================================================