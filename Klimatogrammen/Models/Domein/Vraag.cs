﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Klimatogrammen.Models.Domein;

namespace Klimatogrammen
{
    public abstract class Vraag
    {

        public Resultaat Resultaat { get; set; }

        public Parameter Parameter
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string VraagTekst
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public abstract ICollection<string> GeefMogelijkeAntwoorden();
        public abstract void ValideerVraag(string antwoord, Klimatogram klimatogram);
    }
}
