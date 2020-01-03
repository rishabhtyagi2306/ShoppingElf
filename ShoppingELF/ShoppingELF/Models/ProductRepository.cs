using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingELF.Models
{
    public class ProductRepository
    {
        public List<SuitableForModel> GetSuitableFor()
        {
            using(var context = new ShoppingELFEntities())
            {
                var result = context.SuitableForTable
                    .Select(x => new SuitableForModel()
                    {
                        SuitableID = x.SuitableID,
                        SuitableName = x.SuitableName
                    }).ToList();
                return result;
            }
        }

        public List<CategoryModel> GetCategory(int SuitableID)
        {
            using(var context = new ShoppingELFEntities())
            {
                var result = context.CategoryTable
                    .Where(x => x.SuitableID == SuitableID)
                    .Select(x => new CategoryModel()
                    {
                        CategoryID = x.CategoryID,
                        SuitableID = x.SuitableID,
                        CategoryName = x.CategoryName
                    }).ToList();
                return result;
            }
        }

        public List<SubCategoryModel> GetSubCategory(int CategoryID)
        {
            using(var context = new ShoppingELFEntities())
            {
                var result = context.SubCategoryTable
                    .Where(x => x.CategoryID == CategoryID)
                    .Select(x => new SubCategoryModel()
                    {
                        SubCategoryID = x.SubCategoryID,
                        SubCategoryName = x.SubCategoryName,
                        CategoryID = x.CategoryID
                    }).ToList();
                return result;
            }
        }

        public List<ProductModel> GetAllProduct(int id)
        {
            using(var context = new ShoppingELFEntities())
            {
                var result = context.ProductTable
                    .Where(x => x.SubCategoryID == id)
                    .Select(x => new ProductModel()
                    {
                        ProductID = x.ProductID,
                        productName = x.productName,
                        productBrand = x.productBrand,
                        productDetails = x.productDetails,
                        picture1 = x.picture1,
                        picture2 = x.picture2,
                        picture3 = x.picture3,
                        picture4 = x.picture4,
                        SellerID = x.SellerID,
                        SubCategoryID = x.SubCategoryID,
                        SuitableID = x.SuitableID
                    }).ToList();
                return result;
            }
        }

        public List<ProductModel> GetProductsBySuitableID(int sid)
        {
            using(ShoppingELFEntities context = new ShoppingELFEntities())
            {
                var result = context.ProductTable
                    .Where(x => x.SuitableID == sid)
                    .Select(x => new ProductModel()
                    {
                        ProductID = x.ProductID,
                        productName = x.productName,
                        productBrand = x.productBrand,
                        price = x.price,
                        productDetails = x.productDetails,
                        picture1 = x.picture1,
                        picture2 = x.picture2,
                        picture3 = x.picture3,
                        picture4 = x.picture4,
                        SellerID = x.SellerID,
                        SubCategoryID = x.SubCategoryID,
                        SuitableID = x.SuitableID
                    }).ToList();
                return result;
            }
        }

        public List<SizeModel> GetProductDetails(int id)
        {
            using(var context = new ShoppingELFEntities())
            {
                var result = context.SizeTable
                    .Where(x => x.ProductID == id)
                    .Select(x => new SizeModel()
                    {
                        PID = x.PID,
                        ProductID = x.ProductID,
                        productSize = x.productSize,
                        productQuantity = x.productQuantity,
                        productPrice = x.productPrice,
                        ProductModel = new ProductModel()
                        {
                            productName = x.ProductTable.productName,
                            picture1 = x.ProductTable.picture1,
                            picture2 = x.ProductTable.picture2,
                            picture3 = x.ProductTable.picture3,
                            picture4 = x.ProductTable.picture4,
                            productBrand = x.ProductTable.productBrand,
                            productDetails = x.ProductTable.productDetails,
                            ProductID = x.ProductTable.ProductID
                        }
                    }).ToList();
                return result;
            }
        }

        public int AddProduct(int subid, int sid, int suitid, ProductModel model)
        {
            using(ShoppingELFEntities context = new ShoppingELFEntities())
            {
                ProductTable pt = new ProductTable()
                {
                    productName = model.productName,
                    productBrand = model.productBrand,
                    productDetails = model.productDetails,
                    SubCategoryID = subid,
                    SellerID = sid,
                    SuitableID = suitid
                };
                context.ProductTable.Add(pt);
                context.SaveChanges();
                return pt.ProductID;
            }
        }

        public bool AddProductSize(int pid, SizeModel model)
        {
            using (ShoppingELFEntities context = new ShoppingELFEntities())
            {
                SizeTable st = new SizeTable()
                {
                    ProductID = pid,
                    productSize = model.productSize,
                    productPrice = model.productPrice,
                    productQuantity = model.productQuantity
                };
                context.SizeTable.Add(st);
                context.SaveChanges();
                SelectMininmumPrice(pid);
            }
            return true;
        }

        public void SelectMininmumPrice(int pid)
        {
            using(ShoppingELFEntities context = new ShoppingELFEntities())
            {
                ProductTable pt = new ProductTable();
                SizeTable st = new SizeTable();
                pt = context.ProductTable.FirstOrDefault(x => x.ProductID == pid);
                
                var SizeList = context.SizeTable.Where(m => m.ProductID == pid).ToList();
                int min_price = 2147483647;
                foreach(var i in SizeList)
                {
                    if(i.productPrice < min_price)
                    {
                        min_price = i.productPrice;
                    }
                }
                pt.price = min_price;
                context.SaveChanges();
            }
            
        }

        public bool EditProduct(int pid, SizeModel model)
        {
            using(ShoppingELFEntities context = new ShoppingELFEntities())
            {
                var product = context.SizeTable.FirstOrDefault(x => x.PID == pid);
                if (product != null)
                {
                    product.productPrice = model.productPrice;
                    product.productQuantity = model.productQuantity;
                    product.productSize = model.productSize;
                    context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        public bool DeleteSize(int pid)
        {
            using(ShoppingELFEntities context = new ShoppingELFEntities())
            {
                var size = context.SizeTable.FirstOrDefault(x => x.PID == pid);
                if (size != null)
                {
                    var order = context.CartTable.FirstOrDefault(x => x.PID == pid);
                    if(order != null)
                    {
                        context.CartTable.Remove(order);
                        context.SaveChanges();
                    }
                    context.SizeTable.Remove(size);
                    context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        public bool DeleteProduct(int pid)
        {
            using(ShoppingELFEntities context = new ShoppingELFEntities())
            {
                var size = context.SizeTable.Where(x => x.ProductID == pid).ToList();
                var product = context.ProductTable.FirstOrDefault(x => x.ProductID == pid);
                var cart = context.CartTable.FirstOrDefault(x => x.PID == pid);
                if (size != null && product != null)
                {
                    if(cart != null)
                    {
                        context.CartTable.Remove(cart);
                        context.SaveChanges();
                    }
                    foreach (var i in size)
                        context.SizeTable.Remove(i);
                    context.SaveChanges();
                    context.ProductTable.Remove(product);
                    context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        public List<ProductModel> ShowSellerProduct(int sellID)
        {
            using(ShoppingELFEntities context = new ShoppingELFEntities())
            {
                var result = context.ProductTable
                    .Where(x => x.SellerID == sellID)
                    .Select(x => new ProductModel()
                    {
                        ProductID = x.ProductID,
                        productName = x.productName,
                        productBrand = x.productBrand,
                        price = x.price,
                        productDetails = x.productDetails,
                        picture1 = x.picture1,
                        picture2 = x.picture2,
                        picture3 = x.picture3,
                        picture4 = x.picture4
                    }).ToList();
                return result;
            }
        }

        public List<SizeModel> ShowSellerProductSize(int pid)
        {
            using(ShoppingELFEntities context = new ShoppingELFEntities())
            {
                var result = context.SizeTable
                    .Where(x => x.ProductID == pid)
                    .Select(x => new SizeModel()
                    {
                        PID = x.PID,
                        ProductID = x.ProductID,
                        productSize = x.productSize,
                        productQuantity = x.productQuantity,
                        productPrice = x.productPrice,
                        ProductModel = new ProductModel()
                        {
                            productName = x.ProductTable.productName,
                            picture1 = x.ProductTable.picture1,
                            picture2 = x.ProductTable.picture2,
                            picture3 = x.ProductTable.picture3,
                            picture4 = x.ProductTable.picture4,
                            productBrand = x.ProductTable.productBrand,
                            productDetails = x.ProductTable.productDetails,
                            ProductID = x.ProductTable.ProductID
                        }
                    }).ToList();
                return result;
            }
        }

        public int ImageUpload(int pid, int picimg, string imgpath)
        {
            using(ShoppingELFEntities context = new ShoppingELFEntities())
            {
                ProductTable pt = new ProductTable();
                pt = context.ProductTable.FirstOrDefault(m => m.ProductID == pid);
                if (pt != null)
                {
                    if (picimg == 1)
                    {
                        pt.picture1 = imgpath;
                        context.SaveChanges();
                        return 1;
                    }
                    else if (picimg == 2)
                    {
                        pt.picture2 = imgpath;
                        context.SaveChanges();
                        return 1;
                    }
                    else if (picimg == 3)
                    {
                        pt.picture3 = imgpath;
                        context.SaveChanges();
                        return 1;
                    }
                    else if (picimg == 4)
                    {
                        pt.picture4 = imgpath;
                        context.SaveChanges();
                        return 1;
                    }
                    else
                        return 0;
                }
                else
                    return 2;
                
            }
        }
        
    }
}