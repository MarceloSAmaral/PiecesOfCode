using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace msantana.amaral.LoadGenerator
{
    public sealed class Program
    {
        public int RequestNumber = 0;

        public static async Task Main(string[] args)
        {
            string configFile = "config.json";
            if (args.Length > 0)
            {
                configFile = args[0];
            }

            Config config = Config.GetArguments(configFile);
            if (config == null)
            {
                Console.WriteLine("Failed to parse configuration.");
                return;
            }

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                }).UseConsoleLifetime();

            var host = builder.Build();

            Program programInstance = new Program();

            int totalRequests = 0;
            int totalSuccesses = 0;
            int totalFailures = 0;

            ConcurrentQueue<RequestResult> resultsQueue = new ConcurrentQueue<RequestResult>();
            Dictionary<HttpStatusCode, int> resultsPerStatusCode = new Dictionary<HttpStatusCode, int>();
            Dictionary<string, int> errorMessages = new Dictionary<string, int>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var httpClientFactory = host.Services.GetService<IHttpClientFactory>();
            List<Task> tasks = new List<Task>();
            Stopwatch stopwatch = new Stopwatch();
            var timeStarted = System.DateTime.UtcNow;

            StartRequestAgents(config, programInstance, resultsQueue, cancellationTokenSource, httpClientFactory, tasks);

            while (true)
            {
                await Task.Delay(1_000);
                ReceiveResults(resultsQueue, ref totalRequests, ref totalSuccesses, ref totalFailures, resultsPerStatusCode, errorMessages);

                Console.Clear();
                Console.WriteLine($"Running load against {config.ServerURL}, target rps: {config.TargetRPS}");
                DisplayRunningStatus(timeStarted, totalRequests, config.TargetRPS);
                Console.WriteLine("Press Esc to stop test...");

                if (Console.KeyAvailable == true)
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine();
                        Console.WriteLine(".....");
                        Console.WriteLine("Stopping requests...");
                        cancellationTokenSource.Cancel();
                        await Task.WhenAll(tasks);
                        ReceiveResults(resultsQueue, ref totalRequests, ref totalSuccesses, ref totalFailures, resultsPerStatusCode, errorMessages);
                        DisplayStats(timeStarted, totalRequests, totalSuccesses, totalFailures, resultsPerStatusCode, errorMessages);
                        Console.WriteLine("Press any key to exit...");
                        Console.ReadKey();
                        break;
                    }
                }
            }
        }

        private static void StartRequestAgents(Config config, Program programInstance, ConcurrentQueue<RequestResult> resultsQueue, CancellationTokenSource cancellationTokenSource, IHttpClientFactory httpClientFactory, List<Task> tasks)
        {
            for (int i = 0; i < config.TargetRPS; i++)
            {
                var requestAgent = new RequestAgent(httpClientFactory.CreateClient(), config.ServerURL, config.AuthKey, config.UserName, programInstance, resultsQueue);
                tasks.Add(requestAgent.Start(cancellationTokenSource.Token));
            }
        }

        private static void ReceiveResults(ConcurrentQueue<RequestResult> resultsQueue, ref int totalRequests, ref int totalSuccesses, ref int totalFailures, Dictionary<HttpStatusCode, int> resultsPerStatusCode, Dictionary<string, int> errorMessages)
        {
            RequestResult requestResult = null;
            var queueCount = resultsQueue.Count;
            if (queueCount == 0) return;
            for (int i = 0; i < queueCount; i++)
            {
                if (resultsQueue.TryDequeue(out requestResult))
                {
                    totalRequests++;
                    UpdateStatistics(ref totalSuccesses, ref totalFailures, resultsPerStatusCode, errorMessages, requestResult.Successful, requestResult.StatusCode, requestResult.ErrorMessage);
                }
                else
                {
                    break;
                }
            }
        }

        private static void UpdateStatistics(ref int totalSuccesses, ref int totalFailures, Dictionary<HttpStatusCode, int> resultsPerStatusCode, Dictionary<string, int> errorMessages, bool successful, HttpStatusCode? statusCode, string errorMessage)
        {
            if(statusCode.HasValue)
            {
                if (!resultsPerStatusCode.ContainsKey(statusCode.Value))
                {
                    resultsPerStatusCode.Add(statusCode.Value, 1);
                }
                else
                {
                    resultsPerStatusCode[statusCode.Value] = resultsPerStatusCode[statusCode.Value] + 1;
                }
            }

            if(successful)
            {
                totalSuccesses++;
            }
            else
            {
                totalFailures++;
                if (string.IsNullOrWhiteSpace(errorMessage)) errorMessage = "-- Null or empty error message --";

                if(!errorMessages.ContainsKey(errorMessage))
                {
                    errorMessages.Add(errorMessage, 1);
                }
                else
                {
                    errorMessages[errorMessage] = errorMessages[errorMessage] + 1;
                }
            }
        }

        private static void DisplayStats(DateTime timeStarted, int totalRequests, int totalSuccesses, int totalFailures, Dictionary<HttpStatusCode, int> resultsPerStatusCode, Dictionary<string, int> errorMessages)
        {
            var elapsedTimeInString = (System.DateTime.UtcNow - timeStarted).ToString(@"hh\:mm\:ss");

            Console.WriteLine($"Elapsed time: {elapsedTimeInString}");
            Console.WriteLine($"Total requests: {totalRequests}\t\ttotal sucesses: {totalSuccesses}\t\ttotal failures: {totalFailures}");
            Console.WriteLine("Results per status code:");
            foreach (var item in resultsPerStatusCode)
            {
                Console.WriteLine($"StatusCode {(int)item.Key} {item.Key}:\t\t\t{item.Value}");
            }
            Console.WriteLine();
            if (errorMessages.Count > 0)
            {
                Console.WriteLine("Error messages:");
                foreach (var item in errorMessages)
                {
                    Console.WriteLine($"--> {item.Key}:\t\t{item.Value}");
                }
            }
            Console.WriteLine();
        }

        private static void DisplayRunningStatus(DateTime timeStarted, int totalRequests, uint targetRPS)
        {
            var elapsedTime = System.DateTime.UtcNow - timeStarted;
            var elapsedTimeStr = elapsedTime.ToString(@"hh\:mm\:ss");
            var requestsPerSecond = Convert.ToInt32(totalRequests / elapsedTime.TotalSeconds);
            Console.WriteLine($"Target requests per second: {targetRPS}, current rate: {requestsPerSecond} per second. - Total concluded requests: {totalRequests} - Elapsed time: {elapsedTimeStr}"); /*concluded request*/
        }
    }
}
