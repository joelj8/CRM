using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class CustomerApi
    {

        public int Codigo { get; set; }

        public string Nombre { get; set; }

        public string Email { get; set; }

        public string Direccion {get; set;}
        public string fechaNac { get; set; }
        public string Genero { get; set; }
        public string Telefono { get; set; }


    }
}
