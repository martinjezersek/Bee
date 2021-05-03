using System;
using SQLite;

namespace Bee.Models
{
    public class SimboliZaHrano
    {
        [PrimaryKey, AutoIncrement]
        public int Simbol_hrana_ID { get; set; }
        public string Simbol_hrana_icona { get; set; }
        public string Simbol_hrana_opis { get; set; }
    }
}
