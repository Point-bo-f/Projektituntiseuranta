using ProjektituntiSeuranta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjektituntiSeuranta.Controllers
{
    public class MdHenkiloController : Controller
    {
        // GET: MdHenkilo
        public ActionResult Index()
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();
            try
            {
                List<Henkilot> model = entities.Henkilots.ToList();
                return View(model);
            }

            finally
            {
                entities.Dispose();
            }
            
        }
    }
}