using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;

namespace Infrastructure.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _config;
        private TransactionalEmailsApi emailsApi;

        public EmailSenderService(IConfiguration config)
        {
            _config = config;

            Configuration.Default.ApiKey.Add("api-key", _config["EmailConfiguration:ApiKey"]);

            emailsApi = new TransactionalEmailsApi();
        }

        public async Task SendConfirmationEmailAsync(string to, string token, string username)
        {
            SendSmtpEmail email = new SendSmtpEmail(
                sender: new SendSmtpEmailSender(_config["EmailConfiguration:FromName"], _config["EmailConfiguration:FromEmail"]),
                to: new System.Collections.Generic.List<SendSmtpEmailTo> {
                    new SendSmtpEmailTo(to)
                },
                templateId: int.Parse(_config["EmailConfiguration:ConfirmEmailTemplateId"]),
                _params: new { validationToken = token, email = to, username = username }
            );
            CreateSmtpEmail result = await emailsApi.SendTransacEmailAsync(email);
        }

        public async Task SendPasswordResetEmailAsync(string to, string token, string username)
        {
            SendSmtpEmail email = new SendSmtpEmail(
                sender: new SendSmtpEmailSender(_config["EmailConfiguration:FromName"], _config["EmailConfiguration:FromEmail"]),
                to: new System.Collections.Generic.List<SendSmtpEmailTo> {
                    new SendSmtpEmailTo(to)
                },
                templateId: int.Parse(_config["EmailConfiguration:PasswordChangeTemplateId"]),
                _params: new { validationToken = token, email = to, username = username }
            );
            CreateSmtpEmail result = await emailsApi.SendTransacEmailAsync(email);
        }
    }
}