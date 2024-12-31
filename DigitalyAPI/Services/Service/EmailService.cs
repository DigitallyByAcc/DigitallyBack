using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;

namespace DigitalyAPI.Services.Service
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }




        public async Task<bool> SendEmailAsync (EmailSendDto emailsend )
        {
            MailjetClient client = new MailjetClient(_config["MailJet:ApiKey"], _config["MailJet:SecretKey"]);

            var email = new TransactionalEmailBuilder ()
                .WithFrom( new SendContact(_config["Email:From"], _config["Email:ApplicationName"]))
                .WithSubject(emailsend.Subject)
                .WithHtmlPart(emailsend.Body)
                .WithTo( new SendContact(emailsend.To))
                .Build();

            var response = await client.SendTransactionalEmailAsync (email);
            Console.WriteLine(response.ToString());

            if (response != null)
            {
                if (response.Messages[0].Status == "success")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
