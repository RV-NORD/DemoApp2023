using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.DAL.Entityes
{
    [NotMapped]
    public class Stat001
    {
        public string FIO { get; set; }
        public DateOnly DR { get; set; }
        public int ChildCnt { get; set; }
    }
}
