using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Domain.Enum;
using ProEventos.Domain.Identity;
using SQLitePCL;

namespace ProEventos.Persistence.Contextos;

public class ProEventosContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public ProEventosContext(DbContextOptions<ProEventosContext> options) : base(options) { }
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<Lote> Lotes { get; set; }
    public DbSet<Palestrante> Palestrantes { get; set; }
    public DbSet<PalestranteEvento> PalestranteEventos { get; set; }
    public DbSet<RedeSocial> RedesSociais { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserRole>(userRole =>
        {
            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

            userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            userRole.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });

        modelBuilder.Entity<User>(user => {
            user.Property(p => p.Titulo).HasDefaultValue(Titulo.NaoInformado);
            user.Property(p => p.Descricao).IsRequired(false);
            user.Property(p => p.Funcao).HasDefaultValue(Funcao.NaoInformado);
            user.Property(p => p.Imagem).IsRequired(false);

        });

        modelBuilder.Entity<PalestranteEvento>()
            .HasKey(PE => new { PE.EventoId, PE.PalestranteId });

        modelBuilder.Entity<Evento>()
            .HasMany(e => e.RedeSocials)
            .WithOne(rs => rs.Evento)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Palestrante>()
          .HasMany(p => p.RedeSociais)
          .WithOne(rs => rs.Palestrante)
          .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}
