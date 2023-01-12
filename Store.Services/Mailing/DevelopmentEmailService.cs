using MailKit.Net.Smtp;
using MediatR;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Store.Domain.Entities;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Services.Mailing
{
    public class DevelopmentEmailService : MailingServiceBase
    {
        private readonly IConfiguration configuration;
        private (string smtp, int port, bool useSsl) DEVELOPMENT_SERVER_CREDENTIALS =
        (
            smtp: "smtp.gmail.com",
            port: 587,
            useSsl: false
        );

        public DevelopmentEmailService(ILogger logger, IConfiguration configration) : base(logger)
        {
            this.configuration = configration;
        }
        public async override Task<Unit> SendNewCustomerRegistrationMessage(Customer customer, CancellationToken cancelationToken, string messageText = null)
        {
            MimeMessage message = new MimeMessage
            {
                To = { InternetAddress.Parse(customer.EmailAddress) },
                From = { InternetAddress.Parse("test_account+x@hotmail.com") },
                Subject = "Test subject",
                Body = new TextPart("plain")
                {
                    Text = messageText is null ? $@"Hello {customer.CompanyName},
                        Thank you for registration. 
                        Your activation code is : https://localhost:5001/api/customers/activate?Registration.ActivationCode={customer.Registration.ActivationCode}"
                    : messageText
                }
            };

            await this.ConnectAndSend(message, cancelationToken);
            return await Unit.Task;
        }

        protected async override Task<Unit> ConnectAndSend(MimeMessage message, CancellationToken cancelationToken)
        {
            using (this.client = new SmtpClient())
            {
                await client.ConnectAsync
                (
                    DEVELOPMENT_SERVER_CREDENTIALS.smtp,
                    DEVELOPMENT_SERVER_CREDENTIALS.port,
                    DEVELOPMENT_SERVER_CREDENTIALS.useSsl
                );
                await client.AuthenticateAsync("test_account", "test_account_password");
                await client.SendAsync(message);
                await client.DisconnectAsync(true, cancelationToken);
            }

            return await Unit.Task;
        }
    }
}
