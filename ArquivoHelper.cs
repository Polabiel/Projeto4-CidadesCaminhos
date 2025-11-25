using System;
using System.IO;
using AgendaAlfabetica;

namespace Proj4
{
  /// <summary>
  /// Classe auxiliar para leitura e gravação de arquivos de cidades e ligações.
  /// </summary>
  public static class ArquivoHelper
  {
    /// <summary>
    /// Lê o arquivo de ligações (GrafoOnibusSaoPaulo.txt) e adiciona cada ligação
    /// à lista de ligações da cidade de origem na árvore.
    /// Formato do arquivo: origem;destino;distancia
    /// </summary>
    /// <param name="nomeArquivo">Caminho do arquivo de ligações</param>
    /// <param name="arvore">Árvore AVL de cidades</param>
    public static void LerArquivoLigacoes(string nomeArquivo, Arvore<Cidade> arvore)
    {
      if (!File.Exists(nomeArquivo))
        return;

      using (StreamReader reader = new StreamReader(nomeArquivo))
      {
        string linha;
        while ((linha = reader.ReadLine()) != null)
        {
          if (string.IsNullOrWhiteSpace(linha))
            continue;

          string[] partes = linha.Split(';');
          if (partes.Length >= 3)
          {
            string nomeOrigem = partes[0].Trim();
            string nomeDestino = partes[1].Trim();
            int distancia;
            
            if (int.TryParse(partes[2].Trim(), out distancia))
            {
              // Busca a cidade de origem na árvore
              Cidade cidadeBusca = new Cidade(nomeOrigem);
              if (arvore.Existe(cidadeBusca))
              {
                Cidade cidadeOrigem = arvore.Atual.Info;
                cidadeOrigem.AdicionarLigacao(nomeDestino, distancia);
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// Grava todas as ligações das cidades da árvore no arquivo texto.
    /// Formato do arquivo: origem;destino;distancia
    /// </summary>
    /// <param name="nomeArquivo">Caminho do arquivo de ligações</param>
    /// <param name="arvore">Árvore AVL de cidades</param>
    public static void GravarArquivoLigacoes(string nomeArquivo, Arvore<Cidade> arvore)
    {
      using (StreamWriter writer = new StreamWriter(nomeArquivo, false))
      {
        GravarLigacoesRecursivo(arvore.Raiz, writer);
      }
    }

    /// <summary>
    /// Percorre a árvore em ordem e grava as ligações de cada cidade.
    /// </summary>
    private static void GravarLigacoesRecursivo(NoArvore<Cidade> no, StreamWriter writer)
    {
      if (no == null)
        return;

      // Visita subárvore esquerda
      GravarLigacoesRecursivo(no.Esq, writer);

      // Grava as ligações da cidade atual
      Cidade cidade = no.Info;
      ListaSimples<Ligacao> ligacoes = cidade.Ligacoes;
      
      ligacoes.IniciarPercursoSequencial();
      while (ligacoes.PodePercorrer())
      {
        Ligacao ligacao = ligacoes.Atual.Info;
        writer.WriteLine($"{cidade.Nome.Trim()};{ligacao.CidadeDestino};{ligacao.Distancia}");
      }

      // Visita subárvore direita
      GravarLigacoesRecursivo(no.Dir, writer);
    }
  }
}
