using APIGateway.Contracts.Commands.Email;
using APIGateway.DomainObjects.Company;
using GOSLibraries.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.MailHandler.Service
{
    public interface IEmailService
    {
        Task Send(EmailMessage emailMessage);
        List<EmailMessage> ReceiveEmail(int maxCount = 10);

        Task<bool> SaveUpdateEmailAsync(EmailMessage model);
        Task<List<EmailMessage>> GetAllEmailForAUserAsync(string email, string userid);
        Task<List<EmailAddress>> GetToAndFromEmailAddressesAsync(int EmailId);
        Task<EmailAddress> GetSingleEmailAddressAsync(string email, int emailId);
        Task<bool> DeleteEmailAsync(EmailMessage model);
        Task<EmailMessage> GetSingEmailAsync(int EmailMessageId);

        Task SendEmailAsync(EmailMessage model);

        Task<bool> UpdateCurrentUserEmailAsync(EmailAddress model);
        Task<EmailMessage> GetSingleEmailAsync(int emailId);
        Task<List<EmailAddress>> GetAllEmailAddressForAUserAsync(string email);
        Task<EmailMessage> BuildAndSaveEmail(SendEmailCommand request);
    }
}
