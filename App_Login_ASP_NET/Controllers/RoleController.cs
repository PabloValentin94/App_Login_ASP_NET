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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace App_Login_ASP_NET.Controllers
{

    /*
     
        É necessário o usuário estar autenticado e ser um administrador para usar 
        os recursos desta classe.

    */

    [Authorize(Roles = "Administrador")]
    public class RoleController : Controller
    {

        private readonly MongoDBContext _context;

        private readonly RoleManager<AppRole> _app_roles_manager;

        public RoleController(RoleManager<AppRole> app_roles_manager)
        {

            this._context = new MongoDBContext();

            this._app_roles_manager = app_roles_manager;

        }

        // GET: Role
        public async Task<IActionResult> Index()
        {

            List<Role> roles = new List<Role>();

            foreach (AppRole app_role in (await this._context.Roles.Find(r => r.Id != null).ToListAsync()))
            {

                roles.Add(this.Generate_Equivalent_Object(app_role));

            }

            return View(roles);

        }

        // GET: Role/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var role = await _context.Roles.Find(r => r.Id == id).FirstOrDefaultAsync();

            if (role == null)
            {

                return NotFound();

            }

            return View(this.Generate_Equivalent_Object(role));

        }

        // GET: Role/Create
        public IActionResult Create()
        {

            return View();

        }

        // POST: Role/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Role role)
        {

            if (ModelState.IsValid)
            {

                AppRole app_role = new AppRole();

                app_role.Id = Guid.NewGuid();

                app_role.Name = role.Name;

                IdentityResult role_register_result = await this._app_roles_manager.CreateAsync(app_role);

                if (role_register_result.Succeeded)
                {

                    return RedirectToAction("Index", "Role");

                }

                else
                {

                    foreach (IdentityError error in role_register_result.Errors)
                    {

                        ModelState.AddModelError("", error.Description);

                    }

                }

            }

            return View(role);

        }

        // GET: Role/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var role = await _context.Roles.Find(r => r.Id == id).FirstOrDefaultAsync();

            if (role == null)
            {

                return NotFound();

            }

            return View(this.Generate_Equivalent_Object(role));

        }

        // POST: Role/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] Role role)
        {

            if (id != role.Id)
            {

                return NotFound();

            }

            if (ModelState.IsValid)
            {

                try
                {

                    var app_role = await this._context.Roles.Find(r => r.Id == id).FirstOrDefaultAsync();

                    if (app_role == null)
                    {

                        return NotFound();

                    }

                    app_role.Name = role.Name;

                    IdentityResult role_edit_result = await this._app_roles_manager.UpdateAsync(app_role);

                    if (role_edit_result.Succeeded)
                    {

                        return RedirectToAction("Index", "Role");

                    }

                    else
                    {

                        foreach (IdentityError error in role_edit_result.Errors)
                        {

                            ModelState.AddModelError("", error.Description);

                        }

                    }

                }

                catch (DbUpdateConcurrencyException)
                {

                    if (!RoleExists(role.Id))
                    {

                        return NotFound();

                    }

                    else
                    {

                        throw;

                    }

                }

                return RedirectToAction("Index", "Role");

            }

            return View(role);

        }

        // GET: Role/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {

            if (id == null)
            {

                return NotFound();

            }

            var role = await _context.Roles.Find(r => r.Id == id).FirstOrDefaultAsync();

            if (role == null)
            {

                return NotFound();

            }

            return View(this.Generate_Equivalent_Object(role));

        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            if (this.RoleExists(id))
            {

                await this._context.Roles.DeleteOneAsync(r => r.Id == id);

            }

            return RedirectToAction("Index", "Role");

        }

        private bool RoleExists(Guid id)
        {

            return this._context.Roles.Find(r => r.Id == id).Any();

        }

        private Role Generate_Equivalent_Object(AppRole app_role)
        {

            Role role = new Role()
            {

                Id = app_role.Id,

                Name = app_role.Name

            };

            return role;

        }

    }

}