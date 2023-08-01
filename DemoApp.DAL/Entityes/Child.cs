using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.DAL.Entityes
{
    public class Child
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateOnly BirthDay { get; set; }

        public int WorkerId { get; set; }
        public Worker Worker { get; set; }


        [NotMapped]
        public string Name { get => $"{FullName}".Trim(); }

        public override string ToString()
        {
            return FullName;
        }
    }
}
