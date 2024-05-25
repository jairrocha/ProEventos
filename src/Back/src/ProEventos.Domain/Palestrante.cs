using ProEventos.Domain.Identity;

namespace ProEventos.Domain;

public class Palestrante
{
    public int Id { get; set; }
    public string Minicurriculo { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public IEnumerable<RedeSocial> RedeSociais { get; set; }
    public IEnumerable<PalestranteEvento> PalestranteEventos { get; set; }
}

