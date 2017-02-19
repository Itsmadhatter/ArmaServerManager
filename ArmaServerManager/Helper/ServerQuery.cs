using System;
using QueryMaster;
using QueryMaster.GameServer;

namespace ArmaServerManager.Helper
{
    public class ServerQuery
    {
        Server server;
        public ServerQuery(string IP, ushort Port)
        {
            server = QueryMaster.GameServer.ServerQuery.GetServerInstance(EngineType.Source, IP, Port);
        }

        public int getPlayer()
        {
            try
            {
                QueryMasterCollection<PlayerInfo> player = server.GetPlayers();
                return player.Count;
            }
            catch (NullReferenceException)
            {
                return 0;
            }
        }
    }  
}
