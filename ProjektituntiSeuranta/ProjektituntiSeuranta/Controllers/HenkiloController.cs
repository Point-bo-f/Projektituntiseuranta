using ProjektituntiSeuranta.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCkurssitehtavaJatkokurssi.Controllers
{
    public class HenkiloController : Controller
    {
        public ActionResult Index2()
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();
            List<Henkilot> model = entities.Henkilots.ToList();
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

            var model = (from h in entities.Henkilots
                         select new
                         {
                             HenkiloID = h.HenkiloID,
                             Etunimi = h.Etunimi,
                             Sukunimi = h.Sukunimi,
                             Osoite = h.Osoite,
                             Esimies = h.Esimies

                         }).ToList();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            return Json(json, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetSingleHenkilo(int id)
        {

            {
                AsiakastietokantaEntities entities = new AsiakastietokantaEntities();
                var model = (from h in entities.Henkilots
                             where h.HenkiloID == id
                             select new
                             {
                                 HenkiloID = h.HenkiloID,
                                 Etunimi = h.Etunimi,
                                 Sukunimi = h.Sukunimi,
                                 Osoite = h.Osoite,
                                 Esimies = h.Esimies,
                             }).FirstOrDefault();

                string json = JsonConvert.SerializeObject(model);
                entities.Dispose();

                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Update(Henkilot henk)
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();

            bool OK = false;

            // onko kyseessä muokkaus vai uuden lisääminen?
            if (henk.HenkiloID == 0)
            {
                // kyseessä on uuden asiakkaan lisääminen, kopioidaan kentät
                Henkilot dbItem = new Henkilot()
                {

                    Etunimi = henk.Etunimi,
                    Sukunimi = henk.Sukunimi,
                    Osoite = henk.Osoite,
                    Esimies = henk.Esimies

                };

                // tallennus tietokantaan
                entities.Henkilots.Add(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            else
            {
                // muokkaus, haetaan id:n perusteella riviä tietokannasta
                Henkilot dbItem = (from h in entities.Henkilots
                                   where h.HenkiloID == henk.HenkiloID
                                   select h).FirstOrDefault();
                if (dbItem != null)
                {
                    dbItem.Etunimi = henk.Etunimi;
                    dbItem.Sukunimi = henk.Sukunimi;
                    dbItem.Osoite = henk.Osoite;
                    dbItem.Esimies = henk.Esimies;

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
            Henkilot dbItem = (from h in entities.Henkilots
                               where h.HenkiloID == id
                               select h).FirstOrDefault();
            if (dbItem != null)
            {
                // tietokannasta poisto
                entities.Henkilots.Remove(dbItem);
                entities.SaveChanges();
                ok = true;
            }
            entities.Dispose();

            return Json(ok, JsonRequestBehavior.AllowGet);
        }
    }
}