using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMark.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [StringLength(60, MinimumLength = 2)]
        [Required]
        public string CategoryName { get; set; }
        public ICollection<Products> Products { get; set; }


    }
}
