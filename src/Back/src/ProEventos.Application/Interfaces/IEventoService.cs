using ProEventos.Application.Dtos;

namespace ProEventos.Application.Interfaces;

public interface IEventoService
{
    Task<EventoDto> AddEventos(int userId, EventoDto model);
    Task<EventoDto> UpdateEvento(int userId, int id, EventoDto model);
    Task<bool> DeleteEvento(int userId, int id);

    Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes = false);
    Task<EventoDto[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false);
    Task<EventoDto> GetEventoByIdAsync(int userId, int Eventoid, bool includePalestrantes = false);


}
