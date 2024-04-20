using WordleClash.Core;
using WordleClash.Core.Interfaces;

namespace WordleClash.Tests;

public class MockLobby: BaseLobby
{
    public MockLobby(IDataAccess dataAccess, Player creator): base(dataAccess, creator, 2, 2) {}
    public override void StartGame()
    {
        throw new NotImplementedException();
    }
}