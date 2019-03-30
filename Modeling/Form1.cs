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
                return "y = at + b + sin(t)";
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
    } 
}
