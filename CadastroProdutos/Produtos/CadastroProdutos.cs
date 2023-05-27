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
                while (nomeProduto == "")
                {
                    Console.WriteLine("O nome do produto não pode ser vazio");
                    Console.Write("Nome do Produto: ");
                    nomeProduto = Console.ReadLine();
                }



                Console.Write("Código de Barras: ");
                string codigoBarras = Console.ReadLine();
                while (codigoBarras == "")
                {
                    Console.WriteLine("O código de barras não pode ser vazio");
                    Console.Write("Código de Barras: ");
                    codigoBarras = Console.ReadLine();
                }


                cConexao.Conectar();

                // TODO: Query vulnerável a sqli
                string sql = "SELECT * FROM produtos_cadastrados WHERE codigo_barras = '" + codigoBarras + "'";

                MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);

                // TODO: Sem tratamento de erro
                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    Console.WriteLine("Já existe um produto cadastrado com esse código de barras");
                    Console.WriteLine("Deseja cadastrar o produto mas com outro codigo de barras? (S/N)");


                    // TODO: Se o usuário digitar S e colocar o código de barras já existente novamente o programa continua
                    string opcao1 = Console.ReadLine();
                    if (opcao1.ToUpper() == "S")
                    {
                        Console.Write("Código de Barras: ");
                        codigoBarras = Console.ReadLine();
                        while (codigoBarras == "")
                        {
                            Console.WriteLine("O código de barras não pode ser vazio");
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

                // Bug - ExecuteReader deve ser fechado, é feito automaticamente quando a conexão é fechada
                // Mas nesse caso, ela continua aberta então na próxima query vai dar erro sem fechar ela
                rdr.Close(); // Correção

                Console.Write("Preço do Produto: ");
                string precoProduto = Console.ReadLine();
                while (precoProduto == "")
                {
                    Console.WriteLine("O preço do produto não pode ser vazio");
                    Console.Write("Preço do Produto: ");
                    precoProduto = Console.ReadLine();
                }

                Console.Write("Estoque do Produto: ");
                string estoqueProduto = Console.ReadLine();
                while (estoqueProduto == "")
                {
                    Console.WriteLine("O estoque do produto não pode ser vazio");
                    Console.Write("Estoque do Produto: ");
                    estoqueProduto = Console.ReadLine();
                }

                Mercado produto = new Mercado();
                produto.NomeProduto = nomeProduto;
                produto.CodigoBarras = codigoBarras;

                // TODO: Sem tratamento de caractéres indevidos (aqui não é o único local, revisar todos)
                produto.PrecoProduto = decimal.Parse(precoProduto);
                produto.EstoqueProduto = int.Parse(estoqueProduto);

                try
                {
                    // Bug - Conexão já aberta ali em cima, sendo aberta novamente aqui
                    // cConexao.Conectar(); // Correção

                    // TODO: Query vulnerável a sqli
                    sql = "INSERT INTO produtos_cadastrados (nome_produto, codigo_barras, preco_produto, estoque_produto) VALUES ('" + nomeProduto + "', '" + codigoBarras + "', '" + precoProduto + "', '" + estoqueProduto + "')";

                    cmd = new MySqlCommand(sql, cConexao.conexao);
                    cmd.ExecuteNonQuery();

                    // Se der erro na query, o código vai direto para o catch e não fecha a conexão com o banco de addos
                    cConexao.Desconectar();
                }
                catch (Exception ex)
                {
                    // Correção para fechar a conexão em caso de erro 
                    if (cConexao.conexao.State == System.Data.ConnectionState.Open)
                        cConexao.Desconectar();

                    Console.WriteLine("Erro ao cadastrar produto: " + ex.Message);
                    Console.ReadKey();
                    return;
                }

                // nome_produto string, codigo_barras string, preco_produto decimal, estoque_produto int

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
                    ListarProdutos.cListarProdutos.ListarProdutos();
                    Console.ReadKey();
                    Console.Clear();
                    cMenuPrincipal.MenuPrincipal();
                }
            }
        }

    }
}
