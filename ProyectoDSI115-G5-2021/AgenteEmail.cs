using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Windows;

namespace ProyectoDSI115_G5_2021
{
    class AgenteEmail
    {
        public static string GenerarMail(string usuario, int random)
        {
            string cuerpo = "<font>"+usuario+", su clave para recuperar su contraseña es:</font>";
            cuerpo += "<h2>" + random.ToString("000000") + "</h2>";
            cuerpo += "<font>Si no solicitó el cambio, verifique su acceso.</font>";
            return cuerpo;
        }

        public static void EnviarMail(string texto, string direccion, Remitente remitente)
        {
            try
            {
                MailMessage mensaje = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                mensaje.From = new MailAddress(remitente.correo);
                mensaje.To.Add(new MailAddress(direccion));
                mensaje.Subject = "Recupere su contraseña";
                mensaje.IsBodyHtml = true;
                mensaje.Body = texto;
                smtp.Port = 587;
                smtp.Host = "smtp-mail.outlook.com"; //SMTP para Outlook.com 
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(remitente.correo, remitente.contrasena);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(mensaje);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al enviar correo. Verifique su conexión e intente de nuevo.");
            }
        }
    }
}
