using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Dtos.User;

namespace app.Interfaces
{
    public interface IUserRepository
    {
        Task<UserResponseDto?> GetByIdAsync(int id);
        Task<UserResponseDto?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<UserResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
        Task<bool> DeleteAsync(int id);
    }
}