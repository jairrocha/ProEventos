using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Persistence;

public class EventoPersistence:IEventoPersistence
{

    private readonly ProEventosContext _context;

    public EventoPersistence(ProEventosContext context)
    {
        _context = context;
        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public async Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
    {


        IQueryable<Evento> query = _context.Eventos.Where(e => e.UserId == userId)
                                        .Include(e => e.Lotes).Include(e => e.RedeSocials);

        if (includePalestrantes) query.Include(e => e.PalestranteEventos).ThenInclude(pe => pe.Palestrante);

        query = query.OrderBy(e => e.Id);


        return await query.ToArrayAsync();
    }

    public async Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
    {

        IQueryable<Evento> query = _context.Eventos
            .Where(e => e.Tema.ToLower().Contains(tema.ToLower()) && e.UserId == userId)
            .Include(e => e.Lotes).Include(e => e.RedeSocials);

        if (includePalestrantes) query.Include(e => e.PalestranteEventos).ThenInclude(pe => pe.Palestrante);

        query = query.OrderBy(e => e.Id);

        return await query.ToArrayAsync();

    }


    public async Task<Evento> GetEventoByIdAsync(int userId, int Eventoid, bool includePalestrantes = false)
    {
        IQueryable<Evento> query = _context.Eventos.Where(e => e.Id.Equals(Eventoid) && e.UserId == userId )
            .Include(e => e.Lotes).Include(e => e.RedeSocials);

        if (includePalestrantes) query.Include(e => e.PalestranteEventos).ThenInclude(pe => pe.Palestrante);

        query = query.OrderBy(e => e.Id);

        return await query.FirstOrDefaultAsync();
    }

  
}
