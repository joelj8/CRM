using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class Gender
    {
        [Key]
        public string GenderId { get; set; }

        [Required(ErrorMessage = "Gender required")]
        [DisplayName("Gender")]
        public string GenderName { get; set; }

    }
}
