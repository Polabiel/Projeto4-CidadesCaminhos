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
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;


public class Arvore<Dado>
             where Dado : IComparable<Dado>, IRegistro, new()
{
  private NoArvore<Dado>  raiz,
                          atual,
                          antecessor;

  public NoArvore<Dado> Raiz { get => raiz; set => raiz = value; }
  public NoArvore<Dado> Atual { get => atual; }
  public NoArvore<Dado> Antecessor { get => antecessor; }

  // Nó/dado atualmente selecionado (usado pela UI para destaque)
  public Dado Selecionado { get; set; }

  public Arvore()
  {
    raiz = atual = antecessor = null;
    Selecionado = default(Dado);
  }

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

  private NoArvore<Dado> Balancear(NoArvore<Dado> no, Dado dado)
  {
    AtualizarAltura(no);

    int fator = ObterFatorBalanceamento(no);


    if (fator > 1 && no.Esq != null && dado.CompareTo(no.Esq.Info) < 0)
      return RotacaoDireita(no);

    if (fator < -1 && no.Dir != null && dado.CompareTo(no.Dir.Info) > 0)
      return RotacaoEsquerda(no);

    if (fator > 1 && no.Esq != null && dado.CompareTo(no.Esq.Info) > 0)
    {
      no.Esq = RotacaoEsquerda(no.Esq);
      return RotacaoDireita(no);
    }

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
    if (atual != null)
    {
      VisitarEmOrdem(atual.Esq, lista);
      lista.Add(atual.Info);
      VisitarEmOrdem(atual.Dir, lista); 
    }
  }

  private const double FATOR_REDUCAO_HORIZONTAL = 2.05;

  public void Desenhar(Graphics g, int largura, int altura)
  {
     if (Raiz == null) return;
     
     g.FillRectangle(Brushes.White, 0, 0, largura, altura);
     using (Font fonte = new Font("Arial", 9))
     {
       DesenharNo(g, Raiz, largura / 2, 30, largura / 4, 60, fonte);
     }
  }

  private void DesenharNo(Graphics g, NoArvore<Dado> no, int x, int y, int deslocamentoX, int deslocamentoY, Font fonte)
  {
    if (no == null) return;

    string texto = no.Info.ToString();
    SizeF tamanhoTexto = g.MeasureString(texto, fonte);
    int larguraNo = Math.Max((int)tamanhoTexto.Width + 10, 60);
    int alturaNo = Math.Max((int)tamanhoTexto.Height + 6, 24);

    // Desenha o retângulo do nó (retângulo arredondado é mais legível que elipse para texto)
    Rectangle rect = new Rectangle(x - larguraNo / 2, y - alturaNo / 2, larguraNo, alturaNo);

    // Destacar nó selecionado, se houver
    bool ehSelecionado = !EqualityComparer<Dado>.Default.Equals(Selecionado, default(Dado)) && EqualityComparer<Dado>.Default.Equals(no.Info, Selecionado);
    if (ehSelecionado)
    {
      using (Brush b = new SolidBrush(Color.LightGreen))
      using (Pen p = new Pen(Color.DarkGreen, 2))
      {
        g.FillRectangle(b, rect);
        g.DrawRectangle(p, rect);
      }
    }
    else
    {
      g.FillRectangle(Brushes.LightBlue, rect);
      g.DrawRectangle(Pens.Black, rect);
    }

    // Centraliza o texto no nó
    float textX = x - tamanhoTexto.Width / 2;
    float textY = y - tamanhoTexto.Height / 2;
    g.DrawString(texto, fonte, Brushes.Black, textX, textY);

    int yBase = y + alturaNo / 2;
    
    if (no.Esq != null)
    {
      int xFilhoEsq = x - deslocamentoX;
      int yFilho = y + deslocamentoY;
      g.DrawLine(Pens.Black, x, yBase, xFilhoEsq, yFilho - alturaNo / 2);
      DesenharNo(g, no.Esq, xFilhoEsq, yFilho, (int)(deslocamentoX / FATOR_REDUCAO_HORIZONTAL), deslocamentoY, fonte);
    }

    if (no.Dir != null)
    {
      int xFilhoDir = x + deslocamentoX;
      int yFilho = y + deslocamentoY;
      g.DrawLine(Pens.Black, x, yBase, xFilhoDir, yFilho - alturaNo / 2);
      DesenharNo(g, no.Dir, xFilhoDir, yFilho, (int)(deslocamentoX / FATOR_REDUCAO_HORIZONTAL), deslocamentoY, fonte);
    }
  }

  public bool Existe(Dado procurado)
  {
    antecessor = null;
    atual = raiz;
    while (atual != null)
    {
      if (procurado.CompareTo(atual.Info) == 0)
        return true;

      antecessor = atual;
      if (procurado.CompareTo(atual.Info) < 0)
        atual = atual.Esq;
      else
        atual = atual.Dir;
    }
    return false;
  }

  public bool IncluirNovoDado(Dado novoDado)
  {
    if (Existe(novoDado))
      return false;

    raiz = InserirAVL(raiz, novoDado);
    return true;
  }

  private NoArvore<Dado> InserirAVL(NoArvore<Dado> no, Dado dado)
  {
    if (no == null)
      return new NoArvore<Dado>(dado);

    if (dado.CompareTo(no.Info) < 0)
      no.Esq = InserirAVL(no.Esq, dado);
    else if (dado.CompareTo(no.Info) > 0)
      no.Dir = InserirAVL(no.Dir, dado);
    else
      return no;

    return Balancear(no, dado);
  }
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

    if (dado.CompareTo(no.Info) < 0)
      no.Esq = ExcluirAVL(no.Esq, dado);
    else if (dado.CompareTo(no.Info) > 0)
      no.Dir = ExcluirAVL(no.Dir, dado);
    else
    {
      if (no.Esq == null || no.Dir == null)
      {
        NoArvore<Dado> temp = no.Esq ?? no.Dir;
        if (temp == null)
        {
          no = null;
        }
        else
        {
          no = temp;
        }
      }
      else
      {
        NoArvore<Dado> temp = ObterMaior(no.Esq);
        no.Info = temp.Info;
        no.Esq = ExcluirAVL(no.Esq, temp.Info);
      }
    }

    if (no == null)
      return no;

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

    if (fator > 1 && ObterFatorBalanceamento(no.Esq) >= 0)
      return RotacaoDireita(no);

    if (fator > 1 && ObterFatorBalanceamento(no.Esq) < 0)
    {
      no.Esq = RotacaoEsquerda(no.Esq);
      return RotacaoDireita(no);
    }

    if (fator < -1 && ObterFatorBalanceamento(no.Dir) <= 0)
      return RotacaoEsquerda(no);

    if (fator < -1 && ObterFatorBalanceamento(no.Dir) > 0)
    {
      no.Dir = RotacaoDireita(no.Dir);
      return RotacaoEsquerda(no);
    }

    return no;
  }

  public void LerArquivoDeRegistros(string nomeArquivo)
  {
    raiz = null;
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
        dado = new Dado();
        dado.LerRegistro(arquivo, meio); 
        noAtual = new NoArvore<Dado>(dado);
        var novoEsq = noAtual.Esq;
        Particionar(inicio, meio - 1, ref novoEsq);
        noAtual.Esq = novoEsq;
        var novoDir = noAtual.Dir;
        Particionar(meio + 1, fim, ref novoDir);
        noAtual.Dir = novoDir;
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

  public Dado EncontrarNoPorClique(Graphics g, int largura, int altura, int clickX, int clickY)
  {
    if (Raiz == null) return default(Dado);
    using (Font fonte = new Font("Arial", 9))
    {
      return EncontrarNoPorClique(Raiz, g, largura / 2, 30, largura / 4, 60, fonte, clickX, clickY);
    }
  }

  private Dado EncontrarNoPorClique(NoArvore<Dado> no, Graphics g, int x, int y, int deslocamentoX, int deslocamentoY, Font fonte, int clickX, int clickY)
  {
    if (no == null) return default(Dado);

    string texto = no.Info.ToString();
    SizeF tamanhoTexto = g.MeasureString(texto, fonte);
    int larguraNo = Math.Max((int)tamanhoTexto.Width + 10, 60);
    int alturaNo = Math.Max((int)tamanhoTexto.Height + 6, 24);

    Rectangle rect = new Rectangle(x - larguraNo / 2, y - alturaNo / 2, larguraNo, alturaNo);

    if (rect.Contains(clickX, clickY))
      return no.Info;

    if (no.Esq != null)
    {
      int xFilhoEsq = x - deslocamentoX;
      int yFilho = y + deslocamentoY;
      Dado achou = EncontrarNoPorClique(no.Esq, g, xFilhoEsq, yFilho, (int)(deslocamentoX / FATOR_REDUCAO_HORIZONTAL), deslocamentoY, fonte, clickX, clickY);
      if (!EqualityComparer<Dado>.Default.Equals(achou, default(Dado)))
        return achou;
    }

    if (no.Dir != null)
    {
      int xFilhoDir = x + deslocamentoX;
      int yFilho = y + deslocamentoY;
      Dado achou = EncontrarNoPorClique(no.Dir, g, xFilhoDir, yFilho, (int)(deslocamentoX / FATOR_REDUCAO_HORIZONTAL), deslocamentoY, fonte, clickX, clickY);
      if (!EqualityComparer<Dado>.Default.Equals(achou, default(Dado)))
        return achou;
    }

    return default(Dado);
  }
}

