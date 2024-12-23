﻿using MySql.Data.MySqlClient;

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

        public Usuario()
        {
            conexion = new MySqlConnection(connectionString);
        }
        public Usuario(int id,string nombreUsuario, string correo, string contrasenia)
        {
            conexion = new MySqlConnection(connectionString);
            conexion.Open();
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

        public void AbrirConection()
        {
            conexion = new MySqlConnection(connectionString);
            conexion.Open();
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
            }
            catch (Exception)
            {
                return false;
            }
        }

        public (bool,string?) ValidarUsusario(MySqlCommand command)
        {
            string nombreUsuario = "";

            using(MySqlDataReader reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    nombreUsuario = reader.GetString(1);
                    if(nombreUsuario != null)
                    {
                        Iniciado = true;
                    }
                }
                return (Iniciado, nombreUsuario);
            }
            
        }

        public string CambiarContrasenia()
        {
            if (!ValidarCorreo())
            {
                return "El correo no existe";
            }
            string query = "update blog_prueba.usuarios set contrasenia = ?contraseniaNueva where correo = ?correo;";
            try
            {
                conexion.Open();
                using (MySqlCommand command = new MySqlCommand(query, conexion))
                {
                    command.Parameters.Add("?contraseniaNueva", MySqlDbType.VarChar).Value = getContraseniaEncriptada();
                    command.Parameters.Add("?correo", MySqlDbType.VarChar).Value = getCorreo();
                    command.ExecuteNonQuery();
                    return "Cambio exitoso";
                }    
            }catch(MySqlException e)
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
                    command.Parameters.Add("?correo", MySqlDbType.VarChar).Value = getCorreo();
             
                    using(MySqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            if(reader.GetInt32(0) == 1)
                            {
                                return true;
                            }
                        }
                    }
                }

               
            } catch (MySqlException) {
                return false;
            } finally {
                conexion.Close();
            }
            return false;
        }
    }
}
