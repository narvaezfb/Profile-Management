using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Profile_Management.Data;
using Profile_Management.Interfaces;
using Profile_Management.Models;
using Profile_Management.Models.Requests;
using Profile_Management.Services;

namespace Profile_Management.Controllers
{
    [Route("[controller]")]
    public class AuthInteractionController : ControllerBase
    {
        private readonly ProfileManagementDbContext _context;
        private readonly ISignupService _signupService;

        public AuthInteractionController(ProfileManagementDbContext context, ISignupService signupService)
        {
            _context = context;
            _signupService = signupService;
        }

        [AllowAnonymous]
        [HttpPost("Registration")]
        public async Task<ActionResult> CreateAccount([FromBody] CreateUser model)
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

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    // Call a method to update authentication service
                    bool authUpdateSuccess = await _signupService.CreateAccountInAuthenticationServiceAsync(model.Email, model.Password);

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

