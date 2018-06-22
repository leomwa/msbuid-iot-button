using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using static DirectToTelegram.Utils;

namespace DirectToTelegram
{
    [StorageAccount("AzureWebJobsStorage")]
    public static class OnEventHubTrigger
    {
        private const string BlobPath = "build-button-messages/leons-first-iot-hub/{sys.randguid}.json";

        [FunctionName("OnEventHubTrigger")]
        public static async Task Run(
            [EventHubTrigger("build-iot-button", Connection = "EventHubsConnection")] string eventHubMessage,
            [Blob(BlobPath, FileAccess.Write)] Stream blobStream,
            TraceWriter log)
        {
            var messageWrapper = BuildTextMessage(source: "EventHubTrigger");
            var messageResult = await HttpClient.PostAsync(BuildTelegramBotUrl("sendMessage"), BuildMessageToSend(messageWrapper));

            log.Info(await LogTelegramResponseMessage(messageResult));
            log.Info($"C# Event Hub trigger function processed a message: {eventHubMessage}");
            WriteDeviceMessageIntoStream(eventHubMessage, blobStream);
        }
    }
}