using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using System.Data;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;


namespace RAKHolidayHomesBL
{
    public class Email
    {
        DataTable dt = new DataTable();
        public void SendMail(string ToEmail, string Content, string Subject, string FromDisplayName)
        {
            try
            {
                SmtpClient smtpClinet = new SmtpClient();
                MailMessage msg = new MailMessage();
                msg.Body = Content;
                msg.To.Add(new MailAddress(ToEmail));
                msg.From = new MailAddress(ConfigurationSettings.AppSettings["fromEmailadmin"].ToString(), FromDisplayName);
                msg.Subject = Subject;
                msg.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                client.Host = ConfigurationSettings.AppSettings["smtp"].ToString();
                client.Port = Convert.ToInt32(ConfigurationSettings.AppSettings["port"].ToString());
                client.EnableSsl = false;
                client.UseDefaultCredentials = false;
                client.EnableSsl = Convert.ToBoolean(ConfigurationSettings.AppSettings["EnableSsl"].ToString());
                client.UseDefaultCredentials = Convert.ToBoolean(ConfigurationSettings.AppSettings["DefaultCredentials"].ToString());

                client.Credentials = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["fromEmailadmin"].ToString(), ConfigurationSettings.AppSettings["fromEmailPasswordadmin"].ToString());
                ServicePointManager.ServerCertificateValidationCallback =
                delegate(object s, X509Certificate certificate,
                X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                client.Send(msg);
                msg.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        public void SendMailNew(string ToEmail, string Content, string Subject, string FromDisplayName)
        {
            try
            {
                SmtpClient smtpClinet = new SmtpClient();
                MailMessage msg = new MailMessage();
                msg.Body = Content;
                msg.To.Add(new MailAddress(ToEmail));
                msg.From = new MailAddress(ConfigurationSettings.AppSettings["fromEmailNew"].ToString(), FromDisplayName);
                msg.Subject = Subject;
                msg.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                client.Host = ConfigurationSettings.AppSettings["smtp"].ToString();
                client.Port = Convert.ToInt32(ConfigurationSettings.AppSettings["port"].ToString());
                client.EnableSsl = true;
                client.EnableSsl = Convert.ToBoolean(ConfigurationSettings.AppSettings["EnableSsl"].ToString());
                client.UseDefaultCredentials = Convert.ToBoolean(ConfigurationSettings.AppSettings["DefaultCredentials"].ToString());

                client.Credentials = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["fromEmailNew"].ToString(), ConfigurationSettings.AppSettings["fromEmailPasswordNew"].ToString());
                //ServicePointManager.ServerCertificateValidationCallback =
                //delegate(object s, X509Certificate certificate,
                //X509Chain chain, SslPolicyErrors sslPolicyErrors)
                //{ return true; };

                client.Send(msg);
                msg.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        public string SendMailNoti(string ToEmail, string Content, string Subject, string FromDisplayName)
        {
            try
            {
                SmtpClient smtpClinet = new SmtpClient();
                MailMessage msg = new MailMessage();
                msg.Body = Content;
                msg.To.Add(new MailAddress(ToEmail));
                msg.From = new MailAddress(ConfigurationSettings.AppSettings["fromEmail"].ToString(), FromDisplayName);
                msg.Subject = Subject;
                msg.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = Convert.ToBoolean(ConfigurationSettings.AppSettings["DefaultCredentials"].ToString());
                string pwd = ConfigurationSettings.AppSettings["fromEmailPassword"].ToString();
                string userID = ConfigurationSettings.AppSettings["fromEmail"].ToString();

                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = ConfigurationSettings.AppSettings["smtp"].ToString();
                client.Port = Convert.ToInt32(ConfigurationSettings.AppSettings["port"].ToString());
                client.EnableSsl = Convert.ToBoolean(ConfigurationSettings.AppSettings["EnableSsl"].ToString());
                
               
                client.Credentials = new System.Net.NetworkCredential(userID, pwd);
               
                ServicePointManager.ServerCertificateValidationCallback =
             delegate (object s, X509Certificate certificate,
             X509Chain chain, SslPolicyErrors sslPolicyErrors)
             { return true; };

                client.Send(msg);
                msg.Dispose();
                return ".";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        string ContentForgot;

        public void SendMailAsync(string ToEmail, string Content, string Subject, string FromDisplayName)
        {
            try
            {
                SmtpClient smtpClinet = new SmtpClient();
                MailMessage msg = new MailMessage();
                msg.Body = Content;
                msg.To.Add(new MailAddress(ToEmail));
                msg.From = new MailAddress(ConfigurationSettings.AppSettings["fromEmail"].ToString(), FromDisplayName);
                msg.Subject = Subject;
                msg.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                client.Host = ConfigurationSettings.AppSettings["smtp"].ToString();
                client.EnableSsl = Convert.ToBoolean(ConfigurationSettings.AppSettings["EnableSsl"].ToString());
                client.UseDefaultCredentials = Convert.ToBoolean(ConfigurationSettings.AppSettings["DefaultCredentials"].ToString());

                client.Credentials = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["fromEmail"].ToString(), ConfigurationSettings.AppSettings["fromEmailPassword"].ToString());
                client.SendAsync(msg, new object());
                msg.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        public void SendMailNotiWithAttachment(string ToEmail, string Content, string Subject, string FromDisplayName, System.IO.Stream stream, string AttachName)
        {
            try
            {
                SmtpClient smtpClinet = new SmtpClient();
                MailMessage msg = new MailMessage();
                msg.Body = Content;
                msg.To.Add(new MailAddress(ToEmail));
                msg.From = new MailAddress(ConfigurationSettings.AppSettings["fromEmail"].ToString(), FromDisplayName);
                msg.Subject = Subject;
                msg.IsBodyHtml = true;
                msg.Attachments.Add(new Attachment(stream, AttachName));
                SmtpClient client = new SmtpClient();
                client.Host = ConfigurationSettings.AppSettings["smtp"].ToString();
                client.Port = Convert.ToInt32(ConfigurationSettings.AppSettings["port"].ToString());
                client.EnableSsl = Convert.ToBoolean(ConfigurationSettings.AppSettings["EnableSsl"].ToString());
                client.UseDefaultCredentials = Convert.ToBoolean(ConfigurationSettings.AppSettings["DefaultCredentials"].ToString());
                client.Credentials = new System.Net.NetworkCredential(ConfigurationSettings.AppSettings["fromEmail"].ToString(), ConfigurationSettings.AppSettings["fromEmailPassword"].ToString());
                ServicePointManager.ServerCertificateValidationCallback =
                delegate(object s, X509Certificate certificate,
                X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                client.Send(msg);
                msg.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
    }
}