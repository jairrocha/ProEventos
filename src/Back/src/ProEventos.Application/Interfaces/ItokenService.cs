using ProEventos.Application.Dtos;

namespace ProEventos.Application.Interfaces
{
    public interface ItokenService
    {
        Task<string> CreateToken(UserUpdateDto userUpdateDto);
    }
}