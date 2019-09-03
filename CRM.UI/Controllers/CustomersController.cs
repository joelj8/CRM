using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using CRM.Data;
using CRM.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using AutoMapper;
using CRM.Models.ProfileConfig;

namespace CRM.UI.Controllers
{
    
    public class CustomersController : Controller
    {
        private readonly CDBContext _db;

        public CustomersController() {
            _db = new CDBContext();
            GeneraToken();
        }

        private void GeneraToken() {
            var client = new RestClient("http://localhost/CRM.API/api/login/authenticate");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&Username=Test&Password=123456", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string authoapi = response.Content.ToString().Substring(1);
            authoapi = authoapi.Remove(authoapi.Length - 1, 1);
            System.Web.HttpContext.Current.Session["tokenuser"] = authoapi;
        }

        // GET: Customers
        public ActionResult Index()
        {
            SelectList lsGender = new SelectList(_db.genders, "genderId", "genderName");
            ViewBag.GenderList = lsGender;


            List<string> generosList = (from g in _db.genders where g.GenderId == "M" select g.GenderId).ToList();
            

            List<Customer> customerList = (from c in _db.customers
                                where !generosList.Contains(c.GenderId)
                                select c).ToList();

            //var customerList2 = _db.customers.Where(i => generosList.Contains(i.GenderId));

            //return View(_db.customers.ToList());

            return View(_db.customers.Where(c => c.ID == 2).ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            
            string tokenausar = System.Web.HttpContext.Current.Session["tokenuser"].ToString();

            RestClient client = new RestClient("http://localhost/CRM.API/api/Customers/" + id.ToString());
            RestRequest request = new RestRequest(Method.GET);

            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("authorization", tokenausar);

            IRestResponse response = client.Execute(request);
            string strresponse = response.Content.ToString();

            strresponse = strresponse.Replace("[", "");
            strresponse = strresponse.Replace("]", "");

            CustomerApi responseCustomer = JsonConvert.DeserializeObject<CustomerApi>(strresponse);
            var configprofCustomer = FactoryProfile.CreateProfile<CustomerProfile>().GetProfile();

            /* https://dotnettutorials.net/lesson/automapper-in-c-sharp/ Ejemplo para usar el Mapper*/
            //Mapper mapper = new Mapper(CustomerProfile.GetProfile()); // Necesita el metodo static

            Mapper mapper = new Mapper(configprofCustomer); // Necesita que el metodo no sea static
            var responder = mapper.Map<CustomerApi, Customer>(responseCustomer);
           
            return View(responder);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            
            SelectList lsGender = new SelectList(_db.genders, "genderId", "genderName");

            ViewBag.GenderList = lsGender;
            
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Phone,Mail,FechaNacimiento,Address,GenderId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.customers.Add(customer);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = _db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            SelectList lsGender = new SelectList(_db.genders, "genderId", "genderName");
            ViewBag.GenderList = lsGender;

            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Phone,Mail,Address,FechaNacimiento,GenderId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(customer).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            SelectList lsGender = new SelectList(_db.genders, "genderId", "genderName");
            ViewBag.GenderList = lsGender;

            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = _db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = _db.customers.Find(id);
            _db.customers.Remove(customer);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult findcontact(string ContactoID)
        {
            //List<Customer> objResult = _db.customers.Where(e => e.Phone.Contains(ContactoID)).ToList();
            var objResult = (from c in _db.customers
                             where c.Phone.Contains(ContactoID)
                             group c by new { c.Name, c.Phone, c.Mail, c.GenderId, c.Address, c.fechaNacimiento }
                                  into cgroup
                             let maxID = cgroup.Max(p => p.ID)
                             select new
                             {
                                 ID = maxID,
                                 Name = cgroup.Key.Name,
                                 Phone = cgroup.Key.Phone,
                                 Mail = cgroup.Key.Mail,
                                 GenderId = cgroup.Key.GenderId,
                                 Address = cgroup.Key.Address,
                                 fechaNacimiento = cgroup.Key.fechaNacimiento
                             }).ToList();

            return new JsonResult()
            {
                Data = objResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            //return (objResult, JsonRequestBehavior.AllowGet() )
        }

        [HttpPost]
        public JsonResult findcontactid(int ContactoID)
        {
            //Customer objResult = _db.customers.Where(e => e.ID == ContactoID).FirstOrDefault();
            // c.fechaNacimiento.HasValue? c.fechaNacimiento.ToString("dd/MM/yyyy") : string.Empty

            var objResult = (from c in _db.customers
                                  where c.ID == ContactoID
                                   select new { c.ID, c.Name, c.Phone, c.Mail, c.GenderId, c.Address, c.fechaNacimiento}).ToList()
                                   .Select(x => new { x.ID,x.Name,x.Phone,x.Mail,x.GenderId,x.Address,
                                                    fechaNacimiento = x.fechaNacimiento != null ? 
                                                    x.fechaNacimiento?.ToString("dd/MM/yyyy") : string.Empty }).FirstOrDefault();

            return new JsonResult()
            {
                Data = objResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult CargaData(string nombre, string genero)
        {
            // LoadXLS("c:\\DesaVisual\\CRM\\CRM.UI\\App_Data\\agenda.xlsx", "Hoja", "Gender", "M");

            //LoadExcel();

            var cusList = _db.customers.ToList<Customer>();
           if (genero != string.Empty)
            {
                cusList = _db.customers.Where(c => c.GenderId.Contains(genero.Trim())).ToList();
            }
            
            return Json(new { data = cusList }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void LoadExcel()
        {
            string archivo_xls = "C:\\DesaVisual\\CRM\\CRM.UI\\App_Data\\agenda.xlsx";
            string Connection = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;DataSource={0};Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\";" , archivo_xls);

            //code to read the content of format file 
            OleDbConnection con = new OleDbConnection(Connection);
            OleDbCommand command = new OleDbCommand();

            DataTable dt = new DataTable();
            OleDbDataAdapter myCommand = new OleDbDataAdapter("select * from [Tabelle1$]", con);

            myCommand.Fill(dt);
            Console.Write(dt.Rows.Count);
        }
        private DataTable LoadXLS(string strFile, String sheetName, String column, String value)
        {
            DataTable dtXLS = new DataTable(sheetName);

            try
            {
                string strConnectionString = "";

                if (strFile.Trim().EndsWith(".xlsx"))
                {

                    strConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", strFile);

                }
                else if (strFile.Trim().EndsWith(".xls"))
                {

                    strConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='{0}';Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";", strFile);

                }

                OleDbConnection SQLConn = new OleDbConnection(strConnectionString);

                SQLConn.Open();

                OleDbDataAdapter SQLAdapter = new OleDbDataAdapter();

                 string sql = "SELECT * FROM [" + sheetName + "$] WHERE " + column + " = '" + value +"' ";
               // string sql = "SELECT * FROM [" + sheetName + "$]  " ;

                OleDbCommand selectCMD = new OleDbCommand(sql, SQLConn);

                SQLAdapter.SelectCommand = selectCMD;

                SQLAdapter.Fill(dtXLS);
                

                SQLConn.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return dtXLS;

        }
    }
}
