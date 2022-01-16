using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_TillRemote.Models
{
    public class PasswordModel
    {
        public int P_NO { get; set; }
        public string P_USER { get; set; }
        public string P_CODE { get; set; }
        public int P_LEVEL { get; set; }
        public string WEB_YN { get; set; }
        public string WEB_PWORD { get; set; }
        public string IPAddress { get; set; }
    }

    public class IPAddressModel
    {
        public int IPid { get; set; }
        public string IPAddress { get; set; }
        public string Pwd { get; set; }
        public string Location { get; set; }
        public string UserName { get; set; }

    }

    public class Section
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class SessionLogModel
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public bool LoginOut { get; set; }
        public string Location { get; set; }
        public string IpAddressFrom { get; set; }
        public string IpAddressTo { get; set; }
        public string DeviceId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
