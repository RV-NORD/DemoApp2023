using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.DAL.Interfaces
{
    public interface IEntity
    {
        [Required]
        [Key]
        int Id { get; set; }
    }
}
