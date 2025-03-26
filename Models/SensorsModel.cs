using IoTSolution.Enum;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace IoTSolution.Models
{
    public class SensorsModel
    {
        [Key]
        public int IdSensor { get; set; }
        public int IdDispositivo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataHoraCadastro { get; set; }
    }
}
