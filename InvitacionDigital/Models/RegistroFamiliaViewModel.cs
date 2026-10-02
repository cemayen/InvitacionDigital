using System.ComponentModel.DataAnnotations;

namespace InvitacionDigital.Models
{
    public class RegistroFamiliaViewModel
    {
        [Required(ErrorMessage = "El nombre de la familia es obligatorio.")]
        [Display(Name = "Nombre de la Familia")]
        public string NombreFamilia { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El usuario debe tener entre 3 y 50 caracteres.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Indica la cantidad de boletos.")]
        [Range(1, 20, ErrorMessage = "La cantidad de boletos debe ser entre 1 y 20.")]
        [Display(Name = "Cantidad de Boletos")]
        public int CantidadBoletos { get; set; } = 2;

        [Display(Name = "¿Opción a Cabaña?")]
        public bool OpcionCabana { get; set; }

        [Required(ErrorMessage = "El fecha limite es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime? FechaLimite { get; set; }
    }
}
