using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Operation : BaseEntity
    {
        [Required]
        public DateTime Date { get; set; }

        public bool IsIncome { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int? CategoryId { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
    }
}
