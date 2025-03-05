
namespace Domain.Entities
{
    public class Log
    {

        public int id { get; set; }

        public int userid { get; set; }
        public logmethods  methods { get; set; }

        public string route { get; set; }


        public string? Body { get; set; }


        public LogStatus Status { get; set; }



    }
}
