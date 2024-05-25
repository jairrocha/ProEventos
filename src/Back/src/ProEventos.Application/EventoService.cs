using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Application;
public class EventoService : IEventoService
{
    private readonly IEventoPersistence _eventoPersistence;
    private readonly IGeralPersistence _geralPersistence;
    private readonly IMapper _mapper;

    public EventoService(IEventoPersistence eventoPersistence, 
                         IGeralPersistence geralPersistence, IMapper mapper)
    {
        _mapper = mapper;
        _geralPersistence = geralPersistence;
        _eventoPersistence = eventoPersistence;
    }

    public async Task<EventoDto> AddEventos(int userId, EventoDto model)
    {
        try
        {
            var evento = _mapper.Map<Evento>(model);
            evento.UserId =  userId;

            _geralPersistence.Add(evento);

            if (await _geralPersistence.SaveChangesAsync())
            {
               return _mapper.Map<EventoDto>(_eventoPersistence.GetEventoByIdAsync(userId, evento.Id, false).Result);
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return null;
    }

    public async Task<EventoDto> UpdateEvento(int userId, int id,EventoDto model)
    {
        try
        {
            var evento = await _eventoPersistence.GetEventoByIdAsync(userId, id);
            if (evento == null) throw new Exception($"Evento para delete não foi encontrado.");

            model.Id = evento.Id;
            model.UserId = userId; 
            
            _mapper.Map(model, evento);
            
            _geralPersistence.Update<Evento>(evento);

            if (await _geralPersistence.SaveChangesAsync())
            {
                var eventoRetono = await _eventoPersistence.GetEventoByIdAsync(userId, evento.Id, false);
                return _mapper.Map<EventoDto>(eventoRetono);

            }

            return null;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> DeleteEvento(int userId, int id)
    {
        try
        {
            var evento = await _eventoPersistence.GetEventoByIdAsync(userId, id);
            if (evento == null) return false;

            _geralPersistence.Delete(evento);

            return await _geralPersistence.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
    {
        try
        {
            var eventos = await _eventoPersistence.GetAllEventosAsync(userId, includePalestrantes);
            if (eventos == null) return null;

            return _mapper.Map<EventoDto[]>(eventos);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
    {
        try
        {
            var eventos = await _eventoPersistence.GetAllEventosByTemaAsync(userId, tema, includePalestrantes);
            if (eventos == null) return null;

            return _mapper.Map<EventoDto[]>(eventos);
          

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
    {
        try
        {
            var evento = await _eventoPersistence.GetEventoByIdAsync(userId, eventoId, includePalestrantes);
            if (evento == null) return null;
            
            return _mapper.Map<EventoDto>(evento);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}
