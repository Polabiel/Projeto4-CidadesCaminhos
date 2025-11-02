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
      // Executar testes das estruturas de dados ao carregar a aplicação
      // Os resultados serão exibidos na janela de saída do console/debug
      try
      {
        TestDataStructures.ExecutarTodosTestes();
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine("Erro ao executar testes: " + ex.Message);
      }
    }

    private void pnlArvore_Paint(object sender, PaintEventArgs e)
    {
      arvore.Desenhar(pnlArvore);
    }
  }
}
