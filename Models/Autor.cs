using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Autor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del autor es obligatorio")]
        [StringLength(60)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido del autor es obligatorio")]
        [StringLength(60)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "La nacionalidad es obligatoria")]
        [StringLength(50)]
        public string Nacionalidad { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime Fecha_Nacimiento { get; set; }

        public ICollection<Libro> Libros { get; set; } = new List<Libro>();
    }
}
