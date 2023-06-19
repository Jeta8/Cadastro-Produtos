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

                string login, senha, nomeOperador, nivelAcesso;
                while (true)
                {
                    Console.Write("Login: ");
                    login = Console.ReadLine().Trim();
                    if (string.IsNullOrEmpty(login))
                    {
                        Console.WriteLine("O login não pode ser vazio");
                        continue;
                    }

                    try
                    {
                        // Aqui a conexão é aberta para verificar o login do operador, mas nunca é fechada.
                        cConexao.Conectar();

                        string sql = "SELECT COUNT(*) FROM operadores WHERE login = @login";
                        MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
                        cmd.Parameters.AddWithValue("@login", login);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count > 0)
                        {
                            Console.WriteLine("Já existe um operador cadastrado com esse login");
                            Console.ReadKey();
                            return;
                        }

                        // Correção:
                        cConexao.Desconectar();
                    }
                    catch (Exception ex)
                    {
                        cConexao.Desconectar();
                        Console.WriteLine("Erro ao cadastrar operador: " + ex.Message);
                        Console.ReadKey();
                        return;
                    }


                    break;
                }

                while (true)
                {
                    Console.Write("Senha: ");
                    senha = Console.ReadLine();
                    if (string.IsNullOrEmpty(senha))
                    {
                        Console.WriteLine("A senha não pode ser vazia");
                        continue;
                    }
                    break;
                }

                while (true)
                {
                    Console.Write("Nome do Operador: ");
                    nomeOperador = Console.ReadLine().Trim();
                    if (string.IsNullOrEmpty(nomeOperador) || !nomeOperador.All(char.IsLetter))
                    {
                        Console.WriteLine("O nome do operador não pode ser vazio");
                        continue;
                    }
                    break;
                }

                while (true)
                {
                    Console.Write("Nível de Acesso (1 - Operador || 2 - Administrador): ");
                    nivelAcesso = Console.ReadLine();
                    if (string.IsNullOrEmpty(nivelAcesso))
                    {
                        Console.WriteLine("O nível de acesso não pode ser vazio");
                        continue;
                    }
                    if (nivelAcesso != "1" && nivelAcesso != "2")
                    {
                        Console.WriteLine("Nível de acesso inválido!");
                        continue;
                    }
                    break;
                }

                NiveisAcesso nivelAcessoEnum = nivelAcesso == "1" ? NiveisAcesso.Operador : NiveisAcesso.Administrador;
                Operadores operador = new Operadores
                {
                    Login = login,
                    Senha = senha,
                    NomeOperador = nomeOperador,
                    NivelAcesso = nivelAcessoEnum
                };

                try
                {
                    cConexao.Conectar();
                    string sql = "INSERT INTO operadores (login, senha, nome_operador, nivel_acesso) VALUES (@login, @senha, @nomeOperador, @nivelAcesso)";
                    MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@senha", senha);
                    cmd.Parameters.AddWithValue("@nomeOperador", nomeOperador);
                    cmd.Parameters.AddWithValue("@nivelAcesso", nivelAcesso);
                    cmd.ExecuteNonQuery();
                    cConexao.Desconectar();
                }
                catch (Exception ex)
                {
                    cConexao.Desconectar();
                    Console.WriteLine("Erro ao cadastrar operador: " + ex.Message);
                    Console.ReadKey();
                    return;
                }

                if (operador.NivelAcesso != NiveisAcesso.Administrador)
                {
                    Console.WriteLine("Operador cadastrado com sucesso!");
                }
                else
                {
                    Console.WriteLine("Administrador cadastrado com sucesso!");
                }

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
                    else
                    {
                        rdr.Close();
                        cConexao.Desconectar();
                        Console.WriteLine("Operador não encontrado!");
                        Console.ReadKey();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    cConexao.Desconectar();
                    Console.WriteLine("Erro ao editar operador: " + ex.Message);
                    Console.ReadKey();
                    return;
                }
                finally
                {
                    cConexao.Desconectar();
                }

                Console.WriteLine("Informações Atuais:");
                Console.WriteLine("Login: " + operador.Login);
                Console.WriteLine("Senha: " + operador.Senha);
                Console.WriteLine("Nome do Operador: " + operador.NomeOperador);
                Console.WriteLine("Nível de Acesso: " + operador.NivelAcesso);

                Console.WriteLine("Digite as Novas Informações (ou deixe em branco para manter as informações atuais):");

                Console.Write("Novo login: ");
                string novoLogin = Console.ReadLine();
                if (!string.IsNullOrEmpty(novoLogin))
                {
                    // Verificar se o novo login já existe no banco de dados
                    try
                    {
                        cConexao.Conectar();
                        string sql = "SELECT * FROM operadores WHERE login = '" + novoLogin + "'";
                        MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            rdr.Close();
                            cConexao.Desconectar();
                            Console.WriteLine("Já existe um operador cadastrado com esse login");
                            Console.ReadKey();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        cConexao.Desconectar();
                        Console.WriteLine("Erro ao verificar login: " + ex.Message);
                        Console.ReadKey();
                        return;
                    }
                    finally
                    {
                        cConexao.Desconectar();
                    }
                }

                Console.Write("Nova Senha: ");
                string novaSenha = Console.ReadLine();

                Console.Write("Novo Nome do Operador: ");
                string novoNomeOperador = Console.ReadLine();
                if (!string.IsNullOrEmpty(novoNomeOperador))
                {
                    // Verificar se o novo nome do operador já existe no banco de dados
                    try
                    {
                        cConexao.Conectar();
                        string sql = "SELECT * FROM operadores WHERE nome_operador = '" + novoNomeOperador + "'";
                        MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            rdr.Close();
                            cConexao.Desconectar();
                            Console.WriteLine("Já existe um operador cadastrado com esse nome");
                            Console.ReadKey();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        cConexao.Desconectar();
                        Console.WriteLine("Erro ao verificar nome do operador: " + ex.Message);
                        Console.ReadKey();
                        return;
                    }
                    finally
                    {
                        cConexao.Desconectar();
                    }
                }

                Console.Write("Novo Nível de Acesso (1 - Operador || 2 - Administrador): ");
                string novoNivelAcesso = Console.ReadLine();

                if (!string.IsNullOrEmpty(novaSenha))
                {
                    operador.Senha = novaSenha;
                }

                if (!string.IsNullOrEmpty(novoLogin))
                {
                    operador.Login = novoLogin;
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
                    string sql = "UPDATE operadores SET login = '" + operador.Login + "', senha = '" + operador.Senha + "', nome_operador = '" + operador.NomeOperador + "', nivel_acesso = '" + (int)operador.NivelAcesso + "' WHERE login = '" + loginOperador + "'";
                    MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
                    cmd.ExecuteNonQuery();
                    cConexao.Desconectar();
                }
                catch (Exception ex)
                {
                    cConexao.Desconectar();
                    Console.WriteLine("Erro ao editar operador: " + ex.Message);
                    Console.ReadKey();
                    return;
                }
                finally
                {
                    cConexao.Desconectar();
                }

                if (loginOperador == operador.Login)
                {
                    Console.WriteLine("Operador editado com sucesso! Continua logado com o login anterior.");
                }
                else
                {
                    Console.WriteLine("Operador editado com sucesso!");
                }

                Console.ReadKey();
                Console.Clear();
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
                    cConexao.Desconectar();
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
