using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Name required")]
        [StringLength(80, ErrorMessage = "Name max Length is 80")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Telephone required")]
        [StringLength(15, ErrorMessage = "Telephone max Length is 15")]
        [DisplayName("Telephone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email required")]
        [StringLength(50, ErrorMessage = "Email max Length is 50")]
        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }

        [StringLength(250, ErrorMessage = "Address max Length is 250")]
        public string Address { get; set; }

        
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public DateTime? fechaNacimiento { get; set; }
        public string GenderId { get; set; }

        public virtual Gender GenderGrl {get; set; }

        


    }
}
