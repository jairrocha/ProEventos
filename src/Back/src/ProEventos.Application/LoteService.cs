using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Application;
public class LoteService : ILoteService
{
    private readonly ILotePersistence _lotePersistence;
    private readonly IGeralPersistence _geralPersistence;
    private readonly IMapper _mapper;

    public LoteService(ILotePersistence lotePersistence,
                         IGeralPersistence geralPersistence, IMapper mapper)
    {
        _mapper = mapper;
        _geralPersistence = geralPersistence;
        _lotePersistence = lotePersistence;
    }


    private async Task AddLote(int eventoId, LoteDto model)
    {
        try
        {
            var lote = _mapper.Map<Lote>(model);
            lote.EventoId = eventoId;

            _geralPersistence.Add(lote);

            await _geralPersistence.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }


    }

    private async Task UpdateLote(int eventoId, LoteDto model)
    {
        try
        {

            if (model.EventoId == eventoId)
            {
                var lote = await _lotePersistence.GetLoteByIdsAsync(eventoId, model.Id);
                _mapper.Map(model, lote);
                _geralPersistence.Update<Lote>(lote);
                await _geralPersistence.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<LoteDto[]> SaveLote(int eventoId, LoteDto[] models)
    {
        try
        {

            foreach (var model in models)
            {
                if (model.Id == 0)
                {
                    await AddLote(eventoId, model);
                }
                else
                {

                    await UpdateLote(eventoId, model);
                }
            }

            var loteRetono = await _lotePersistence.GetLotesByEventoIdAsync(eventoId);
            return _mapper.Map<LoteDto[]>(loteRetono);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> DeleteLote(int eventoId, int loteId)
    {
        try
        {
            var lote = await _lotePersistence.GetLoteByIdsAsync(eventoId, loteId);
            if (lote == null) throw new Exception("Lote para exclusão não encontrado.");

            _geralPersistence.Delete(lote);

            return await _geralPersistence.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<LoteDto[]> GetLotesByEventoIdAsync(int enventoId)
    {
        try
        {
            var lotes = await _lotePersistence.GetLotesByEventoIdAsync(enventoId);
            if (lotes == null) return null;

            return _mapper.Map<LoteDto[]>(lotes);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId)
    {
        try
        {
            var lote = await _lotePersistence.GetLoteByIdsAsync(eventoId, loteId);
            if (lote == null) return null;

            return _mapper.Map<LoteDto>(lote);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


}


