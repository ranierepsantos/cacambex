using Domain.Cacambas.Agregacao;
using Domain.Clientes.Agrecacao;
using Domain.Identidade.Agregacao;
using Domain.Pedidos.Agregacao;
using Domain.Pedidos.Eventos;
using Microsoft.EntityFrameworkCore;

namespace Infra.Dados;

public class DataContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Cliente> Clientes { get; set; } = null!;
    public DbSet<EnderecoEntrega> EnderecosEntrega { get; set; } = null!;
    public DbSet<EnderecoCobranca> EnderecosCobranca { get; set; } = null!;
    public DbSet<Cacamba> Cacambas { get; set; } = null!;
    public DbSet<Pedido> Pedidos { get; set; } = null!;
    public DbSet<PedidoItem> PedidoItens { get; set; } = null!;
    public DbSet<Evento> Eventos { get; set; } = null!;



    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder mb)
    {

        mb.Entity<EnderecoEntrega>().HasKey(ee => ee.Id);
        mb.Entity<EnderecoEntrega>().Property(ee => ee.CEP).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<EnderecoEntrega>().Property(ee => ee.Logradouro).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<EnderecoEntrega>().Property(ee => ee.Numero).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<EnderecoEntrega>().Property(ee => ee.Complemento).HasColumnType("varchar").HasMaxLength(255);
        mb.Entity<EnderecoEntrega>().Property(ee => ee.Bairro).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<EnderecoEntrega>().Property(ee => ee.Cidade).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<EnderecoEntrega>().Property(ee => ee.UF).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<EnderecoEntrega>()
        .HasOne<Cliente>()
        .WithMany(x => x.EnderecosEntrega);

        mb.Entity<EnderecoCobranca>().HasKey(ec => ec.Id);
        mb.Entity<EnderecoCobranca>().Property(ec => ec.CEP).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<EnderecoCobranca>().Property(ec => ec.Logradouro).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<EnderecoCobranca>().Property(ec => ec.Numero).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<EnderecoCobranca>().Property(ec => ec.Complemento).HasColumnType("varchar").HasMaxLength(255);
        mb.Entity<EnderecoCobranca>().Property(ec => ec.Bairro).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<EnderecoCobranca>().Property(ec => ec.Cidade).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<EnderecoCobranca>().Property(ec => ec.UF).HasColumnType("varchar").HasMaxLength(255).IsRequired();

        mb.Entity<Cliente>().HasKey(c => c.Id);
        mb.Entity<Cliente>().Property(c => c.Codigo_cliente_integracao).HasColumnType("varchar").HasMaxLength(255);
        mb.Entity<Cliente>().Property(c => c.Codigo_cliente_omie).HasColumnType("varchar").HasMaxLength(255);
        mb.Entity<Cliente>().Property(c => c.Nome).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<Cliente>().Property(c => c.Documento).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<Cliente>().Property(c => c.DataNascimento).HasColumnType("datetime");
        mb.Entity<Cliente>().Property(c => c.Telefone).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<Cliente>().Property(c => c.Email).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<Cliente>().Property(c => c.Contribuinte).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<Cliente>().Property(c => c.TipoDocumento).IsRequired();
        mb.Entity<Cliente>().Property(c => c.Pessoa_fisica).HasColumnType("varchar").HasMaxLength(255);
        mb.Entity<Cliente>().Property(c => c.Ativo).HasColumnType("bit").IsRequired();

        mb.Entity<Usuario>().HasKey(u => u.Id);
        mb.Entity<Usuario>().Property(u => u.Nome).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<Usuario>().Property(u => u.Email).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<Usuario>().Property(u => u.Senha).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<Usuario>().Property(u => u.Ativo).HasColumnType("bit").IsRequired();
        mb.Entity<Usuario>().Property(u => u.Funcao).IsRequired();

        mb.Entity<Cacamba>().HasKey(c => c.Id);
        mb.Entity<Cacamba>().Property(c => c.Numero).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<Cacamba>().Property(c => c.Volume).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<Cacamba>().Property(c => c.Status).IsRequired();
        mb.Entity<Cacamba>().Property(c => c.Preco).HasColumnType("decimal").HasPrecision(12, 2).IsRequired();
        mb.Entity<Cacamba>().Property(c => c.cCodIntServ).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<Cacamba>().Property(c => c.nCodServ).HasColumnType("bigint");
        mb.Entity<Cacamba>().Property(c => c.Ativo).HasColumnType("bit").IsRequired();

        mb.Entity<Pedido>().HasKey(p => p.Id);
        mb.Entity<Pedido>().Property(p => p.NumeroNotaFiscal).HasColumnType("varchar").HasMaxLength(255);
        mb.Entity<Pedido>().Property(p => p.NumeroCTR).HasColumnType("varchar").HasMaxLength(255);
        mb.Entity<Pedido>().Property(p => p.Observacao).HasColumnType("varchar").HasMaxLength(255);
        mb.Entity<Pedido>().Property(p => p.TipoDePagamento).IsRequired();
        mb.Entity<Pedido>().Property(p => p.ValorPedido).HasColumnType("decimal").HasPrecision(12, 2).IsRequired();
        mb.Entity<Pedido>().Property(p => p.Ativo).HasColumnType("bit").IsRequired();

        mb.Entity<PedidoItem>().HasKey(p => p.Id);
        mb.Entity<PedidoItem>().Property(p => p.VolumeCacamba).HasColumnType("varchar").HasMaxLength(255).IsRequired();
        mb.Entity<PedidoItem>().Property(p => p.ValorUnitario).HasColumnType("decimal").HasPrecision(12, 2).IsRequired();
    }
}
