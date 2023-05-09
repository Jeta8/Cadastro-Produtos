using System.Diagnostics;

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
            Operadores[0] = new Operadores();
            Operadores[0].Login = "teste";
            Operadores[0].Senha = "1";
            Operadores[0].NivelAcesso = NiveisAcesso.Administrador;
            Operadores[0].NomeOperador = "Wallas";
            QuantidadeOperadores = 1;

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
                    Console.Clear();
                    cMenuPrincipal.MenuPrincipal();
                }
            }
        }


    }

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

            // Verifica se o login e a senha estão corretos
            for (int i = 0; i < App.QuantidadeOperadores; i++)
            {
                if (App.Operadores[i].Login == login && App.Operadores[i].Senha == senha)
                {
                    // Se estiverem, faz o login
                    App.OperadorLogado = App.Operadores[i];
                    return;
                }
            }

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


    public class cMenuPrincipal
    {
        public static void MenuPrincipal()
        {
            Console.Clear();

            Console.WriteLine("Menu Principal");

            Console.WriteLine("Operador: " + App.OperadorLogado.NomeOperador);

            // Mostra o menu
            if (App.OperadorLogado.NivelAcesso == NiveisAcesso.Administrador)
            {
                Console.WriteLine("1 - Cadastrar Operador");
                Console.WriteLine("2 - Cadastrar Produto");
                Console.WriteLine("3 - Listar Produtos");
                Console.WriteLine("4 - Vender Produto");
                Console.WriteLine("5 - Sair");
            }
            else
            {
                Console.WriteLine("1 - Listar Produtos");
                Console.WriteLine("2 - Vender Produto");
                Console.WriteLine("3 - Sair");
            }



            Console.Write("Opção: ");
            string opcao = Console.ReadLine();

            if (App.OperadorLogado.NivelAcesso == NiveisAcesso.Administrador)
            {
                switch (opcao)
                {
                    case "1":
                        cCadastrarOperador.CadastrarOperador();
                        break;
                    case "2":
                        cCadastrarProduto.CadastrarProduto();
                        break;
                    case "3":
                        cListarProdutos.ListarProdutos();
                        break;
                    case "4":
                        cVenderProduto.VenderProduto();
                        break;
                    case "5":
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


    public class cCadastrarOperador
    {
        public static void CadastrarOperador()
        {
            Console.Clear();

            Console.WriteLine("Cadastrar Operador");

            Console.Write("Login: ");
            string login = Console.ReadLine();

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

            Console.Write("Senha: ");
            string senha = Console.ReadLine();

            Console.Write("Nome do Operador: ");
            string nomeOperador = Console.ReadLine();

            Console.Write("Nível de Acesso (1 - Operador  ||  2 - Administrador): ");
            string nivelAcesso = Console.ReadLine();

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

            App.Operadores[App.QuantidadeOperadores] = operador;
            App.QuantidadeOperadores++;

            if (operador.NivelAcesso != NiveisAcesso.Administrador)
            {
                Console.WriteLine("Operador cadastrado com sucesso!");
            }
            else { Console.WriteLine("Administrador cadastrado com sucesso!"); }

            Console.ReadKey();
            Console.Clear();
        }
    }


    public class cCadastrarProduto
    {
        public static void CadastrarProduto()
        {
            Console.Clear();

            Console.WriteLine("Cadastrar Produto");

            Console.Write("Nome do Produto: ");
            string nomeProduto = Console.ReadLine();

            // Aqui deixo um exemplo de código mais limpo em relação ao for, pode ser usado tanto em arrays como em listas
            if (App.Mercado.FirstOrDefault(x => x != null && x.NomeProduto == nomeProduto) != null)
            {
                Console.WriteLine("Produto já existe!");
                Console.ReadKey();
                return;
            }

            // Aqui é a forma original que você fez
            for (int i = 0; i < App.QuantidadeProdutos; i++)
            {
                if (App.Mercado[i].NomeProduto == nomeProduto)
                {
                    Console.WriteLine("Produto já existe!");
                    Console.ReadKey();
                    return;
                }
            }

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

            Console.Write("Preço do Produto: ");
            string precoProduto = Console.ReadLine();

            Console.Write("Estoque do Produto: ");
            string estoqueProduto = Console.ReadLine();

            Mercado produto = new Mercado();
            produto.NomeProduto = nomeProduto;
            produto.CodigoBarras = codigoBarras;
            produto.PrecoProduto = decimal.Parse(precoProduto);
            produto.EstoqueProduto = int.Parse(estoqueProduto);

            App.Mercado[App.QuantidadeProdutos] = produto;
            App.QuantidadeProdutos++;

            Console.WriteLine("Produto cadastrado com sucesso!\n");
            Console.WriteLine("Nome do Produto: " + nomeProduto);
            Console.WriteLine("Código de Barras: " + codigoBarras);
            Console.WriteLine("Preço do Produto: " + precoProduto);
            Console.WriteLine("Estoque do Produto: " + estoqueProduto);
            Console.WriteLine();
            Console.WriteLine("Deseja cadastrar um novo produto? (S/N)");
            string confirmacao = Console.ReadLine();

            if (confirmacao.ToUpper() == "S")
            {
                Console.Clear();
                CadastrarProduto();

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


    public class cListarProdutos
    {
        public static void ListarProdutos()
        {
            Console.Clear();

            Console.WriteLine("Listar Produtos");
            if ( App.QuantidadeProdutos == 0 )
            {
                Console.WriteLine("\nNão há produtos registrados ainda\n");
            }
            else
            {
                for (int i = 0; i < App.QuantidadeProdutos; i++)
                {
                    // Aqui um exemplo de como a função String.Format pode ser utilizada
                    Console.WriteLine(String.Format(
                        "Nome do Produto: {0}\n"+
                        "Código de Barras: {1}\n"+
                        "Preço do Produto: {2}\n"+
                        "Estoque do Produto: {3}\n", 
                        App.Mercado[i].NomeProduto, 
                        App.Mercado[i].CodigoBarras, 
                        App.Mercado[i].PrecoProduto, 
                        App.Mercado[i].EstoqueProduto));
                }
            }



            Console.WriteLine("Pressione uma tecla para continuar...");
            Console.ReadKey();
        }
    }


    public class cVenderProduto
    {
        public static void VenderProduto()
        {
            Console.Clear();

            Console.WriteLine("Vender Produto");

            Console.Write("Código de Barras: ");
            string codigoBarras = Console.ReadLine();

            Mercado produto = null;
            for (int i = 0; i < App.QuantidadeProdutos; i++)
            {
                if (App.Mercado[i].CodigoBarras == codigoBarras)
                {
                    produto = App.Mercado[i];
                    break;
                }
            }

            if (produto == null)
            {
                Console.WriteLine("Produto não existe!");
                Console.ReadKey();
                return;
            }

            Console.Write("Quantidade: ");
            string quantidade = Console.ReadLine();

            if (int.Parse(quantidade) > produto.EstoqueProduto)
            {
                Console.WriteLine("Quantidade inválida!");
                Console.WriteLine("Estoque disponivel: " + produto.EstoqueProduto);
                Console.ReadKey();
                return;
            }

            decimal total = produto.PrecoProduto * int.Parse(quantidade);

            Console.WriteLine("Total: R$" + total);

            Console.Write("Confirmar a venda? (S/N): ");
            string confirmacao = Console.ReadLine();

            if (confirmacao.ToUpper() == "S")
            {
                produto.EstoqueProduto -= int.Parse(quantidade);

                Console.WriteLine("Venda realizada com sucesso!");
                Console.ReadKey();
            }
        }
    }
}



