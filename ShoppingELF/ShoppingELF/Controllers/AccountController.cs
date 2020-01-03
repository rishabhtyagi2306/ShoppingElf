using ShoppingELF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Security;

namespace ShoppingELF.Controllers
{
    //[EnableCors(origins: "http://16f4c92f.ngrok.io", headers: "*", methods: "*")]
    //[EnableCors("https://9f906e85.ngrok.io", "*","*")]
    public class AccountController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage PostSignup([FromBody]UserTable user)
        {
            try
            {
                var x = new UserModel().IsEmailExist(user.email);
                if (x)
                {
                    return Request.CreateResponse(HttpStatusCode.Forbidden, "Account already exist");
                }
                else
                {
                    new UserModel().AddUser(user);
                    EmailVerification(user.UserID, user.email, Convert.ToString(user.ActivationCode));
                    return Request.CreateResponse(HttpStatusCode.Created, "A verification link has been sent to your email , Please Verify it to continue access");
                }


            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("api/Account/UserLogin")]
        public HttpResponseMessage Login([FromBody]UserTable user)
        {
            try
            {
                UserTable u = new UserRepository().GetUser(user.email);
                if (u == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                         "The Account was not found.");
                var password = new UserModel().Password(user.email);
                var y = new UserModel().verification(user.email);
                string pass = Crypto.Hash(user.password);
                bool credentials = pass.Equals(password);
                if (credentials && y)
                    return Request.CreateResponse(HttpStatusCode.OK, TokenManager.GenerateToken(user.email));
                else
                    return Request.CreateResponse(HttpStatusCode.Forbidden, "The email/password combination was wrong.");
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage Validate(string token, string username)
        {
            bool exists = new UserRepository().GetUser(username) != null;
            if (!exists) return Request.CreateResponse(HttpStatusCode.NotFound,
                 "The Account was not found.");
            string x = TokenManager.ValidateToken(token);
            if (x == username)
                return Request.CreateResponse(HttpStatusCode.OK);
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [Route("api/Account/{id}")]
        [HttpGet]
        public HttpResponseMessage VerifyAccount([FromUri]string id)
        {
            //bool status = false;
            try
            {
                using (ShoppingELFEntities context = new ShoppingELFEntities())
                {

                    UserTable us = new UserTable();
                    var v = context.UserTable.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                    if (v != null)
                    {
                        us.IsEmailVerified = true;
                        v.IsEmailVerified = Convert.ToBoolean(us.IsEmailVerified);
                        context.SaveChanges();
                        //status = true;
                        return Request.CreateResponse(HttpStatusCode.OK, "Your account has been successfully verified, Please go back on the login page to continue shopping with us<br/>Thank You!!!<br/>Team ShoppingELF");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Unable to activate account");
                    }

                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("api/Account/ForgotPassword")]
        public IHttpActionResult ForgotPassword(ChangePasswordModel model)
        {
            
            try
            {
                using (ShoppingELFEntities context = new ShoppingELFEntities())
                {
                    var acc = context.UserTable.Where(x => x.email == model.EmailID).FirstOrDefault();
                    if (acc != null)
                    {
                        string ResetCode = Guid.NewGuid().ToString();
                        EmailVerification(acc.UserID, acc.email, ResetCode, "ResetPassword");
                        acc.ResetPasswordCode = ResetCode;
                        context.Configuration.ValidateOnSaveEnabled = false;
                        context.SaveChanges();
                        return Ok("Reset password code has been sent to your email");
                    }
                    else
                    {
                        return Ok("Account Not found");
                    }
                }
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/ResetPassword/{ID}")]
        public IHttpActionResult ResetPassword(string ID)
        {
            try
            {
                using (ShoppingELFEntities context = new ShoppingELFEntities())
                {
                    var user = context.UserTable.Where(x => x.ResetPasswordCode == ID).FirstOrDefault();
                    if (user != null)
                    {
                        ChangePasswordModel model = new ChangePasswordModel();
                        user.IsResetPassword = true;

                        context.SaveChanges();
                        return Ok("Account Verified");
                    }
                    else
                    {
                        return Ok("Something Went Wrong");
                    }
                }
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("api/Account/ResetPassword")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult ResetPassword(ChangePasswordModel model)
        {
            try
            {
                using (ShoppingELFEntities context = new ShoppingELFEntities())
                {
                    var user = context.UserTable.Where(x => x.email == model.EmailID).FirstOrDefault();
                    if (user != null && Convert.ToBoolean(user.IsResetPassword))
                    {
                        user.password = Crypto.Hash(model.NewPassword);
                        user.ResetPasswordCode = "";
                        user.IsResetPassword = false;
                        context.Configuration.ValidateOnSaveEnabled = false;
                        context.SaveChanges();
                        return Ok("New Password updated successfully");
                    }
                    else
                    {
                        return Ok("unable to reach account please try again");
                    }
                }
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [NonAction]
        public void EmailVerification(int UserID, string Email, string ActivationCode, string EmailFor = "Account")
        {
            var verifyUrl = "/api/" + EmailFor + "/" + ActivationCode;
            //var link = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, verifyUrl);
            var link = "https://b6fc7ab9.ngrok.io/api/" + EmailFor + "/" + ActivationCode;
            var FromEmail = new MailAddress("4as1827000224@gmail.com", "ShoppingELF");
            var ToEmail = new MailAddress(Email);
            var FromEmailPassword = "Rishabh@2306";
            string Subject = "";
            string Body = "";
            if (EmailFor == "Account")
            {
                Subject = "Email Verification for ShoppingELF Account";
                Body = "<br/>Please click on the link below to verify your account" +
                    "<br/><br/><a href = '" + link + "'>" + link + "<a/>";
            }
            else if (EmailFor == "ResetPassword")
            {
                Subject = "Reset Password";
                Body = "Hi,<br/><br/>Forgot your password , Don't worry click on the link below to reset your password<br/><br/><a href= '" + link + "'>" + link + "<a/>";
            }

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
    }
}
