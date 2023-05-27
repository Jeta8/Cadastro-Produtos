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

                
                Console.WriteLine("Listar Produtos");
                cConexao.Conectar();
                string sql = "SELECT * FROM produtos_cadastrados";
                MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);

                // TODO: Sem tratamento de erro
                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        Console.WriteLine("Nome do Produto: " + rdr["nome_produto"]);
                        Console.WriteLine("Código de Barras: " + rdr["codigo_barras"]);
                        Console.WriteLine("Preço do Produto: " + rdr["preco_produto"]);
                        Console.WriteLine("Estoque do Produto: " + rdr["estoque_produto"]);
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("Não há produtos cadastrados");
                }
                cConexao.Desconectar();


                Console.WriteLine("Pressione uma tecla para continuar...");
                Console.ReadKey();
            }
        }
    }
}
