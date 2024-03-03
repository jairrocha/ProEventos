using ProEventos.Domain;

namespace ProEventos.Persistence.Interfaces;

public interface ILotePersistence
{

    /// <summary>
    /// Método get que retornará uma lista de lote por eventoId
    /// </summary>
    /// <param name="eventoId">Código chave da tabela Evento</param>
    /// <returns>Retorna uma array de objetos do tipo lote</returns>
    Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);

    /// <summary>
    /// Método get que retornará apenas 1 lote
    /// </summary>
    /// <param name="eventoid"> Código chave da tabela Evento</param>
    /// <param name="loteId">Código chave da tabela lote</param>
    /// <returns>Retorna objeto do tipo lote</returns>
     Task<Lote> GetLoteByIdsAsync(int eventoid, int loteId);
}
