using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace DirectToTelegram
{
    public static class Utils
    {
        public static readonly HttpClient HttpClient = new HttpClient();
        private const string DefaultMessage = "Leon's MS //Build teXXmo button pressed. So cool. 😍";
        private const string IotPlaygroundGroupId = "-228328775";

        public static object BuildTextMessage(string source) => new
        {
            chat_id = GetConfiguredValueOrDefault("TelegramChatOrChannelId", IotPlaygroundGroupId),
            text = $"{GetConfiguredValueOrDefault("TextMessageToTelegram", DefaultMessage)}{Environment.NewLine}Source:{source}"
        };

        private static string GetConfiguredValueOrDefault(string key, string defaultIfNone)
        {
            string environmentVariable = GetEnvironmentVariable(key);
            return string.IsNullOrWhiteSpace(environmentVariable)
                ? defaultIfNone
                : environmentVariable;
        }

        public static string BuildTelegramBotUrl(string method)
        {
            var telegramBotApiBaseUrl = GetEnvironmentVariable("TelegramBotApiUrl");
            var telegramBotToken = GetEnvironmentVariable("TelegramBotToken");
            return $"https://{telegramBotApiBaseUrl}/bot{telegramBotToken}/{method}";
        }

        public static StringContent BuildMessageToSend(object message)
            => new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

        private static string GetEnvironmentVariable(string key) => Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);
    }
}