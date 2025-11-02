using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;


public class Arvore<Dado> 
             where Dado : IComparable<Dado>, IRegistro, new()
{
  private NoArvore<Dado>  raiz,   // raiz da árvore; nó principal
                          atual,  // ponteiro para o nó visitado atualmente
                          antecessor;  // ponteiro para o "pai" do nó noAtual

  public NoArvore<Dado> Raiz { get => raiz; set => raiz = value; }
  public NoArvore<Dado> Atual { get => atual; }
  public NoArvore<Dado> Antecessor { get => antecessor; }

  public Arvore()
  {
    raiz = atual = antecessor = null;
  }

  // Métodos auxiliares para AVL
  private int Altura(NoArvore<Dado> no)
  {
    return no == null ? 0 : no.Altura;
  }

  private int FatorBalanceamento(NoArvore<Dado> no)
  {
    return no == null ? 0 : Altura(no.Esq) - Altura(no.Dir);
  }

  private void AtualizarAltura(NoArvore<Dado> no)
  {
    if (no != null)
      no.Altura = 1 + Math.Max(Altura(no.Esq), Altura(no.Dir));
  }

  // Rotação simples à direita
  private NoArvore<Dado> RotacaoDireita(NoArvore<Dado> y)
  {
    NoArvore<Dado> x = y.Esq;
    NoArvore<Dado> T2 = x.Dir;

    x.Dir = y;
    y.Esq = T2;

    AtualizarAltura(y);
    AtualizarAltura(x);

    return x;
  }

  // Rotação simples à esquerda
  private NoArvore<Dado> RotacaoEsquerda(NoArvore<Dado> x)
  {
    NoArvore<Dado> y = x.Dir;
    NoArvore<Dado> T2 = y.Esq;

    y.Esq = x;
    x.Dir = T2;

    AtualizarAltura(x);
    AtualizarAltura(y);

    return y;
  }

  // Balancear nó após inserção ou remoção
  private NoArvore<Dado> Balancear(NoArvore<Dado> no)
  {
    if (no == null)
      return null;

    AtualizarAltura(no);
    int balance = FatorBalanceamento(no);

    // Caso Esquerda-Esquerda
    if (balance > 1 && FatorBalanceamento(no.Esq) >= 0)
      return RotacaoDireita(no);

    // Caso Esquerda-Direita
    if (balance > 1 && FatorBalanceamento(no.Esq) < 0)
    {
      no.Esq = RotacaoEsquerda(no.Esq);
      return RotacaoDireita(no);
    }

    // Caso Direita-Direita
    if (balance < -1 && FatorBalanceamento(no.Dir) <= 0)
      return RotacaoEsquerda(no);

    // Caso Direita-Esquerda
    if (balance < -1 && FatorBalanceamento(no.Dir) > 0)
    {
      no.Dir = RotacaoDireita(no.Dir);
      return RotacaoEsquerda(no);
    }

    return no;
  }

  public void VisitarEmOrdem(List<Dado> lista)
  {
    VisitarEmOrdem(raiz, lista);
  }

  private void VisitarEmOrdem(NoArvore<Dado> atual, List<Dado> lista)
  {
    if (atual != null)    // se existe um nó
    {
      VisitarEmOrdem(atual.Esq, lista);             // chamada 1
      lista.Add(atual.Info);                // seu dado é listado
      VisitarEmOrdem(atual.Dir, lista);             // chamada 2
    }
  }

  public void Desenhar(Control tela)
  {
     if (Raiz == null) return;
     
     Graphics g = tela.CreateGraphics();
     g.Clear(Color.White);
     DesenharNo(g, Raiz, tela.Width / 2, 20, tela.Width / 4, 50);
  }

  private void DesenharNo(Graphics g, NoArvore<Dado> no, int x, int y, int deslocamentoX, int deslocamentoY)
  {
    if (no == null) return;

    Rectangle rect = new Rectangle(x - 15, y - 15, 40, 30);
    g.FillEllipse(Brushes.LightBlue, rect);
    g.DrawEllipse(Pens.Black, rect);
    g.DrawString(no.Info.ToString(), new Font("Arial", 10), Brushes.Black, x - 10, y - 10);

    if (no.Esq != null)
    {
      g.DrawLine(Pens.Black, x, y + 15, x - deslocamentoX, y + deslocamentoY);
      DesenharNo(g, no.Esq, x - deslocamentoX, y + deslocamentoY, (int)(deslocamentoX / 2.05), deslocamentoY);
    }

    if (no.Dir != null)
    {
      g.DrawLine(Pens.Black, x, y + 15, x + deslocamentoX, y + deslocamentoY);
      DesenharNo(g, no.Dir, x + deslocamentoX, y + deslocamentoY, (int) (deslocamentoX / 2.05), deslocamentoY);
    }
  }

  public bool Existe(Dado procurado)
  {
    antecessor = null;    // a raiz não tem um antecessor
    atual = raiz;         // posiciona ponteiro de percurso no 1o nó da árvore
    while (atual != null)
    {
      if (procurado.CompareTo(atual.Info) == 0)
        return true;    // achamos e noAtual aponta o nó do procurado

      antecessor = atual; // mudaremos para o nível debaixo deste
      if (procurado.CompareTo(atual.Info) < 0)
        atual = atual.Esq;  // à esquerda, ficam os menores que noAtual
      else
        atual = atual.Dir;  // à direita, ficam os maiores que noAtual
    }
    return false;
  }

  public bool IncluirNovoDado(Dado novoDado)
  {
    if (Existe(novoDado)) // achou!
      return false;       // não incluiu pois já existe

    raiz = InserirAVL(raiz, novoDado);
    return true;    // feita a inclusão
  }

  // Método recursivo para inserção com balanceamento AVL
  private NoArvore<Dado> InserirAVL(NoArvore<Dado> no, Dado dado)
  {
    // 1. Inserção BST normal
    if (no == null)
      return new NoArvore<Dado>(dado);

    if (dado.CompareTo(no.Info) < 0)
      no.Esq = InserirAVL(no.Esq, dado);
    else if (dado.CompareTo(no.Info) > 0)
      no.Dir = InserirAVL(no.Dir, dado);
    else
      return no; // duplicata não permitida

    // 2. Atualizar altura e balancear
    return Balancear(no);
  }

  public bool Excluir(Dado dadoAExcluir)
  {
    if (!Existe(dadoAExcluir))
      return false;

    raiz = ExcluirAVL(raiz, dadoAExcluir);
    return true;
  }

  // Método recursivo para remoção com balanceamento AVL
  private NoArvore<Dado> ExcluirAVL(NoArvore<Dado> no, Dado dado)
  {
    if (no == null)
      return null;

    // 1. Remoção BST normal
    if (dado.CompareTo(no.Info) < 0)
      no.Esq = ExcluirAVL(no.Esq, dado);
    else if (dado.CompareTo(no.Info) > 0)
      no.Dir = ExcluirAVL(no.Dir, dado);
    else
    {
      // Nó com um filho ou sem filhos
      if (no.Esq == null)
        return no.Dir;
      else if (no.Dir == null)
        return no.Esq;

      // Nó com dois filhos: pegar o menor da subárvore direita
      NoArvore<Dado> temp = ObterMenorNo(no.Dir);
      no.Info = temp.Info;
      no.Dir = ExcluirAVL(no.Dir, temp.Info);
    }

    // 2. Atualizar altura e balancear
    return Balancear(no);
  }

  // Método auxiliar para encontrar o menor nó
  private NoArvore<Dado> ObterMenorNo(NoArvore<Dado> no)
  {
    NoArvore<Dado> atual = no;
    while (atual.Esq != null)
      atual = atual.Esq;
    return atual;
  }

  public void LerArquivoDeRegistros(string nomeArquivo)
  {
    raiz = null;    // arvore fica vazia
    Dado dado = new Dado();
    var origem = new FileStream(nomeArquivo, FileMode.OpenOrCreate);
    var arquivo = new BinaryReader(origem);
    int posicaoFinal = (int)origem.Length / dado.TamanhoRegistro - 1;
    Particionar(0, posicaoFinal, ref raiz);
    origem.Close();
    
    // Recalcula alturas em uma única passagem após carregar toda a árvore
    RecalcularAlturas(raiz);
    
    void Particionar(long inicio, long fim, ref NoArvore<Dado> noAtual)
    {
      if (inicio <= fim)
      {
        long meio = (inicio + fim) / 2;
        dado = new Dado(); // cria um objeto para armazenar os dados
        dado.LerRegistro(arquivo, meio); // 
        noAtual = new NoArvore<Dado>(dado);
        var novoEsq = noAtual.Esq;
        Particionar(inicio, meio - 1, ref novoEsq); // Particiona à esquerda 
        noAtual.Esq = novoEsq;
        var novoDir = noAtual.Dir;
        Particionar(meio + 1, fim, ref novoDir); // Particiona à direita 
        noAtual.Dir = novoDir;
      }
    }
  }

  // Recalcula alturas de toda a árvore em uma única passagem pós-ordem
  private void RecalcularAlturas(NoArvore<Dado> no)
  {
    if (no != null)
    {
      RecalcularAlturas(no.Esq);
      RecalcularAlturas(no.Dir);
      AtualizarAltura(no);
    }
  }

  public void GravarArquivoDeRegistros(string nomeArquivo)
  {
    var destino = new FileStream(nomeArquivo, FileMode.Create);
    var arquivo = new BinaryWriter(destino);
    GravarInOrdem(raiz);
    arquivo.Close();

    void GravarInOrdem(NoArvore<Dado> noAtual)
    {
      if (noAtual != null)
      {
        GravarInOrdem(noAtual.Esq);
        noAtual.Info.GravarRegistro(arquivo);
        GravarInOrdem(noAtual.Dir);
      }
    }
  }
}

