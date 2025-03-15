
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Log
    {

        public int Id { get; set; }
        
        public LogMethods  Method { get; set; }

        public string Route { get; set; }


        public string? Body { get; set; }


        public LogStatus Status { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

    }
}
