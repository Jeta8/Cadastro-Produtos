﻿using MySql.Data.MySqlClient;
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
                // Limpa a tela
                Console.Clear();

                // Mostra o título
                Console.WriteLine("Cadastrar Produto");

                // Pede o nome do produto mas bloqueando campos vazios
                Console.Write("Nome do Produto: ");
                string nomeProduto = Console.ReadLine();
                while (nomeProduto == "")
                {
                    Console.WriteLine("O nome do produto não pode ser vazio");
                    Console.Write("Nome do Produto: ");
                    nomeProduto = Console.ReadLine();
                }
                


                // Pede o código de barras
                Console.Write("Código de Barras: ");
                string codigoBarras = Console.ReadLine();
                while (codigoBarras == "")
                {
                    Console.WriteLine("O código de barras não pode ser vazio");
                    Console.Write("Código de Barras: ");
                    codigoBarras = Console.ReadLine();
                }


               // Verifica se o produto ja existe buscando exatamente o mesmo codigo de barras
                cConexao.Conectar();
                string sql = "SELECT * FROM produtos_cadastrados WHERE codigo_barras = '" + codigoBarras + "'";
                MySqlCommand cmd = new MySqlCommand(sql, cConexao.conexao);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    Console.WriteLine("Já existe um produto cadastrado com esse código de barras");
                    Console.WriteLine("Deseja cadastrar o produto mas com outro codigo de barras? (S/N)");
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

                // Pede o preço do produto
                Console.Write("Preço do Produto: ");
                string precoProduto = Console.ReadLine();
                while (precoProduto == "")
                {
                    Console.WriteLine("O preço do produto não pode ser vazio");
                    Console.Write("Preço do Produto: ");
                    precoProduto = Console.ReadLine();
                }

                // Pede o estoque do produto
                Console.Write("Estoque do Produto: ");
                string estoqueProduto = Console.ReadLine();
                while (estoqueProduto == "")
                {
                    Console.WriteLine("O estoque do produto não pode ser vazio");
                    Console.Write("Estoque do Produto: ");
                    estoqueProduto = Console.ReadLine();
                }

                // Cria o produto
                Mercado produto = new Mercado();
                produto.NomeProduto = nomeProduto;
                produto.CodigoBarras = codigoBarras;
                produto.PrecoProduto = decimal.Parse(precoProduto);
                produto.EstoqueProduto = int.Parse(estoqueProduto);

                // Adiciona produto no banco de dados
                cConexao.Conectar();
                sql = "INSERT INTO produtos_cadastrados (nome_produto, codigo_barras, preco_produto, estoque_produto) VALUES ('" + nomeProduto + "', '" + codigoBarras + "', '" + precoProduto + "', '" + estoqueProduto + "')";
                cmd = new MySqlCommand(sql, cConexao.conexao);
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
                    ListarProdutos.cListarProdutos.ListarProdutos();
                    Console.ReadKey();
                    Console.Clear();
                    cMenuPrincipal.MenuPrincipal();
                }
            }
        }

    }
}