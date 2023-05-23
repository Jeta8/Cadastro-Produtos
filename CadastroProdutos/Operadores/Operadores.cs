using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroProdutos
{
    public class OperadoresControl
    {
        public class cCadastrarOperador
        {
            public static void CadastrarOperador()
            {
                // Limpa a tela
                Console.Clear();

                // Mostra o título
                Console.WriteLine("Cadastrar Operador");

                // Pede o login
                Console.Write("Login: ");
                string login = Console.ReadLine();
                while (login == "")
                {
                    Console.WriteLine("O login não pode ser vazio");
                    Console.Write("Login: ");
                    login = Console.ReadLine();
                }


              

                // Pede a senha
                Console.Write("Senha: ");
                string senha = Console.ReadLine();
                while (senha == "")
                {
                    Console.WriteLine("A senha não pode ser vazia");
                    Console.Write("Senha: ");
                    senha = Console.ReadLine();
                }

                // Pede o nome do operador
                Console.Write("Nome do Operador: ");
                string nomeOperador = Console.ReadLine();
                while (nomeOperador == "")
                {
                    Console.WriteLine("O nome do operador não pode ser vazio");
                    Console.Write("Nome do Operador: ");
                    nomeOperador = Console.ReadLine();
                }

                // Pede o nível de acesso
                Console.Write("Nível de Acesso (1 - Operador  ||  2 - Administrador): ");
                string nivelAcesso = Console.ReadLine();
                while (nivelAcesso == "")
                {
                    Console.WriteLine("O nível de acesso não pode ser vazio");
                    Console.Write("Nível de Acesso (1 - Operador  ||  2 - Administrador): ");
                    nivelAcesso = Console.ReadLine();
                }

                // Verifica o nível de acesso
                NiveisAcesso nivelAcessoEnum = NiveisAcesso.Nenhum;
                switch (nivelAcesso)
                {
                    case "1":
                        nivelAcessoEnum = NiveisAcesso.Operador;
                        break;
                    case "2":
                        nivelAcessoEnum = NiveisAcesso.Administrador;
                        break;
                    default:
                        Console.WriteLine("Nível de acesso inválido!");
                        Console.ReadKey();
                        return;
                }

                // Cria o operador
                Operadores operador = new Operadores();
                operador.Login = login;
                operador.Senha = senha;
                operador.NomeOperador = nomeOperador;
                operador.NivelAcesso = nivelAcessoEnum;


                // Adiciona operador no banco de dados
                cConexao.Conectar();
                string sql = "INSERT INTO operadores (login, senha, nome_operador, nivel_acesso) VALUES ('" + login + "', '" + senha + "', '" + nomeOperador + "', '" + nivelAcesso + "')";
                MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
                cmd.ExecuteNonQuery();
                cConexao.Desconectar();


                // Mostra uma mensagem de sucesso
                if (operador.NivelAcesso != NiveisAcesso.Administrador)
                {
                    Console.WriteLine("Operador cadastrado com sucesso!");
                }
                else { Console.WriteLine("Administrador cadastrado com sucesso!"); }

                Console.ReadKey();
                Console.Clear();
            }

            public static void AdmOperador()
            {
                // Limpa a tela
                Console.Clear();

                // Mostra o título
                Console.WriteLine("Editar Informações do Operador");

                // Pede o login do operador a ser editado
                Console.Write("Login do Operador: ");
                string loginOperador = Console.ReadLine();
                while (loginOperador == "")
                {
                    Console.WriteLine("O login não pode ser vazio");
                    Console.Write("Login do Operador: ");
                    loginOperador = Console.ReadLine();
                }

                // Procura o operador pelo login no banco de dados
                Operadores operador = null;
                cConexao.Conectar();
                string sql = "SELECT * FROM operadores WHERE login = '" + loginOperador + "'";
                MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    // Se existir, armazena o operador
                    operador = new Operadores();
                    operador.Login = rdr["login"].ToString();
                    operador.Senha = rdr["senha"].ToString();
                    operador.NomeOperador = rdr["nome_operador"].ToString();
                    operador.NivelAcesso = (NiveisAcesso)int.Parse(rdr["nivel_acesso"].ToString());
                    rdr.Close();
                    cConexao.Desconectar();
                }


                // Verifica se o operador foi encontrado
                if (operador == null)
                {
                    Console.WriteLine("Operador não encontrado!");
                    Console.ReadKey();
                    return;
                }

                // Mostra as informações atuais do operador
                Console.WriteLine("Informações Atuais:");
                Console.WriteLine("Login: " + operador.Login);
                Console.WriteLine("Senha: " + operador.Senha);
                Console.WriteLine("Nome do Operador: " + operador.NomeOperador);
                Console.WriteLine("Nível de Acesso: " + operador.NivelAcesso);

                // Pede as novas informações do operador
                Console.WriteLine("Digite as Novas Informações (ou deixe em branco para manter as informações atuais):");

                Console.Write("Novo login: ");
                string novoLogin = Console.ReadLine();
                while (novoLogin == "")
                {
                    Console.WriteLine("O login não pode ser vazio");
                    Console.Write("Novo login: ");
                    novoLogin = Console.ReadLine();
                }

                Console.Write("Nova Senha: ");
                string novaSenha = Console.ReadLine();
                while (novaSenha == "")
                {
                    Console.WriteLine("A senha não pode ser vazia");
                    Console.Write("Nova Senha: ");
                    novaSenha = Console.ReadLine();
                }

                Console.Write("Novo Nome do Operador: ");
                string novoNomeOperador = Console.ReadLine();
                while (novoNomeOperador == "")
                {
                    Console.WriteLine("O nome do operador não pode ser vazio");
                    Console.Write("Novo Nome do Operador: ");
                    novoNomeOperador = Console.ReadLine();
                }

                Console.Write("Novo Nível de Acesso (1 - Operador || 2 - Administrador): ");
                string novoNivelAcesso = Console.ReadLine();
                while (novoNivelAcesso == "")
                {
                    Console.WriteLine("O nível de acesso não pode ser vazio");
                    Console.Write("Novo Nível de Acesso (1 - Operador || 2 - Administrador): ");
                    novoNivelAcesso = Console.ReadLine();
                }

                // Verifica as novas informações
                if (!string.IsNullOrEmpty(novaSenha))
                {
                    operador.Senha = novaSenha;
                }

                if (!string.IsNullOrEmpty(novoLogin))
                {
                    operador.loginTemp = novoLogin;
                }

                if (!string.IsNullOrEmpty(novoNomeOperador))
                {
                    operador.NomeOperador = novoNomeOperador;
                }

                if (!string.IsNullOrEmpty(novoNivelAcesso))
                {
                    NiveisAcesso nivelAcessoEnum = NiveisAcesso.Nenhum;
                    switch (novoNivelAcesso)
                    {
                        case "1":
                            nivelAcessoEnum = NiveisAcesso.Operador;
                            break;
                        case "2":
                            nivelAcessoEnum = NiveisAcesso.Administrador;
                            break;
                        default:
                            Console.WriteLine("Nível de acesso inválido!");
                            Console.ReadKey();
                            return;
                    }

                    operador.NivelAcesso = nivelAcessoEnum;
                }

                // Atualiza as informações do operador no banco de dados
                cConexao.Conectar();
                sql = "UPDATE operadores SET login = '" + operador.loginTemp + "', senha = '" + operador.Senha + "', nome_operador = '" + operador.NomeOperador + "', nivel_acesso = '" + (int)operador.NivelAcesso + "' WHERE login = '" + operador.Login + "'";
                cmd = new MySqlCommand(sql, cConexao.conexao);
                cmd.ExecuteNonQuery();
                cConexao.Desconectar();


                Console.WriteLine("Informações do Operador atualizadas com sucesso!");
                Console.ReadKey();
                Console.Clear();
            }

            public static void ListarOperadores()
            {
                Console.Clear();
                // Conecta ao banco de dados
                cConexao.Conectar();

                // Define a consulta SQL para recuperar os operadores
                string sql = "SELECT * FROM operadores";

                // Cria o objeto MySqlCommand
                MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);

                // Executa a consulta SQL e obtém um objeto MySqlDataReader para ler os resultados
                MySqlDataReader reader = cmd.ExecuteReader();

                // Exibe os operadores
                Console.WriteLine("Operadores cadastrados:");
                Console.WriteLine("-----------------------");
                while (reader.Read())
                {
                    string login = reader.GetString("login");
                    string senha = reader.GetString("senha");
                    string nomeOperador = reader.GetString("nome_operador");
                    string nivelAcesso = reader.GetString("nivel_acesso");

                    Console.WriteLine("Login: " + login);
                    Console.WriteLine("Senha: " + senha);
                    Console.WriteLine("Nome do Operador: " + nomeOperador);
                    Console.WriteLine("Nível de Acesso: " + nivelAcesso);
                    Console.WriteLine("-----------------------");


                }

                // Fecha o reader
                Console.ReadKey();
                reader.Close();

                // Desconecta do banco de dados
                cConexao.Desconectar();
            }
        }
    }
}
