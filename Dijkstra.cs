// ============================================================
// Projeto 4 - Cadastro de Cidades e Caminhos
// Disciplina: Estruturas de Dados - 2o DSNot 2025
// ------------------------------------------------------------
// Alunos:
//   Andrew Douglas Nithack - RA: 23305
//   Gabriel Oliveira dos Santos - RA: 23600
// ============================================================

using System;
using System.Collections.Generic;

namespace Proj4
{

  public class ResultadoDijkstra
  {

    public List<string> Caminho { get; private set; }


    public int DistanciaTotal { get; private set; }

    public bool CaminhoEncontrado { get { return Caminho.Count > 0; } }

    public ResultadoDijkstra(List<string> caminho, int distanciaTotal)
    {
      Caminho = caminho ?? new List<string>();
      DistanciaTotal = distanciaTotal;
    }
  }

  public static class Dijkstra
  {
    
    public static ResultadoDijkstra BuscarMenorCaminho(Arvore<Cidade> arvore, string nomeOrigem, string nomeDestino)
    {
      // Obtém todas as cidades da árvore
      List<Cidade> todasCidades = new List<Cidade>();
      arvore.VisitarEmOrdem(todasCidades);

      // Cria um dicionário para acesso rápido às cidades por nome normalizado
      Dictionary<string, Cidade> cidadesPorNome = new Dictionary<string, Cidade>();
      foreach (Cidade cidade in todasCidades)
      {
        string nomeNormalizado = cidade.Nome.Trim();
        cidadesPorNome[nomeNormalizado] = cidade;
      }

      // Verifica se as cidades de origem e destino existem
      string origemNormalizada = nomeOrigem?.Trim() ?? "";
      string destinoNormalizado = nomeDestino?.Trim() ?? "";

      if (!cidadesPorNome.ContainsKey(origemNormalizada) || !cidadesPorNome.ContainsKey(destinoNormalizado))
      {
        return new ResultadoDijkstra(new List<string>(), int.MaxValue);
      }

      // Se origem e destino são iguais, retorna caminho com apenas a cidade
      if (origemNormalizada == destinoNormalizado)
      {
        return new ResultadoDijkstra(new List<string> { origemNormalizada }, 0);
      }

      // Inicialização do algoritmo de Dijkstra
      Dictionary<string, int> distancias = new Dictionary<string, int>();
      Dictionary<string, string> anteriores = new Dictionary<string, string>();
      HashSet<string> visitados = new HashSet<string>();

      // Inicializa todas as distâncias como infinito
      foreach (Cidade cidade in todasCidades)
      {
        string nome = cidade.Nome.Trim();
        distancias[nome] = int.MaxValue;
        anteriores[nome] = null;
      }

      // Distância da origem para ela mesma é 0
      distancias[origemNormalizada] = 0;

      // Executa o algoritmo de Dijkstra
      while (visitados.Count < todasCidades.Count)
      {
        // Encontra a cidade não visitada com menor distância
        string cidadeAtual = null;
        int menorDistancia = int.MaxValue;

        foreach (var kvp in distancias)
        {
          if (!visitados.Contains(kvp.Key) && kvp.Value < menorDistancia)
          {
            menorDistancia = kvp.Value;
            cidadeAtual = kvp.Key;
          }
        }

        // Se não encontrou cidade acessível, não há caminho
        if (cidadeAtual == null || menorDistancia == int.MaxValue)
        {
          break;
        }

        // Se chegamos ao destino, podemos parar
        if (cidadeAtual == destinoNormalizado)
        {
          break;
        }

        // Marca como visitado
        visitados.Add(cidadeAtual);

        // Obtém a cidade e suas ligações
        Cidade cidade = cidadesPorNome[cidadeAtual];
        var ligacoes = cidade.Ligacoes.Listar();

        // Atualiza as distâncias dos vizinhos
        foreach (var ligacao in ligacoes)
        {
          string nomeVizinho = ligacao.CidadeDestino.Trim();
          
          // Verifica se o vizinho existe no grafo
          if (!cidadesPorNome.ContainsKey(nomeVizinho))
            continue;

          // Verifica se já foi visitado
          if (visitados.Contains(nomeVizinho))
            continue;

          // Calcula a nova distância
          int novaDistancia = distancias[cidadeAtual] + ligacao.Distancia;

          // Se a nova distância for menor, atualiza
          if (novaDistancia < distancias[nomeVizinho])
          {
            distancias[nomeVizinho] = novaDistancia;
            anteriores[nomeVizinho] = cidadeAtual;
          }
        }
      }

      // Verifica se chegamos ao destino
      if (distancias[destinoNormalizado] == int.MaxValue)
      {
        return new ResultadoDijkstra(new List<string>(), int.MaxValue);
      }

      // Reconstrói o caminho do destino até a origem
      List<string> caminho = new List<string>();
      string atual = destinoNormalizado;
      
      while (atual != null)
      {
        caminho.Add(atual);
        atual = anteriores[atual];
      }

      // Inverte o caminho para ir da origem ao destino
      caminho.Reverse();

      return new ResultadoDijkstra(caminho, distancias[destinoNormalizado]);
    }
  }
}
