using ConnectHub.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.IO.Compression;

namespace ConnectHub
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var ct = new CancellationTokenSource();

            string sessionId = "02d04349-1705-4de1-affb-c9223224649a";
            string hubUrl = "https://localhost:44346/live-emotions";

            _ = Task.Run(async () => 
            {
                //hub init
                HubConnection hubConnection = new HubConnectionBuilder()
                    .WithUrl(hubUrl, options =>
                    {
                        //options.AccessTokenProvider = () => Task.FromResult(bearerToken); // now not auth.
                        options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
                    })
                    .Build();
                hubConnection.ServerTimeout = new TimeSpan(0, 1, 30);
                hubConnection.HandshakeTimeout = new TimeSpan(0, 1, 30);

                await hubConnection.StartAsync(ct.Token);

                //connect by sessionId
                await hubConnection.SendAsync("JoinToStream", sessionId, ct.Token);

                //recevive data
                hubConnection.On<EmotionsResult>("EmotionsLiveResultResponse", EmotionsLiveResponse);
                hubConnection.On<List<OperatorRankingToday>>("RankingLiveResultResponse", RankingLiveResultResponse);
                hubConnection.On<SpeechRateML>("SpeechRateResultResponse", SpeechRateResultResponse);
                hubConnection.On<OperatorInfo>("OperatorInfoResultResponse", OperatorInfoResultResponse);
            });
            

            Console.ReadLine();
        }


        static public async Task EmotionsLiveResponse(EmotionsResult emotionsResultResponse)
        {
            var response = emotionsResultResponse;

            Console.WriteLine($"{response.AudioTimeString} {response.Category} {response.Score}");

            await Task.CompletedTask;
        }

        static public async Task RankingLiveResultResponse(List<OperatorRankingToday> responseRankingOperators)
        {
            foreach (var item in responseRankingOperators)
            {
                Console.WriteLine($"Operator Name: {item.OperatorName}, Position: {item.Position} Score: {item.OpeartorScore}, Sale: {item.OperatorSale}, SpeechRate: {item.OperatorSpechRate}");
            }

            Console.WriteLine("---------------------------------------------------");
        }

        static public async Task SpeechRateResultResponse(SpeechRateML responseSpeechRate)
        {
            Console.WriteLine($"SpeechRate by operator: {responseSpeechRate.OperatorSpeechRate}, SpeechRate by customer: {responseSpeechRate.CustomerSpeechRate}");
            Console.WriteLine("---------------------------------------------------");
        }

        static public async Task OperatorInfoResultResponse(OperatorInfo responseOperatorInfo)
        {
            Console.WriteLine($"Operator Points: {responseOperatorInfo.Points}");
            Console.WriteLine("---------------------------------------------------");
        }
    }
}