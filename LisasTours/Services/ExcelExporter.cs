using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LisasTours.Models;
using System;
using System.Linq;
using Syncfusion.XlsIO;

namespace LisasTours.Services
{
    public class ExcelExporter : IExporter
    {
        public async Task<byte[]> GenerateCompaniesReport(IEnumerable<Company> companies)
        {
            try
            {
                byte[] byteArray = null;

                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Excel2013;

                    IWorkbook workbook = application.Workbooks.Create(1);
                    IWorksheet sheet = workbook.Worksheets.Create("Report");

                    sheet.SetColumnWidth(1, 40);
                    sheet.SetColumnWidth(2, 40);
                    sheet.SetColumnWidth(3, 40);

                    sheet.Range["A1:C1"].CellStyle.Font.Bold = true;

                    sheet.Range[1, 1].Text = "Компания";
                    sheet.Range[1, 2].Text = "Имя контакта";
                    sheet.Range[1, 3].Text = "Почта";

                    int rowIndex = 2;
                    foreach (var company in companies)
                    {
                        var contact = company.Contacts?.FirstOrDefault();
                        sheet.Range[rowIndex, 1].Text = company.Name;
                        sheet.Range[rowIndex, 2].Text = contact?.FirstName;
                        sheet.Range[rowIndex, 3].Text = contact?.Mail;
                        rowIndex++;
                    }

                    //Saving the workbook as stream
                    using (var ms = new MemoryStream())
                    {
                        workbook.SaveAs(ms);
                        byteArray = ms.ToArray();
                    }
                }

                return byteArray;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
