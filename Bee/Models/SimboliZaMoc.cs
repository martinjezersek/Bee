using SQLite;
using System;

namespace Bee.Models
{
    public class SimboliZaMoc
    {
        [PrimaryKey, AutoIncrement]
        public int Simbol_moc_ID { get; set; }
        public string Simbol_moc_icona { get; set; }
        public string Simbol_moc_opis { get; set; }
    }
}
