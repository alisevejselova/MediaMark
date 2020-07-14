using MediaMark.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMark.ViewModels
{
    public class ProductPicture
    { 
        [Key]
        public int ProductID { get; set; }

        public IFormFile Picture { get; set; }
        
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }


        [Display(Name = "Description")]
        public string ProductDescription { get; set; }

        [Required]
        [Display(Name = "Price")]
        public string ProductPrice { get; set; }

        public int RefCateogryID { get; set; }
        public Category Category { get; set; }



        [Display(Name = "Shopping Cart")]
        public ICollection<Cart> ShoppingCart { get; set; }

        public ICollection<OrderDetails> OrderDetail { get; set; }



    }
}
