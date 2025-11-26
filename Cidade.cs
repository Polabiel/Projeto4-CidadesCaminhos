// ============================================================
// Projeto 4 - Cadastro de Cidades e Caminhos
// Disciplina: Estruturas de Dados - 2o DSNot 2025
// ------------------------------------------------------------
// Alunos:
//   Andrew Douglas Nithack - RA: 23305
//   Gabriel Oliveira dos Santos - RA: 23600
// ============================================================

using AgendaAlfabetica;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Proj4
{
  public class Cidade : IComparable<Cidade>, IRegistro
  {
    string nome;
    double x, y;
    ListaSimples<Ligacao> ligacoes = new ListaSimples<Ligacao>();

    const int tamanhoNome = 25;
    const int tamanhoRegistro = tamanhoNome + (2 * sizeof(double));
    
    // Encoding para compatibilidade com arquivos existentes (Windows-1252/Latin1)
    private static readonly Encoding encodingArquivo = Encoding.GetEncoding(1252);

    public string Nome
    {
      // Guarda o nome sem espaços extras. Pad apenas ao gravar em arquivo.
      get => nome;
      set => nome = (value ?? string.Empty).Trim();
    }

    public ListaSimples<Ligacao> Ligacoes
    {
      get => ligacoes;
    }

    public Cidade(string nome, double x, double y)
    {
      this.Nome = nome;
      this.x = x;
      this.y = y;
    }

    public override string ToString()
    {
      return Nome + " (" + ligacoes.QuantosNos + ")";
    }

    public Cidade()
    {
      this.Nome = "";
      this.x = 0;
      this.y = 0;
    }

    public Cidade(string nome)
    {
      this.Nome = nome;
    }

    public int CompareTo(Cidade outraCid)
    {
      return Nome.CompareTo(outraCid.Nome);
    }

    public override bool Equals(object obj)
    {
      if (obj == null || GetType() != obj.GetType())
        return false;
      
      Cidade outra = (Cidade)obj;
      if (Nome == null && outra.Nome == null) return true;
      if (Nome == null || outra.Nome == null) return false;
      return Nome.Equals(outra.Nome);
    }

    public override int GetHashCode()
    {
      return Nome?.GetHashCode() ?? 0;
    }

    /// <summary>
    /// Verifica se existe uma ligação desta cidade para outra cidade pelo nome.
    /// </summary>
    /// <param name="nomeCidadeDestino">Nome da cidade de destino</param>
    /// <returns>True se a ligação existir, false caso contrário</returns>
    public bool ExisteLigacao(string nomeCidadeDestino)
    {
      Ligacao procurada = new Ligacao(nomeCidadeDestino, 0);
      return ligacoes.ExisteDado(procurada);
    }

    /// <summary>
    /// Adiciona uma ligação para outra cidade se ainda não existir.
    /// </summary>
    /// <param name="cidadeDestino">Nome da cidade de destino</param>
    /// <param name="distancia">Distância em km</param>
    /// <returns>True se adicionou, false se já existia</returns>
    public bool AdicionarLigacao(string cidadeDestino, int distancia)
    {
      Ligacao novaLigacao = new Ligacao(cidadeDestino, distancia);
      return ligacoes.InserirEmOrdem(novaLigacao);
    }

    /// <summary>
    /// Remove uma ligação para outra cidade.
    /// </summary>
    /// <param name="cidadeDestino">Nome da cidade de destino</param>
    /// <returns>True se removeu, false se não existia</returns>
    public bool RemoverLigacao(string cidadeDestino)
    {
      Ligacao aRemover = new Ligacao(cidadeDestino, 0);
      return ligacoes.RemoverDado(aRemover);
    }

    public int TamanhoRegistro { get => tamanhoRegistro; }
    public double X { get => x; set => x = value; }
    public double Y { get => y; set => y = value; }

    public void LerRegistro(BinaryReader arquivo, long qualRegistro)
    {
      arquivo.BaseStream.Seek(qualRegistro * tamanhoRegistro, SeekOrigin.Begin);
      byte[] nomeBytes = arquivo.ReadBytes(tamanhoNome);
      // Remove o padding ao ler do arquivo
      this.nome = encodingArquivo.GetString(nomeBytes).TrimEnd();
      this.x = arquivo.ReadDouble();
      this.y = arquivo.ReadDouble();
    }

    public void GravarRegistro(BinaryWriter arquivo)
    {
      // Ao gravar, padroniza em tamanho fixo
      byte[] nomeBytes = encodingArquivo.GetBytes(Nome.PadRight(tamanhoNome, ' ').Substring(0, tamanhoNome));
      arquivo.Write(nomeBytes);
      arquivo.Write(x);
      arquivo.Write(y);
    }
  }

}
