using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace budget_app.Data.Models
{
    public class Week
    {
        public int? Id { get; set; }
        [Required]
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}