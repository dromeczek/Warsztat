using System;

namespace WorkshoManager.Models
{
    public class ReportViewModel
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Vehicle { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal LaborCost { get; set; }
        public decimal PartsCost { get; set; }
        public decimal TotalCost => LaborCost + PartsCost;
    }
}