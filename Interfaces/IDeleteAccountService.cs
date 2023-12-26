using System;
namespace Profile_Management.Interfaces
{
	public interface IDeleteAccountService
	{
		public Task<bool> DeleteAccountInAuthenticationServiceAsync(string email);

    }
}

