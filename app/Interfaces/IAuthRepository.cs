using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Dtos.User;

namespace app.Interfaces
{
    public interface IAuthRepository
    {
        Task<UserResponseDto?> GetByIdAsync(Guid id);
        Task<UserResponseDto?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<UserResponseDto> RegisterAsync(RegisterDto dto, string? role = "Customer");
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}