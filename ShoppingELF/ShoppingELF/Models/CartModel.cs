using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingELF.Models
{
    public class CartModel
    {
        public int CartID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> PID { get; set; }
        public int Quantity { get; set; }

        public virtual SizeModel SizeModel { get; set; }
        public virtual UserTable UserTable { get; set; }
        //public virtual SizeModel SizeModel { get; set; }
    }
}