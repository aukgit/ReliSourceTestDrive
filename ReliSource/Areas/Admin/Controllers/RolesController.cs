﻿using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ReliSource.Models.Context;
using ReliSource.Models.POCO.Identity;
using ReliSource.Modules.Role;

namespace ReliSource.Areas.Admin.Controllers {
    public class RolesController : Controller {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index() {
            var roles = RoleManager.GetRoles();
            var appname = AppVar.Setting.ApplicationDescription;
            return View(roles);
        }

        public ActionResult Create() {
            var roles = RoleManager.GetRoles();
            if (roles != null && roles.Count > 0) {
                ViewBag.Max = roles.Max(n => n.PriorityLevel) + 1;
            } else {
                ViewBag.Max = 0;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(ApplicationRole role) {
            if (ModelState.IsValid) {
                _db.Entry(role).State = EntityState.Added;
                if (_db.SaveChanges() > -1) {
                    RoleManager.ResetManager();
                    return RedirectToAction("Index");
                }
            }

            var error = AppConfig.GetNewErrorCollector();
            error.Add("Not a valid form.");
            AppConfig.SetGlobalError(error);
            return View(role);
        }

        public ActionResult Edit(long id) {
            var role = RoleManager.GetRole(id);
            return View(role);
        }

        [HttpPost]
        public ActionResult Edit(ApplicationRole role) {
            if (ModelState.IsValid) {
                _db.Entry(role).State = EntityState.Modified;
                if (_db.SaveChanges() > -1) {
                    RoleManager.ResetManager();

                    return RedirectToAction("Index");
                }
            }

            var error = AppConfig.GetNewErrorCollector();
            error.Add("Not a valid form.");
            AppConfig.SetGlobalError(error);
            return View(role);
        }

        public ActionResult Delete(long id) {
            var relatedUsers = RoleManager.GetUsersInRole(id);
            if (relatedUsers != null && relatedUsers.Count() > 0) {
                ViewBag.RoleID = id;
                RoleManager.ResetManager();

                return View(relatedUsers);
            }
            RoleManager.RemoveRole(id);
            return RedirectToActionPermanent("Index");
            //var users = UserManager.GetEveryUser();
            //return View(users);
        }

        public ActionResult DeleteConfirmed(long id) {
            var relatedUsers = RoleManager.GetUsersInRole(id);

            if (relatedUsers != null) {
                var role = RoleManager.GetRole(id);
                foreach (var user in relatedUsers) {
                    //RoleManager.(id);
                    RoleManager.RemoveUserRole(user, role.Name);
                }
                RoleManager.ResetManager();

                RoleManager.RemoveRole(id);
                RoleManager.ResetManager();
            }
            return RedirectToActionPermanent("Index");
        }
    }
}