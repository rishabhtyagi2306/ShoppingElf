using ShoppingELF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Helpers;
using System.Web.Http;

namespace ShoppingELF.Controllers
{
    public class SellerController : ApiController
    {
        [HttpPost]
        [Route("api/Seller/Signup")]
        public HttpResponseMessage PostSignup([FromBody]SellerTable seller)
        {
            try
            {
                var x = new SellerAccountModel().IsSellerExist(seller.email);
                if (x)
                {
                    var timeSent1 = DateTime.Now.TimeOfDay.Seconds;
                    return Request.CreateResponse(HttpStatusCode.Forbidden, "Account already exist");
                }
                else
                {
                    new SellerAccountModel().AddSeller(seller);
                    EmailVerification(seller.SellerID, seller.email, seller.OTP);
                    new SellerAccountModel().OTPSentTime(seller.email);
                    return Request.CreateResponse(HttpStatusCode.Created, seller.SellerID);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("api/Seller/Login")]
        public HttpResponseMessage Login([FromBody]SellerTable seller)
        {
            try
            {
                var y = new SellerAccountModel().verification(seller.email);
                var password = new SellerAccountModel().Password(seller.email);
                SellerTable u = new SellerAccountModel().GetSeller(seller.email);

                if (u == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                         "The Account was not found.");
                string pass = Crypto.Hash(seller.password);
                bool credentials = pass.Equals(password);
                if (credentials && y)
                    return Request.CreateResponse(HttpStatusCode.OK, TokenManager.GenerateToken(seller.email));
                else
                    return Request.CreateResponse(HttpStatusCode.Forbidden, "The email/password combination was wrong.");
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        
        [HttpPost]
        [Route("api/Seller/EnterOTP/{sid}")]
        public IHttpActionResult EnterOTP(int sid, SellerModel model)
        {
            try
            {
                using (ShoppingELFEntities context = new ShoppingELFEntities())
                {
                    SellerTable seller = new SellerTable();
                    seller = context.SellerTable.FirstOrDefault(m => m.SellerID == sid);
                    bool x = new SellerAccountModel().IsOTPExpired(sid);
                    if (seller.OTP == model.OTP && !x)
                    {
                        seller.IsAccountVerified = true;
                        context.SaveChanges();
                        return Ok(TokenManager.GenerateToken(seller.email));
                    }
                    else
                        return BadRequest("Please enter a valid OTP");
                }
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/Seller/ResendOTP/{sid}")]
        public IHttpActionResult ResendOTP(int sid)
        {
            try
            {
                using (ShoppingELFEntities context = new ShoppingELFEntities())
                {
                    new SellerAccountModel().ResendOTP(sid);
                    SellerTable st = new SellerTable();
                    st = context.SellerTable.FirstOrDefault(m => m.SellerID == sid);
                    EmailVerification(sid, st.email, st.OTP);
                    return Ok("OTP sent sucessfully");
                }
            }
            catch(Exception ex)
            {
                return BadRequest();
            }

        }

        [HttpPost]
        [Route("api/Seller/EnterDetails")]
        public IHttpActionResult EnterDetails(SellerDetailsModel model, string token)
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
                        bool x = new SellerModel().EnterDetails(seller.SellerID, model);
                        if (x)
                            return Ok("Details addded successfully");
                        else
                            return Ok("Something went wrong");
                    }
                    else
                        return Unauthorized();
                }
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/Seller/EditSellerDetails")]
        public IHttpActionResult EditDetails(SellerDetailsModel model, string token)
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
                        bool x = new SellerModel().EditDetails(seller.SellerID, model);
                        if (x)
                            return Ok("Details Edited Successfully");
                        else
                            return Ok("Something went wrong please try again later");
                    }
                    else
                        return Unauthorized();
                }
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/Seller/Change/Password/{sid}")]
        public IHttpActionResult ChangePassword(ChangePasswordModel model, string token)
        {
            try
            {
                using (ShoppingELFEntities context = new ShoppingELFEntities())
                {
                    SellerTable seller = new SellerTable();
                    string username = TokenManager.ValidateToken(token);
                    seller = context.SellerTable.FirstOrDefault(m => m.email == username);

                    if (seller != null && seller.Role == "Seller")
                    {
                        int x = new SellerModel().ChangePassword(seller.SellerID, model);
                        if (x == 1)
                            return Ok("Please enter correct old password");
                        else if (x == 4)
                            return Ok("new password cannot be equal to old password");
                        else if (x == 2)
                            return Ok("Password Updated successfully");
                        else
                            return BadRequest("Something went wrong");
                    }
                    else
                        return Unauthorized();
                }
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/Seller/Show/OrderPlaced")]
        public IHttpActionResult ShowOrderedItems(string token)
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
                        var x = new SellerModel().ShowOrderedItems(seller.SellerID);
                        return Ok(x);
                    }
                    else
                        return Unauthorized();
                }
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [NonAction]
        public void EmailVerification(int UserID, string Email, string OTP, string EmailFor = "Account")
        {
            //var verifyUrl = "/api/" + EmailFor + "/" + ActivationCode;
            //var link = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, verifyUrl);
            //var link = "http://localhost:54039/api/" + EmailFor + "/" + ActivationCode;
            try
            {
                var FromEmail = new MailAddress("4as1827000224@gmail.com", "ShoppingELF");
                var ToEmail = new MailAddress(Email);
                var FromEmailPassword = "*************";
                string Subject = "";
                string Body = "";
                if (EmailFor == "Account")
                {
                    Subject = "Verification for ShoppingELF Account";
                    Body = "<br/>Never Share your OTP with others <br/>Enter this OTP " + OTP + " to verify your account if it was not you then ignore this message <br/>Team ShoppingELF <br/> Thank you!";

                }
                //else if (EmailFor == "ResetPassword")
                //{
                //    Subject = "Reset Password";
                //    Body = "Hi,<br/><br/>Forgot your password , Don't worry click on the link below to reset your password<br/><br/><a href= '" + link + "'>" + link + "<a/>";
                //}

                SmtpClient smtp = new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(FromEmail.Address, FromEmailPassword)
                };

                using (var message = new MailMessage(FromEmail, ToEmail)
                {
                    Subject = Subject,
                    Body = Body,
                    IsBodyHtml = true
                })

                    smtp.Send(message);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
