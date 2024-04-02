using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;

namespace ProEventos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{
    public IEventoService _eventoService { get; }
    public IWebHostEnvironment _webHostEnvironment { get; }

    public EventoController(IEventoService eventoService, IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
        _eventoService = eventoService;
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var eventos = await _eventoService.GetAllEventosAsync(true);
            if (eventos == null) return NoContent();
            return Ok(eventos);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
             $"Erro ao tentar recuperar eventos. Erro:  {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(id, true);
            if (evento == null) return NoContent();
            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
             $"Erro ao tentar recuperar o evento. Erro:  {ex.Message}");
        }
    }

    [HttpGet("tema/{tema}")]
    public async Task<IActionResult> GetByTema(string tema)
    {
        try
        {
            var eventos = await _eventoService.GetAllEventosByTemaAsync(tema, true);
            if (eventos == null) return NoContent();
            return Ok(eventos);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
             $"Erro ao tentar recuperar o evento por tema. Erro:  {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(EventoDto model)
    {
        try
        {
            var evento = await _eventoService.AddEventos(model);
            if (evento == null) return BadRequest("Erro ao tentar adicionar evento.");
            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
             $"Erro ao tentar adicionar evento. Erro:  {ex.Message}");
        }
    }
    
    [HttpPost("upload-image/{eventoId}")]
    public async Task<IActionResult> UploadImage(int eventoId)
    {
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(eventoId);
            if (evento == null) return NoContent();

            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                DeleteImage(evento.ImagemURL);
                evento.ImagemURL = await SaveImage(file);

            }
            
            var EventoRetorno = await _eventoService.UpdateEvento(eventoId, evento);
            return Ok(EventoRetorno);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
             $"Erro ao tentar Adicionar/Atualizar Imagem. Erro:  {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
             var evento = await _eventoService.GetEventoByIdAsync(id);
            if (evento == null) return NoContent();

            if (await _eventoService.DeleteEvento(id)) {

                DeleteImage(evento.ImagemURL);
                return Ok(new {message = "Deletado"} );
            }
            else {
                throw new Exception("Erro ao tentar deletar evento.");
            }
            
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
             $"Erro ao tentar deletar evento. Erro:  {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, EventoDto model)
    {
        try
        {
            var evento = await _eventoService.UpdateEvento(id, model);
            if (evento == null) return NoContent();
            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
             $"Erro ao tentar atualizar evento. Erro:  {ex.Message}");
        }
    }

    [NonAction]
    public void DeleteImage(string imageName){

        var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, @"Resources/Images", imageName);

        if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);

    }

     [NonAction]
    public async Task<string> SaveImage(IFormFile imageFile){

         string imageName = new string(Path
                .GetFileNameWithoutExtension(imageFile.FileName)
                .Take(10)
                .ToArray()).Replace(' ', '-');

        imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";  

        var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, @"Resources/Images", imageName);

        using (var fileStream = new FileStream(imagePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }

        return imageName;
    }



}
