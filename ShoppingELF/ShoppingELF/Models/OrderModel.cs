using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingELF.Models
{
    public class OrderModel
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public string productBrand { get; set; }
        public string ProductName { get; set; }
        public int productPrice { get; set; }
        public string productSize { get; set; }
        public string productPicture { get; set; }
        public int productQuantity { get; set; }
        public Nullable<int> PID { get; set; }

        public virtual SizeTable SizeTable { get; set; }
        public virtual UserTable UserTable { get; set; }
    }
}