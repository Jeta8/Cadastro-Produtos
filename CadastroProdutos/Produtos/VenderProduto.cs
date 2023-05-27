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
                // Limpa a tela
                Console.Clear();
                List<Mercado> produtosParaVenda = new List<Mercado>();
                // Mostra o título
                string quantidade = "";
                Console.WriteLine("Vender Produto");
                while (true)
                {
                    // Pede o código de barras
                    Console.Write("Código de Barras: ");
                    string codigoBarras = Console.ReadLine();
                    while (codigoBarras == "")
                    {
                        Console.WriteLine("O código de barras não pode ser vazio");
                        Console.Write("Código de Barras: ");
                        codigoBarras = Console.ReadLine();
                    }

                    // Verifica se o produto existe no banco de dados

                    cConexao.Conectar();
                    string sql = "SELECT * FROM produtos_cadastrados WHERE codigo_barras = '" + codigoBarras + "'";

                    MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);

                    // TODO: Sem tratamento de erro
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    Mercado produto = null;
                    if (rdr.Read())
                    {
                        // Se existir, armazena o produto
                        produto = new Mercado();
                        produto.NomeProduto = rdr["nome_produto"].ToString();
                        produto.CodigoBarras = rdr["codigo_barras"].ToString();
                        produto.PrecoProduto = decimal.Parse(rdr["preco_produto"].ToString());
                        produto.EstoqueProduto = int.Parse(rdr["estoque_produto"].ToString());
                        rdr.Close();
                        cConexao.Desconectar();
                    }
                    // Verifica se o produto foi encontrado
                    if (produto == null)
                    {
                        Console.WriteLine("Produto não encontrado!");
                        Console.ReadKey();
                        return;
                    }

                    // Pede a quantidade e permite apenas numeros
                    Console.Write("Quantidade: ");
                    quantidade = Console.ReadLine();
                    while (quantidade == "")
                    {
                        Console.WriteLine("A quantidade não pode ser vazia");
                        Console.Write("Quantidade: ");
                        quantidade = Console.ReadLine();
                    }
                    foreach (char c in quantidade)
                    {
                        if (!char.IsNumber(c))
                        {
                            Console.WriteLine("A quantidade deve ser um número");
                            Console.Write("Quantidade: ");
                            quantidade = Console.ReadLine();
                        }
                    }
                   


                    // Verifica se a quantidade é válida no banco de dados
                    if (int.Parse(quantidade) > produto.EstoqueProduto)
                    {
                        Console.WriteLine("Quantidade inválida!");
                        Console.ReadKey();
                        return;
                    }
                    produtosParaVenda.Add(produto);

                    // coloque em uma lista todos os produtos que o usuario colocou para vender e pergunte se deseja finalizar a compra
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

                // Calcula o total de todos os produtos colocados para vender no loop anterior
                decimal total = 0;
                foreach (var produto in produtosParaVenda)
                {
                    total += produto.PrecoProduto * int.Parse(quantidade);
                }

                // Mostra o total
                Console.WriteLine("Total: R$" + total);

                Console.WriteLine("Método de pagamento: Cartão (C) ou Dinheiro (D)?");
                string metodoPagamento = Console.ReadLine();

                // Esse while está errado, um ou outro sempre vai ser verdadeiro e a venda nunca será finalizada
                // Correção feita.
                while (metodoPagamento != "C" && metodoPagamento != "D")
                {
                    Console.WriteLine("O método de pagamento não pode ser vazio");
                    Console.WriteLine("Método de pagamento: Cartão (C) ou Dinheiro (D)?");
                    metodoPagamento = Console.ReadLine();
                }

                decimal valorPago = 0;
                decimal troco = 0;

                if (metodoPagamento.ToUpper() == "D")
                {
                    // Pergunta o valor pago em dinheiro
                    Console.Write("Valor pago em dinheiro: R$");
                    valorPago = decimal.Parse(Console.ReadLine());
                  
                    foreach(char c in valorPago.ToString())
                    {
                        if (!char.IsNumber(c))
                        {
                            Console.WriteLine("O valor pago deve ser um número");
                            Console.Write("Valor pago em dinheiro: R$");
                            valorPago = decimal.Parse(Console.ReadLine());
                        }
                    }
                    while (valorPago < total)
                    {
                        Console.WriteLine("O valor pago não pode ser menor que o total");
                        Console.Write("Valor pago em dinheiro: R$");
                        valorPago = decimal.Parse(Console.ReadLine());
                    }


                    // Calcula o troco

                    // TODO : Cálculo do troco errado, não está considerando a quantidade
                    decimal valorTotal = produtosParaVenda.Sum(p => p.PrecoProduto);
                    troco = valorPago - valorTotal;

                    // Informa o valor do troco
                    Console.Write(" O troco a ser devolvido e de: R$" + troco);
                    Console.ReadKey();
                }

                // Pede para o usuário confirmar a venda
                Console.Write("Confirmar a venda? (S/N): ");
                string confirmacao = Console.ReadLine();
                while (confirmacao == "")
                {
                    Console.WriteLine("A confirmação não pode ser vazia");
                    Console.Write("Confirmar a venda? (S/N): ");
                    confirmacao = Console.ReadLine();
                }

                // Verifica a confirmação, para atualizar o estoque no banco de dados, salvando a venda e mostrando cada item que foi vendido e o metodo de pagamento, alem de gerar uma ID unica de venda, salvar tambem os IDs de cada item vendido
                
                // TODO: Não está atualizando o estoque
                if (confirmacao.ToUpper() == "S")
                {
                    cConexao.Conectar();
                    string sql = "INSERT INTO vendas (valor_total, metodo_pagamento, valor_pago, troco) VALUES ('" + total + "', '" + metodoPagamento + "', '" + valorPago + "', '" + troco + "')";
                    
                   
                    MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);

                    // TODO: Sem tratamento de erro
                    cmd.ExecuteNonQuery();

                    cConexao.Desconectar();
                    // valor_total decimal(10,2) NOT NULL, metodo_pagamento varchar(255) NOT NULL, valor_pago decimal(10,2) NOT NULL, troco decimal(10,2) NOT NULL, PRIMARY KEY (id_venda)

                    cConexao.Conectar();
                    sql = "SELECT * FROM vendas ORDER BY id_venda DESC LIMIT 1";
                    cmd = new MySqlCommand(sql, cConexao.conexao);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    int idVenda = 0;

                    if (rdr.Read())
                    {
                        idVenda = int.Parse(rdr["id_venda"].ToString());
                        rdr.Close();
                        cConexao.Desconectar();
                    }

                    // Abrindo uma conexão para cada item da venda, sem necessidade.
                    cConexao.Conectar();


                    foreach (var produto in produtosParaVenda)
                    {
                        // query vulnerável a sqli
                        sql = "INSERT INTO itens_vendidos (id_venda, nome_produto, codigo_barras, preco_produto, quantidade) VALUES ('" + idVenda + "', '" + produto.NomeProduto + "', '" + produto.CodigoBarras + "', '" + produto.PrecoProduto + "', '" + quantidade + "')";
                        cmd = new MySqlCommand(sql, cConexao.conexao);


                        // TODO: Sem tratamento de erro
                        cmd.ExecuteNonQuery();
                    }

                    // Retirado de dentro do foreach
                    cConexao.Desconectar();

                    // id_venda int(11) NOT NULL AUTO_INCREMENT, nome_produto varchar(255) NOT NULL, codigo_barras varchar(255) NOT NULL, preco_produto decimal(10,2) NOT NULL, quantidade int(11) NOT NULL, PRIMARY KEY (id_venda)

                    Console.WriteLine("Venda realizada com sucesso!");
                    Console.ReadKey();
                    Console.Clear();
                    cMenuPrincipal.MenuPrincipal();

                }
            }
        }
    }
}
