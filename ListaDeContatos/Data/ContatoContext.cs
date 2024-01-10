using ListaDeContatos.Models;
using Microsoft.EntityFrameworkCore;

namespace ListaDeContatos.Data
{
	public class ContatoContext : DbContext
	{
		public ContatoContext(DbContextOptions<ContatoContext> options) : base(options)
		{
		}

		public DbSet<Contatos> Contatos { get; set; }
	}
}
