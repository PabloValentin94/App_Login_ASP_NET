using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using App_Login_ASP_NET.Data;
using App_Login_ASP_NET.Models;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;

namespace App_Login_ASP_NET.Controllers
{

    public class UserController : Controller
    {

        private readonly MongoDBContext _context;

        private readonly UserManager<AppUser> _app_users_manager;

        public UserController(UserManager<AppUser> app_users_manager)
        {

            this._context = new MongoDBContext();

            this._app_users_manager = app_users_manager;

        }

        // GET: User
        public async Task<IActionResult> Index()
        {

            List<User> users = new List<User>();

            foreach (AppUser app_user in (await _context.Users.Find(u => u.Id != null).ToListAsync()))
            {

                users.Add(this.Generate_Equivalent_Object(app_user));

            }

            return View(users);

        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var user = await _context.Users.Find(u => u.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {

                return NotFound();

            }

            return View(this.Generate_Equivalent_Object(user));

        }

        // GET: User/Create
        public IActionResult Create(string role)
        {

            ViewBag.Role = role;

            return View();

        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone,Password")] User user, string Role)
        {

            if (ModelState.IsValid)
            {

                AppUser app_user = new AppUser();

                app_user.Id = Guid.NewGuid();

                app_user.UserName = Regex.Replace(Models.User.Remove_Accents(user.Name), @"[^a-zA-Z0-9]", "");

                app_user.Email = user.Email;

                app_user.PhoneNumber = user.Phone;

                // Aprofundamento: https://gist.github.com/PabloValentin94/67343c258863eb1d157b881bf5adb074

                app_user.Name = user.Name;

                IdentityResult user_sign_up_result = await this._app_users_manager.CreateAsync(app_user, user.Password);

                if (user_sign_up_result.Succeeded)
                {

                    try
                    {

                        IdentityResult role_attribution_result = await this._app_users_manager.AddToRoleAsync(app_user, Role);

                        return RedirectToAction("Index", "Home");

                    }

                    catch (Exception)
                    {

                        /*

                            Caso ocorra um erro relacionado a atribuição do cargo especificado 
                            (Como ele não existir, por exemplo.), o usuário que foi cadastrado 
                            será excluído, evitando inconsistência de dados.

                        */

                        await this._context.Users.DeleteOneAsync(u => u.Id == app_user.Id);

                        ModelState.AddModelError("", "The specified role does not exists.");

                    }

                }

                else
                {

                    foreach (IdentityError error in user_sign_up_result.Errors)
                    {

                        ModelState.AddModelError("", error.Description);

                    }

                }

            }

            return View(user);

        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var app_user = await this._context.Users.Find(u => u.Id == id).FirstOrDefaultAsync();

            if (app_user == null)
            {

                return NotFound();

            }

            User user = this.Generate_Equivalent_Object(app_user);

            user.Password = app_user.PasswordHash;

            return View(user);

        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Email,Phone,Password")] User user)
        {

            if (id != user.Id)
            {

                return NotFound();

            }

            if (ModelState.IsValid)
            {

                try
                {

                    var app_user = await this._context.Users.Find(u => u.Id == id).FirstOrDefaultAsync();

                    if (app_user == null)
                    {

                        return NotFound();

                    }

                    app_user.UserName = Regex.Replace(Models.User.Remove_Accents(user.Name), @"[^a-zA-Z0-9]", "");

                    app_user.Email = user.Email;

                    app_user.PhoneNumber = user.Phone;

                    // Aprofundamento: https://gist.github.com/PabloValentin94/67343c258863eb1d157b881bf5adb074

                    app_user.Name = user.Name;

                    IdentityResult user_edit_result = await this._app_users_manager.UpdateAsync(app_user);

                    if (user_edit_result.Succeeded)
                    {

                        return RedirectToAction("Index", "User");

                    }

                    else
                    {

                        foreach (IdentityError error in user_edit_result.Errors)
                        {

                            ModelState.AddModelError("", error.Description);

                        }

                    }

                }

                catch (DbUpdateConcurrencyException)
                {

                    if (!UserExists(user.Id))
                    {

                        return NotFound();

                    }

                    else
                    {

                        throw;

                    }

                }

            }

            return View(user);

        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var user = await _context.Users.Find(u => u.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {

                return NotFound();

            }

            return View(this.Generate_Equivalent_Object(user));

        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            if (this.UserExists(id))
            {

                await this._context.Users.DeleteOneAsync(u => u.Id == id);

            }

            return RedirectToAction("Index", "User");

        }

        private bool UserExists(Guid? id)
        {

            return this._context.Users.Find(u => u.Id == id).Any();

        }

        private User Generate_Equivalent_Object(AppUser app_user)
        {

            User user = new User()
            {

                Id = app_user.Id,

                Name = app_user.Name,

                Email = app_user.Email,

                Phone = app_user.PhoneNumber

            };

            return user;

        }

    }

}