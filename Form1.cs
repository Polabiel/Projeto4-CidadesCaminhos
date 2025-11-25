using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Proj4
{
  public partial class Form1 : Form
  {
    Arvore<Cidade>  arvore = new Arvore<Cidade>();
    
    // Caminhos dos arquivos de dados
    private readonly string arquivoCidades = Path.Combine(Application.StartupPath, "Dados", "cidades.dat");
    private readonly string arquivoLigacoes = Path.Combine(Application.StartupPath, "Dados", "GrafoOnibusSaoPaulo.txt");
    
    public Form1()
    {
      InitializeComponent();
    }

    private void tpCadastro_Click(object sender, EventArgs e)
    {

    }

    private void label2_Click(object sender, EventArgs e)
    {

    }

    private void Form1_Load(object sender, EventArgs e)
    {
      CarregarDados();
    }
    
    /// <summary>
    /// Carrega os dados das cidades e ligações dos arquivos.
    /// </summary>
    private void CarregarDados()
    {
      try
      {
        // Lê o arquivo binário de cidades para a árvore AVL
        if (File.Exists(arquivoCidades))
        {
          arvore.LerArquivoDeRegistros(arquivoCidades);
        }
        
        // Lê o arquivo texto de ligações e adiciona às cidades
        if (File.Exists(arquivoLigacoes))
        {
          ArquivoHelper.LerArquivoLigacoes(arquivoLigacoes, arvore);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    
    /// <summary>
    /// Salva os dados das cidades e ligações nos arquivos.
    /// </summary>
    private void SalvarDados()
    {
      try
      {
        // Garante que o diretório Dados existe
        string diretorioDados = Path.GetDirectoryName(arquivoCidades);
        if (!Directory.Exists(diretorioDados))
        {
          Directory.CreateDirectory(diretorioDados);
        }
        
        // Grava o arquivo binário de cidades (árvore AVL em ordem)
        arvore.GravarArquivoDeRegistros(arquivoCidades);
        
        // Grava o arquivo texto de ligações
        ArquivoHelper.GravarArquivoLigacoes(arquivoLigacoes, arvore);
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erro ao salvar dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
      SalvarDados();
      base.OnFormClosing(e);
    }

    private void pnlArvore_Paint(object sender, PaintEventArgs e)
    {
      arvore.Desenhar(pnlArvore);
    }
  }
}
