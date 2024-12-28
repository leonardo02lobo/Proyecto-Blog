using MySql.Data.MySqlClient;

namespace Proyecto_Blog.Models
{
    public class Usuario
    {
        private int id { get; set; }
        private string nombreUsuario { get; set; } = "";
        private string correo { get; set; } = "";
        private string contrasenia { get; set; } = "";
        private bool Iniciado { get; set; } = false;

        public Usuario()
        {
        }
        public Usuario(int id,string nombreUsuario, string correo, string contrasenia)
        {
            this.id = id;
            this.nombreUsuario = nombreUsuario;
            this.correo = correo;
            this.contrasenia = contrasenia;
        }

        public void setNombreUsuario(string nombreUsuario)
        {
            this.nombreUsuario = nombreUsuario;
        }
        public void setIniciado(bool iniciado)
        {
            this.Iniciado = iniciado;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public void setCorreo(string correo)
        {
            this.correo = correo;
        }
        public void setContrasenia(string contrasenia)
        {
            this.contrasenia = contrasenia;
        }
        public int getId() => id;
        public string getNombreUsuario() => nombreUsuario;
        public string getCorreo() => correo;
        public string getContraseniaEncriptada() => EncriptarContrasenia.EncriptarContraseña(contrasenia);

        public bool getIniciado() => Iniciado;
    }
}
