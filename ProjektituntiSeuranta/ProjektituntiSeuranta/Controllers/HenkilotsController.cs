using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjektituntiSeuranta.Models;

namespace ProjektituntiSeuranta.Controllers
{
    public class HenkilotsController : Controller
    {
        private AsiakastietokantaEntities db = new AsiakastietokantaEntities();

        // GET: Henkilots
        public ActionResult Index()
        {
            return View(db.Henkilots.ToList());
        }

        // GET: Henkilots/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Henkilot henkilot = db.Henkilots.Find(id);
            if (henkilot == null)
            {
                return HttpNotFound();
            }
            return View(henkilot);
        }

        // GET: Henkilots/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projektits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HenkiloID,Etunimi,Sukunimi,Osoite,Esimies")] Henkilot henkilot)
        {
            if (ModelState.IsValid)
            {
                db.Henkilots.Add(henkilot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(henkilot);
        }

        // GET: Henkilots/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Henkilot henkilot = db.Henkilots.Find(id);
            if (henkilot == null)
            {
                return HttpNotFound();
            }
            return View(henkilot);
        }

        // POST: Henkilots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HenkiloID,Etunimi,Sukunimi,Osoite,Esimies")] Henkilot henkilot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(henkilot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(henkilot);
        }

        // GET: Henkilots/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Henkilot henkilot = db.Henkilots.Find(id);
            if (henkilot == null)
            {
                return HttpNotFound();
            }
            return View(henkilot);
        }

        // POST: Henkilots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Henkilot henkilot = db.Henkilots.Find(id);
            db.Henkilots.Remove(henkilot);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult GetProjektit(string id)
        {
            int iid = int.Parse(id);


            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();
            List<Tunnit> tunnit = (from t in entities.Tunnits
                                where t.HenkiloID == iid
                                select t).ToList();

            List<SimpleProjektiData> result = new List<SimpleProjektiData>();

                foreach (Tunnit tunti in tunnit)
            {
                SimpleProjektiData data = new SimpleProjektiData();
                data.HenkiloID = (int)tunti.HenkiloID;
                data.ProjektiID = (int)tunti.ProjektiID;
                data.Projektitunnit = (int)tunti.Projektitunnit;
                result.Add(data);

            }
        
        return Json(tunnit, JsonRequestBehavior.AllowGet);


            
        

        }
    }
}

        
       

    

