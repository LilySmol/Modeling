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
    public partial class Form3 : Form
    {
        private DataTable resultTable = new DataTable();
        private DataTable dataTable = new DataTable();
        private int countFuzzySet;

        public Form3(DataTable dataTable, int countLingvisticVariables)
        {
            this.dataTable = dataTable;
            this.countFuzzySet = countLingvisticVariables;
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            WorkWithTimeRow workWithTimeRow = new WorkWithTimeRow();
            double halfInterval = workWithTimeRow.getHalfInterval(workWithTimeRow.getMin(dataTable), workWithTimeRow.getMax(dataTable), countFuzzySet);
            resultTable = workWithTimeRow.getMatrixTransformationWithVector(dataTable, countFuzzySet, workWithTimeRow.getMin(dataTable) - halfInterval,
                workWithTimeRow.getHalfInterval(workWithTimeRow.getMin(dataTable) - halfInterval,
                workWithTimeRow.getMax(dataTable) + halfInterval, countFuzzySet));
            dataGridView1.DataSource = resultTable;
            dataGridView2.DataSource = workWithTimeRow.getInfoTable(workWithTimeRow.getMin(dataTable) - halfInterval,
                workWithTimeRow.getMax(dataTable) + halfInterval, countFuzzySet);
        }
    }
}
