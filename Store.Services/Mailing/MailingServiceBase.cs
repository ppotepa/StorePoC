using MailKit.Net.Smtp;
using MediatR;
using MimeKit;
using Store.Domain.Entities;
using Store.Utilities;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Services.Mailing
{
    public abstract class MailingServiceBase : Disposable, IMailingService
    {
        protected readonly ILogger logger;
        protected ISmtpClient client;

        protected MailingServiceBase(ILogger logger)
        {
            this.logger = logger;
        }

        
        public abstract Task<Unit> SendNewCustomerRegistrationMessage(Customer customer, CancellationToken cancelationToken, string text = null);        
        protected abstract Task<Unit> ConnectAndSend(MimeMessage message, CancellationToken cancelationToken);
        
    }
}
