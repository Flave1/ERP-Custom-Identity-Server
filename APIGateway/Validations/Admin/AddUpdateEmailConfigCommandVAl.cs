using APIGateway.Contracts.Commands.Email;
using FluentValidation;

namespace APIGateway.Validations.Admin
{
    public class AddUpdateEmailConfigCommandVAl : AbstractValidator<AddUpdateEmailConfigCommand>
    {
        public AddUpdateEmailConfigCommandVAl()
        {
            RuleFor(x => x.BaseFrontend).NotEmpty();
            RuleFor(x => x.MailCaption).NotEmpty().MinimumLength(10);
            RuleFor(x => x.SenderEmail).NotEmpty().EmailAddress();
            RuleFor(x => x.SenderPassword).NotEmpty().MinimumLength(4);
            RuleFor(x => x.SenderUserName).NotEmpty().MinimumLength(2);
            RuleFor(x => x.SmtpClient).NotEmpty();
            RuleFor(x => x.SMTPPort).NotEmpty(); 
        }
    }
}
