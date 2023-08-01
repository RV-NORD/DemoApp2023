using DemoApp.WPF.Extensions;
using DemoApp.WPF.Services.Interfaces;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.WPF.Services
{
    public class XlsReport :IReport
    {
        public void XlsExportFromList<T>(IList<T> data, string from, string to)
        {
            var saveTo = App.Configuration.GetSection("AppSettings")["XLS"];
            SLDocument sl = new SLDocument(Environment.CurrentDirectory + from, "STAT");
            int iStartRowIndex = 2;
            int iStartColumnIndex = 1;
            DataTable rd = data.ToDataTable();
            sl.ImportDataTable(iStartRowIndex, iStartColumnIndex, rd, false);


            string fs = Environment.CurrentDirectory + to + $"STAT001_{DateTime.Now.ToString("yy-MM-dd-HH-mm-ss")}.xlsx";
            sl.SaveAs(fs);
            Process.Start("explorer.exe", Environment.CurrentDirectory + to);
        }
    }
}
