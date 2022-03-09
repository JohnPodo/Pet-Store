using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMeUp.Models.Models
{
    public class LogMeUp
    {
        public int Id { get; set; }

        public Guid ProcessSession { get; set; }

        public Severity Severity { get; set; }

        public string Message { get; set; }

        public DateTime InsertDate { get; set; }

    }
}
