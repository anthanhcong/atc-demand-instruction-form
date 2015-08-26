using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Job_Assignment
{
    public class ExcelHelper
    {
        public static string ExportGridviewToExcel(string file_path, string fInfo, string tieude, DataGridView gridView,
                                    ToolStripProgressBar probar,
                                    ToolStripStatusLabel status1, ToolStripStatusLabel status2)
        {
            //khoi tao cac doi tuong Com Excel de lam viec
            Excel.ApplicationClass xlApp;
            Excel.Worksheet xlSheet;
            Excel.Workbook xlBook;
            //doi tuong Trống để thêm  vào xlApp sau đó lưu lại sau
            object missValue = System.Reflection.Missing.Value;
            //khoi tao doi tuong Com Excel moi
            xlApp = new Excel.ApplicationClass();
            xlBook = xlApp.Workbooks.Add(missValue);
            //su dung Sheet dau tien de thao tac
            xlSheet = (Excel.Worksheet)xlBook.Worksheets.get_Item(1);
            //không cho hiện ứng dụng Excel lên để tránh gây đơ máy
            //xlApp.Visible = false;
            int i, j;
            bool allow_add_row = gridView.AllowUserToAddRows;
            gridView.AllowUserToAddRows = false;
            int socot = gridView.Columns.Count;
            int sohang = gridView.Rows.Count;
            //if (gridView.AllowUserToAddRows == true) {
            //    sohang--;
            //}

            try
            {
                if (file_path != "")
                {
                    //set thuoc tinh cho tieu de
                    xlSheet.get_Range(xlSheet.Cells[1, 1], xlSheet.Cells[1, socot + 1]).Merge(false);
                    // Excel.Range caption = xlSheet.get_Range("A1", Convert.ToChar(socot + 65) + "1");
                    Excel.Range caption = xlSheet.get_Range(xlSheet.Cells[1, 1], xlSheet.Cells[1, socot + 1]);
                    caption.Select();
                    caption.FormulaR1C1 = tieude;
                    //căn lề cho tiêu đề
                    caption.HorizontalAlignment = Excel.Constants.xlCenter;
                    caption.Font.Bold = true;
                    caption.VerticalAlignment = Excel.Constants.xlCenter;
                    caption.Font.Size = 15;
                    //màu nền cho tiêu đề
                    caption.Interior.ColorIndex = 20;
                    caption.RowHeight = 30;
                    //set thuoc tinh cho cac header
                    // Excel.Range header = xlSheet.get_Range("A2", Convert.ToChar(socot + 65) + "2");
                    Excel.Range header = xlSheet.get_Range(xlSheet.Cells[1, 2], xlSheet.Cells[1, socot + 1]);
                    header.Select();

                    header.HorizontalAlignment = Excel.Constants.xlCenter;
                    header.Font.Bold = true;
                    header.Font.Size = 10;
                    //điền tiêu đề cho các cột trong file excel
                    for (i = 0; i < socot; i++)
                    {
                        xlSheet.Cells[2, i + 2] = gridView.Columns[i].HeaderCell.Value.ToString().Trim();// .ColumnName;
                    }
                    //dien cot stt
                    xlSheet.Cells[2, 1] = "No.";
                    for (i = 0; i < sohang; i++)
                    {
                        xlSheet.Cells[i + 3, 1] = i + 1;
                    }

                    // Dien du lieu vao sheet
                    probar.Visible = true;
                    status2.Visible = true;
                    status1.Visible = true;
                    status1.Text = "Loading File";
                    for (i = 0; i < sohang; i++)
                    {
                        for (j = 0; j < socot; j++)
                        {
                            if (gridView.Columns[j].ValueType == typeof(float))
                            {
                                ((Excel.Range)xlSheet.Cells[i + 3, j + 2]).NumberFormat = "0.000000";
                            }
                            else if (gridView.Columns[j].ValueType == typeof(double))
                            {
                                ((Excel.Range)xlSheet.Cells[i + 3, j + 2]).NumberFormat = "0.000000";
                            }
                            else if (gridView.Columns[j].ValueType == typeof(decimal))
                            {
                                ((Excel.Range)xlSheet.Cells[i + 3, j + 2]).NumberFormat = "0.000000";
                            }
                            else if (gridView.Columns[j].ValueType == typeof(int))
                            {
                                ((Excel.Range)xlSheet.Cells[i + 3, j + 2]).NumberFormat = "0";
                            }
                            else if (gridView.Columns[j].ValueType == typeof(DateTime))
                            {
                                ((Excel.Range)xlSheet.Cells[i + 3, j + 2]).NumberFormat = "[$-409]d-MMM-yyyy;@";
                            }
                            else if (gridView.Columns[j].ValueType == typeof(TimeSpan))
                            {
                                ((Excel.Range)xlSheet.Cells[i + 3, j + 2]).NumberFormat = "h:mm:ss;@";
                            }
                            else
                            {
                                ((Excel.Range)xlSheet.Cells[i + 3, j + 2]).NumberFormat = "@";
                            }
                            xlSheet.Cells[i + 3, j + 2] = gridView.Rows[i].Cells[j].Value == null ? "" : gridView.Rows[i].Cells[j].Value.ToString();
                        }
                        // Update progress Bar
                        probar.Value = i % 100;
                        status2.Text = "Line " + i + " of " + sohang;
                    }
                    probar.Visible = false;
                    status2.Visible = false;
                    status1.Visible = false;

                    //autofit độ rộng cho các cột
                    for (i = 0; i <= socot; i++)
                    {
                        ((Excel.Range)xlSheet.Cells[1, i + 1]).EntireColumn.AutoFit();
                    }


                    if (fInfo.Trim() == ".xls")
                    {
                        xlBook.SaveAs(file_path, Excel.XlFileFormat.xlWorkbookNormal, missValue, missValue, missValue, missValue, Excel.XlSaveAsAccessMode.xlExclusive, missValue, missValue, missValue, missValue, missValue);
                    }
                    else if (fInfo.Trim() == ".xlsx")
                    {
                        xlBook.SaveAs(file_path, Excel.XlFileFormat.xlOpenXMLWorkbook, missValue, missValue, missValue, missValue, Excel.XlSaveAsAccessMode.xlExclusive, missValue, missValue, missValue, missValue, missValue);
                    }
                    xlBook.Close(true, missValue, missValue);
                    xlApp.Quit();

                    // release cac doi tuong COM
                    gridView.AllowUserToAddRows = allow_add_row;
                }
            }
            catch (Exception e)
            {
                probar.Visible = false;
                status2.Visible = false;
                status1.Visible = false;
                gridView.AllowUserToAddRows = allow_add_row;
                return e.Message;
            }
            finally
            {
                // release cac doi tuong COM
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
            }
            return "";
        }

    }
}
