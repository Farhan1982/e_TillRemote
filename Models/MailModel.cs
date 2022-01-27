using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_TillRemote.Models
{
    public class MailModel
    {
        public string smtpAddress
        { get; set; }
        public int portNumber
        {

          get; set;   
           

        }
        //public bool enableSSL
        //{
        //    set
        //    {
        //        value = false;
        //    }

        //    get { return enableSSL; }
        //}

        public string emailFromAddress { get; set; }  
        public string password { get; set; }// = "Abc@123$%^"; //Sender Password  
        public string emailToAddress { get; set; } //= "receiver@gmail.com"; //Receiver Email Address
        public string subject { get; set; } //= "Hello";
        public string body { get; set; } // = "Hello, This is Email sending test using gmail.";
    }
}
