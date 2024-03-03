using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;

namespace ProEventos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoteController : ControllerBase
{
    public ILoteService _loteService { get; }

    public LoteController(ILoteService loteService)
    {
        _loteService = loteService;
    }


    [HttpGet("{eventoId}")]
    public async Task<IActionResult> Get(int eventoId)
    {
        try
        {
            var lotes = await _loteService.GetLotesByEventoIdAsync(eventoId);
            if (lotes == null) return NoContent();
            return Ok(lotes);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
             $"Erro ao tentar recuperar lotes. Erro:  {ex.Message}");
        }
    }

    [HttpPut("{eventoId}")]
    public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
    {
        try
        {

            var lotes = await _loteService.SaveLote(eventoId, models);
            if (lotes == null) return NoContent();
            return Ok(lotes); 
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
             $"Erro ao tentar atualizar lote. Erro:  {ex.Message}");
        }
    }

    [HttpDelete("{eventoId}/{loteId}")]
    public async Task<IActionResult> Delete(int eventoId, int loteId)
    {
        try
        {
             var lote = await _loteService.GetLoteByIdsAsync(eventoId, loteId);
            if (lote == null) return NoContent();

            if (await _loteService.DeleteLote(lote.EventoId, lote.Id))
                return Ok(new {message = "Lote Deletado"} );
            else
                throw new Exception("Erro ao tentar deletar lote.");
            
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
             $"Erro ao tentar deletar lote. Erro:  {ex.Message}");
        }
    }



}
