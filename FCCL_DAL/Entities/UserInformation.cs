using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCCL_DAL.Entities
{
    public class UserInformation
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserCod { get; set; }
        public bool IsAsoc { get; set; }
        public int AsocId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
