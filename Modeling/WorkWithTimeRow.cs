using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeling
{
    public class WorkWithTimeRow
    {
        public double getHalfInterval(double minValue, double maxValue, int countFuzzySet)
        {
            return (maxValue - minValue) / (countFuzzySet + 1);
        }

        public double getDegreeOfBelonging(double x, double a, double b, double c)
        {
            if (x >= a && x <= b)
            {
                return ((x - a) / (b - a));
            }
            else if (x > b && x <= c)
            {
                return ((c - x) / (c - b));
            }
            else
            {
                return 0;
            }
        }

        private double getA(int numberTriangle, double minValue, double halfInterval)
        {
            return (minValue + (numberTriangle * halfInterval));
        }

        public DataTable getMatrixTransformation(DataTable timeRowTable, int countFuzzySet, double minValue, double halfInterval)
        {
            DataTable resaltTable = new DataTable();
            resaltTable.Columns.Add("t");
            resaltTable.Columns.Add("value");
            for (int i = 0; i < countFuzzySet; i++)
            {
                resaltTable.Columns.Add("A" + (i + 1));
            }
            for (int j = 0; j < timeRowTable.Rows.Count; j++)
            {
                int count = 0;
                double a = 0;
                List<double> listTransformation = new List<double>();
                resaltTable.Rows.Add();
                resaltTable.Rows[j][0] = j;
                resaltTable.Rows[j][1] = timeRowTable.Rows[j][1];
                for (int i = 0; i < countFuzzySet; i++)
                {
                    a = getA(count, minValue, halfInterval);
                    resaltTable.Rows[j][i + 2] = getDegreeOfBelonging(Convert.ToDouble(timeRowTable.Rows[j][1]), a, a + halfInterval, a + halfInterval + halfInterval);
                    count++;
                }
            }
            return resaltTable;
        }

        public DataTable getMatrixTransformationWithVector(DataTable timeRowTable, int countFuzzySet, double minValue, double halfInterval)
        {
            DataTable resaltTable = new DataTable();
            resaltTable.Columns.Add("t");
            resaltTable.Columns.Add("vector");
            resaltTable.Columns.Add("value");
            for (int i = 0; i < countFuzzySet; i++)
            {
                resaltTable.Columns.Add("A" + (i + 1));
            }
            for (int j = 0; j < timeRowTable.Rows.Count; j++)
            {
                int count = 0;
                double a = 0;
                List<double> listTransformation = new List<double>();
                resaltTable.Rows.Add();
                resaltTable.Rows[j][0] = j;
                resaltTable.Rows[j][1] = "y" + j;
                resaltTable.Rows[j][2] = timeRowTable.Rows[j][1];
                for (int i = 0; i < countFuzzySet; i++)
                {
                    a = getA(count, minValue, halfInterval);
                    resaltTable.Rows[j][i + 3] = getDegreeOfBelonging(Convert.ToDouble(timeRowTable.Rows[j][1]), a, a + halfInterval, a + halfInterval + halfInterval);
                    count++;
                }
            }
            return resaltTable;
        }

        public DataTable getMatrixFuzzyLingvistic(DataTable timeRowTable, int countFuzzySet, double minValue, double halfInterval)
        {
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("t");
            resultTable.Columns.Add("value");
            resultTable.Columns.Add("linguistic variable");

            DataTable matrixTransformationTable = getMatrixTransformation(timeRowTable, countFuzzySet, minValue, halfInterval);
            int count = 0;
            foreach (DataRow row in matrixTransformationTable.Rows)
            {
                double max = 0;
                int numberColumn = 1;
                for (int i = 2; i < matrixTransformationTable.Columns.Count; i++)
                {
                    double ii = Convert.ToDouble(row[i]);
                    if (Convert.ToDouble(row[i]) > max)
                    {
                        max = Convert.ToDouble(row[i]);
                        numberColumn = i;
                    }
                }
                resultTable.Rows.Add();
                resultTable.Rows[count][0] = count;
                resultTable.Rows[count][1] = row[1];
                if (max == 0)
                {
                    resultTable.Rows[count][2] = "";
                }
                else
                {
                    resultTable.Rows[count][2] = matrixTransformationTable.Columns[numberColumn].ColumnName;
                }
                count++;
            }
            return resultTable;
        }

        public DataTable getMatrixNumberFuzzyLingvistic(DataTable timeRowTable, int countFuzzySet, double minValue, double halfInterval)
        {
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("t");
            resultTable.Columns.Add("value");
            resultTable.Columns.Add("belongs");
            resultTable.Columns.Add("linguistic variable");            
            
            DataTable matrixTransformationTable = getMatrixTransformation(timeRowTable, countFuzzySet, minValue, halfInterval);
            int count = 0;
            foreach (DataRow row in matrixTransformationTable.Rows)
            {
                double max = 0;
                int numberColumn = 1;
                for (int i = 2; i < matrixTransformationTable.Columns.Count; i++)
                {
                    if (Convert.ToDouble(row[i]) > max)
                    {
                        max = Convert.ToDouble(row[i]);
                        numberColumn = i;
                    }
                }
                resultTable.Rows.Add();
                resultTable.Rows[count][0] = count;
                resultTable.Rows[count][1] = row[1];
                if (max == 0)
                {
                    resultTable.Rows[count][2] = "";
                }
                else
                {
                    resultTable.Rows[count][2] = max;
                    resultTable.Rows[count][3] = matrixTransformationTable.Columns[numberColumn].ColumnName;
                }
                count++;
            }

            return resultTable;
        }

        public double getMin(DataTable timeRowTable)
        {
            double minValue = double.MaxValue;            
            foreach (DataRow dataRow in timeRowTable.Rows)
            {
                double value = Convert.ToDouble(dataRow[1]);
                minValue = Math.Min(minValue, value);
            }
            return minValue;
        }

        public double getMax(DataTable timeRowTable)
        {
            double maxValue = double.MinValue;
            foreach (DataRow dataRow in timeRowTable.Rows)
            {
                double value = Convert.ToDouble(dataRow[1]);
                maxValue = Math.Max(maxValue, value);
            }
            return maxValue;
        }

        public DataTable getInfoTable(double minValue, double maxValue, int countFuzzySet)
        {
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("name");
            resultTable.Columns.Add("start point");
            resultTable.Columns.Add("middle point");
            resultTable.Columns.Add("end point");
            double halfInterval = getHalfInterval(minValue, maxValue, countFuzzySet);
            double startPoint;
            for (int i = 0; i < countFuzzySet; i++)
            {
                startPoint = getA(i, minValue, halfInterval);
                resultTable.Rows.Add("A" + (i + 1), startPoint, startPoint + halfInterval, startPoint + halfInterval + halfInterval);
            }
            return resultTable;
        }

        public int getCountIncrease(DataTable dataTable)
        {
            int count = 0;
            for (int i = 1; i < dataTable.Rows.Count; i++)
            {
                if (Convert.ToDouble(dataTable.Rows[i][1]) > Convert.ToDouble(dataTable.Rows[i - 1][1]))
                {
                    count++;
                }
            }
            return count;
        }

        public int getCountFall(DataTable dataTable)
        {
            int count = 0;
            for (int i = 1; i < dataTable.Rows.Count; i++)
            {
                if (Convert.ToDouble(dataTable.Rows[i][1]) < Convert.ToDouble(dataTable.Rows[i - 1][1]))
                {
                    count++;
                }
            }
            return count;
        }

        public int getCountStagnation(DataTable dataTable)
        {
            int count = 0;
            for (int i = 1; i < dataTable.Rows.Count; i++)
            {
                if (Convert.ToDouble(dataTable.Rows[i][1]) == Convert.ToDouble(dataTable.Rows[i - 1][1]))
                {
                    count++;
                }
            }
            return count;
        }
    }
}
