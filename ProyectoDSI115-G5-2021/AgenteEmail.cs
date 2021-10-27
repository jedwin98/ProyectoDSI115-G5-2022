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
        // Genera el cuerpo del mensaje de código a enviar.
        // AUTOR: Félix Eduardo Henríquez Cruz
        public static string GenerarMail(string usuario, int random)
        {
            string cuerpo = "<font>"+usuario+", su código de seguridad a utilizar es:</font>";
            cuerpo += "<h2>" + random.ToString("000000") + "</h2>";
            cuerpo += "<font>No responda a este mensaje autogenerado.</font>";
            return cuerpo;
        }

        // Prepara el envío de un mensaje de correo. Definir texto, destinatario, remitente y asunto.
        // AUTOR: Félix Eduardo Henríquez Cruz
        public static void EnviarMail(string texto, string direccion, Remitente remitente, string asunto)
        {
            try
            {
                MailMessage mensaje = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                mensaje.From = new MailAddress(remitente.correo);
                mensaje.To.Add(new MailAddress(direccion));
                mensaje.Subject = asunto;
                mensaje.IsBodyHtml = true;
                mensaje.Body = texto;
                // Puerto SMTP default. (Recomendado)
                // En caso de agregar nuevas configuraciones con otro puerto, ajuste en el respectivo apartado.
                smtp.Port = 587;
                // Selección basada en dominio de correo.
                if (remitente.correo.Contains("@hotmail") || remitente.correo.Contains("@outlook"))
                {
                    smtp.Host = "smtp-mail.outlook.com"; //Servidor SMTP para Outlook.com
                }
                else if (remitente.correo.Contains("@gmail"))
                {
                    smtp.Host = "smtp-relay.gmail.com"; //Servidor SMTP para Gmail
                }
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(remitente.correo, remitente.contrasena);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(mensaje);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al enviar correo. Verifique su conexión o la configuración del remitente e intente de nuevo.");
            }
        }
    }
}
