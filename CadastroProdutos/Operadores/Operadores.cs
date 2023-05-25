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
                Console.Clear();

                Console.WriteLine("Cadastrar Operador");


                Console.Write("Login: ");
                string login = Console.ReadLine();
                while (login == "")
                {
                    Console.WriteLine("O login não pode ser vazio");
                    Console.Write("Login: ");
                    login = Console.ReadLine();
                }
                try
                {
                    cConexao.Conectar();
                    string sql = "SELECT * FROM operadores WHERE login = '" + login + "'";
                    MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        Console.WriteLine("Já existe um operador cadastrado com esse login");
                        Console.ReadKey();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao cadastrar operador: " + ex.Message);
                    Console.ReadKey();
                    return;
                }
                finally
                {
                    cConexao.Desconectar();
                }



                Console.Write("Senha: ");
                string senha = Console.ReadLine();
                while (senha == "")
                {
                    Console.WriteLine("A senha não pode ser vazia");
                    Console.Write("Senha: ");
                    senha = Console.ReadLine();
                }

                Console.Write("Nome do Operador: ");
                string nomeOperador = Console.ReadLine();
                while (nomeOperador == "")
                {
                    Console.WriteLine("O nome do operador não pode ser vazio");
                    Console.Write("Nome do Operador: ");
                    nomeOperador = Console.ReadLine();
                }

                Console.Write("Nível de Acesso (1 - Operador  ||  2 - Administrador): ");
                string nivelAcesso = Console.ReadLine();
                while (nivelAcesso == "")
                {
                    Console.WriteLine("O nível de acesso não pode ser vazio");
                    Console.Write("Nível de Acesso (1 - Operador  ||  2 - Administrador): ");
                    nivelAcesso = Console.ReadLine();
                }

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

                Operadores operador = new Operadores();
                operador.Login = login;
                operador.Senha = senha;
                operador.NomeOperador = nomeOperador;
                operador.NivelAcesso = nivelAcessoEnum;

                try
                {
                    cConexao.Conectar();
                    string sql = "INSERT INTO operadores (login, senha, nome_operador, nivel_acesso) VALUES ('" + login + "', '" + senha + "', '" + nomeOperador + "', '" + nivelAcesso + "')";
                    MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
                    cmd.ExecuteNonQuery();
                    cConexao.Desconectar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao cadastrar operador: " + ex.Message);
                    Console.ReadKey();
                    return;
                }



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
                Console.Clear();

                Console.WriteLine("Editar Informações do Operador");

                Console.Write("Login do Operador: ");
                string loginOperador = Console.ReadLine();
                while (loginOperador == "")
                {
                    Console.WriteLine("O login não pode ser vazio");
                    Console.Write("Login do Operador: ");
                    loginOperador = Console.ReadLine();
                }

                Operadores operador = null;
                try
                {
                    cConexao.Conectar();
                    string sql = "SELECT * FROM operadores WHERE login = '" + loginOperador + "'";
                    MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        operador = new Operadores();
                        operador.Login = rdr["login"].ToString();
                        operador.Senha = rdr["senha"].ToString();
                        operador.NomeOperador = rdr["nome_operador"].ToString();
                        operador.NivelAcesso = (NiveisAcesso)int.Parse(rdr["nivel_acesso"].ToString());
                        rdr.Close();
                        cConexao.Desconectar();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao editar operador: " + ex.Message);
                    Console.ReadKey();
                    return;
                }
                finally
                {
                    cConexao.Desconectar();
                }



                if (operador == null)
                {
                    Console.WriteLine("Operador não encontrado!");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Informações Atuais:");
                Console.WriteLine("Login: " + operador.Login);
                Console.WriteLine("Senha: " + operador.Senha);
                Console.WriteLine("Nome do Operador: " + operador.NomeOperador);
                Console.WriteLine("Nível de Acesso: " + operador.NivelAcesso);

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

                try
                {
                    cConexao.Conectar();
                    string sql = "UPDATE operadores SET login = '" + operador.loginTemp + "', senha = '" + operador.Senha + "', nome_operador = '" + operador.NomeOperador + "', nivel_acesso = '" + (int)operador.NivelAcesso + "' WHERE login = '" + operador.Login + "'";
                    MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
                    cmd.ExecuteNonQuery();
                    cConexao.Desconectar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao editar operador: " + ex.Message);
                    Console.ReadKey();
                    return;
                }
                finally
                {
                    cConexao.Desconectar();
                }

            }

            public static void ListarOperadores()
            {
                Console.Clear();

                try
                {
                    cConexao.Conectar();

                    string sql = "SELECT * FROM operadores";

                    MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);

                    MySqlDataReader reader = cmd.ExecuteReader();

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

                    Console.ReadKey();
                    reader.Close();

                    cConexao.Desconectar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao listar operadores: " + ex.Message);
                    Console.ReadKey();
                    return;
                }
                finally
                {
                    cConexao.Desconectar();
                }
            }
        }
    }
}
