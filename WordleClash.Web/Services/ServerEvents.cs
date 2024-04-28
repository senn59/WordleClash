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
        var clients = _client.GetClients();
        if (clients.Any())
        {
            await _client.SendEventAsync(
                new ServerSentEvent
                {
                    Type = $"PlayerUpdate/{lobbyCode}",
                    Data = new List<string>() {""}
                });
        }
    }
    
}