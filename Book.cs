using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace ekz_wpf_sql
{
    internal class Book
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string FIO { get; set; }
        public string Izdatel { get; set; }
        public string Ganre { get; set; }
        public string Date { get; set; }
        public int Price { get; set; }
        public int CountSTR { get; set; }
    }
}
