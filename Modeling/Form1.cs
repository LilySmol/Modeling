using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Modeling
{
    public partial class Form1 : Form
    {
        private DataTable dataTable = new DataTable();

        private PointPairList getTimeSeriesFromFile(String filePath, int count)
        {
            dataTable.Clear();
            PointPairList pointsList = new PointPairList();
            var list = new List<string>();
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                int numberLine = 0;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (numberLine >= count)
                    {
                        return pointsList;
                    }
                    if (line.Contains("."))
                    {
                        line = line.Replace(".", ",");
                    }
                    dataTable.Rows.Add(Convert.ToDouble(numberLine), Convert.ToDouble(line));
                    double d = Convert.ToDouble(line);
                    pointsList.Add(Convert.ToDouble(numberLine), Convert.ToDouble(line));
                    numberLine++;
                }
            }
            return pointsList;
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

        private DataTable getDataTableFromFile(String filePath, int startNumber, int endNumber)
        {
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("t");
            resultTable.Columns.Add("value");
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                int numberLine = 0;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (numberLine >= startNumber && numberLine <= endNumber)
                    {
                        if (line.Contains("."))
                        {
                            line = line.Replace(".", ",");
                        }
                        resultTable.Rows.Add(Convert.ToDouble(numberLine), Convert.ToDouble(line));
                    }                   
                    numberLine++;
                }
            }
            return resultTable;
        }

        private void DrawGraph(PointPairList list)
        {
            GraphPane pane = zedGraphControl1.GraphPane;
            pane.CurveList.Clear();
            LineItem myCurve = pane.AddCurve("", list, Color.Blue, SymbolType.None);          
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        public String getModel(String filePath)
        {
            if (filePath.Contains("\\RANDOM\\"))
            {
                return "y = e(t)";
            } else if (filePath.Contains("\\RANDOM_SEASON\\"))
            {
                return "y = e(t) + sin(t)";
            }
            else if (filePath.Contains("\\SEASON\\"))
            {
                return "y = sin(t)";
            }
            else if (filePath.Contains("\\TREND\\"))
            {
                return "y = at + b";
            }
            else if (filePath.Contains("\\TREND_RANDOM\\"))
            {
                return "y = at + b + e(t)";
            }
            else if (filePath.Contains("\\TREND_RANDOM_SEASON\\"))
            {
                return "y = at + b + e(t) + sin(t)";
            }
            else if (filePath.Contains("\\TREND_SEASON\\"))
            {
                return "y = at + b + sin(t) * a * 20";
            }
            return "";
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataTable.Columns.Add("time point");
            dataTable.Columns.Add("value");
        }       

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filePath = openFileDialog1.FileName;
            int count = Convert.ToInt32(textBox1.Text);

            DrawGraph(getTimeSeriesFromFile(filePath, count));
            dataGridView1.DataSource = dataTable.DefaultView;
            label3.Text = getModel(filePath);
            label9.Text = filePath.Split('\\')[3];           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int countFuzzySet = Convert.ToInt32(textBox2.Text);
            Form2 form2 = new Form2(dataTable, countFuzzySet);
            form2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int countFuzzySet = Convert.ToInt32(textBox2.Text);
            Form4 form4 = new Form4(dataTable, countFuzzySet);
            form4.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int countFuzzySet = Convert.ToInt32(textBox2.Text);
            Form5 form5 = new Form5(dataTable, countFuzzySet);
            form5.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int countFuzzySet = Convert.ToInt32(textBox2.Text);
            Form3 form3 = new Form3(dataTable, countFuzzySet);
            form3.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filePath = openFileDialog1.FileName;
            int startNumber = Convert.ToInt32(textBox3.Text);
            int endNumber = Convert.ToInt32(textBox4.Text);
            DataTable anomaliTable = getDataTableFromFile(filePath, startNumber, endNumber);
            foreach (DataRow row in anomaliTable.Rows)
            {
                dataTable.Rows[Convert.ToInt32(row[0])][1] = row[1];
            }
            DrawGraph(getPointsFromDataTable(dataTable));
            dataGridView1.DataSource = dataTable.DefaultView;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6(dataTable);
            form6.Show();
        }
    } 
}
