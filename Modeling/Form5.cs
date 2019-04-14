using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.XlsIO;

namespace Modeling
{
    public partial class Form5 : Form
    {
        DataTable resultTable = new DataTable();
        DataTable dataTable = new DataTable();
        private string fileName;
        private int countFuzzySet;

        private void addToXlsx(DataTable writingDataTable, String fileName)
        {
            ExcelEngine ExcelEngineObject = new Syncfusion.XlsIO.ExcelEngine();
            IApplication Application = ExcelEngineObject.Excel;
            Application.DefaultVersion = ExcelVersion.Excel2013;
            IWorkbook Workbook = Application.Workbooks.Create(1);
            IWorksheet Worksheet = Workbook.Worksheets[0];
            Worksheet.ImportDataTable(writingDataTable, true, 1, 1);
            Workbook.SaveAs("D:\\modeling\\RESULTS\\" + fileName + ".xlsx");
            Workbook.Close();
            ExcelEngineObject.Dispose();
        }

        public Form5(DataTable dataTable, int countFuzzySet, string fileName)
        {
            this.dataTable = dataTable;
            this.countFuzzySet = countFuzzySet;
            this.fileName = fileName;
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

            addToXlsx(resultTable, fileName);

            dataGridView2.DataSource = workWithTimeRow.getInfoTable(workWithTimeRow.getMin(dataTable) - halfInterval,
                workWithTimeRow.getMax(dataTable) + halfInterval, countFuzzySet);
        }
    }
}
