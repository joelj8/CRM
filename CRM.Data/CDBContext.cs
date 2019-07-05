using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CRM.Models;

namespace CRM.Data
{
    public class CDBContext : DbContext
    {
        public CDBContext() : base("StringDBContext") {

        }

        public DbSet<Customer> customers { get; set; }
        public DbSet<Gender> genders { get; set; }
    }
}
