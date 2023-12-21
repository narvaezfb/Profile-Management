using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile_Management.Data;
using Profile_Management.Models;
using Profile_Management.Models.Requests;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Profile_Management.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly ProfileManagementDbContext _context;

        public UserController(ProfileManagementDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Signup([FromBody] CreateUser model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid body");
            }

            if (!model.ConfirmPasswords(model.Password, model.PasswordConfirm))
            {
                return BadRequest("Passwords do not match");
            }

            User user = new User(model.Username, model.FirstName, model.LastName, model.Email, model.Country, model.Gender, model.Language);

            //_context.Users.Add(user);
            //await _context.SaveChangesAsync();

            //return Ok("User account created successfully");

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    // Call a method to update authentication service
                    bool authUpdateSuccess = await user.UpdateAuthenticationServiceAsync(model.Email, model.Password);

                    if (!authUpdateSuccess)
                    {
                        // If authentication service update fails, roll back the transaction
                        transaction.Rollback();
                        return StatusCode(500, "Failed to update authentication service");
                    }

                    // Commit the transaction if everything succeeds
                    transaction.Commit();

                    return Ok("User account created successfully");
                }
                catch (Exception ex)
                {
                    // Handle exceptions or specific error cases if needed
                    transaction.Rollback();
                    return StatusCode(500, "An error occurred: " + ex.Message);
                }
            }
        }
    }
}

