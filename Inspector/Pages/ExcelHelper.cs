using Microsoft.Office.Interop.Excel;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;

namespace Inspector.Pages
{

    public static class ExcelHelper
    {
        public static void DataGridToSheet(string name, DataGrid dGrid)
        {
            var db = new dbMalukovEntities();
            var application = new Excel.Application();
            application.Visible = true;
            Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
            Excel.Worksheet sheet1 = application.Worksheets.Item[1]; //Sheets[1];
            sheet1.Name = name;

            Range myRange = sheet1.Range[sheet1.Cells[1, 1], sheet1.Cells[1, dGrid.Columns.Count]];
            myRange.MergeCells = true;
            myRange.Value2 = name;
            myRange.HorizontalAlignment = -4108;
            myRange.Font.Bold = true;
            for (int j = 1; j < dGrid.Columns.Count + 1; j++)
            {
                myRange = (Excel.Range)sheet1.Cells[2, j];
                //sheet1.Columns[j].ColumnWidth = 25;
                myRange.Value2 = dGrid.Columns[j - 1].Header;
                myRange.Font.Bold = true;
                myRange.Columns.AutoFit();
            }
            for (int j = 0; j < dGrid.Items.Count; j++)
                for (int i = 1; i <= dGrid.Columns.Count; i++)
                {
                    TextBlock b = dGrid.Columns[i - 1].GetCellContent(dGrid.Items[j]) as TextBlock;
                    if (b == null)
                        continue;

                    myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 3, i];
                    myRange.Value2 = b.Text;
                }
            sheet1.Columns.EntireColumn.AutoFit();
            //workbook.Save();
            //application.Quit();
        }
        
    }
}

