using GODP.APIsContinuation.DomainObjects.UserAccount;
using GOSLibraries.GOS_Financial_Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Validations.Recovery
{
    public static class GenericValidators
    {
        public static async Task<bool> IsPasswordCharactersValid(string password, CancellationToken cancellationToken)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            return await Task.Run(() => hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password));
        }
    }
}
