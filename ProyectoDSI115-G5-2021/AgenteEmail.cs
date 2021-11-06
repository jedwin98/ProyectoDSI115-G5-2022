using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace ProyectoDSI115_G5_2021
{
    class AgenteEmail
    {
        // Puerto de conexión estándar. Defina otro puerto como entero en la respectiva conexión.
        internal static int DEFAULT_PORT = 587;
        // Genera el cuerpo del mensaje de código a enviar.
        // AUTOR: Félix Eduardo Henríquez Cruz
        public static string GenerarMail(string usuario, int random)
        {
            string cuerpo = "<font>"+usuario+", su código de seguridad a utilizar es:</font>";
            cuerpo += "<h2>" + random.ToString("000000") + "</h2>";
            cuerpo += "<font>No responda a este mensaje autogenerado.</font>";
            return cuerpo;
        }

        // Prepara el envío de un mensaje de correo. Definir texto, nombre y corrreo del destinatario, remitente y asunto.
        // AUTOR: Félix Eduardo Henríquez Cruz
        public static bool EnviarMail(string texto, string direccion, Remitente remitente, string destinatario, string asunto)
        {
            try
            {
                MimeMessage mensaje = new MimeMessage();
                SmtpClient smtp = new SmtpClient();
                mensaje.From.Add(new MailboxAddress("Mensajero", remitente.correo));
                mensaje.To.Add(new MailboxAddress(destinatario, direccion));
                mensaje.Subject = asunto;
                mensaje.Body = new TextPart("html") {
                    Text = texto
                };
                // Selección basada en dominio de correo.
                // Configure cada conexión adicional por separado. Para puerto 587, ocupe 'DEFAULT_PORT'.
                if (remitente.correo.Contains("@hotmail") || remitente.correo.Contains("@outlook"))
                {
                    smtp.Connect("smtp-mail.outlook.com", DEFAULT_PORT, SecureSocketOptions.StartTls); // Servidor SMTP para Outlook.com
                }
                else if (remitente.correo.Contains("@gmail"))
                {
                    smtp.Connect("smtp.gmail.com", DEFAULT_PORT, SecureSocketOptions.StartTls); // Servidor SMTP para Gmail
                }
                smtp.Authenticate(remitente.correo, remitente.contrasena);
                smtp.Send(mensaje);
                smtp.Disconnect(true);
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Error al enviar correo. Verifique su conexión o la configuración del remitente e intente de nuevo.");
                return false;
            }
        }
    }
}
