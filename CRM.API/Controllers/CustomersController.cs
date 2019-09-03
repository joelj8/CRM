using CRM.API.Models;
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

        //[HttpPost]
        //[Route("Getid")]
        [HttpGet]
        public IHttpActionResult GetId(int id )
        {
            //CustomersRequest objRequest
            //int id = objRequest.id;

            //var customerFake = "customer-fake";

            var customersList = (from cust in db.customers
                                 where cust.ID == id
                                 select new { codigo = cust.ID, nombre = cust.Name, email = cust.Mail,
                                     direccion = cust.Address, fechaNac = cust.fechaNacimiento,
                                     genero = cust.GenderId, telefono = cust.Phone }).ToList()
                                              .Select(t => new { t.codigo, t.nombre, t.email,
                                                  t.direccion,
                                                  fechaNac = t.fechaNac != null ? t.fechaNac?.ToString("dd/MM/yyyy") : string.Empty,
                                                  t.genero, t.telefono }).ToList();

            var result = db.customers.Include("GenderGrl").FirstOrDefault(i => i.ID == id);

            return Ok(customersList);
        }
        [HttpGet]
        //[HttpPost]
        //[Route("GetAll")]
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
