// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGridExtensionConfig?functionName={functionname}

using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Host;
using static DirectToTelegram.Utils;

namespace DirectToTelegram
{
    public static class OnEventGridTrigger
    {
        [FunctionName("OnEventGridTrigger")]
        public static async Task Run([EventGridTrigger] EventGridEvent eventGridEvent, TraceWriter log)
        {
            var messageWrapper = BuildTextMessage(source: "EventGridTrigger");
            var messageResult = await HttpClient.PostAsync(BuildTelegramBotUrl("sendMessage"), BuildMessageToSend(messageWrapper));

            log.Info(eventGridEvent.Data.ToString());
            log.Info($"Message result: {messageResult}");
        }
    }
}