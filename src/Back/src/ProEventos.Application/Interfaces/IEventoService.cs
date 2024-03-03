using ProEventos.Application.Dtos;

namespace ProEventos.Application.Interfaces;

public interface IEventoService
{
    Task<EventoDto> AddEventos(EventoDto model);
    Task<EventoDto> UpdateEventos(int id, EventoDto model);
    Task<bool> DeleteEvento(int id);

    Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false);
    Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
    Task<EventoDto> GetEventoByIdAsync(int Eventoid, bool includePalestrantes = false);


}
