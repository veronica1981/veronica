using System;

namespace FCCL_DAL.Entities
{
    public class Sms
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateSend { get; set; }
        public string CellNr { get; set; }
        public string Message { get; set; }
        public int TryNr { get; set; }
    }
}
