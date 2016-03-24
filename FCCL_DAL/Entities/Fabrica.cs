using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCCL_DAL.Entities
{
    public class Fabrica
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public string Strada { get; set; }
        public string Numar { get; set; }
        public string Oras { get; set; }
        public int? Judet { get; set; }
        public string CodPostal { get; set; }
        public string Telefon { get;set;}
        public string Fax { get; set; }
        public string Email { get; set; }
        public string PersoanaDeContact { get; set; }
        public string TelPersContact { get; set; }
    }
}
