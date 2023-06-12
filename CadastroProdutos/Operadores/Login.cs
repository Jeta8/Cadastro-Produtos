using MySql.Data.MySqlClient;
using System;

namespace CadastroProdutos
{
    internal class Login
    {
        public class cLogin
        {
            public static void Login()
            {
                Console.Clear();

                Console.WriteLine("Login");

                Console.Write("Login: ");
                string login = Console.ReadLine();

                Console.Write("Senha: ");
                string senha = Console.ReadLine();

                try
                {
                    cConexao.Conectar();
                    string sql = "SELECT * FROM operadores WHERE login = @login AND senha = @senha";
                    using (MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao))
                    {
                        cmd.Parameters.AddWithValue("@login", login);
                        cmd.Parameters.AddWithValue("@senha", senha);
                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();

                                Operadores operador = new Operadores();

                                operador.Login = rdr.GetString("login");
                                operador.Senha = rdr.GetString("senha");
                                operador.NomeOperador = rdr.GetString("nome_operador");
                                operador.NivelAcesso = (NiveisAcesso)Convert.ToInt32(rdr["nivel_acesso"]);

                                App.OperadorLogado = operador;

                                Console.Clear();
                                Console.WriteLine("Login efetuado com sucesso");
                                Console.ReadKey();

                                cMenuPrincipal.MenuPrincipal();
                            }
                            else
                            {
                                Console.WriteLine("Login ou senha incorretos");
                                Console.ReadKey();
                                return;
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Erro ao efetuar o login: " + ex.Message);
                    Console.ReadKey();
                    return;
                }
                finally
                {
                    cConexao.Desconectar();
                }
            }

            public static void Logout()
            {
                if (App.OperadorLogado != null)
                {
                    App.OperadorLogado = null;
                    Console.Clear();
                    Console.WriteLine("Você saiu do sistema com sucesso");
                    Console.ReadKey();
                    Login();
                }
            }
        }
    }
}
