using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Prestamo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de préstamo es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Préstamo")]
        public DateTime Fecha_Prestamo { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Devolución")]
        public DateTime? Fecha_Devolucion { get; set; }

        [Required(ErrorMessage = "El libro es obligatorio")]
        [Display(Name = "Libro")]
        public int LibroId { get; set; }
        public Libro Libro { get; set; }

        [Required(ErrorMessage = "El socio es obligatorio")]
        [Display(Name = "Socio")]
        public int SocioId { get; set; }
        public Socio Socio { get; set; }
    }
}
