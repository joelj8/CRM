using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRM.Data;
using CRM.Models;

namespace CRM.UI.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CDBContext db ;

        public CustomersController() {
            db = new CDBContext();
        }
        // GET: Customers
        public ActionResult Index()
        {
            SelectList lsGender = new SelectList(db.genders, "genderId", "genderName");
            ViewBag.GenderList = lsGender;

            return View(db.customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            
            SelectList lsGender = new SelectList(db.genders, "genderId", "genderName");

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
                db.customers.Add(customer);
                db.SaveChanges();
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
            Customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            SelectList lsGender = new SelectList(db.genders, "genderId", "genderName");
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
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SelectList lsGender = new SelectList(db.genders, "genderId", "genderName");
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
            Customer customer = db.customers.Find(id);
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
            Customer customer = db.customers.Find(id);
            db.customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CargaData(string nombre, string genero)
        {
            // LoadXLS("c:\\DesaVisual\\CRM\\CRM.UI\\App_Data\\agenda.xlsx", "Hoja", "Gender", "M");

            //LoadExcel();

            var cusList = db.customers.ToList<Customer>();
           if (genero != string.Empty)
            {
                cusList = db.customers.Where(c => c.GenderId.Contains(genero.Trim())).ToList();
            }
            
            return Json(new { data = cusList }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
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
