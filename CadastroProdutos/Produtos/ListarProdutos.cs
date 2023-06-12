using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroProdutos
{
    internal class ListarProdutos
    {
        public class cListarProdutos
        {
            public static void ListarProdutos()
            {
                Console.Clear();

                try
                {
                    Console.WriteLine("Listar Produtos");
                    cConexao.Conectar();
                    string sql = "SELECT * FROM produtos_cadastrados";
                    using (MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao))
                    {
                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                while (rdr.Read())
                                {
                                    Console.WriteLine("Nome do Produto: " + rdr.GetString("nome_produto"));
                                    Console.WriteLine("Código de Barras: " + rdr.GetString("codigo_barras"));
                                    Console.WriteLine("Preço do Produto: " + rdr.GetDecimal("preco_produto"));
                                    Console.WriteLine("Estoque do Produto: " + rdr.GetInt32("estoque_produto"));
                                    Console.WriteLine();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Não há produtos cadastrados");
                                cConexao.Desconectar();
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    cConexao.Desconectar();
                    Console.WriteLine("Ocorreu um erro ao listar os produtos: " + ex.Message);
                }
                finally
                {
                    cConexao.Desconectar();
                }

                Console.WriteLine("Pressione uma tecla para continuar...");
                Console.ReadKey();
            }

        }
    }
}
