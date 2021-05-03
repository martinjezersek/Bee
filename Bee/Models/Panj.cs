using SQLite;
using System;

namespace Bee.Models
{
    public class Panj
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int StPanj { get; set; }
        public string Matica { get; set; }
        public DateTime Datum { get; set; }
    }
}
