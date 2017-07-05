using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Coins
{
    public partial class frmGrafico : Form
    {
        public frmGrafico()
        {
            InitializeComponent();
            PreencherChart();
        }

        private void PreencherChart()
        {
            chtGrafico.Series[0].Points.Clear();
            chtGrafico.Legends.Clear();

            //chtGrafico.Series[0].ChartType = SeriesChartType.Area;

            //define o tipo de gráfico
            chtGrafico.Series[0].ChartType = SeriesChartType.Line;
            chtGrafico.ChartAreas[0].Area3DStyle.LightStyle = LightStyle.Simplistic;

            //define o texto da legenda 
            chtGrafico.Series[0].LegendText = "Volume X Preço";

            //define o titulo do eixo y , sua fonte e a cor
            chtGrafico.ChartAreas[0].AxisY.Title = "Volume";
            chtGrafico.ChartAreas[0].AxisY.TitleFont = new Font("Times New Roman", 12, FontStyle.Bold);
            chtGrafico.ChartAreas[0].AxisY.TitleForeColor = Color.Blue;

            //define o titulo do eixo x , sua fonte e a cor
            chtGrafico.ChartAreas[0].AxisX.Title = "Preço";
            chtGrafico.ChartAreas[0].AxisX.TitleFont = new Font("Times New Roman", 12, FontStyle.Bold);
            chtGrafico.ChartAreas[0].AxisX.TitleForeColor = Color.Blue;

            //define a paleta de cores usada
            chtGrafico.Palette = ChartColorPalette.Chocolate;

            foreach (Orderbook item in Coin.lOrderbookLite)
            {

                //if (chtGrafico.Series[0].Points.Count < 220)//265)
                if (chtGrafico.Series[0].Points.Count < 400)
                //{
                    chtGrafico.Series[0].Points.AddXY(double.Parse(item.Volume), double.Parse(item.Preco));
                    //chtGrafico.Series[0].Label = (item.Volume + " - " + item.Preco);
                    //chtGrafico.Series[0].LegendText = (item.Volume + " - " + item.Preco);
                    //chtGrafico.Series[0].IsValueShownAsLabel  = true;
                //}
            }
            chtGrafico.ChartAreas[0].CursorX.IntervalType = DateTimeIntervalType.Auto;
            chtGrafico.ChartAreas[0].CursorX.Interval = 1;

            //chtGrafico.Series[0].IsValueShownAsLabel = true;

            //desabilita a exibição 3D
            chtGrafico.ChartAreas[0].Area3DStyle.Enable3D = false;

            //exibe os valores nos eixos
            //chtGrafico.Series[0].IsValueShownAsLabel = true;
            //https://msdn.microsoft.com/pt-br/library/system.web.ui.datavisualization.charting.axisscalebreakstyle(v=vs.110).aspx

            //chtGrafico.ChartAreas[0].AxisY.ScaleBreakStyle.BreakLineStyle =  BreakLineStyle.None;

            // Enable scale breaks.
            //chtGrafico.ChartAreas[0].AxisY.ScaleBreakStyle.Enabled = true;

            // Show scale break if more than 25% of the chart is empty space.
            //chtGrafico.ChartAreas[0].AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 25;

            // Set the line width of the scale break.
            //chtGrafico.ChartAreas[0].AxisY.ScaleBreakStyle.LineWidth = 2;

            // Set the color of the scale break.
            //chtGrafico.ChartAreas[0].AxisY.ScaleBreakStyle.LineColor = Color.Red;

            // If all data points are significantly far from zero, the chart will calculate the scale minimum value.
            //chtGrafico.ChartAreas[0].AxisY.ScaleBreakStyle.StartFromZero = StartFromZero.Auto;

            // Set the spacing gap between the lines of the scale break (as a percentage of the Y-axis).
            //chtGrafico.ChartAreas[0].AxisY.ScaleBreakStyle.Spacing = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PreencherChart();
        }

        //private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        //{
        //    System.Drawing.Font printFont = new System.Drawing.Font("Arial", 10);
        //    myRec = new System.Drawing.Rectangle(100, 50, 900, 450);
        //    chart1.Printing.PrintPaint(ev.Graphics, myRec);
        //}

        //private void btnImprimir_Click(object sender, EventArgs e)
        //{
        //    // //define os objetos printdocument e os eventos associados
        //    PrintDocument pd = new PrintDocument();
        //    pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
        //    pd.DefaultPageSettings.Landscape = true;

        //    //define o objeto para visualizar a impressao
        //    PrintPreviewDialog objPrintPreview = new PrintPreviewDialog();
        //    try
        //    {
        //        //define o formulário como maximizado e com Zoom
        //        var _with1 = objPrintPreview;
        //        _with1.Document = pd;
        //        _with1.WindowState = FormWindowState.Maximized;
        //        _with1.PrintPreviewControl.Zoom = 1;
        //        _with1.ShowDialog();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}
    }
}
