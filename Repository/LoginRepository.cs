using Dapper;
using e_TillRemote.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Settings;
using e_TillRemote.Provider;

namespace e_TillRemote.Repository
{
    public class LoginRepository
    {
        static string ConnectionString = DbConnections.ConnectionString;
        SqlConnection primarycon = new SqlConnection(ConnectionString);


        public static SqlConnection remoteConnectionString;




        public static SqlConnection GetRemoteConnected(IPAddressModel Ipdet)
        {
            try
            {
                remoteConnectionString = DbConnections.GetRemoteConnected(Ipdet);
            }
            catch (Exception ex)
            {
                return remoteConnectionString=null;
            }
            return remoteConnectionString;
        }

        ///Repository code Started

        public PasswordModel getUserLogin(PasswordModel pwd)
        {
            var param = new DynamicParameters();
            param.Add("@p_no", pwd.P_NO);
            param.Add("@Web_PWORD", pwd.WEB_PWORD);
            var passwordModel = primarycon.QuerySingle<PasswordModel>("usp_Login", param: param, commandType: CommandType.StoredProcedure);
            return passwordModel;
        }

        public IPAddressModel GetAllIpAddress(IPAddressModel Ipdet)
        {
            var param = new DynamicParameters();
            param.Add("@Ipaddress", Ipdet.IPAddress);
            var Ipaddressdet = primarycon.QuerySingle<IPAddressModel>("[usp_GetDbConnectionByIpaddress]", param: param, commandType: CommandType.StoredProcedure);
            return Ipaddressdet;
        }

        public List<IPAddressModel> GetIpAddressByPno(int pno)
        {
            var param = new DynamicParameters();
            param.Add("@pno", Convert.ToInt32(pno));
            var Ipaddress = primarycon.Query<IPAddressModel>("usp_GetIpByPNO", param: param, commandType: CommandType.StoredProcedure).ToList();
            return Ipaddress;
        }



        public List<Section> GetSection1()
        {
            List<Section> Sectiondet = new List<Section>();
            try
            {
                SqlConnection RemoteCon = remoteConnectionString;
                Sectiondet = RemoteCon.Query<Section>("SectionProc1", commandType: CommandType.StoredProcedure).ToList();
                return Sectiondet;
            }
            catch (Exception ex)
            {
                return Sectiondet = null;

            }

        }

        public int SaveSessionLogIn(SessionLogModel slm)
        {
            var param = new DynamicParameters();
            param.Add("@Location", slm.Location);
            param.Add("@IpAddressFrom", slm.IpAddressFrom);
            param.Add("@IpAddressTo", slm.IpAddressTo);
            param.Add("@DeviceId", slm.DeviceId);
            param.Add("@UserId", slm.UserId);
            param.Add("@UserName", slm.UserName);

            try
            {
                if (remoteConnectionString.State == ConnectionState.Closed)
                {
                    remoteConnectionString.Open();
                }
                int resultLocal = primarycon.Execute("usp_InsertSessionLog", param: param, commandType: CommandType.StoredProcedure);
                int resultRemote = remoteConnectionString.Execute("usp_InsertSessionLog", param: param, commandType: CommandType.StoredProcedure);
                if (resultLocal > 0 && resultRemote > 0)
                {
                    return resultRemote;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int SaveSessionLogOut(int UserId)
        {
            var param = new DynamicParameters();

            param.Add("@UserId", UserId);

            int resultLocal = primarycon.Execute("usp_SessionLogOut", param: param, commandType: CommandType.StoredProcedure);
            int resultRemote = remoteConnectionString.Execute("usp_SessionLogOut", param: param, commandType: CommandType.StoredProcedure);
            if (resultLocal > 0 && resultRemote > 0)
            {
                return resultRemote;
            }
            else
            {
                return 0;
            }

        }

    }
}
