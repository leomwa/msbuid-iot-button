using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace DirectToTelegram
{
    public static class OnBlobTrigger
    {
        private const string DefaultMessage = "Leon's MS //Build teXXmo button pressed. So cool. 😍";
        private const string IotPlaygroundGroupId = "-228328775";
        private static readonly HttpClient HttpClient = new HttpClient();

        [FunctionName("OnBlobTrigger")]
        public static async Task Run(
            [BlobTrigger("build-button-messages/{name}", Connection = "AzureWebJobsStorage")]
            Stream myBlob,
            string name,
            TraceWriter log)
        {
            var messageWrapper = BuildTextMessage();
            var messageResult = await HttpClient.PostAsync(BuildTelegramBotUrl("sendMessage"), BuildMessageToSend(messageWrapper));

            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            log.Info($"Message result: {messageResult}");
        }

        private static object BuildTextMessage() => new
        {
            chat_id = GetConfiguredValueOrDefault("TelegramChatOrChannelId", IotPlaygroundGroupId),
            text = GetConfiguredValueOrDefault("TextMessageToTelegram", DefaultMessage)
        };

        private static string GetConfiguredValueOrDefault(string key, string defaultIfNone)
        {
            string environmentVariable = GetEnvironmentVariable(key);
            return string.IsNullOrWhiteSpace(environmentVariable)
                ? defaultIfNone
                : environmentVariable;
        }

        private static string BuildTelegramBotUrl(string method)
        {
            var telegramBotApiBaseUrl = GetEnvironmentVariable("TelegramBotApiUrl");
            var telegramBotToken = GetEnvironmentVariable("TelegramBotToken");
            return $"https://{telegramBotApiBaseUrl}/bot{telegramBotToken}/{method}";
        }

        private static StringContent BuildMessageToSend(object message)
            => new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

        private static string GetEnvironmentVariable(string key) => Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);
    }
}