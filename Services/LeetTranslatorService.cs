
using AFS_L33tSp34k3r_App.Interfaces;
using AFS_L33tSp34k3r_App.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AFS_L33tSp34k3r_App.Services
{
    /// <summary>
    /// The LeetTranslatorService clas responsible for translating input text to "Leet Speak" using an external API or a fallback native implementation.
    /// </summary>
    /// <param name="input">The input text to be translated to Leet Speak.</param>
    /// <returns>The translated Leet Speak text.</returns>
    public class LeetTranslatorService : ITranslatorService
    {
        private readonly HttpClient _httpClient;

        private readonly LeetTranslatorServiceOptions _options;

        private readonly Dictionary<char, string> _leetDictionary = new Dictionary<char, string>
        {
            {'a', "4"}, {'b', "8"}, {'e', "3"}, {'g', "9"}, {'l', "1"},
            {'o', "0"}, {'s', "5"}, {'t', "7"}, {'z', "2"}
        };
        
        //private readonly string _logFilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="LeetTranslatorService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client used for making API requests.</param>
        /// <param name="logFilePath">The path to the log file. Default is "C:\Logs\LeetTranslatorErrorLog.txt".</param>
        public LeetTranslatorService(HttpClient httpClient, IOptions<LeetTranslatorServiceOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        /// <summary>
        /// Main function calling the exteranl api passing user input to translate.
        /// Has internal fallback if api tells us to go away:(
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> TranslateAsync(string input)
        {
            // Validate the input parameter for scheananigans
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input), "Input string cannot be null or empty.");

            try
            {
                var response = await _httpClient.GetAsync($"https://api.funtranslations.com/translate/leetspeak.json?text={input}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    LogToFile("API status code: " + response.StatusCode);

                    var translationResponder = JsonConvert.DeserializeObject<TranslationResponder>(content);
                    if (translationResponder != null && translationResponder.Contents != null)
                    {
                        return translationResponder.Contents.Translated;
                    }
                    else
                    {
                        LogToFile($"API error: {response.StatusCode}");
                        return TranslateToLeetNative(input);
                    }
                }
                else
                {
                    LogToFile($"API Error: {response.StatusCode} - {content}. Falling back to native translation method with input: "+input);
                    return TranslateToLeetNative(input);
                }
            }
            catch (Exception ex)
            {
                LogToFile($"Unexpected error occurred during translation: {ex.Message}");
                return TranslateToLeetNative(input);
            }
        }

        /// <summary>
        /// Simple native leetspeaker fallback for when the api inevitably says no.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string TranslateToLeetNative(string input)
            {
                char[] characters = input.ToLower().ToCharArray();
                for (int i = 0; i < characters.Length; i++)
                {
                    if (_leetDictionary.ContainsKey(characters[i]))
                    {
                        characters[i] = _leetDictionary[characters[i]][0]; // Replace with LeetSpeak equivalent
                    }
                }
                return new string(characters);
            }

        /// <summary>
        ///Logs things (api or other error or success) to file for future reference.
        /// </summary>
        private void LogToFile(string message)
        {
            File.AppendAllText(_options.LogFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
        }
    }

    /// <summary>
    /// Container for service related things to be expanded later:)
    /// </summary>
    public class LeetTranslatorServiceOptions
    {
        public string? LogFilePath { get; set; }
    }
}
