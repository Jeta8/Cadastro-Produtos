using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroProdutos
{
    internal class VenderProduto
    {
        public class cVenderProduto
        {
            public static void VenderProduto()
            {
                Console.Clear();
                List<Mercado> produtosParaVenda = new List<Mercado>();

                string quantidade = "";
                Console.WriteLine("Vender Produto");
                while (true)
                {
                    Console.Write("Código de Barras: ");
                    string codigoBarras = Console.ReadLine();
                    while (string.IsNullOrEmpty(codigoBarras) || !codigoBarras.All(char.IsDigit))
                    {
                        Console.WriteLine("O código de barras deve ser um número");
                        Console.Write("Código de Barras: ");
                        codigoBarras = Console.ReadLine();
                    }                 

                    // Verifica se o produto existe no banco de dados
                    Mercado produto = null;
                    try
                    {                        
                        cConexao.Conectar();
                        string sql = "SELECT * FROM produtos_cadastrados WHERE codigo_barras = @codigoBarras";

                        MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
                        cmd.Parameters.AddWithValue("@codigoBarras", codigoBarras);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                       
                        if (rdr.Read())
                        {
                            produto = new Mercado();
                            produto.NomeProduto = rdr["nome_produto"].ToString();
                            produto.CodigoBarras = rdr["codigo_barras"].ToString();
                            produto.PrecoProduto = decimal.Parse(rdr["preco_produto"].ToString());
                            produto.EstoqueProduto = int.Parse(rdr["estoque_produto"].ToString());
                            rdr.Close();
                            cConexao.Desconectar();
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine("Ocorreu um erro ao buscar o produto: " + ex.Message);
                        Console.ReadKey();
                        Console.Clear();
                        VenderProduto();
                    }


                    if (produto == null)
                    {
                        Console.WriteLine("Produto não encontrado!");
                        Console.ReadKey();
                        return;
                    }

                    // Pede a quantidade e permite apenas números
                    Console.Write("Quantidade: ");
                    quantidade = Console.ReadLine();
                    while (string.IsNullOrEmpty(quantidade) || !quantidade.All(char.IsDigit))
                    {
                        Console.WriteLine("A quantidade deve ser um número");
                        Console.Write("Quantidade: ");
                        quantidade = Console.ReadLine();
                    }

                    // Verifica se a quantidade é válida no banco de dados
                    int quantidadeVenda = int.Parse(quantidade);
                    if (quantidadeVenda > produto.EstoqueProduto)
                    {
                        Console.WriteLine("Quantidade inválida!");
                        Console.ReadKey();
                        return;
                    }
                    produtosParaVenda.Add(produto);

                    Console.WriteLine("Produto: " + produto.NomeProduto);
                    Console.WriteLine("Quantidade: " + quantidade);
                    Console.WriteLine("Preço: R$" + produto.PrecoProduto);
                    Console.WriteLine("Deseja adicionar mais produtos? (S/N)");
                    string conf = Console.ReadLine();

                    // Verifica a confirmação
                    if (conf.ToUpper() == "S")
                    {
                        Console.Clear();
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                Console.WriteLine("Produtos para venda:");
                foreach (var produto in produtosParaVenda)
                {
                    Console.WriteLine("Produto: " + produto.NomeProduto);
                    Console.WriteLine("Quantidade: " + quantidade);
                    Console.WriteLine("Preço: R$" + produto.PrecoProduto);
                    Console.WriteLine();
                }

                decimal total = produtosParaVenda.Sum(p => p.PrecoProduto * int.Parse(quantidade));

                // Mostra o total
                Console.WriteLine("Total: R$" + total);

                Console.WriteLine("Método de pagamento: Cartão (C) ou Dinheiro (D)?");
                string metodoPagamento = Console.ReadLine();
                while (metodoPagamento != "C" && metodoPagamento != "D")
                {
                    Console.WriteLine("O método de pagamento deve ser 'C' para Cartão ou 'D' para Dinheiro");
                    Console.WriteLine("Método de pagamento: Cartão (C) ou Dinheiro (D)?");
                    metodoPagamento = Console.ReadLine();
                }

                decimal valorPago = 0;
                decimal troco = 0;

                if (metodoPagamento.ToUpper() == "D")
                {
                    // Pergunta o valor pago em dinheiro
                    Console.Write("Valor pago em dinheiro: R$");
                    string valorPagoStr = Console.ReadLine();
                    while (string.IsNullOrEmpty(valorPagoStr) || !decimal.TryParse(valorPagoStr, out valorPago))
                    {
                        Console.WriteLine("O valor pago deve ser um número");
                        Console.Write("Valor pago em dinheiro: R$");
                        valorPagoStr = Console.ReadLine();
                    }

                    while (valorPago < total)
                    {
                        Console.WriteLine("O valor pago não pode ser menor que o total");
                        Console.Write("Valor pago em dinheiro: R$");
                        valorPagoStr = Console.ReadLine();
                        while (string.IsNullOrEmpty(valorPagoStr) || !decimal.TryParse(valorPagoStr, out valorPago))
                        {
                            Console.WriteLine("O valor pago deve ser um número");
                            Console.Write("Valor pago em dinheiro: R$");
                            valorPagoStr = Console.ReadLine();
                        }
                    }

                    troco = valorPago - total;

                    Console.WriteLine(" O troco a ser devolvido é de: R$" + troco);
                    Console.ReadKey();
                }

                string operador = App.OperadorLogado.NomeOperador;

                Console.Write("Confirmar a venda? (S/N): ");
                string confirmacao = Console.ReadLine();
                while (string.IsNullOrEmpty(confirmacao) || (confirmacao.ToUpper() != "S" && confirmacao.ToUpper() != "N"))
                {
                    Console.WriteLine("A confirmação deve ser 'S' para Sim ou 'N' para Não");
                    Console.Write("Confirmar a venda? (S/N): ");
                    confirmacao = Console.ReadLine();
                }

                if (confirmacao.ToUpper() == "S")
                {
                    try
                    {
                        cConexao.Conectar();
                        string sql = "INSERT INTO vendas (valor_total, metodo_pagamento, valor_pago, troco, operador, data_hora) VALUES (@valor_total, @metodo_pagamento, @valor_pago, @troco, @operador, @data_hora)";
                        MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
                        cmd.Parameters.AddWithValue("@valor_total", total);
                        cmd.Parameters.AddWithValue("@metodo_pagamento", metodoPagamento);
                        cmd.Parameters.AddWithValue("@valor_pago", valorPago);
                        cmd.Parameters.AddWithValue("@troco", troco);
                        cmd.Parameters.AddWithValue("@operador", operador);
                        cmd.Parameters.AddWithValue("@data_hora", DateTime.Now);
                        cmd.ExecuteNonQuery();

                        int idVenda = (int)cmd.LastInsertedId;

                        foreach (var produto in produtosParaVenda)
                        {
                            sql = "INSERT INTO itens_venda (id_venda, id_produto, quantidade) VALUES (@id_venda, @id_produto, @quantidade)";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@id_venda", idVenda);
                            cmd.Parameters.AddWithValue("@id_produto", produto.CodigoBarras);
                            cmd.Parameters.AddWithValue("@quantidade", quantidade);
                            cmd.ExecuteNonQuery();
                        }

                        foreach (var produto in produtosParaVenda)
                        {
                            sql = "UPDATE produtos_cadastrados SET estoque_produto = estoque_produto - @quantidade WHERE codigo_barras = @codigo_barras";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@quantidade", quantidade);
                            cmd.Parameters.AddWithValue("@codigo_barras", produto.CodigoBarras);
                            cmd.ExecuteNonQuery();
                        }

                        cConexao.Desconectar();

                        Console.WriteLine("Venda realizada com sucesso!");
                        Console.ReadKey();
                        Console.Clear();
                        cMenuPrincipal.MenuPrincipal();
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine("Ocorreu um erro ao realizar a venda: " + ex.Message);
                        Console.ReadKey();
                        Console.Clear();
                        cMenuPrincipal.MenuPrincipal();

                    }
                }
                else
                {
                    Console.WriteLine("Venda cancelada!");
                    Console.ReadKey();
                    Console.Clear();
                    cMenuPrincipal.MenuPrincipal();
                }
            }

            public static void ListarVendas()
            {
                Console.Clear();
                Console.WriteLine("Listar Vendas");
                Console.WriteLine();
                try
                {
                    cConexao.Conectar();
                    string sql = "SELECT * FROM vendas";
                    MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Console.WriteLine("ID: " + rdr["id_venda"].ToString());
                        Console.WriteLine("Valor Total: " + rdr["valor_total"].ToString());
                        Console.WriteLine("Método de Pagamento: " + rdr["metodo_pagamento"].ToString());
                        Console.WriteLine("Valor Pago: " + rdr["valor_pago"].ToString());
                        Console.WriteLine("Troco: " + rdr["troco"].ToString());
                        Console.WriteLine("Operador: " + rdr["operador"].ToString());
                        Console.WriteLine("Data e Hora: " + rdr["data_hora"].ToString());
                        Console.WriteLine();
                    }
                    rdr.Close();
                    cConexao.Desconectar();
                    Console.ReadKey();
                    Console.Clear();
                    cMenuPrincipal.MenuPrincipal();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Ocorreu um erro ao listar as vendas: " + ex.Message);
                    Console.ReadKey();
                    Console.Clear();
                    cMenuPrincipal.MenuPrincipal();

                }
            }
        }
    }
}
