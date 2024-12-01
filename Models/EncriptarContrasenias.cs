using System.Security.Cryptography;
using System.Text;

namespace Proyecto_Blog.Models
{
    public class EncriptarContrasenia
    {

        public static string EncriptarContraseña(string contraseña)
        {
            // Salt fijo (esto reduce seguridad, usar con precaución)
            string saltFijo = "SaltFijoSeguridad123!@#";

            // Combinar contraseña con el salt
            string contraseñaConSalt = contraseña + saltFijo;

            // Convertir la cadena a bytes
            byte[] bytes = Encoding.UTF8.GetBytes(contraseñaConSalt);

            // Crear el hash usando SHA256
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Convertir el hash a una cadena hexadecimal
                StringBuilder hash = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hash.Append(b.ToString("x2"));
                }
                return hash.ToString();
            }
        }
    }
}
