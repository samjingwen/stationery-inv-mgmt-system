using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace Team7ADProject.Entities
{
    //Author: Teh Li Heng 18/1/2019
    //Allow sending of email by using async
    public static class Email
    {
        public static void Send(string recipientEmail, string subject, string content)
        {
            #region Teh Li Heng
            var fromAddress = new MailAddress("team7logicdb@gmail.com", "Team 7 Logic DB"); //email from
            var toAddress = new MailAddress(recipientEmail, "To Name"); //email to
            const string fromPassword = "SA47Team7LogicDB1";
            string emailSubject = subject;
            string emailBody = content;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = emailSubject,
                Body = emailBody
            })
            {
                smtp.Send(message);
            }
            #endregion
        }
    }
}