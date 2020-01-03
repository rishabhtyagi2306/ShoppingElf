using ShoppingELF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ShoppingELF.Controllers
{
    public class ProductController : ApiController
    {
        [HttpGet]
        [Route("api/Product/Home")]
        public IHttpActionResult GetSuitableFor()
        {
            var x = new ProductRepository().GetSuitableFor();
            return Ok(x);
        }

        [HttpGet]
        [Route("api/Product/Category/{Suitableid}")]
        public IHttpActionResult GetCategory(int Suitableid)
        {
            var x = new ProductRepository().GetCategory(Suitableid);
            return Ok(x);
        }

        [HttpGet]
        [Route("api/Product/SubCategory/{Categoryid}")]
        public IHttpActionResult GetSubCategory(int Categoryid)
        {
            var x = new ProductRepository().GetSubCategory(Categoryid);
            return Ok(x);
        }

        [HttpGet]
        [Route("api/Product/GetProduct/{SubCategoryid}")]
        public IHttpActionResult GetProducts(int SubCategoryid)
        {
            var x = new ProductRepository().GetAllProduct(SubCategoryid);
            return Ok(x);
        }

        [HttpGet]
        [Route("api/Product/ProductDetails/{Productid}")]
        public IHttpActionResult GetProductDetails(int Productid)
        {
            var x = new ProductRepository().GetProductDetails(Productid);
            return Ok(x);
        }

        [HttpGet]
        [Route("api/Product/GetProductBySuitableID/{sid}")]
        public IHttpActionResult GetProductsBySuitableID(int sid)
        {
            //var y = new ProductRepository().GetCategory(sid);
            var x = new ProductRepository().GetProductsBySuitableID(sid);
            return Ok(x);
        }

        [HttpPost]
        [Route("api/Product/AddProduct/{subid}/{Suitid}")]
        public HttpResponseMessage AddProduct(int subid, int suitid, ProductModel model, string token)
        {
            try
            {
                using (ShoppingELFEntities context = new ShoppingELFEntities())
                {
                    SellerTable seller = new SellerTable();
                    string username = TokenManager.ValidateToken(token);
                    seller = context.SellerTable.FirstOrDefault(x => x.email == username);

                    if (seller != null && seller.Role == "Seller")
                    {
                        int x = new ProductRepository().AddProduct(subid, seller.SellerID, suitid, model);
                        return Request.CreateResponse(HttpStatusCode.OK, x);
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, "Access to this page is denied");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("api/Product/AddSize/{pid}")]
        public HttpResponseMessage AddProductSize(int pid, SizeModel model, string token)
        {
            try
            {
                using (ShoppingELFEntities context = new ShoppingELFEntities())
                {
                    SellerTable seller = new SellerTable();
                    string username = TokenManager.ValidateToken(token);
                    seller = context.SellerTable.FirstOrDefault(x => x.email == username);

                    if (seller != null && seller.Role == "Seller")
                    {
                        bool x = new ProductRepository().AddProductSize(pid, model);
                        if (x)
                            return Request.CreateResponse(HttpStatusCode.OK, "Product Size Added");
                        else
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Their is some problem adding the product size");
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, "Access to this page is denied");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("api/Product/EditProduct/{pid}")]
        public HttpResponseMessage EditProduct(int pid, SizeModel model, string token)
        {
            try
            {
                using (ShoppingELFEntities context = new ShoppingELFEntities())
                {
                    SellerTable seller = new SellerTable();
                    string username = TokenManager.ValidateToken(token);
                    seller = context.SellerTable.FirstOrDefault(x => x.email == username);

                    if (seller != null && seller.Role == "Seller")
                    {
                        bool x = new ProductRepository().EditProduct(pid, model);
                        if (x)
                            return Request.CreateResponse(HttpStatusCode.OK, "Product Details Updated successfully");
                        else
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Something Went Wrong , please try again later");
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, "Access to this page is denied");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("api/Product/Delete/Size/{pid}")]
        public HttpResponseMessage DeleteProductSize(int pid, string token)
        {
            try
            {
                using (ShoppingELFEntities context = new ShoppingELFEntities())
                {
                    SellerTable seller = new SellerTable();
                    string username = TokenManager.ValidateToken(token);
                    seller = context.SellerTable.FirstOrDefault(x => x.email == username);

                    if (seller != null && seller.Role == "Seller")
                    {
                        bool x = new ProductRepository().DeleteSize(pid);
                        if (x)
                            return Request.CreateResponse(HttpStatusCode.OK, "Size Removed successfully");
                        else
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Unable to proccess request");
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, "Access to this page is denied");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("api/Product/Delete/{pid}")]
        public HttpResponseMessage DeleteProduct(int pid, string token)
        {
            try
            {
                using (ShoppingELFEntities context = new ShoppingELFEntities())
                {
                    SellerTable seller = new SellerTable();
                    string username = TokenManager.ValidateToken(token);
                    seller = context.SellerTable.FirstOrDefault(x => x.email == username);

                    if (seller != null && seller.Role == "Seller")
                    {
                        bool x = new ProductRepository().DeleteProduct(pid);
                        if (x)
                            return Request.CreateResponse(HttpStatusCode.OK, "Product deleted successfully");
                        else
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Unable to delete product");
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, "Access to this page is denied");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        [Route("api/Product/Seller/Show")]
        public HttpResponseMessage ShowSellerProduct(string token)
        {
            try
            {
                using (ShoppingELFEntities context = new ShoppingELFEntities())
                {
                    SellerTable seller = new SellerTable();
                    string username = TokenManager.ValidateToken(token);
                    seller = context.SellerTable.FirstOrDefault(x => x.email == username);

                    if (seller != null && seller.Role == "Seller")
                    {
                        var x = new ProductRepository().ShowSellerProduct(seller.SellerID);
                        return Request.CreateResponse(HttpStatusCode.OK, x);
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.Forbidden, "Access to this page is denied");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        [Route("api/Product/Seller/Show/Size/{pid}")]
        public HttpResponseMessage ShowSellerProductSize(int pid, string token)
        {
            try
            {
                using (ShoppingELFEntities context = new ShoppingELFEntities())
                {
                    SellerTable seller = new SellerTable();
                    string username = TokenManager.ValidateToken(token);
                    seller = context.SellerTable.FirstOrDefault(x => x.email == username);

                    if (seller != null && seller.Role == "Seller")
                    {
                        var x = new ProductRepository().ShowSellerProductSize(pid);
                        return Request.CreateResponse(HttpStatusCode.OK, x);
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.Forbidden, "Access to this page is denied");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("api/UploadImage/{pid}/{picimg}")]
        public IHttpActionResult PostUserImage(int pid, int picimg, string token)
        {
            using(ShoppingELFEntities context = new ShoppingELFEntities())
            {
                SellerTable seller = new SellerTable();
                string username = TokenManager.ValidateToken(token);
                seller = context.SellerTable.FirstOrDefault(m => m.email == username);
                if (seller != null && seller.Role == "Seller")
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    try
                    {
                        
                        var httpRequest = HttpContext.Current.Request;
                        foreach (string file in httpRequest.Files)
                        {
                            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                            var postedFile = httpRequest.Files[file];
                            if (postedFile != null && postedFile.ContentLength > 0)
                            {
                                IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png", ".jpeg" };
                                var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                                var extension = ext.ToLower();
                                if (!AllowedFileExtensions.Contains(extension))
                                {
                                    var message = string.Format("Please Upload image of type .jpg,.gif,.png.");
                                    dict.Add("error", message);
                                    return BadRequest("Please Upload image of type .jpg,.gif,.png.,.jpeg");
                                }
                                else
                                {
                                    var filePath = HttpContext.Current.Server.MapPath("~/ProductImage/" + postedFile.FileName);
                                    postedFile.SaveAs(filePath);
                                    string imagepath = "/ProductImage/" + postedFile.FileName;
                                    int image = new ProductRepository().ImageUpload(pid, picimg, imagepath);
                                    if (image == 2)
                                        return NotFound();
                                    if (image == 0)
                                        return BadRequest("Your image might be greater than the 1mb ,please upload a valid image");
                                       

                                }
                            }

                            var message1 = "/ProductImage/" + postedFile.FileName;
                            return Ok(message1); 
                        }
                        var res = string.Format("Please Upload a image.");
                        dict.Add("error", res);
                        return NotFound();
                    }
                    catch (Exception ex)
                    {
                        var res = string.Format("please check your internet connection");
                        dict.Add("error", res);
                        return NotFound();
                    }
                }
                else
                    return Unauthorized();
            }
        }

    }
}
