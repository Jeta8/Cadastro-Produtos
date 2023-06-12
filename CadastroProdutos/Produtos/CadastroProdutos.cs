using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroProdutos
{
    internal class CadastroProdutos
    {
        public class cCadastrarProduto
        {
            public static void CadastrarProduto()
            {
                Console.Clear();

                Console.WriteLine("Cadastrar Produto");

                Console.Write("Nome do Produto: ");
                string nomeProduto = Console.ReadLine();
                while (string.IsNullOrEmpty(nomeProduto) || !nomeProduto.All(char.IsLetterOrDigit))
                {
                    Console.WriteLine("O nome do produto não pode ser vazio");
                    Console.Write("Nome do Produto: ");
                    nomeProduto = Console.ReadLine();
                }

                Console.Write("Código de Barras: ");
                string codigoBarras = Console.ReadLine();
                while (string.IsNullOrEmpty(codigoBarras) || !codigoBarras.All(char.IsDigit))
                {
                    Console.WriteLine("O código de barras não pode ser vazio e deve ser um número inteiro");
                    Console.Write("Código de Barras: ");
                    codigoBarras = Console.ReadLine();
                }

                cConexao.Conectar();
                string sql = "SELECT * FROM produtos_cadastrados WHERE codigo_barras = '" + codigoBarras + "'";
                MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    Console.WriteLine("Já existe um produto cadastrado com esse código de barras");
                    Console.WriteLine("Deseja cadastrar o produto com outro código de barras? (S/N)");
                    string opcao1 = Console.ReadLine();
                    if (opcao1.ToUpper() == "S")
                    {
                        Console.Write("Código de Barras: ");
                        codigoBarras = Console.ReadLine();
                        while (string.IsNullOrEmpty(codigoBarras) || !codigoBarras.All(char.IsDigit))
                        {
                            Console.WriteLine("O código de barras não pode ser vazio e deve ser um número inteiro");
                            Console.Write("Código de Barras: ");
                            codigoBarras = Console.ReadLine();
                        }

                    }
                    else if (opcao1.ToUpper() == "N")
                    {
                        cConexao.Desconectar();
                        return;
                    }
                }
                rdr.Close();
                cConexao.Desconectar();

                Console.Write("Preço do Produto: ");
                string precoProduto = Console.ReadLine();
                while (string.IsNullOrEmpty(precoProduto) || !precoProduto.All(char.IsDigit))
                {
                    Console.WriteLine("O preço do produto não pode ser vazio");
                    Console.Write("Preço do Produto: ");
                    precoProduto = Console.ReadLine();
                }

                Console.Write("Estoque do Produto: ");
                string estoqueProduto = Console.ReadLine();
                while (string.IsNullOrEmpty(estoqueProduto) || !estoqueProduto.All(char.IsDigit))
                {
                    Console.WriteLine("O estoque do produto não pode ser vazio e deve ser um número inteiro");
                    Console.Write("Estoque do Produto: ");
                    estoqueProduto = Console.ReadLine();
                }

                Mercado produto = new Mercado();
                produto.NomeProduto = nomeProduto;
                produto.CodigoBarras = codigoBarras;
                produto.PrecoProduto = decimal.Parse(precoProduto);
                produto.EstoqueProduto = int.Parse(estoqueProduto);

                try
                {
                    cConexao.Conectar();
                    sql = "INSERT INTO produtos_cadastrados (nome_produto, codigo_barras, preco_produto, estoque_produto) VALUES (@nomeProduto, @codigoBarras, @precoProduto, @estoqueProduto)";
                    cmd = new MySqlCommand(sql, cConexao.conexao);
                    cmd.Parameters.AddWithValue("@nomeProduto", nomeProduto);
                    cmd.Parameters.AddWithValue("@codigoBarras", codigoBarras);
                    cmd.Parameters.AddWithValue("@precoProduto", precoProduto);
                    cmd.Parameters.AddWithValue("@estoqueProduto", estoqueProduto);
                    cmd.ExecuteNonQuery();
                    cConexao.Desconectar();
                }
                catch (Exception ex)
                {
                    cConexao.Desconectar();
                    Console.WriteLine("Erro ao cadastrar produto: " + ex.Message);
                    Console.ReadKey();
                    return;
                }

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
                    Console.WriteLine("Esses foram os produtos cadastrados até o momento:");
                    ListarProdutos.cListarProdutos.ListarProdutos();
                    Console.ReadKey();
                    Console.Clear();
                    cMenuPrincipal.MenuPrincipal();
                }
            }
        }

    }
}
