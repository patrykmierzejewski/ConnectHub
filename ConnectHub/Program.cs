using Microsoft.AspNetCore.SignalR.Client;
using System.IO.Compression;

namespace ConnectHub
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var ct = new CancellationTokenSource();

            string sessionId = "c34f3919-b386-4f90-8557-fe0bf1098e4d";
            string hubUrl = "https://test-api.hishoo.online/live-emotions";

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
            });
            

            Console.ReadLine();
        }


        static public async Task EmotionsLiveResponse(EmotionsResult emotionsResultResponse)
        {
            var response = emotionsResultResponse;

            Console.WriteLine($"{response.AudioTimeString} {response.Category} {response.Score}");

            await Task.CompletedTask;
        }
    }
}