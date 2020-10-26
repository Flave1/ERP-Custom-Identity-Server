using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace APIGateway.AuthGrid
{
    public class ScrewIdentifierGrid
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ScrewIdentifierGridId { get; set; }
        public bool ShouldAthenticate { get; set; }
        public int Media { get; set; }
        public int Module { get; set; }
        public bool ActiveOnMobileApp { get; set; }
        public bool ActiveOnWebApp { get; set; }
        public bool UseActiveDirectory { get; set; }
        public string ActiveDirectory { get; set; }
        public bool EnableLoginFailedLockout { get; set; }
        public int NumberOfFailedLoginBeforeLockout { get; set; }
        public int NumberOfFailedAttemptsBeforeSecurityQuestions { get; set; }
        public TimeSpan RetryTimeInMinutes { get; set; }
        public bool EnableRetryOnMobileApp { get; set; }
        public bool EnableRetryOnWebApp { get; set; }
        public bool ShouldRetryAfterLockoutEnabled { get; set; }
        public TimeSpan InActiveSessionTimeout { get; set; }
        public int PasswordUpdateCycle { get; set; }
        public bool SecuritySettingActiveOnMobileApp { get; set; }
        public bool SecuritySettingsActiveOnWebApp { get; set; }
        public bool EnableLoadBalance { get; set; }
        public int LoadBalanceInHours { get; set; }
        public DateTime WhenNextToUpdatePassword { get; set; }
    }
    
}
