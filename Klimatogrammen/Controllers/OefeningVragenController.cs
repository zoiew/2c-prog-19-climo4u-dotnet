﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Klimatogrammen.Models.Domein;
using Klimatogrammen.ViewModels;
using WebGrease.Css.Extensions;

namespace Klimatogrammen.Controllers
{
    public class OefeningVragenController : Controller
    {
        public ActionResult Index(Leerling leerling)
        {
            ActionResult route = RedirectIndienNodig(leerling);
            if (route != null)
                return route;
            VragenIndexViewModel vraagVM = new VragenIndexViewModel(leerling.Graad.Vragen, leerling.Klimatogram);
            return View(vraagVM);
        }

        [HttpPost]
        public ActionResult Index(Leerling leerling, [Bind(Prefix = "Antwoorden")] AntwoordViewModel antwoorden)
        {
            ActionResult route = RedirectIndienNodig(leerling);
            if (route != null)
                return route;

            string[] antwden= new string[antwoorden.Antwoord.Length];
            
            for (int i = 0; i < leerling.Graad.Vragen.Count; i++)
            {
                leerling.Graad.Vragen.ElementAt(i).ValideerVraag(antwoorden.Antwoord[i], leerling.Klimatogram);
                if (leerling.Graad.Vragen.ElementAt(i).Resultaat == Resultaat.Juist)
                {
                    antwden[i] = antwoorden.Antwoord[i];
                }
                else
                {
                    antwden[i] = null;
                }
            }
            AntwoordViewModel antw = new AntwoordViewModel(antwden);
            VragenIndexViewModel vraagVM = new VragenIndexViewModel(leerling.Graad.Vragen, leerling.Klimatogram){Antwoorden=antw};
            return View(vraagVM);
        }

        public ActionResult GetJSON(Leerling leerling)
        {
            Klimatogram klimatogram = leerling.Klimatogram;
            object klim = new
            {
                GemiddeldeTemperatuur = klimatogram.Maanden.Select(maand => maand.Temperatuur).ToList(),
                GemiddeldeNeerslag = klimatogram.Maanden.Select(maand => maand.Neerslag).ToList(),
                klimatogram.BeginJaar,
                klimatogram.EindJaar,
                Land = klimatogram.Land.Naam,
                klimatogram.Locatie,
                TotaalGemiddeldeTemperatuur = klimatogram.GeefGemiddeldeTemperatuur(),
                TotaalNeerslag = klimatogram.GeefTotaleNeerslag()
            };
            return Json(klim, JsonRequestBehavior.AllowGet);
        }

        private ActionResult RedirectIndienNodig(Leerling leerling)
        {
            if (leerling == null || leerling.Graad == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (leerling.Graad.Nummer == 3)
            {
                //TODO: controller voor kaartje bestaat nog niet dus nu naar home
                return RedirectToAction("Index", "Home");
            }
            if (leerling.Graad.Nummer == 2 && leerling.Klimatogram != null)
            {
                return RedirectToAction("Index", "Determinatie");
            }
            if (leerling.Klimatogram == null || (leerling.Graad.Nummer == 2 && leerling.Klimatogram == null) )
            {
                return RedirectToAction("Index", "Klimatogram");
            }
            return null;
        }
    }
}