﻿using ModelsLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers.Admin
{
    [RoutePrefix("Admin")]
    public class HomeController : Controller
    {
        [Route("Index")]
        // GET: Home
        public ActionResult AdminHome()
        {
            var serviceCus = new CustomerService();
            var serviceOd = new OrderDetailsService();
            var serviceOr = new OrderService();
            var servicePro = new ProductsService();
            ViewBag.CostomerNumber = serviceCus.GetAll().Count();
            ViewBag.OrderNumber = serviceOr.GetAll().Count();
            var ProductNumber = serviceOd.GetAll();
            var Number = 0;
            foreach (var item in ProductNumber)
            {
                Number += item.Quantity;
            }
            ViewBag.ProductNumber = Number;
            var GetAllOrder = serviceOd.GetAll();
            decimal total = 0;
            foreach (var item in GetAllOrder)
            {
                total += servicePro.FindByID(item.ProductID).UnitPrice * item.Quantity;
            }
            ViewBag.Total = Decimal.Round(total);

            return View();  
        }
    }
}