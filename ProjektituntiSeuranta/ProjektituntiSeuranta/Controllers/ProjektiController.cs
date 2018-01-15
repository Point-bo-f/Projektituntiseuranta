using ProjektituntiSeuranta.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCkurssitehtavaJatkokurssi.Controllers
{
    public class ProjektiController : Controller
    {
        public ActionResult Index2()
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();
            List<Projektit> model = entities.Projektits.ToList();
            entities.Dispose();

            return View(model);
        }
        public ActionResult Index()
        {
            return View();
        }

        // GET: Projekti
        public JsonResult GetList()
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();
            //List<Projektit> model = entities.Projektit.ToList();

            var model = (from p in entities.Projektits
                         select new
                         {
                             ProjektiID = p.ProjektiID,
                             Projektinimi = p.Projektinimi

                         }).ToList();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            return Json(json, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetSingleProjekti(int id)
        {

            {
                AsiakastietokantaEntities entities = new AsiakastietokantaEntities();
                var model = (from p in entities.Projektits
                             where p.ProjektiID == id
                             select new
                             {
                                 ProjektiID = p.ProjektiID,
                                 Projektinimi = p.Projektinimi,
                             }).FirstOrDefault();

                string json = JsonConvert.SerializeObject(model);
                entities.Dispose();

                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Update(Projektit proj)
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();

            bool OK = false;

            // onko kyseessä muokkaus vai uuden lisääminen?
            if (proj.ProjektiID == 0)
            {
                // kyseessä on uuden asiakkaan lisääminen, kopioidaan kentät
                Projektit dbItem = new Projektit()
                {

                    Projektinimi = proj.Projektinimi

                };

                // tallennus tietokantaan
                entities.Projektits.Add(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            else
            {
                // muokkaus, haetaan id:n perusteella riviä tietokannasta
                Projektit dbItem = (from p in entities.Projektits
                                    where p.ProjektiID == proj.ProjektiID
                                    select p).FirstOrDefault();
                if (dbItem != null)
                {
                    dbItem.Projektinimi = proj.Projektinimi;

                    // tallennus tietokantaan
                    entities.SaveChanges();
                    OK = true;
                }
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();

            // etsitään id:n perusteella asiakasrivi kannasta
            bool ok = false;
            Projektit dbItem = (from p in entities.Projektits
                                where p.ProjektiID == id
                                select p).FirstOrDefault();
            if (dbItem != null)
            {
                // tietokannasta poisto
                entities.Projektits.Remove(dbItem);
                entities.SaveChanges();
                ok = true;
            }
            entities.Dispose();

            return Json(ok, JsonRequestBehavior.AllowGet);
        }
    }
}