using System;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using HomeSecurityAPI.Models;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Formatter;
using MQTTnet.Protocol;

namespace HomeSecurityAPI.Services
{
    public class MqttService
    {
        private IMqttClient mqttClient;
        public MqttService()
        {
            //configuration.Port = 31233;
            //var credentials = new MqttClientCredentials("WebApi", "Kalpens", "weaksecurity123");

            //mqttClient = MqttClient.CreateAsync("m20.cloudmqtt.com", configuration ).Result;
            var factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();
        }

        public async Task sendTakenPictureName(string pictureName)
        {
            //string rcvTopic = "";
            //string sendTopic = "my/mqtt";
            var options = new MqttClientOptionsBuilder()
                .WithClientId("webapi")
                .WithWebSocketServer("wss://m20.cloudmqtt.com:31233")
                //.WithTcpServer("m20.cloudmqtt.com", 11233)
                .WithCredentials("bbutcued", "aFm5G2Rqk101")
                .WithTls()
                .Build();
            await mqttClient.ConnectAsync(options);
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("my/mqtt")
                .WithPayload(pictureName)
                .WithRetainFlag(false)
                .Build();

            await mqttClient.PublishAsync(message);
        }
    }
}
