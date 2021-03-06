﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Periodicals.Areas.Admin.Models;
using Periodicals.Infrastructure.Data;
using Periodicals.Infrastructure.Identity;

namespace Periodicals.Areas.Admin.Controllers
{

    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        // GET: Admin/Admin
        public ActionResult Administration()
        {
            List<UserModel> users;
            using (var db = new PeriodicalDbContext())
            {
                var userList = db.Users.ToList();
                var admin = db.Users.First(adm => adm.UserName=="admin");
                userList.Remove(admin);
                users = UserModel.FromUserList(userList);
            }
            return View(users);
        }

        public ActionResult UserPage()
        {
            return View();
        }

        public ActionResult BlockUser(string userId)
        {
            using (var db = new PeriodicalDbContext())
            {
                var user = db.Users.Find(userId);
                user.LockoutEnabled = !user.LockoutEnabled;
                db.SaveChanges();
                if (user.LockoutEnabled) user.LockoutEndDateUtc = DateTime.UtcNow.AddMinutes(5);
            }

                return RedirectToAction("Administration");
        }

        public ActionResult AppointmentModerators(string userId)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(userId);
            if (user.LockoutEnabled) return RedirectToAction("Administration"); 
            //user.LockoutEnabled = !user.LockoutEnabled;
            var addModeratorRoleResult = userManager.AddToRole(userId, "Moderator");
            var removeSubscriberRoleResult = userManager.RemoveFromRole(userId, "Subscriber");

            if (addModeratorRoleResult.Succeeded)
            {

            }
            else
            {
                var removeModeratorRoleResult = userManager.RemoveFromRole(userId, "Moderator");
                var addSubscriberRoleResult = userManager.AddToRole(userId, "Subscriber");
            }
            return RedirectToAction("Administration");
        }

        [HttpPost]
        public ActionResult SearchUser(string search)
        {
            List<UserModel> users;
            using (var db = new PeriodicalDbContext())
            {
                
                var userList = db.Users.Where(user => user.UserName.Contains(search)).ToList();
                var admin = db.Users.First(adm => adm.UserName == "admin");
                if(admin!=null) userList.Remove(admin);
                users = UserModel.FromUserList(userList);
            }
            return PartialView("_SearchUser", users);
        }

    }
}