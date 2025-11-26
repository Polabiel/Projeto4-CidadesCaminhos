// ============================================================
// Projeto 4 - Cadastro de Cidades e Caminhos
// Disciplina: Estruturas de Dados - 2o DSNot 2025
// ------------------------------------------------------------
// Alunos:
//   Andrew Douglas Nithack - RA: 23305
//   Gabriel Oliveira dos Santos - RA: 23600
// ============================================================

using System;
using System.IO;
using AgendaAlfabetica;

namespace Proj4
{
  public static class ArquivoHelper
  {
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

    public static void GravarArquivoLigacoes(string nomeArquivo, Arvore<Cidade> arvore)
    {
      using (StreamWriter writer = new StreamWriter(nomeArquivo, false))
      {
        GravarLigacoesRecursivo(arvore.Raiz, writer);
      }
    }
    private static void GravarLigacoesRecursivo(NoArvore<Cidade> no, StreamWriter writer)
    {
      if (no == null)
        return;

      GravarLigacoesRecursivo(no.Esq, writer);

      Cidade cidade = no.Info;
      ListaSimples<Ligacao> ligacoes = cidade.Ligacoes;
      
      ligacoes.IniciarPercursoSequencial();
      while (ligacoes.PodePercorrer())
      {
        Ligacao ligacao = ligacoes.Atual.Info;
        writer.WriteLine($"{cidade.Nome.Trim()};{ligacao.CidadeDestino};{ligacao.Distancia}");
      }

      GravarLigacoesRecursivo(no.Dir, writer);
    }
  }
}
