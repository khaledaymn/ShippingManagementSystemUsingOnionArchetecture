using Castle.Core.Configuration;
using MailKit.Security;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using ShippingManagementSystem.Application.Settings;
using ShippingManagementSystem.Domain.Interfaces;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace ShippingManagementSystem.Application.Services.ExternalServices.EmailServices
{
    //    public class EmailServices : IEmailServices
    //    {
    //        private readonly EmailConfiguration _configuration;
    //        public EmailServices(IOptions<EmailConfiguration> configuration)
    //        {
    //            _configuration = configuration.Value;
    //        }

    //        public async Task<string> SendEmailAsync(string Name, string Email,string token)
    //        {

    //            using var client = new MailKit.Net.Smtp.SmtpClient();
    //            try
    //            {
    //                // Create an email message based on the provided authentication details
    //                var mailMessage = CreateEmailMessage(Name,Email,token);

    //                // Connect to the SMTP server using the configured settings
    //                await client.ConnectAsync(_configuration.SmtpServer, _configuration.Port, true);

    //                // Remove the XOAUTH2 authentication mechanism
    //                client.AuthenticationMechanisms.Remove("XOAUTH2");

    //                // Authenticate with the SMTP server using the provided credentials
    //                await client.AuthenticateAsync(_configuration.Email, _configuration.Password);

    //                // Send the email message
    //                await client.SendAsync(mailMessage);

    //                // If the email is sent successfully, return "success"
    //                return "success";
    //            }
    //            catch (SmtpException ex)
    //            {
    //                // Return a meaningful error message
    //                return "An SMTP error occurred while sending an email. Please try again later.";
    //            }
    //            catch (Exception ex)
    //            {
    //                // Return a generic error message
    //                return "An error occurred while sending an email. Please try again later.";
    //            }
    //            finally
    //            {
    //                // Disconnect and dispose the client
    //                await client.DisconnectAsync(true);
    //                client.Dispose();
    //            }
    //        }
    //        private MimeMessage CreateEmailMessage(string name, string email, string token)
    //        {
    //            // Validate inputs
    //            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
    //                throw new ArgumentException("Name, email, and token cannot be null or empty.");


    //            var link = $"{_configuration.PasswordResetLink}?email={email}&token={token}";

    //            // Build HTML email content
    //            var content = $@"
    //<!DOCTYPE html>
    //<html>
    //<head>
    //    <style>
    //        body {{
    //            font-family: 'Tajawal', Arial, sans-serif;
    //            color: #333;
    //            direction: rtl;
    //            text-align: right;
    //            background-color: #f0f4f8;
    //            margin: 0;
    //            padding: 20px;
    //        }}
    //        .container {{
    //            max-width: 600px;
    //            margin: 0 auto;
    //            padding: 30px;
    //            background-color: white;
    //            border-radius: 15px;
    //            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
    //        }}
    //        .header {{
    //            text-align: center;
    //            margin-bottom: 30px;
    //        }}
    //        .header h2 {{
    //            font-size: 28px;
    //            color: #008d7f;
    //            margin: 0;
    //            font-weight: bold;
    //        }}
    //        .header p {{
    //            font-size: 14px;
    //            color: #666;
    //            margin: 5px 0 0;
    //        }}
    //        .content p {{
    //            font-size: 16px;
    //            line-height: 1.8;
    //            margin: 15px 0;
    //            color: #444;
    //        }}
    //        .button {{
    //            display: inline-block;
    //            padding: 12px 40px;
    //            background-color: #008d7f;
    //            color: white !important;
    //            text-decoration: none;
    //            border-radius: 7px;
    //            font-size: 18px;
    //            font-weight: bold;
    //            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
    //            transition: background-color 0.3s ease;
    //        }}
    //        .button:hover {{
    //            background-color: #008d7fd8;
    //        }}
    //        .footer {{
    //            margin-top: 30px;
    //            font-size: 14px;
    //            color: #555;
    //            text-align: center;
    //            border-top: 1px solid #eee;
    //            padding-top: 15px;
    //        }}
    //        .footer a {{
    //            color: #008d7f;
    //            text-decoration: none;
    //            font-weight: bold;
    //        }}
    //        .footer a:hover {{
    //            color: #008d7f;
    //        }}
    //    </style>
    //    <link href='https://fonts.googleapis.com/css2?family=Tajawal:wght@400;700&display=swap' rel='stylesheet'>
    //</head>
    //<body>
    //    <div class='container'>
    //        <div class='header'>
    //            <h2>Reset Your Password</h2>
    //        </div>
    //        <div class='content'>
    //            <p>Hello {name},</p>
    //            <p>We received a request to reset the password for your account on <strong>Al-Balsam Medical Pharmacy</strong>.</p>
    //            <p>If you initiated this request, please click the button below:</p>
    //            <p><a href='{link}' class='button'>Reset Password</a></p>
    //            <p>If you did not make this request, you can safely ignore this message. Your password will remain unchanged.</p>
    //        </div>
    //        <div class='footer'>
    //            <p>Thank you for trusting us,</p>
    //            <p>The Al-Balsam Medical Pharmacy Team</p>
    //            <p><a href='mailto:{_configuration.Email}'>Contact Support</a></p>
    //        </div>
    //    </div>
    //</body>
    //</html>";

    //            // Create email message
    //            var mailMessage = new MimeMessage();
    //            mailMessage.From.Add(new MailboxAddress("Shipping Management System", _configuration.Email));
    //            mailMessage.To.Add(new MailboxAddress(name, email));
    //            mailMessage.Subject = "Reset Password";
    //            mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = content };

    //            return mailMessage;
    //        }
    //    }
    public class EmailService : IEmailServices
    {
        private readonly EmailConfiguration _email;
        public EmailService(IOptions<EmailConfiguration> configuration)
        {
            _email = configuration.Value;
        }

        public async Task<string> SendEmailAsync(string Name, string Email, string token)
        {

            using var client = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                // Create an email message based on the provided authentication details
                var mailMessage = CreateEmailMessage(Name, Email, token);

                // Connect to the SMTP server using the configured settings
                await client.ConnectAsync(_email.SmtpServer, _email.Port, true);

                // Remove the XOAUTH2 authentication mechanism
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Authenticate with the SMTP server using the provided credentials
                await client.AuthenticateAsync(_email.UserName, _email.Password);

                // Send the email message
                await client.SendAsync(mailMessage);

                // If the email is sent successfully, return "success"
                return "success";
            }
            catch (SmtpException ex)
            {
                // Return a meaningful error message
                return "An SMTP error occurred while sending an email. Please try again later.";
            }
            catch (Exception ex)
            {
                // Return a generic error message
                return "An error occurred while sending an email. Please try again later.";
            }
            finally
            {
                // Disconnect and dispose the client
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
        private MimeMessage CreateEmailMessage(string name, string email, string token)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                throw new ArgumentException("Name, email, and token cannot be null or empty.");


            var link = $"{_email.PasswordResetLink}?email={email}&token={token}";
            var content = $@"
           <!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <title>Reset Your Password</title>
</head>
<body style=""font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 0; margin: 0;"">
    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #f4f4f4; padding: 20px 0;"">
        <tr>
            <td align=""center"">
                <table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.1);"">
                    <tr>
                        <td style=""background-color: #004080; color: white; padding: 20px; text-align: center;"">
                            <h2 style=""margin: 0;"">Reset Your Password</h2>
                        </td>
                    </tr>
                    <tr>
                        <td style=""padding: 30px;"">
                            <p style=""font-size: 16px; color: #333;"">Hello {name},</p>
                            <p style=""font-size: 16px; color: #333;"">
                                We received a request to reset the password for your account on the <strong>Shipping Management System</strong>.
                            </p>
                            <p style=""font-size: 16px; color: #333;"">
                                If you made this request, please click the button below to proceed:
                            </p>
                            <p style=""text-align: center; margin: 30px 0;"">
                                <a href=""{link}"" style=""background-color: #007BFF; color: white; padding: 12px 24px; text-decoration: none; font-size: 16px; border-radius: 5px;"">Reset Password</a>
                            </p>
                            <p style=""font-size: 16px; color: #333;"">
                                If you did not request this change, you can safely ignore this email. Your password will remain unchanged.
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td style=""background-color: #f0f0f0; padding: 20px; text-align: center; font-size: 14px; color: #666;"">
                            <p style=""margin: 0;"">Thank you for using our system,</p>
                            <p style=""margin: 0;"">The Shipping Management System Team</p>
                            <p style=""margin: 10px 0;"">
                                <a href=""mailto:{_email.From}"" style=""color: #004080; text-decoration: none;"">Contact Support</a>
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>
";

            // Create email message
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("Shipping System", _email.From));
            mailMessage.To.Add(new MailboxAddress(name, email));
            mailMessage.Subject = "Reset Passord";
            mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = content };

            return mailMessage;
        }
    }
}
