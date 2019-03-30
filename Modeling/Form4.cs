using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modeling
{
    public partial class Form4 : Form
    {
        DataTable resultTable = new DataTable();
        DataTable dataTable = new DataTable();
        private int countFuzzySet;

        public Form4(DataTable dataTable, int countFuzzySet)
        {
            this.countFuzzySet = countFuzzySet;
            this.dataTable = dataTable;
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            WorkWithTimeRow workWithTimeRow = new WorkWithTimeRow();
            double halfInterval = workWithTimeRow.getHalfInterval(workWithTimeRow.getMin(dataTable), workWithTimeRow.getMax(dataTable), countFuzzySet);
            resultTable = workWithTimeRow.getMatrixFuzzyLingvistic(dataTable, countFuzzySet, workWithTimeRow.getMin(dataTable) - halfInterval,
                workWithTimeRow.getHalfInterval(workWithTimeRow.getMin(dataTable) - halfInterval, 
                workWithTimeRow.getMax(dataTable) + halfInterval, countFuzzySet));
            dataGridView1.DataSource = resultTable;
            foreach (DataRow row in resultTable.Rows)
            {
                label1.Text += " " + row[2];
            }
            dataGridView2.DataSource = workWithTimeRow.getInfoTable(workWithTimeRow.getMin(dataTable) - halfInterval,
                workWithTimeRow.getMax(dataTable) + halfInterval, countFuzzySet);
        }
    }
}
