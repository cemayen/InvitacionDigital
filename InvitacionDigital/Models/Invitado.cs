using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvitacionDigital.Models
{
    public class Invitado
    {
        [Key]
        public int Id { get; set; }

        [StringLength(150)]
        public string? NombreCompleto { get; set; } // Puede estar nulo al inicio si la familia no los ha capturado

        public bool Confirmado { get; set; } = false;

        // Clave Foránea apuntando a Usuario (Familia)
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }
    }
}
