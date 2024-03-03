using ProEventos.Domain;

namespace ProEventos.Persistence.Interfaces;

public interface IPalestrantePersistence
{
    Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos= false);
    Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string tema, bool includeEventos= false);
    Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool includeEventos= false);
}
