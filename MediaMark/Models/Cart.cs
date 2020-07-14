using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMark.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }
        public int RefUserID { get; set; } 
        public User User { get; set; }

        public int RefProductID { get; set; } 
        public Products Products { get; set; }

        public int  Quantity { get; set; }
        [Display(Name = "Unit Price")]
       
        public int TotalPrice { get; set; }

      
      

    }
}
