

using MySql.Data.MySqlClient;

namespace Proyecto_Blog.Models
{
    public class Usuario
    {
        private int id { get; set; }
        private string nombreUsuario { get; set; } = "";
        private string correo { get; set; } = "";
        private string contrasenia { get; set; } = "";
        public MySqlConnection conexion;
        private string connectionString = "Server=localhost;Database=blog_prueba;Uid=root;Pwd=root1234;";
        private bool Iniciado { get; set; } = false;

        public Usuario(string nombreUsuario, string correo, string contrasenia)
        {
            conexion = new MySqlConnection(connectionString);
            conexion.Open();
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
            return EncriptarContrasenia.EncriptarContraseña(contrasenia);
        }

        public bool AgregarUsuario(MySqlCommand command)
        {
            try
            {
                command.Parameters.Add("?nombreUsuario", MySqlDbType.VarChar).Value = getNombreUsuario();
                command.Parameters.Add("?correo", MySqlDbType.VarChar).Value = getCorreo();
                command.Parameters.Add("?contrasenia", MySqlDbType.VarChar).Value = getContraseniaEncriptada();
                command.ExecuteNonQuery();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public bool ValidarUsusario(MySqlCommand command)
        {
            Iniciado = Convert.ToInt32(command.ExecuteScalar()) <= 0;
            return Iniciado;
        }
    }
}
