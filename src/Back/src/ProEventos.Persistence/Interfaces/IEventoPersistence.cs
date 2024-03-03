using ProEventos.Domain;

namespace ProEventos.Persistence.Interfaces;

public interface IEventoPersistence
{
    Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false);
    Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
    Task<Evento> GetEventoByIdAsync(int eventoid, bool includePalestrantes = false);
}
