using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace CadastroProdutos
{
    public enum NiveisAcesso
    {
        Nenhum,
        Operador,
        Administrador
    }

    public class Operadores
    {
        public string Login = "";
        public string loginTemp = "";
        public string Senha = "";
        public string NomeOperador = "";
        public NiveisAcesso NivelAcesso = NiveisAcesso.Nenhum;
    }

    public class Mercado
    {
        public string NomeProduto = "";
        public string CodigoBarras = "";
        public decimal PrecoProduto = 0;
        public int EstoqueProduto = 0;
    }

    public class App
    {
        public static Operadores[] Operadores = new Operadores[10];
        public static Mercado[] Mercado = new Mercado[100];
        public static int QuantidadeOperadores = 0;
        public static int QuantidadeProdutos = 0;
        public static Operadores OperadorLogado = null;

        public static void Main()
        {

            // Loop principal do sistema
            while (true)
            {
                // Verifica se o operador está logado
                if (OperadorLogado == null)
                {
                    // Se não estiver, faz o login
                    Console.WriteLine("Fazer login");
                    Console.WriteLine("1 - Fazer login");
                    Console.WriteLine("2 - Cadastrar Operador");
                    Console.Write("Opção: ");
                    string opcao = Console.ReadLine();
                    switch (opcao)
                    {
                        case "1":
                            cLogin.Login();
                            break;
                        case "2":
                            cCadastrarOperador.CadastrarOperador();
                            break;
                        default:
                            Console.WriteLine("Opção inválida!");
                            Console.ReadKey();
                            break;
                    }

                }
                else
                {
                    // Se estiver, mostra o menu principal
                    Console.Clear();
                    cMenuPrincipal.MenuPrincipal();
                }
            }
        }


    }

    // Path: Login.cs

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

            // SQL INJECT FÁCIL AQUI
            string sql = "SELECT * FROM operadores WHERE login = '" + login + "' AND senha = '" + senha + "'";
            MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);

            // E se ocorrer uma falha na consulta? aonde tá o tratamento desse erro?
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


            //// Verifica se o login e a senha estão corretos
            //for (int i = 0; i < App.QuantidadeOperadores; i++)
            //{
            //    if (App.Operadores[i].Login == login && App.Operadores[i].Senha == senha)
            //    {
            //        // Se estiverem, faz o login
            //        App.OperadorLogado = App.Operadores[i];
            //        return;
            //    }
            //}

            // Se não estiverem, mostra uma mensagem de erro
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



    // Path: MenuPrincipal.cs

    public class cMenuPrincipal
    {
        public static void MenuPrincipal()
        {
            // Limpa a tela
            Console.Clear();

            // Mostra o título
            Console.WriteLine("Menu Principal");

            // Mostra o nome do operador logado
            Console.WriteLine("Operador: " + App.OperadorLogado.NomeOperador);

            // Mostra o menu
            if (App.OperadorLogado.NivelAcesso == NiveisAcesso.Administrador)
            {
                Console.WriteLine("1 - Cadastrar Operador");
                Console.WriteLine("2 - Editar Operadores");
                Console.WriteLine("3 - Cadastrar Produto");
                Console.WriteLine("4 - Listar Produtos");
                Console.WriteLine("5 - Vender Produto");
                Console.WriteLine("6 - Listar Operadores");
                Console.WriteLine("7 - Sair");
            }
            else
            {
                Console.WriteLine("1 - Listar Produtos");
                Console.WriteLine("2 - Vender Produto");
                Console.WriteLine("3 - Sair");
            }



            // Pede a opção
            Console.Write("Opção: ");
            string opcao = Console.ReadLine();

            // Verifica a opção
            if (App.OperadorLogado.NivelAcesso == NiveisAcesso.Administrador)
            {
                switch (opcao)
                {

                    case "1":
                        cCadastrarOperador.CadastrarOperador();
                        break;
                    case "2":
                        cCadastrarOperador.AdmOperador();
                        break;
                    case "3":
                        cCadastrarProduto.CadastrarProduto();
                        break;
                    case "4":
                        cListarProdutos.ListarProdutos();
                        break;
                    case "5":
                        cVenderProduto.VenderProduto();
                        break;
                    case "6":
                        cCadastrarOperador.ListarOperadores();
                        break;

                    case "7":
                        cLogin.Logout();
                        break;

                    default:
                        Console.WriteLine("Opção inválida!");
                        Console.ReadKey();
                        break;
                }
            }
            else
            {
                switch (opcao)
                {
                    case "1":
                        cListarProdutos.ListarProdutos();
                        break;
                    case "2":
                        cVenderProduto.VenderProduto();
                        break;
                    case "3":
                        cLogin.Logout();
                        break;

                    default:
                        Console.WriteLine("Opção inválida!");
                        Console.ReadKey();
                        break;
                }
            }

        }
    }

    // Path: CadastrarOperador.cs

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

            // Verifica se o login já existe
            for (int i = 0; i < App.QuantidadeOperadores; i++)
            {
                if (App.Operadores[i].Login == login)
                {
                    // Se existir, mostra uma mensagem de erro
                    Console.WriteLine("Login já existe!");
                    Console.ReadKey();
                    return;
                }
            }

            // Pede a senha
            Console.Write("Senha: ");
            string senha = Console.ReadLine();

            // Pede o nome do operador
            Console.Write("Nome do Operador: ");
            string nomeOperador = Console.ReadLine();

            // Pede o nível de acesso
            Console.Write("Nível de Acesso (1 - Operador  ||  2 - Administrador): ");
            string nivelAcesso = Console.ReadLine();

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

            // Adiciona o operador no array de operadores
            App.Operadores[App.QuantidadeOperadores] = operador;
            App.QuantidadeOperadores++;

            // Adiciona operador no banco de dados
            cConexao.Conectar();
            string sql = "INSERT INTO operadores (login, senha, nome_operador, nivel_acesso) VALUES ('" + login + "', '" + senha + "', '" + nomeOperador + "', '" + nivelAcesso + "')";
            MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
            
            // Executa a query mas não existe validação nenhuma
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

            // Procura o operador pelo login no banco de dados
            Operadores operador = null;
            cConexao.Conectar();

            // SQL inject..
            string sql = "SELECT * FROM operadores WHERE login = '" + loginOperador + "'";
            MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);

            // Sem tratamento
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

            Console.Write("Nova Senha: ");
            string novaSenha = Console.ReadLine();

            Console.Write("Novo Nome do Operador: ");
            string novoNomeOperador = Console.ReadLine();

            Console.Write("Novo Nível de Acesso (1 - Operador || 2 - Administrador): ");
            string novoNivelAcesso = Console.ReadLine();

            // Verifica as novas informações
            if (!string.IsNullOrEmpty(novaSenha))
            {
                operador.Senha = novaSenha;
            }

            if(!string.IsNullOrEmpty(novoLogin)) { 
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

            // Sem tratamento
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

            // Sem tratamento
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

    // Path: CadastrarProduto.cs

    public class cCadastrarProduto
    {
        public static void CadastrarProduto()
        {
            // Limpa a tela
            Console.Clear();

            // Mostra o título
            Console.WriteLine("Cadastrar Produto");

            // Pede o nome do produto
            Console.Write("Nome do Produto: ");
            string nomeProduto = Console.ReadLine();

            // Verifica se o produto já existe
            for (int i = 0; i < App.QuantidadeProdutos; i++)
            {
                if (App.Mercado[i].NomeProduto == nomeProduto)
                {
                    // Se existir, mostra uma mensagem de erro
                    Console.WriteLine("Produto já existe!");

                    Console.ReadKey();
                    return;
                }
            }

            // Pede o código de barras
            Console.Write("Código de Barras: ");
            string codigoBarras = Console.ReadLine();

            for (int i = 0; i < App.QuantidadeProdutos; i++)
            {
                if (App.Mercado[i].CodigoBarras == codigoBarras)
                {
                    Console.WriteLine("Codigo de barras ja registrado!");
                    Console.WriteLine(" Deseja cadastrar o produto mas com outro codigo de barras? (S/N)");
                    string opcao = Console.ReadLine();
                    if (opcao.ToUpper() == "S")
                    {
                        Console.Write("Código de Barras: ");
                        codigoBarras = Console.ReadLine();
                        continue;
                    }
                    else if (opcao.ToUpper() == "N")
                    {
                        return;
                    }
                }
            }

            // Pede o preço do produto
            Console.Write("Preço do Produto: ");
            string precoProduto = Console.ReadLine();

            // Pede o estoque do produto
            Console.Write("Estoque do Produto: ");
            string estoqueProduto = Console.ReadLine();

            // Cria o produto
            Mercado produto = new Mercado();
            produto.NomeProduto = nomeProduto;
            produto.CodigoBarras = codigoBarras;
            produto.PrecoProduto = decimal.Parse(precoProduto);
            produto.EstoqueProduto = int.Parse(estoqueProduto);

            // Adiciona o produto no array de produtos
            App.Mercado[App.QuantidadeProdutos] = produto;
            App.QuantidadeProdutos++;

            // Adiciona produto no banco de dados
            cConexao.Conectar();
            string sql = "INSERT INTO produtos_cadastrados (nome_produto, codigo_barras, preco_produto, estoque_produto) VALUES ('" + nomeProduto + "', '" + codigoBarras + "', '" + precoProduto + "', '" + estoqueProduto + "')";
            MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);

            // Sem tratamento
            cmd.ExecuteNonQuery();

            cConexao.Desconectar();

            // Mostra uma mensagem de sucesso
            Console.WriteLine("Produto cadastrado com sucesso!\n");
            Console.WriteLine("Nome do Produto: " + nomeProduto);
            Console.WriteLine("Código de Barras: " + codigoBarras);
            Console.WriteLine("Preço do Produto: " + precoProduto);
            Console.WriteLine("Estoque do Produto: " + estoqueProduto);
            Console.WriteLine();
            Console.WriteLine("Deseja cadastrar um novo produto? (S/N)");
            string confirmacao = Console.ReadLine();

            // Verifica a confirmação
            if (confirmacao.ToUpper() == "S")
            {
                Console.Clear();
                cCadastrarProduto.CadastrarProduto();

            }
            else
            {
                Console.WriteLine("Esses foram os produtos cadastrados ate o momento:");
                cListarProdutos.ListarProdutos();
                Console.ReadKey();
                Console.Clear();
                cMenuPrincipal.MenuPrincipal();
            }
        }
    }

    // Path: ListarProdutos.cs

    public class cListarProdutos
    {
        public static void ListarProdutos()
        {
            // Limpa a tela
            Console.Clear();

            // Mostra o título
            Console.WriteLine("Listar Produtos");
            if (App.QuantidadeProdutos == 0)
            {
                Console.WriteLine("\nNão há produtos registrados ainda\n");
            }
            else
            {
                // lista produtos do banco de dados
                cConexao.Conectar();
                string sql = "SELECT * FROM produtos_cadastrados";
                MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);

                // Sem tratamento
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Nome do Produto: " + rdr["nome_produto"]);
                    Console.WriteLine("Código de Barras: " + rdr["codigo_barras"]);
                    Console.WriteLine("Preço do Produto: " + rdr["preco_produto"]);
                    Console.WriteLine("Estoque do Produto: " + rdr["estoque_produto"]);
                    Console.WriteLine();
                }
                rdr.Close();
                cConexao.Desconectar();
            }



            // Pede para o usuário pressionar uma tecla para continuar
            Console.WriteLine("Pressione uma tecla para continuar...");
            Console.ReadKey();
        }
    }

    // Path: VenderProduto.cs

    public class cVenderProduto
    {
        public static void VenderProduto()
        {
            // Limpa a tela
            Console.Clear();

            // Mostra o título
            Console.WriteLine("Vender Produto");

            // Pede o código de barras
            Console.Write("Código de Barras: ");
            string codigoBarras = Console.ReadLine();

            // Verifica se o produto existe
            Mercado produto = null;
            for (int i = 0; i < App.QuantidadeProdutos; i++)
            {
                if (App.Mercado[i].CodigoBarras == codigoBarras)
                {
                    // Se existir, armazena o produto
                    produto = App.Mercado[i];
                    break;
                }
            }

            // Verifica se o produto existe
            if (produto == null)
            {
                // Se não existir, mostra uma mensagem de erro
                Console.WriteLine("Produto não existe!");
                Console.ReadKey();
                return;
            }

            // Pede a quantidade
            Console.Write("Quantidade: ");
            string quantidade = Console.ReadLine();

            // Verifica se a quantidade é válida

            // Se der erro na conversão = BUUUUM
            if (int.Parse(quantidade) > produto.EstoqueProduto)
            {
                // Se não for, mostra uma mensagem de erro
                Console.WriteLine("Quantidade inválida!");
                Console.WriteLine("Estoque disponivel: " + produto.EstoqueProduto);
                Console.ReadKey();
                return;
            }

            // Calcula o total
            decimal total = produto.PrecoProduto * int.Parse(quantidade);

            // Mostra o total
            Console.WriteLine("Total: R$" + total);

            // Pede para o usuário confirmar a venda
            Console.Write("Confirmar a venda? (S/N): ");
            string confirmacao = Console.ReadLine();

            // Verifica a confirmação
            if (confirmacao.ToUpper() == "S")
            {
                // Se for S, atualiza o estoque
                produto.EstoqueProduto -= int.Parse(quantidade);

                // Atualiza o banco de dados
                cConexao.Conectar();

                // SQL inject...
                string sql = "UPDATE produtos_cadastrados SET estoque_produto = '" + produto.EstoqueProduto + "' WHERE codigo_barras = '" + produto.CodigoBarras + "'";
                MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);

                // Sem tratamento
                cmd.ExecuteNonQuery();

                cConexao.Desconectar();

                // Mostra uma mensagem de sucesso
                Console.WriteLine("Venda realizada com sucesso!");
                Console.ReadKey();
            }
        }
    }

    // Path: Conexao.cs

    public class cConexao
    {
        public static MySqlConnection conexao = new MySqlConnection("Server=localhost;Database=cadastro_produtos;Uid=root;Pwd=123456;");

        public static void Conectar()
        {
            conexao.Open();
        }

        public static void Desconectar()
        {
            conexao.Close();
        }
    }


}



