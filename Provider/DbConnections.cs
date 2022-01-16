using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using e_TillRemote.Models;
using Settings;
namespace e_TillRemote.Provider
{
    public class DbConnections
    {
        public static string ConnectionString = ConnectionSetting.GetConnectionString();
        public static SqlConnection RemoteConnectionString
        {
            get;set;
        }

        public static SqlConnection GetRemoteConnected(IPAddressModel Ip)
        {
            string conn = ConnectionSetting.GetRemoteConnectionString(Ip.IPAddress, Ip.UserName, Ip.Pwd);
            RemoteConnectionString = new SqlConnection(conn);
            RemoteConnectionString.Open();
            return RemoteConnectionString;
        }
    }
}
