using Lib.AspNetCore.ServerSentEvents;

namespace WordleClash.Web.Services;

public class ServerEvents
{

    private readonly IServerSentEventsService _client;

    public ServerEvents(IServerSentEventsService client)
    {
        _client = client;
    }

    public async Task UpdatePlayers(string lobbyCode)
    {
        await SendEvent($"PlayerUpdate/{lobbyCode}");
    }
    
    public async Task UpdateField(string lobbyCode)
    {
        await SendEvent($"FieldUpdate/{lobbyCode}");
    }

    private async Task SendEvent(string type)
    {
        var clients = _client.GetClients();
        if (clients.Any())
        {
            await _client.SendEventAsync(
                new ServerSentEvent
                {
                    Type = type,
                    Data = new List<string>() {""}
                });
        }
    }
}