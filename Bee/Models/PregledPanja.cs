using System;
using SQLite;

namespace Bee.Models
{
    public class PregledPanja
    {
        [PrimaryKey, AutoIncrement]
        public int ID_pregleda { get; set; }
        public int Pripada_panju { get; set; }
        public DateTime Datum_pregleda { get; set; }
        public int Moc_ob_pregledu { get; set; }
        public int Hrana_ob_pregledu { get; set; }
        public int Zalega_ob_pregledu { get; set; }
        public string Dodatni_zapiski { get; set; }
    }
}
