using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.DAL.Entityes
{
    public class Worker
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string SurName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateOnly BirthDay { get; set; }

        [Required]
        public bool Pol { get; set; }

        public List<Child> Childs { get; set; } = new();


        [NotMapped]
        public string Name { get => $"{SurName} {FirstName} {LastName}".Trim(); }

        public override string ToString()
        {
            return Name;
        }
    }
}
