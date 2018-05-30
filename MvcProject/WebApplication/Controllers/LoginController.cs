﻿using ModelsLibrary.DtO_Models;
using ModelsLibrary.Repositories;
using ModelsLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [RoutePrefix("Log")]
    public class LoginController : Controller
    {
        // GET: SignUp
        [Route("Signup")]
        public ActionResult Signup()
        {
            return View();
        }

        [Route("Signup")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(SignupModel model)
        {
            var Service = new CustomerService();
            var passwordSaltService = new PasswordSaltService();
            try
            {
                if (!passwordSaltService.Validate(model.Password))
                {
                    ViewBag.Msg = "密碼不符合規範";
                    return View();
                }
                if (model.Password != model.Password2)
                {
                    ViewBag.Msg = "密碼與確認密碼不符";
                    return View();
                }
                if (Service.GetAccount(model.Account) != null)
                {
                    ViewBag.Msg = "帳號名稱以存在";
                    return View();
                }


                var model2 = new Customer()
                {
                    CustomerName = model.Name,
                    Birthday = model.Birthday,
                    Password = model.Password,
                    Account = model.Account,
                    Phone = model.Phone,
                    Email = model.Email,
                };
                Service.Create(model2);
                return View();
            }
            catch
            {
                ViewBag.Msg = "不可為空白";
                return View();
            }
        }

        [Route("Login")]
        public ActionResult Login()
        {
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (cookie == null)
            {
                ViewBag.IsAuthenticated = false;
                return View();
            }

            var ticket = FormsAuthentication.Decrypt(cookie.Value);

            if (ticket.UserData == "abcdefg")
            {
                ViewBag.IsAuthenticated = true;
                ViewBag.User = ticket.Name;
            }
            else
            {
                ViewBag.IsAuthenticated = false;
            }

            return View();
        }

        [Route("Login")]
        [HttpPost]
        public ActionResult Login(loginModel model)
        {
            var service = new CustomerService();
            var passwordSaltService = new PasswordSaltService();
            var customer_list = service.GetAll().ToList();

            if (customer_list.Any((x) => x.Account == model.User) == false)
            {
                return RedirectToAction("Login");
            };

            if (passwordSaltService.PasswordsCheck(service.FindByCustomerAccount(model.User), model.Password))
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, model.User, DateTime.Now, DateTime.Now.AddMinutes(30), false, "abcdefg");

                var ticketData = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticketData);
                cookie.Expires = ticket.Expiration; //設定Cookie到期日與憑證同時

                Response.Cookies.Add(cookie);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("loginModel", "Error");
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            cookie.Expires = DateTime.Now;
            Response.Cookies.Add(cookie);

            return RedirectToAction("Login", "Login");
        }
    }
}