using System.ComponentModel.DataAnnotations;

namespace InvitacionDigital.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la familia o usuario es obligatorio.")]
        [StringLength(100)]
        public string NombreFamilia { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [Range(1, 20, ErrorMessage = "La cantidad de boletos debe ser entre 1 y 20.")]
        public int CantidadBoletos { get; set; }

        public bool OpcionCabana { get; set; }

        // Nuevo campo para la fecha límite
        public DateTime? FechaLimite { get; set; }

        [Required]
        public string Rol { get; set; } = "Invitado"; // "Admin" o "Invitado"

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Relación 1 a Muchos: Una familia tiene varios invitados (según la cantidad de boletos)
        public ICollection<Invitado> Invitados { get; set; } = new List<Invitado>();
    }
}
