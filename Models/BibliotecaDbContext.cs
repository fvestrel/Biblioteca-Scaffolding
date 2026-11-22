using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Models
{
    public class BibliotecaDbContext : DbContext
    {
        public BibliotecaDbContext(DbContextOptions<BibliotecaDbContext> options) : base(options)
        { }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Socio> Socios { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Libro>()
                .HasOne(l => l.Categoria).WithMany(c => c.Libros)
                .HasForeignKey(l => l.CategoriaId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Libro>()
                .HasOne(l => l.Autor).WithMany(a => a.Libros)
                .HasForeignKey(l => l.AutorId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Libro).WithMany(l => l.Prestamos)
                .HasForeignKey(p => p.LibroId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Socio).WithMany(s => s.Prestamos)
                .HasForeignKey(p => p.SocioId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Socio>().HasIndex(s => s.Cedula).IsUnique();
            modelBuilder.Entity<Categoria>().HasIndex(c => c.Codigo).IsUnique();

            modelBuilder.Entity<Autor>()
                .HasIndex(a => new { a.Nombre, a.Apellido, a.Fecha_Nacimiento }).IsUnique();
        }
    }
}
