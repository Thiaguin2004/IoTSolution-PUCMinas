using IoTSolution.Enum;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace IoTSolution.Models
{
    public class LogsModel
    {
        [Key]
        public int IdLog { get; set; }
        public int IdDispositivo { get; set; }
        public int IdSensor { get; set; }
        public string LogString { get; set; }
        public DateTime DataHoraLog { get; set; }
    }
}
