using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.DAL.Entityes
{
    [NotMapped]
    public class WorkerChildCountStatistic
    {
        public string FullName { get; set; }
        public DateOnly BirthDay { get; set; }
        public int ChildCount { get; set; }
    }
}
