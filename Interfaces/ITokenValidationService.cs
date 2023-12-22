using System;
namespace Profile_Management.Interfaces
{
	public interface ITokenValidationService
	{
        Task<bool> ValidateTokenAsync(string token);
    }
}

