using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Persistence;

public class PalestrantePersistence:IPalestrantePersistence
{

    private readonly ProEventosContext _context;

    public PalestrantePersistence(ProEventosContext context)
    {
        _context = context;
    }

    public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
    {
        IQueryable<Palestrante> query = _context.Palestrantes.Include(p => p.RedeSociais);

        if (includeEventos) query.Include(e => e.PalestranteEventos).ThenInclude(pe => pe.Evento);

        query = query.OrderBy(e => e.Id);

        return await query.ToArrayAsync();
    }

    public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false)
    {
        IQueryable<Palestrante> query = _context.Palestrantes
            .Where(p => p.User.PrimeiroNome.ToLower().Contains(nome.ToLower())).Include(p => p.RedeSociais);

        if (includeEventos) query.Include(e => e.PalestranteEventos).ThenInclude(pe => pe.Evento);

        query = query.OrderBy(e => e.Id);

        return await query.ToArrayAsync();
    }

    public async Task<Palestrante> GetPalestranteByIdAsync(int PalestranteId, bool includeEventos = false)
    {
        IQueryable<Palestrante> query = _context.Palestrantes
            .Where(p => p.Id.Equals(PalestranteId)).Include(p => p.RedeSociais);

        if (includeEventos) query.Include(e => e.PalestranteEventos).ThenInclude(pe => pe.Evento);

        query = query.OrderBy(e => e.Id);

        return await query.FirstOrDefaultAsync();
    }

}
