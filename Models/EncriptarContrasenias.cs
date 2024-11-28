using System.Security.Cryptography;

namespace Proyecto_Blog.Models
{
    public class EncriptarContrasenia
    {

        public static string HashPassword(string password)
        {
            // Generar un salt único
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[16];
                rng.GetBytes(saltBytes);
                string salt = Convert.ToBase64String(saltBytes);

                // Crear el hash de la contraseña con el salt
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000))
                {
                    byte[] hashBytes = pbkdf2.GetBytes(32);
                    string hashedPassword = Convert.ToBase64String(hashBytes);

                    return hashedPassword;
                }
            }
        }
    }
}
