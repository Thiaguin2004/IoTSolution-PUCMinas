using System.ComponentModel.DataAnnotations;

namespace IoTSolution.Models
{
    public class UsuariosModel
    {
        [Key]
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? Email { get; set; }
        public string? CPF { get; set; }
    }
}
