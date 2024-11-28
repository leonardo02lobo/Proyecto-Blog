

namespace Proyecto_Blog.Models
{
    public class Usuario
    {
        private int id { get; set; }
        private string nombreUsuario { get; set; } = "";
        private string correo { get; set; } = "";
        private string contrasenia { get; set; } = "";

        public Usuario(string nombreUsuario, string correo, string contrasenia)
        {
            this.nombreUsuario = nombreUsuario;
            this.correo = correo;
            this.contrasenia = contrasenia;
        }
        public string getNombreUsuario()
        {
            return nombreUsuario;
        }
        public string getCorreo()
        {
            return correo;
        }
        public string getContraseniaEncriptada()
        {
            return EncriptarContrasenia.HashPassword(contrasenia);
        }
    }
}
