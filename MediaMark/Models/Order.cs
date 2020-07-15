using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMark.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public int RefUserID { get; set; }
        public User User { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Identity Number")]
        public string IdentityNumber { get; set; }
        [Display(Name = "Date")]
        public Nullable<System.DateTime> OrderDate { get; set; }


        public ICollection<OrderDetails> OrderDetail { get; set; }

        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }
    }
}
