using GODP.APIsContinuation.DomainObjects.Account;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODPAPIs.Contracts.GeneralExtension;
using GOSLibraries.GOS_Financial_Identity;
using System;
using System.ComponentModel.DataAnnotations;
using Wangkanai.Detection.Services;

namespace APIGateway.AuthGrid
{
    public static class Measures
    {
        public static Tracker CollectAsMuchAsPossible(cor_useraccount user, AuthenticationResult result, IDetectionService device)
        {
            var tr = new Tracker();

            tr.Browser = device.Browser.Name.ToString();
            tr.Device = device.Device.Type.ToString();
            tr.Os = device.Engine.Name.ToString();
            tr.Os_Device = device.UserAgent.ToString();
            tr.Token = result.Token;
            tr.UserId = user.Id;
            tr.UserAgent = device.UserAgent.ToString();
            tr.Email = user.Email;
            return tr;
        }
       
        public static Tracker CollectAsMuchAsPossible(cor_useraccount user, AuthenticationResult result, OTPLoginCommand request)
        {
            return new Tracker
            { 
                Email = user.Email
            };
        }
        //public static Tracker CollectAsMuchAsPossible(ApplicationUser user, AuthenticationResult result, LoginCommand request)
        //{
        //    return new Tracker
        //    {
        //        Browser = request.Browser,
        //        Device = request.Device,
        //        Os = request.Os,
        //        Os_Device = request.Os_Device,
        //        Token = request.Token,
        //        UserId = user.Id,
        //        UserAgent = request.UserAgent,
        //    };
        //}
    }
    public class Tracker : GeneralEntity
    {
        [Key]
        public int MeasureId { get; set; }
        public string UserId { get; set; }
        public string Browser { get; set; }
        public string Os { get; set; }
        public string Device { get; set; }
        public string UserAgent { get; set; }
        public string Os_Device { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }

    public class OTPTracker : GeneralEntity
    {
        [Key]
        public int OTPId { get; set; }
        public string OTP { get; set; }
        public DateTime DateIssued { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}
