using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingELF.Models
{
    public class SoldModel
    {
        public int SoldID { get; set; }
        public int SellerID { get; set; }
        public string productName { get; set; }
        public string productBrand { get; set; }
        public int productPrice { get; set; }
        public int productQuantity { get; set; }
        public string productSize { get; set; }
        public string productPicture { get; set; }
        public string UserName { get; set; }
        public Nullable<int> PID { get; set; }

        public virtual SellerTable SellerTable { get; set; }
        public virtual SizeTable SizeTable { get; set; }
    }
}