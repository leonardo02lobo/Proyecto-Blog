using MySql.Data.MySqlClient;
using Proyecto_Blog.Models;

namespace Proyecto_Blog.Controllers
{
    public class ControladorUsuario
    {
        private MySqlConnection? conexion = null;
        private CadenaConexion? cadena = new CadenaConexion();
        Usuario usuario { get; set; } = null;

        public ControladorUsuario(Usuario usuario)
        {
            this.usuario = usuario;
            conexion = new MySqlConnection(cadena.ObtenerConexion());
        }

        public void InicializarConexion()
        {
            conexion = new MySqlConnection(cadena.ObtenerConexion());
        }

        public async Task<bool> AgregarUsuario()
        {
            conexion.Open();
            string query = "insert into blog_prueba.usuarios(nombreUsuario,correo,contrasenia) values (?nombreUsuario,?correo,?contrasenia);";
            MySqlCommand command = new MySqlCommand(query, conexion);
            try
            {
                command.Parameters.Add("?nombreUsuario", MySqlDbType.VarChar).Value = usuario.getNombreUsuario();
                command.Parameters.Add("?correo", MySqlDbType.VarChar).Value = usuario.getCorreo();
                command.Parameters.Add("?contrasenia", MySqlDbType.VarChar).Value = usuario.getContraseniaEncriptada();
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                conexion.Close();
            }
        }

        public (bool, string) ValidarUsusario()
        {
            conexion.Open();
            string query = $"select * from blog_prueba.usuarios where correo = ?correo and contrasenia  = ?contrasenia;";
            MySqlCommand command = new MySqlCommand(query, conexion);

            command.Parameters.Add("?correo", MySqlDbType.VarChar).Value = usuario.getCorreo();
            command.Parameters.Add("?contrasenia", MySqlDbType.VarChar).Value = usuario.getContraseniaEncriptada();
            string nombreUsuario = "";
            try
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        nombreUsuario = reader.GetString(1);
                        if (nombreUsuario != null)
                        {
                            usuario.setIniciado(true);
                        }
                    }
                    return (usuario.getIniciado(), nombreUsuario);
                }
            }
            catch(MySqlException ex)
            {
                return (false, ex.Message);
            }finally { conexion.Close(); }
            
        }

        public async Task<string> CambiarContrasenia()
        {
            if (!ValidarCorreo())
            {
                return "El correo no existe";
            }
            conexion.Open();
            string query = "update blog_prueba.usuarios set contrasenia = ?contraseniaNueva where correo = ?correo;";
            try
            {
                conexion.Open();
                using (MySqlCommand command = new MySqlCommand(query, conexion))
                {
                    command.Parameters.Add("?contraseniaNueva", MySqlDbType.VarChar).Value = usuario.getContraseniaEncriptada();
                    command.Parameters.Add("?correo", MySqlDbType.VarChar).Value = usuario.getCorreo();
                    await command.ExecuteNonQueryAsync();
                    return "Cambio exitoso";
                }
            }
            catch (MySqlException e)
            {
                return e.Message;
            }
            finally
            {
                conexion.Close();
            }
        }

        private bool ValidarCorreo()
        {
            string query = $"select count(*) from blog_prueba.usuarios where correo = ?correo";
            try
            {
                using (MySqlCommand command = new MySqlCommand(query, conexion))
                {
                    command.Parameters.Add("?correo", MySqlDbType.VarChar).Value = usuario.getCorreo();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetInt32(0) == 1)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                return false;
            }
            finally
            {
                conexion.Close();
            }
            return false;
        }
    }
}
