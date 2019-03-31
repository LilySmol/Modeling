using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Modeling
{
    public partial class Form6 : Form
    {
        private DataTable dataTable = new DataTable();
        private DataTable dataTableResult = new DataTable();

        public Form6(DataTable dataTable)
        {
            this.dataTable = dataTable;
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            dataTableResult.Columns.Add("t");
            dataTableResult.Columns.Add("value");
            for (int i = 0; i < dataTable.Rows.Count - 1; i++)
            {
                dataTableResult.Rows.Add(i, (Convert.ToDouble(dataTable.Rows[i][1]) + Convert.ToDouble(dataTable.Rows[i + 1][1])) / 2);
            }
            dataTableResult.Rows.Add(dataTable.Rows.Count, (Convert.ToDouble(dataTable.Rows[dataTable.Rows.Count - 1][1]) + Convert.ToDouble(dataTable.Rows[dataTable.Rows.Count - 2][1])) / 2);
            DrawFirstGraph(getPointsFromDataTable(dataTableResult));
            DrawSecondGraph(getPointsFromDataTable(dataTable));
            label3.Text = getMapeValue(dataTable, dataTableResult).ToString();
        }

        private PointPairList getPointsFromDataTable(DataTable inDataTable)
        {
            PointPairList pointsList = new PointPairList();
            int numberLine = 0;
            foreach (DataRow row in inDataTable.Rows)
            {
                pointsList.Add(Convert.ToDouble(numberLine), Convert.ToDouble(row[1]));
                numberLine++;
            }
            return pointsList;
        }

        private void DrawFirstGraph(PointPairList list)
        {
            GraphPane pane = zedGraphControl1.GraphPane;
            pane.CurveList.Clear();
            LineItem myCurve = pane.AddCurve("", list, Color.Red, SymbolType.None);
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        private void DrawSecondGraph(PointPairList list)
        {
            GraphPane pane = zedGraphControl1.GraphPane;
            LineItem myCurve = pane.AddCurve("", list, Color.Blue, SymbolType.None);
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        private double getMapeValue(DataTable mainTable, DataTable modelingTable)
        {
            double sum = 0;
            double sumMainTable = 0;
            for (int i = 0; i < mainTable.Rows.Count; i++)
            {
                sum += Convert.ToDouble(mainTable.Rows[i][1]) - Convert.ToDouble(modelingTable.Rows[i][1]);
                sumMainTable += Convert.ToDouble(mainTable.Rows[i][1]);
            }
            double one = (double) 1 / mainTable.Rows.Count;
            double two = Math.Abs(sum / sumMainTable);
            return one * two * 100;
        }
    }
}
