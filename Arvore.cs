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
  private int ObterAltura(NoArvore<Dado> no)
  {
    return no == null ? 0 : no.Altura;
  }

  private int ObterFatorBalanceamento(NoArvore<Dado> no)
  {
    return no == null ? 0 : ObterAltura(no.Esq) - ObterAltura(no.Dir);
  }

  private void AtualizarAltura(NoArvore<Dado> no)
  {
    if (no != null)
      no.Altura = 1 + Math.Max(ObterAltura(no.Esq), ObterAltura(no.Dir));
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

  // Balancear o nó após inserção ou remoção
  private NoArvore<Dado> Balancear(NoArvore<Dado> no, Dado dado)
  {
    AtualizarAltura(no);

    int fator = ObterFatorBalanceamento(no);

    // Caso Esquerda-Esquerda
    if (fator > 1 && no.Esq != null && dado.CompareTo(no.Esq.Info) < 0)
      return RotacaoDireita(no);

    // Caso Direita-Direita
    if (fator < -1 && no.Dir != null && dado.CompareTo(no.Dir.Info) > 0)
      return RotacaoEsquerda(no);

    // Caso Esquerda-Direita
    if (fator > 1 && no.Esq != null && dado.CompareTo(no.Esq.Info) > 0)
    {
      no.Esq = RotacaoEsquerda(no.Esq);
      return RotacaoDireita(no);
    }

    // Caso Direita-Esquerda
    if (fator < -1 && no.Dir != null && dado.CompareTo(no.Dir.Info) < 0)
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

  // Inserção balanceada AVL
  public bool IncluirNovoDado(Dado novoDado)
  {
    if (Existe(novoDado))
      return false;  // não incluiu pois já existe

    raiz = InserirAVL(raiz, novoDado);
    return true;
  }

  private NoArvore<Dado> InserirAVL(NoArvore<Dado> no, Dado dado)
  {
    // Inserção padrão BST
    if (no == null)
      return new NoArvore<Dado>(dado);

    if (dado.CompareTo(no.Info) < 0)
      no.Esq = InserirAVL(no.Esq, dado);
    else if (dado.CompareTo(no.Info) > 0)
      no.Dir = InserirAVL(no.Dir, dado);
    else
      return no;  // duplicatas não permitidas

    // Balancear o nó após inserção
    return Balancear(no, dado);
  }

  // Remoção balanceada AVL
  public bool Excluir(Dado dadoAExcluir)
  {
    if (!Existe(dadoAExcluir))
      return false;

    raiz = ExcluirAVL(raiz, dadoAExcluir);
    return true;
  }

  private NoArvore<Dado> ExcluirAVL(NoArvore<Dado> no, Dado dado)
  {
    if (no == null)
      return no;

    // Navegação BST padrão
    if (dado.CompareTo(no.Info) < 0)
      no.Esq = ExcluirAVL(no.Esq, dado);
    else if (dado.CompareTo(no.Info) > 0)
      no.Dir = ExcluirAVL(no.Dir, dado);
    else
    {
      // Nó com um filho ou nenhum filho
      if (no.Esq == null || no.Dir == null)
      {
        NoArvore<Dado> temp = no.Esq ?? no.Dir;
        if (temp == null)
        {
          // Caso sem filhos
          no = null;
        }
        else
        {
          // Caso com um filho
          no = temp;
        }
      }
      else
      {
        // Nó com dois filhos: buscar o maior da subárvore esquerda
        NoArvore<Dado> temp = ObterMaior(no.Esq);
        no.Info = temp.Info;
        no.Esq = ExcluirAVL(no.Esq, temp.Info);
      }
    }

    if (no == null)
      return no;

    // Atualizar altura e balancear
    AtualizarAltura(no);
    return BalancearAposRemocao(no);
  }

  private NoArvore<Dado> ObterMaior(NoArvore<Dado> no)
  {
    NoArvore<Dado> atual = no;
    while (atual.Dir != null)
      atual = atual.Dir;
    return atual;
  }

  private NoArvore<Dado> BalancearAposRemocao(NoArvore<Dado> no)
  {
    int fator = ObterFatorBalanceamento(no);

    // Caso Esquerda-Esquerda
    if (fator > 1 && ObterFatorBalanceamento(no.Esq) >= 0)
      return RotacaoDireita(no);

    // Caso Esquerda-Direita
    if (fator > 1 && ObterFatorBalanceamento(no.Esq) < 0)
    {
      no.Esq = RotacaoEsquerda(no.Esq);
      return RotacaoDireita(no);
    }

    // Caso Direita-Direita
    if (fator < -1 && ObterFatorBalanceamento(no.Dir) <= 0)
      return RotacaoEsquerda(no);

    // Caso Direita-Esquerda
    if (fator < -1 && ObterFatorBalanceamento(no.Dir) > 0)
    {
      no.Dir = RotacaoDireita(no.Dir);
      return RotacaoEsquerda(no);
    }

    return no;
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
        // Atualizar altura do nó após construir subárvores
        AtualizarAltura(noAtual);
      }
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

