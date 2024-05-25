
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos;

public class EventoDto
{
    public int Id { get; set; }

    public string Local { get; set; }
   
    public  DateTime? DataEvento { get; set; }

    [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "O campo deve ter no minímo 3 caracteres e no máximo 50")]
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Tema { get; set; }

    [Range(1, 150, ErrorMessage = "{0} não pode ser menor que 1 e maior que 150")]
    [Display(Name = "Qtd pessoas")]
    public int QtdPessoas { get; set; }

    
    [RegularExpression(@"^.*\.(jpg|jpeg|png|gif)$", ErrorMessage = "O campo {0} não possui uma imagem válida. (jpg, jpeg, png, gif)")]
    public string ImagemURL { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Phone(ErrorMessage = "O campo {0} não é válido")]
    public string Telefone { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} não é válido")]
    [Display(Name = "E-mail")]
    public string Email { get; set; }
    public IEnumerable<LoteDto>? Lotes { get; set; }
    public IEnumerable<RedeSocialDto>? RedeSocials { get; set; }
    public IEnumerable<PalestranteDto>? PalestranteEventos { get; set; }
    public int? UserId { get;  set; }
    public UserDto? UserDto { get; set; }
}
