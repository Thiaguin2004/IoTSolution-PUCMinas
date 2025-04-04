using IoTSolution.Enum;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace IoTSolution.Models
{
    public class AlertasModel
    {
        [Key]
        public int IdAlerta { get; set; }
        public int IdDispositivo { get; set; }
        public int IdSensor { get; set; }
        public int IdUsuario { get; set; }
        public string? Mensagem { get; set; }
        public bool MensagemEnviada { get; set; }
        public DateTime DataHoraAlerta { get; set; }
    }
}
