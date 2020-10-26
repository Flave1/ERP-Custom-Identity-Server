using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.AuthGrid
{
    public class LogingFailedChecker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Userid { get; set; }
        public int Counter { get; set; }
        public int QuestionTimeCount { get; set; }
        public DateTime RetryTime { get; set; }
        
    }
    public class SessionChecker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Userid { get; set; } 
        public int Module { get; set; }
        public DateTime LastRefreshed { get; set; }

    }
    public class AccountsLocked
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Userid { get; set; }
        public bool IsQuestionTime { get; set; }
        public DateTime UnlockAt { get; set; }
    }
    public class SessionCheckerRespObj
    {
        public int StatusCode { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class LogingFailedRespObj
    { 
        public bool IsSecurityQuestion { get; set; }
        public DateTime UnLockAt { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
