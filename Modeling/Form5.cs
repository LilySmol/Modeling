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
    public partial class Form5 : Form
    {
        DataTable resultTable = new DataTable();
        DataTable dataTable = new DataTable();
        private int countFuzzySet;

        public Form5(DataTable dataTable, int countFuzzySet)
        {
            this.dataTable = dataTable;
            this.countFuzzySet = countFuzzySet;
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            WorkWithTimeRow workWithTimeRow = new WorkWithTimeRow();
            double halfInterval = workWithTimeRow.getHalfInterval(workWithTimeRow.getMin(dataTable), workWithTimeRow.getMax(dataTable), countFuzzySet);
            resultTable = workWithTimeRow.getMatrixNumberFuzzyLingvistic(dataTable, countFuzzySet, workWithTimeRow.getMin(dataTable) - halfInterval,
                workWithTimeRow.getHalfInterval(workWithTimeRow.getMin(dataTable) - halfInterval,
                workWithTimeRow.getMax(dataTable) + halfInterval, countFuzzySet));
            dataGridView1.DataSource = resultTable;
            dataGridView2.DataSource = workWithTimeRow.getInfoTable(workWithTimeRow.getMin(dataTable) - halfInterval,
                workWithTimeRow.getMax(dataTable) + halfInterval, countFuzzySet);
        }
    }
}
