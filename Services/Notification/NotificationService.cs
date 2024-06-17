using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MultipleAreas_BlazorTemplate.Models;
namespace MultipleAreas_BlazorTemplate.Services.Notification
{
    public class NotificationService : IAsyncDisposable
    {
        public HubConnection HubConnection { get; }
        public bool IsConnected => HubConnection.State == HubConnectionState.Connected;
        public NotificationService()
        {
            HubConnection = new HubConnectionBuilder()
                .WithUrl(GlobalConfigModel.configuration?.GetValue<string>("NotificationHUB"))
                .Build();

        }
        public async Task StartAsync()
        {
            if (HubConnection.State != HubConnectionState.Connected)
            {
                await HubConnection.StartAsync();
            }
        }
        public async Task AuthenticateApp(string appName)
        {
            await HubConnection.SendAsync("AuthenticateApp", appName);
        }

        public async Task AuthenticateClient(string appName, string clientName)
        {
            await HubConnection.SendAsync("AuthenticateClient", appName, clientName);
        }
        public async ValueTask DisposeAsync()
        {
            if (HubConnection != null)
            {
                await HubConnection.DisposeAsync();
            }
        }
    }
}
