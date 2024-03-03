using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Persistence;

public class LotePersistence:ILotePersistence
{

    private readonly ProEventosContext _context;

    public LotePersistence(ProEventosContext context)
    {
        _context = context;
    }

    public async Task<Lote> GetLoteByIdsAsync(int eventoid, int loteId)
    {
        IQueryable<Lote> query = _context.Lotes;

        query = query.AsNoTracking()
            .Where(l => l.EventoId.Equals(eventoid)
                && l.Id.Equals(loteId));

        return await query.FirstOrDefaultAsync();       
    }

    public async Task<Lote[]> GetLotesByEventoIdAsync(int eventoId)
    {
         IQueryable<Lote> query = _context.Lotes;

        query = query.AsNoTracking()
            .Where(l => l.EventoId.Equals(eventoId));

        return await query.ToArrayAsync();       
    }
}
