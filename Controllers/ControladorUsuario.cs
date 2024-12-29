using System.Data;
using MySql.Data.MySqlClient;
using Proyecto_Blog.Models;

namespace Proyecto_Blog.Controllers
{
    public class ControladorUsuario
    {
        private MySqlConnection? conexion = null;
        private CadenaConexion? cadena = new CadenaConexion();
        private Usuario? usuario { get; set; } = null;

        public ControladorUsuario(Usuario usuario)
        {
            this.usuario = usuario;
            conexion = new MySqlConnection(cadena.ObtenerConexion());
        }
        public void InicializarConexion()
        {
            conexion = new MySqlConnection(cadena?.ObtenerConexion());
            if (conexion != null)
            {
                conexion.Open();
            }
        }
        public void CerrarConexion()
        {
            if (conexion != null && conexion.State == ConnectionState.Open)
            {
                conexion.Close();
            }
        }

        public async Task<bool> AgregarUsuario()
        {
            InicializarConexion();
            string query = "insert into blog_prueba.usuarios(nombreUsuario,correo,contrasenia) values (?nombreUsuario,?correo,?contrasenia);";
            MySqlCommand command = new MySqlCommand(query, conexion);
            try
            {
                command.Parameters.Add("?nombreUsuario", MySqlDbType.VarChar).Value = usuario?.getNombreUsuario();
                command.Parameters.Add("?correo", MySqlDbType.VarChar).Value = usuario?.getCorreo();
                command.Parameters.Add("?contrasenia", MySqlDbType.VarChar).Value = usuario?.getContraseniaEncriptada();
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }finally
            {
                CerrarConexion();
            }
        }

        public (bool, string, int) ValidarUsusario()
        {
            InicializarConexion();
            string query = $"select * from blog_prueba.usuarios where correo = ?correo and contrasenia  = ?contrasenia;";
            MySqlCommand command = new MySqlCommand(query, conexion);

            command.Parameters.Add("?correo", MySqlDbType.VarChar).Value = usuario?.getCorreo();
            command.Parameters.Add("?contrasenia", MySqlDbType.VarChar).Value = usuario?.getContraseniaEncriptada();
            string nombreUsuario = "";
            int id = 0;
            try
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                        nombreUsuario = reader.GetString(1);
                        if (nombreUsuario != null)
                        {
                            usuario?.setIniciado(true);
                        }
                    }
                    return (usuario?.getIniciado() ?? false, nombreUsuario ?? string.Empty, id);
                }
            }
            catch (MySqlException ex)
            {
                return (false, ex.Message, 0);
            }
            catch (InvalidOperationException ex)
            {
                return (false, ex.Message, 0);
            }
            finally
            {
                CerrarConexion();
            }
        }

        public async Task<string> CambiarContrasenia()
        {
            if (!ValidarCorreo())
            {
                return "El correo no existe";
            }
            string query = "update blog_prueba.usuarios set contrasenia = ?contraseniaNueva where correo = ?correo;";
            try
            {
                using (MySqlCommand command = new MySqlCommand(query, conexion))
                {
                    command.Parameters.Add("?contraseniaNueva", MySqlDbType.VarChar).Value = usuario?.getContraseniaEncriptada();
                    command.Parameters.Add("?correo", MySqlDbType.VarChar).Value = usuario?.getCorreo();
                    await command.ExecuteNonQueryAsync();
                    return "Cambio exitoso";
                }
            }
            catch (MySqlException e)
            {
                return e.Message;
            }
        }

        private bool ValidarCorreo()
        {
            string query = $"select count(*) from blog_prueba.usuarios where correo = ?correo";
            try
            {
                using (MySqlCommand command = new MySqlCommand(query, conexion))
                {
                    command.Parameters.Add("?correo", MySqlDbType.VarChar).Value = usuario?.getCorreo();

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
            catch (MySqlException)
            {
                return false;
            }
            return false;
        }
        public async Task<Usuario?> ObtenerDatos(int id)
        {
            InicializarConexion();
            string query = $"select * from blog_prueba.usuarios where id = ?id;";
            Usuario? usuario = new Usuario();
            try
            {
                using (MySqlCommand command = new MySqlCommand(query, conexion))
                {
                    command.Parameters.Add("?id", MySqlDbType.Int32).Value = id;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            usuario?.setId(reader.GetInt32(0));
                            usuario?.setNombreUsuario(reader.GetString(1));
                            usuario?.setCorreo(reader.GetString(2));
                            usuario?.setContrasenia(reader.GetString(3));
                            usuario?.setIniciado(true);
                        }
                    }
                }
            }
            catch (MySqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            finally
            {
                CerrarConexion();
            }
            return usuario;
        }
    }
}
