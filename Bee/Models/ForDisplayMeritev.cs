using System;
using System.Collections.Generic;
using System.Text;

namespace Bee.Models
{
    public class ForDisplayMeritev
    {
        public DateTime Meritev_datum { get; set; } 
        public string Meritev_zalega { get; set; }
        public string Meritev_hrana { get; set;}
        public string Meritev_moc { get; set; }
        public string Meritev_opis { get; set; }
        public string Meritev__pregled_ID { get; set; }
    }
}
