using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.WPF.Services.Interfaces
{
    public interface IReport
    {
        void XlsExportFromList<T>(IList<T> data, string from, string to);
    }
}
