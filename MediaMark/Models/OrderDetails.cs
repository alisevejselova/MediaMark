using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMark.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailsID { get; set; }
        public int RefOrderID { get; set; }
        public Order Order { get; set; }

        public int RefProductID { get; set; }
        public Products Products { get; set; }
        public int Quantity { get; set; }

        [Display(Name = "Total Price")]
        public int TotalPrice { get; set; }

    }
}
