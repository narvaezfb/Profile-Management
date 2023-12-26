using System;
namespace Profile_Management.Interfaces
{
	public interface ISignupService
	{
		public Task<bool> CreateAccountInAuthenticationServiceAsync(string email, string password);

    }
}

