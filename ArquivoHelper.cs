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
              // Adiciona ligação A→B
              Cidade cidadeBuscaOrigem = new Cidade(nomeOrigem);
              if (arvore.Existe(cidadeBuscaOrigem))
              {
                Cidade cidadeOrigem = arvore.Atual.Info;
                cidadeOrigem.AdicionarLigacao(nomeDestino, distancia);
              }
              
              // Adiciona ligação recíproca B→A (as ligações são bidirecionais conforme documentação)
              Cidade cidadeBuscaDestino = new Cidade(nomeDestino);
              if (arvore.Existe(cidadeBuscaDestino))
              {
                Cidade cidadeDestino = arvore.Atual.Info;
                cidadeDestino.AdicionarLigacao(nomeOrigem, distancia);
              }
            }
          }
        }
      }
    }

    public static void GravarArquivoLigacoes(string nomeArquivo, Arvore<Cidade> arvore)
    {
      // Usamos um HashSet para evitar gravar ligações duplicadas (A→B e B→A são a mesma ligação)
      HashSet<string> ligacoesGravadas = new HashSet<string>();
      
      using (StreamWriter writer = new StreamWriter(nomeArquivo, false))
      {
        GravarLigacoesRecursivo(arvore.Raiz, writer, ligacoesGravadas);
      }
    }
    private static void GravarLigacoesRecursivo(NoArvore<Cidade> no, StreamWriter writer, HashSet<string> ligacoesGravadas)
    {
      if (no == null)
        return;

      GravarLigacoesRecursivo(no.Esq, writer, ligacoesGravadas);

      Cidade cidade = no.Info;
      string nomeOrigem = cidade.Nome.Trim();
      ListaSimples<Ligacao> ligacoes = cidade.Ligacoes;
      
      ligacoes.IniciarPercursoSequencial();
      while (ligacoes.PodePercorrer())
      {
        Ligacao ligacao = ligacoes.Atual.Info;
        string nomeDestino = ligacao.CidadeDestino.Trim();
        
        // Cria uma chave única para a ligação (ordem alfabética para garantir unicidade)
        string chave;
        if (string.Compare(nomeOrigem, nomeDestino, StringComparison.OrdinalIgnoreCase) < 0)
          chave = $"{nomeOrigem.ToUpperInvariant()}|{nomeDestino.ToUpperInvariant()}";
        else
          chave = $"{nomeDestino.ToUpperInvariant()}|{nomeOrigem.ToUpperInvariant()}";
        
        // Só grava se ainda não foi gravada (evita duplicatas de ligações bidirecionais)
        if (!ligacoesGravadas.Contains(chave))
        {
          writer.WriteLine($"{nomeOrigem};{nomeDestino};{ligacao.Distancia}");
          ligacoesGravadas.Add(chave);
        }
      }

      GravarLigacoesRecursivo(no.Dir, writer, ligacoesGravadas);
    }
  }
}
