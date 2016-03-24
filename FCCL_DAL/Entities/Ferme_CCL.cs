using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCCL_DAL.Entities
{
    public class Ferme_CCL
    {
        public Int32? Id { get; set; }
        public String Cod { get; set; }
        public String Nume { get; set; }

        public Int32? FabricaId { get; set; }
        public String Strada { get; set; }
        public String Numar { get; set; }
        public String Oras { get; set; }
        public String Judet { get; set; }
        public Int32? JudetId { get; set; }
        public String Telefon { get; set; }
        public String Email { get; set; }
        public Int32? FermierId { get; set; }
        public String CodPostal { get; set; }
        public String Fax { get; set; }
        public String PersoanaDeContact { get; set; }
        public String TelPersoanaContact { get; set; }
        public DateTime DataAchizitie { get; set; }

        public String Ferme { get; set; }
        public bool SendSms { get; set; }

    }
}