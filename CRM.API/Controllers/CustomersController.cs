using CRM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace CRM.API.Controllers
{
    /// <summary>
    /// customer controller class for testing security token
    /// </summary>
    [Authorize]
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController
    {
        private readonly CDBContext db = new CDBContext();
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            //var customerFake = "customer-fake";

            var customersList = (from cust in db.customers
                                 where cust.ID == id
                                 select new { codigo = cust.ID, nombre = cust.Name, email = cust.Mail }).ToList();

            return Ok(customersList);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            //var customersList = db.customers.ToList();

            var customersList  = (from cust in db.customers
                         select new { codigo = cust.ID, nombre = cust.Name, email = cust.Mail }).ToList();

            
            //var customersFake = new string[] { "customer-1", "customer-2", "customer-3" };
            return Ok(customersList);
        }
    }
}
