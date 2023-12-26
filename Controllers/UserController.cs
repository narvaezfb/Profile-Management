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
    public class UserController : ControllerBase
    {
        private readonly ProfileManagementDbContext _context;
        private readonly IDeleteAccountService _deleteAccountService;

        public UserController(ProfileManagementDbContext context, IDeleteAccountService deleteAccountService)
        {
            _context = context;
            _deleteAccountService = deleteAccountService;
        }

        [HttpDelete("Account/Delete/{email}")]
        public async Task<ActionResult> DeleteAccount(string email)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                    if (user == null)
                    {
                        return BadRequest("User not found with that ID");
                    }

                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();


                    // Call a method to update authentication service
                    bool isAccountDeletedSuccess = await _deleteAccountService.DeleteAccountInAuthenticationServiceAsync(email);

                    if (!isAccountDeletedSuccess)
                    {
                        transaction.Rollback();
                        return StatusCode(500, "Failed to delete account in authentication service");
                    }

                    transaction.Commit();

                    return Ok("Account deleted");

                }
                catch (DbUpdateException dbUpdateException)
                {
                    transaction.Rollback();
                    return BadRequest("Database error occurred: " + dbUpdateException.InnerException?.Message);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return StatusCode(500, "An error occurred while processing the request: " + e.Message);
                }
            }    
        }
    }
}

