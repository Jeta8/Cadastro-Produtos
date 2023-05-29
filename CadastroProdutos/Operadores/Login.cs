using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroProdutos
{
    internal class Login
    {
        public class cLogin
        {
            public static void Login()
            {
                // Limpa a tela
                Console.Clear();

                // Mostra o título
                Console.WriteLine("Login");

                // Pede o login
                Console.Write("Login: ");
                string login = Console.ReadLine();

                // Pede a senha
                Console.Write("Senha: ");
                string senha = Console.ReadLine();

                cConexao.Conectar();
                string sql = "SELECT * FROM operadores WHERE login = @login AND senha = @senha";
                MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@senha", senha);
                MySqlDataReader rdr = cmd.ExecuteReader();

                // Se o usuario existe
                if (rdr.HasRows)
                {
                    // Ler o usuario
                    rdr.Read();

                    // Cria um objeto do tipo Operadores
                    Operadores operador = new Operadores();

                    // Preenche o objeto com os dados do banco de dados
                    operador.Login = rdr["login"].ToString();
                    operador.Senha = rdr["senha"].ToString();
                    operador.NomeOperador = rdr["nome_operador"].ToString();
                    operador.NivelAcesso = (NiveisAcesso)Convert.ToInt32(rdr["nivel_acesso"]);

                    // Define o operador logado
                    App.OperadorLogado = operador;

                    // Desconecta do banco de dados
                    cConexao.Desconectar();

                    // Limpa a tela
                    Console.Clear();

                    // Mostra uma mensagem de sucesso
                    Console.WriteLine("Login efetuado com sucesso");
                    Console.ReadKey();

                    // Chama o menu principal
                    cMenuPrincipal.MenuPrincipal();
                }
                else
                {
                    // Desconecta do banco de dados
                    cConexao.Desconectar();

                    // Mostra uma mensagem de erro
                    Console.WriteLine("Login ou senha incorretos");
                    Console.ReadKey();
                    return;
                   
                }
                
            }

            public static void Logout()
            {
                if (App.OperadorLogado != null)
                {
                    App.OperadorLogado = null;
                    Console.Clear();
                    Console.WriteLine("Voce saiu do sistema com sucesso");
                    Console.ReadKey();
                    Login();
                }
            }
        }
    }
}
