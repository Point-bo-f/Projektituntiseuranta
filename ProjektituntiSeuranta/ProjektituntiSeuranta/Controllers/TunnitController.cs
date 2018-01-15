using ProjektituntiSeuranta.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCkurssitehtavaJatkokurssi.Controllers
{
    public class TunnitController : Controller
    {
        public ActionResult Index2()
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();
            List<Tunnit> model = entities.Tunnits.ToList();
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

            var model = (from t in entities.Tunnits
                         select new
                         {
                             TuntiID = t.TuntiID,
                             ProjektiID = t.ProjektiID,
                             HenkiloID = t.HenkiloID,
                             Pvm = t.Pvm,
                             Projektitunnit = t.Projektitunnit

                         }).ToList();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            return Json(json, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetSingleTunti(int id)
        {

            {
                AsiakastietokantaEntities entities = new AsiakastietokantaEntities();
                var model = (from t in entities.Tunnits
                             where t.TuntiID == id
                             select new
                             {
                                 TuntiID = t.TuntiID,
                                 ProjektiID = t.ProjektiID,
                                 HenkiloID = t.HenkiloID,
                                 Pvm = t.Pvm,
                                 Projektitunnit = t.Projektitunnit,
                             }).FirstOrDefault();

                string json = JsonConvert.SerializeObject(model);
                entities.Dispose();

                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Update(Tunnit tunt)
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();

            bool OK = false;

            // onko kyseessä muokkaus vai uuden lisääminen?
            if (tunt.TuntiID == 0)
            {
                // kyseessä on uuden asiakkaan lisääminen, kopioidaan kentät
                Tunnit dbItem = new Tunnit()
                {

                    ProjektiID = tunt.ProjektiID,
                    HenkiloID = tunt.HenkiloID,
                    Pvm = tunt.Pvm,
                    Projektitunnit = tunt.Projektitunnit

                };

                // tallennus tietokantaan
                entities.Tunnits.Add(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            else
            {
                // muokkaus, haetaan id:n perusteella riviä tietokannasta
                Tunnit dbItem = (from t in entities.Tunnits
                                 where t.TuntiID == tunt.TuntiID
                                 select t).FirstOrDefault();
                if (dbItem != null)
                {
                    dbItem.ProjektiID = tunt.ProjektiID;
                    dbItem.HenkiloID = tunt.HenkiloID;
                    dbItem.Pvm = tunt.Pvm;
                    dbItem.Projektitunnit = tunt.Projektitunnit;

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
            Tunnit dbItem = (from t in entities.Tunnits
                             where t.TuntiID == id
                             select t).FirstOrDefault();
            if (dbItem != null)
            {
                // tietokannasta poisto
                entities.Tunnits.Remove(dbItem);
                entities.SaveChanges();
                ok = true;
            }
            entities.Dispose();

            return Json(ok, JsonRequestBehavior.AllowGet);
        }
    }
}