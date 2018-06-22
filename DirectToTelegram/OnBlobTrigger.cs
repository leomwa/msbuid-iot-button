using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using static DirectToTelegram.Utils;

namespace DirectToTelegram
{
    public static class OnBlobTrigger
    {
        //[FunctionName("OnBlobTrigger")]
        // Due to cold start, a blob trigger is just too painful. Solved in EventHub trigger.
        public static async Task Run(
            [BlobTrigger("build-button-messages/{name}", Connection = "AzureWebJobsStorage")]
            Stream myBlob,
            string name,
            TraceWriter log)
        {
            var messageWrapper = BuildTextMessage(source: "BlobTrigger");
            var messageResult = await HttpClient.PostAsync(BuildTelegramBotUrl("sendMessage"), BuildMessageToSend(messageWrapper));

            log.Info(await LogTelegramResponseMessage(messageResult));
            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}