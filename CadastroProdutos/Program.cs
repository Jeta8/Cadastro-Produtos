using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CadastroProdutos;
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
        public static Operadores Operadores = new Operadores();
        public static Mercado Mercado = new Mercado();
        public static int QuantidadeOperadores = 0;
        public static int QuantidadeProdutos = 0;
        public static Operadores OperadorLogado = null;

        public static void Main()
        {

            while (true)
            {
                if (OperadorLogado == null)
                {
                    Console.WriteLine("Fazer login");
                    Console.WriteLine("1 - Fazer login");
                    Console.WriteLine("2 - Cadastrar Operador");
                    Console.Write("Opção: ");
                    string opcao = Console.ReadLine();
                    switch (opcao)
                    {
                        case "1":
                            Login.cLogin.Login();
                            break;
                        case "2":
                            OperadoresControl.cCadastrarOperador.CadastrarOperador();
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
    

    public class cMenuPrincipal
    {
        public static void MenuPrincipal()
        {
            Console.Clear();

            Console.WriteLine("Menu Principal");

            Console.WriteLine("Operador: " + App.OperadorLogado.NomeOperador);

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

            Console.Write("Opção: ");
            string opcao = Console.ReadLine();

            if (App.OperadorLogado.NivelAcesso == NiveisAcesso.Administrador)
            {
                switch (opcao)
                {

                    case "1":
                        OperadoresControl.cCadastrarOperador.CadastrarOperador();
                        break;
                    case "2":
                        OperadoresControl.cCadastrarOperador.AdmOperador();
                        break;
                    case "3":
                        CadastroProdutos.cCadastrarProduto.CadastrarProduto();
                        break;
                    case "4":
                        ListarProdutos.cListarProdutos.ListarProdutos();
                        break;
                    case "5":
                        VenderProduto.cVenderProduto.VenderProduto();
                        break;
                    case "6":
                        OperadoresControl.cCadastrarOperador.ListarOperadores();
                        break;

                    case "7":
                        Login.cLogin.Logout();
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
                        ListarProdutos.cListarProdutos.ListarProdutos();
                        break;
                    case "2":
                        VenderProduto.cVenderProduto.VenderProduto();
                        break;
                    case "3":
                        Login.cLogin.Logout();
                        break;

                    default:
                        Console.WriteLine("Opção inválida!");
                        Console.ReadKey();
                        break;
                }
            }

        }
    }


    public class cConexao
    {
        public static MySqlConnection conexao = new MySqlConnection("Server=localhost;Database=cadastro_produtos;Uid=root;Pwd=Jederson@28180622;");

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




