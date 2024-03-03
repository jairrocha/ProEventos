using ProEventos.Application.Dtos;

namespace ProEventos.Application.Interfaces;

public interface ILoteService
{
    Task<LoteDto[]> SaveLote(int eventoId, LoteDto[] models);
    Task<bool> DeleteLote(int eventoId, int loteId);

    Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId);
    Task<LoteDto> GetLoteByIdsAsync(int eventoid, int loteId);

}
