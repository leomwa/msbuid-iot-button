using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using static DirectToTelegram.Utils;

namespace DirectToTelegram
{
    public static class OnEventHubTrigger
    {
        [FunctionName("OnEventHubTrigger")]
        public static async Task Run(
            [EventHubTrigger("build-iot-button", Connection = "EventHubsConnection")]
            string myEventHubMessage,
            TraceWriter log)
        {
            var messageWrapper = BuildTextMessage(source: "EventHubTrigger");
            var messageResult = await HttpClient.PostAsync(BuildTelegramBotUrl("sendMessage"), BuildMessageToSend(messageWrapper));

            log.Info(await LogTelegramResponseMessage(messageResult));
            log.Info($"C# Event Hub trigger function processed a message: {myEventHubMessage}");
        }
    }
}