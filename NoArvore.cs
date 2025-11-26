// ============================================================
// Projeto 4 - Cadastro de Cidades e Caminhos
// Disciplina: Estruturas de Dados - 2o DSNot 2025
// ------------------------------------------------------------
// Alunos:
//   Gabriel da Silva Nascimento - RA: 24.01266-2
//   Claudio Correa Gorza Filho - RA: 24.01214-0
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class NoArvore<Dado> : IComparable<NoArvore<Dado>>
             where Dado : IComparable<Dado>, IRegistro, new()
{
  Dado info;           // informação armazenada neste nó da árvore
  private NoArvore<Dado> esq, dir;
  private int altura;  // altura do nó para balanceamento AVL

  public NoArvore(Dado informacao)
  {
    info = informacao;  
    esq = dir = null;
    altura = 1;  // nó folha tem altura 1
  }

  public NoArvore(Dado dados, NoArvore<Dado> esquerdo, NoArvore<Dado> direito)
  {
    this.Info = dados;
    this.Esq = esquerdo;
    this.Dir = direito;
    // Calcular altura baseado nos filhos
    int alturaEsq = esquerdo != null ? esquerdo.Altura : 0;
    int alturaDir = direito != null ? direito.Altura : 0;
    this.altura = 1 + Math.Max(alturaEsq, alturaDir);
  }

  public Dado Info 
  { 
    get => info; 
    set => info = value; 
  }
  public NoArvore<Dado> Esq { get => esq; set => esq = value; }
  public NoArvore<Dado> Dir { get => dir; set => dir = value; }
  public int Altura { get => altura; set => altura = value; }

  public int CompareTo(NoArvore<Dado> other)
  {
    if (other == null)
      return 1;
    return this.info.CompareTo(other.info);
  }
}

