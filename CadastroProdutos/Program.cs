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
            // Inicializa o sistema
            InicializarSistema();

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
                    Console.WriteLine("3 - Sair");
                    Console.Write("Opção: ");
                    string opcao = Console.ReadLine();
                    switch (opcao)
                    {
                        case "1":
                            Login.Login();
                            break;
                        case "2":
                            CadastrarOperador.CadastrarOperador();
                            break;
                        case "3":
                            Sair.Sair();
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
                    MenuPrincipal();
                }
            }
        }
    }

    // Path: Login.cs

    public class Login
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

            // Se não estiverem, mostra uma mensagem de erro
            Console.WriteLine("Login ou senha incorretos!");
            Console.ReadKey();
        }
    }

    // Path: MenuPrincipal.cs

    public class MenuPrincipal
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
            Console.WriteLine("1 - Cadastrar Operador");
            Console.WriteLine("2 - Cadastrar Produto");
            Console.WriteLine("3 - Listar Produtos");
            Console.WriteLine("4 - Vender Produto");
            Console.WriteLine("5 - Sair");

            // Pede a opção
            Console.Write("Opção: ");
            string opcao = Console.ReadLine();

            // Verifica a opção
            switch (opcao)
            {
                case "1":
                    CadastrarOperador();
                    break;
                case "2":
                    CadastrarProduto();
                    break;
                case "3":
                    ListarProdutos();
                    break;
                case "4":
                    VenderProduto();
                    break;
                case "5":
                    Sair();
                    break;
                default:
                    Console.WriteLine("Opção inválida!");
                    Console.ReadKey();
                    break;
            }
        }
    }

    // Path: CadastrarOperador.cs

    public class CadastrarOperador
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
            Console.Write("Nível de Acesso (1 - Operador, 2 - Administrador): ");
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

            // Mostra uma mensagem de sucesso
            Console.WriteLine("Operador cadastrado com sucesso!");
            Console.ReadKey();
        }
    }

    // Path: CadastrarProduto.cs

    public class CadastrarProduto
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

            // Mostra uma mensagem de sucesso
            Console.WriteLine("Produto cadastrado com sucesso!");
            Console.ReadKey();
        }
    }

    // Path: ListarProdutos.cs

    public class ListarProdutos
    {
        public static void ListarProdutos()
        {
            // Limpa a tela
            Console.Clear();

            // Mostra o título
            Console.WriteLine("Listar Produtos");

            // Mostra os produtos
            for (int i = 0; i < App.QuantidadeProdutos; i++)
            {
                Console.WriteLine("Nome do Produto: " + App.Mercado[i].NomeProduto);
                Console.WriteLine("Código de Barras: " + App.Mercado[i].CodigoBarras);
                Console.WriteLine("Preço do Produto: " + App.Mercado[i].PrecoProduto);
                Console.WriteLine("Estoque do Produto: " + App.Mercado[i].EstoqueProduto);
                Console.WriteLine();
            }

            // Pede para o usuário pressionar uma tecla para continuar
            Console.WriteLine("Pressione uma tecla para continuar...");
            Console.ReadKey();
        }
    }

    // Path: VenderProduto.cs

    public class VenderProduto
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
            if (int.Parse(quantidade) > produto.EstoqueProduto)
            {
                // Se não for, mostra uma mensagem de erro
                Console.WriteLine("Quantidade inválida!");
                Console.ReadKey();
                return;
            }

            // Calcula o total
            decimal total = produto.PrecoProduto * int.Parse(quantidade);

            // Mostra o total
            Console.WriteLine("Total: " + total);

            // Pede para o usuário confirmar a venda
            Console.Write("Confirmar a venda? (S/N): ");
            string confirmacao = Console.ReadLine();

            // Verifica a confirmação
            if (confirmacao.ToUpper() == "S")
            {
                // Se for S, atualiza o estoque
                produto.EstoqueProduto -= int.Parse(quantidade);

                // Mostra uma mensagem de sucesso
                Console.WriteLine("Venda realizada com sucesso!");
                Console.ReadKey();
            }
        }
    }

    // Path: Program.cs

    class Program
    {
        static void Main(string[] args)
        {
            // Inicializa o array de operadores
            App.Operadores = new Operadores[100];

            // Inicializa o array de produtos
            App.Mercado = new Mercado[100];

            // Inicializa a quantidade de operadores
            App.QuantidadeOperadores = 0;

            // Inicializa a quantidade de produtos
            App.QuantidadeProdutos = 0;

            // Mostra o menu
            while (true)
            {
                // Limpa a tela
                Console.Clear();

                // Mostra o menu
                Console.WriteLine("1 - Cadastrar Operador");
                Console.WriteLine("2 - Cadastrar Produto");
                Console.WriteLine("3 - Listar Produtos");
                Console.WriteLine("4 - Vender Produto");
                Console.WriteLine("5 - Sair");

                // Pede a opção
                Console.Write("Opção: ");
                string opcao = Console.ReadLine();

                // Verifica a opção
                switch (opcao)
                {
                    case "1":
                        CadastrarOperador.CadastrarOperador();
                        break;
                    case "2":
                        CadastrarProduto.CadastrarProduto();
                        break;
                    case "3":
                        ListarProdutos.ListarProdutos();
                        break;
                    case "4":
                        VenderProduto.VenderProduto();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }

    // Path: Operadores.cs

    public class Operadores
    {
        public string NomeOperador { get; set; }
        public string LoginOperador { get; set; }
        public string SenhaOperador { get; set; }
    }

    // Path: Mercado.cs

    public class Mercado
    {
        public string NomeProduto { get; set; }
        public string CodigoBarras { get; set; }
        public decimal PrecoProduto { get; set; }
        public int EstoqueProduto { get; set; }
    }

    // Path: App.cs

    public class App
    {
        public static Operadores[] Operadores { get; set; }
        public static Mercado[] Mercado { get; set; }
        public static int QuantidadeOperadores { get; set; }
        public static int QuantidadeProdutos { get; set; }
    }
}



