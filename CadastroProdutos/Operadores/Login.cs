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

                // Verifica se o usuario existe no banco de dados

                cConexao.Conectar();
                string sql = "SELECT * FROM operadores WHERE login = '" + login + "' AND senha = '" + senha + "'";
                MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    // Se existir, armazena o operador
                    App.OperadorLogado = new Operadores();
                    App.OperadorLogado.Login = rdr["login"].ToString();
                    App.OperadorLogado.Senha = rdr["senha"].ToString();
                    App.OperadorLogado.NomeOperador = rdr["nome_operador"].ToString();
                    App.OperadorLogado.NivelAcesso = (NiveisAcesso)int.Parse(rdr["nivel_acesso"].ToString());
                    rdr.Close();
                    cConexao.Desconectar();
                    return;
                }
                rdr.Close();
                cConexao.Desconectar();

                Console.WriteLine("Login ou senha incorretos!");
                Console.ReadKey();
                Console.Clear();
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
