using CloneControllerAccount.Dtos;
using Microsoft.AspNetCore.Identity;

namespace CloneControllerAccount.Services
{
    public interface IAccountServices
    {
        Task<IdentityResult> RegisterUsersAsync(RegisterDto registerDto);
        Task<string> LoginUsersAsync(LoginDto loginDto);
    }
}
