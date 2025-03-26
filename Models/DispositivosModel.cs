using IoTSolution.Models;
using IoTSolution.Enum;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace IoTSolution.Models
{
    public class DispositivosModel
    {
        [Key]
        public int IdDispositivo { get; set; }
        public int IdUsuario { get; set; }
        public string Descricao { get; set; }
        public DateTime DataHoraCadastro { get; set; }
    }
}
