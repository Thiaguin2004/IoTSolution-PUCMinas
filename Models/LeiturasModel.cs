﻿using IoTSolution.Enum;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace IoTSolution.Models
{
    public class LeiturasModel
    {
        [Key]
        public int IdLeitura { get; set; }
        public int IdDispositivo { get; set; }
        public int IdSensor { get; set; }
        public decimal Temperatura { get; set; }
        public DateTime DataHoraLeitura { get; set; }
    }
}
