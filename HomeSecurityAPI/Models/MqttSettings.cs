using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeSecurityAPI.Models
{
    public class MqttSettings
    {
        [Required]
        public string BrokerHostName { get; set; }

        [Required]
        public uint Port { get; set; }

        [Required]
        public bool UseSecureConnection { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Topic { get; set; }
    }
}
