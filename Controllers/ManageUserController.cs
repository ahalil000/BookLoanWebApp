using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookLoan.Data;
using BookLoan.Services;
using BookLoan.Models;


namespace BookLoan.Controllers
{
    public class ManageUserController : Controller
    {
        ApplicationDbContext _db;
        IUserRoleService _userRoleService;

        public ManageUserController(ApplicationDbContext db, IUserRoleService userRoleService)
        {
            _db = db;
            _userRoleService = userRoleService;
        }

        // GET: ManageUsers
        public ActionResult Index()
        {
            return View();
        }

        // GET: ManageUsers/Users
        public async Task<ActionResult> UserList()
        {
            return View(await _userRoleService.GetUsers());
        }


        // GET: ManageUsers/UserRoleEdit/5
        public async Task<ActionResult> UserRoleEdit(string id)
        {
            var userrole = await _userRoleService.GetUserRoleDetailsById(id);
            return View(userrole);
        }


        /// <summary>
        /// AddRole()
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<ActionResult> AddRole(string id, string role)
        {
            var userrole = await _userRoleService.GetUserRoleAction(id, "Add", role);
            return View(userrole);
        }


        /// <summary>
        /// DeleteRole()
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteRole(string id, string role)
        {
            var userrole = await _userRoleService.GetUserRoleAction(id, "Delete", role);
            return View(userrole);
        }



        // POST: ManageUsers/AddRole/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRole(Models.ManageUserViewModels.UserRoleConfirmAction model)
        {
            try
            {
                // TODO: Add update logic here
                await _userRoleService.AddUserToRole(model.LoginName, model.SelectedRole);
                //return RedirectToAction("Index", "Home");
                return RedirectToAction("UserRoleEdit", new { id = model.UserID });
            }
            catch
            {
                return View();
            }
        }


        // POST: ManageUsers/DeleteRole/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRole(Models.ManageUserViewModels.UserRoleConfirmAction model)
        {
            try
            {
                // TODO: Add update logic here
                await _userRoleService.AddUserToRole(model.LoginName, model.SelectedRole);
                return RedirectToAction("UserRoleEdit", new { id = model.UserID });
            }
            catch
            {
                return View();
            }
        }



        // GET: ManageUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageUsers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ManageUsers/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManageUsers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ManageUsers/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManageUsers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}