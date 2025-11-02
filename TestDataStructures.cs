using System;
using System.Collections.Generic;
using AgendaAlfabetica;
using Proj4;

/// <summary>
/// Classe para testar as estruturas de dados básicas do projeto
/// </summary>
public static class TestDataStructures
{
  /// <summary>
  /// Testa todas as estruturas de dados: ListaSimples, Arvore AVL e FilaLista
  /// </summary>
  public static void ExecutarTodosTestes()
  {
    Console.WriteLine("=== INICIANDO TESTES DAS ESTRUTURAS DE DADOS ===\n");
    
    TestarListaSimples();
    Console.WriteLine();
    
    TestarArvoreAVL();
    Console.WriteLine();
    
    TestarFilaLista();
    Console.WriteLine();
    
    Console.WriteLine("=== TODOS OS TESTES CONCLUÍDOS ===");
  }

  /// <summary>
  /// Testa a ListaSimples com inserção, busca, remoção e percurso
  /// </summary>
  private static void TestarListaSimples()
  {
    Console.WriteLine("--- Testando ListaSimples ---");
    var lista = new ListaSimples<int>();

    // Teste de inserção em ordem
    Console.WriteLine("Inserindo elementos: 5, 3, 8, 1, 9, 2");
    lista.InserirEmOrdem(5);
    lista.InserirEmOrdem(3);
    lista.InserirEmOrdem(8);
    lista.InserirEmOrdem(1);
    lista.InserirEmOrdem(9);
    lista.InserirEmOrdem(2);

    // Verificar lista ordenada
    var elementos = lista.Listar();
    Console.Write("Lista ordenada: ");
    foreach (var e in elementos)
      Console.Write(e + " ");
    Console.WriteLine();

    // Teste de busca
    Console.WriteLine("Busca por 8: " + (lista.ExisteDado(8) ? "Encontrado" : "Não encontrado"));
    Console.WriteLine("Busca por 10: " + (lista.ExisteDado(10) ? "Encontrado" : "Não encontrado"));

    // Teste de remoção
    Console.WriteLine("Removendo elemento 3");
    lista.RemoverDado(3);
    elementos = lista.Listar();
    Console.Write("Lista após remoção: ");
    foreach (var e in elementos)
      Console.Write(e + " ");
    Console.WriteLine();

    // Teste de percurso
    Console.WriteLine("Contagem de elementos: " + lista.QuantosNos);
    Console.WriteLine("Lista está vazia? " + (lista.EstaVazia ? "Sim" : "Não"));

    Console.WriteLine("✓ Testes de ListaSimples concluídos");
  }

  /// <summary>
  /// Testa a Árvore AVL com inserção balanceada, busca, remoção e percurso em ordem
  /// </summary>
  private static void TestarArvoreAVL()
  {
    Console.WriteLine("--- Testando Árvore AVL ---");
    var arvore = new Arvore<CidadeTeste>();

    // Inserção que deve causar rotações AVL
    Console.WriteLine("Inserindo cidades (valores que causam rotações AVL):");
    var cidades = new[] { 
      new CidadeTeste("Cidade10", 10, 10),
      new CidadeTeste("Cidade05", 5, 5),
      new CidadeTeste("Cidade15", 15, 15),
      new CidadeTeste("Cidade03", 3, 3),
      new CidadeTeste("Cidade07", 7, 7),
      new CidadeTeste("Cidade12", 12, 12),
      new CidadeTeste("Cidade20", 20, 20),
      new CidadeTeste("Cidade01", 1, 1),  // Deve causar rotação
      new CidadeTeste("Cidade25", 25, 25)  // Deve causar rotação
    };

    foreach (var cidade in cidades)
    {
      bool inserido = arvore.IncluirNovoDado(cidade);
      Console.WriteLine($"  {cidade.Nome.Trim()}: " + (inserido ? "Inserido" : "Duplicado"));
    }

    // Teste de busca
    Console.WriteLine("\nTeste de busca:");
    var cidadeBusca = new CidadeTeste("Cidade07", 0, 0);
    Console.WriteLine("Busca por Cidade07: " + (arvore.Existe(cidadeBusca) ? "Encontrado" : "Não encontrado"));
    
    var cidadeNaoExiste = new CidadeTeste("Cidade99", 0, 0);
    Console.WriteLine("Busca por Cidade99: " + (arvore.Existe(cidadeNaoExiste) ? "Encontrado" : "Não encontrado"));

    // Teste de percurso em ordem
    Console.WriteLine("\nPercurso em ordem (deve estar ordenado):");
    var lista = new List<CidadeTeste>();
    arvore.VisitarEmOrdem(lista);
    Console.Write("  ");
    foreach (var c in lista)
      Console.Write(c.Nome.Trim() + " ");
    Console.WriteLine();

    // Teste de remoção com rebalanceamento
    Console.WriteLine("\nRemovendo Cidade05 (pode causar rebalanceamento):");
    bool removido = arvore.Excluir(new CidadeTeste("Cidade05", 0, 0));
    Console.WriteLine("  Removido: " + (removido ? "Sim" : "Não"));
    
    lista.Clear();
    arvore.VisitarEmOrdem(lista);
    Console.Write("  Árvore após remoção: ");
    foreach (var c in lista)
      Console.Write(c.Nome.Trim() + " ");
    Console.WriteLine();

    Console.WriteLine("\n✓ Testes de Árvore AVL concluídos");
  }

  /// <summary>
  /// Testa a FilaLista com enfileirar, desenfileirar e operações de fila
  /// </summary>
  private static void TestarFilaLista()
  {
    Console.WriteLine("--- Testando FilaLista ---");
    var fila = new FilaLista<string>();

    // Teste de enfileirar
    Console.WriteLine("Enfileirando: A, B, C, D");
    fila.Enfileirar("A");
    fila.Enfileirar("B");
    fila.Enfileirar("C");
    fila.Enfileirar("D");

    // Verificar início e fim
    Console.WriteLine($"Início da fila: {fila.OInicio()}");
    Console.WriteLine($"Fim da fila: {fila.OFim()}");
    Console.WriteLine($"Tamanho da fila: {fila.Tamanho}");

    // Teste de desenfileirar
    Console.WriteLine("\nDesenfileirando elementos:");
    while (!fila.EstaVazia)
    {
      var elemento = fila.Retirar();
      Console.WriteLine($"  Retirado: {elemento}, Tamanho restante: {fila.Tamanho}");
    }

    Console.WriteLine("Fila está vazia? " + (fila.EstaVazia ? "Sim" : "Não"));

    // Teste de exceção quando fila vazia
    Console.WriteLine("\nTestando exceção em fila vazia:");
    try
    {
      fila.Retirar();
      Console.WriteLine("  ERRO: Deveria ter lançado exceção!");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"  ✓ Exceção capturada: {ex.Message}");
    }

    Console.WriteLine("\n✓ Testes de FilaLista concluídos");
  }

  /// <summary>
  /// Classe auxiliar para testes da árvore AVL
  /// </summary>
  private class CidadeTeste : IComparable<CidadeTeste>, IRegistro
  {
    private const int TAMANHO_NOME = 25;
    private const int TAMANHO_REGISTRO = TAMANHO_NOME + (2 * sizeof(double)); // 25 + 16 = 41

    public string Nome { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public int TamanhoRegistro => TAMANHO_REGISTRO;

    public CidadeTeste(string nome, double x, double y)
    {
      Nome = nome.PadRight(TAMANHO_NOME);
      X = x;
      Y = y;
    }

    public int CompareTo(CidadeTeste other)
    {
      return Nome.CompareTo(other.Nome);
    }

    public void LerRegistro(System.IO.BinaryReader arquivo, long qualRegistro)
    {
      // Não implementado para testes
    }

    public void GravarRegistro(System.IO.BinaryWriter arquivo)
    {
      // Não implementado para testes
    }
  }
}
