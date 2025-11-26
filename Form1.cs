using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Proj4
{
  public partial class Form1 : Form
  {
    Arvore<Cidade>  arvore = new Arvore<Cidade>();
    
    // Caminhos dos arquivos de dados
    private readonly string arquivoCidades = Path.Combine(Application.StartupPath, "Dados", "cidades.dat");
    private readonly string arquivoLigacoes = Path.Combine(Application.StartupPath, "Dados", "GrafoOnibusSaoPaulo.txt");
    
    // Cidade atualmente selecionada/carregada no formulário
    private Cidade cidadeAtual = null;
    
    // Resultado da busca do menor caminho (Dijkstra)
    private ResultadoDijkstra caminhoAtual = null;
    
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
      AtualizarComboBoxDestino();
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
      arvore.Desenhar(e.Graphics, pnlArvore.Width, pnlArvore.Height);
    }
    
    #region Manutenção de Cidades
    
    /// <summary>
    /// Evento Leave do txtNomeCidade - valida se a cidade existe.
    /// </summary>
    private void txtNomeCidade_Leave(object sender, EventArgs e)
    {
      string nomeCidade = txtNomeCidade.Text.Trim();
      if (string.IsNullOrEmpty(nomeCidade))
      {
        cidadeAtual = null;
        LimparCamposCidade();
        return;
      }
      
      Cidade cidadeBusca = new Cidade(nomeCidade);
      if (arvore.Existe(cidadeBusca))
      {
        cidadeAtual = arvore.Atual.Info;
        ExibirCidade(cidadeAtual);
      }
      else
      {
        cidadeAtual = null;
        LimparCamposCidade();
      }
    }
    
    /// <summary>
    /// Evento Click do pbMapa usando MouseClick para coordenadas.
    /// </summary>
    private void pbMapa_MouseClick(object sender, MouseEventArgs e)
    {
      // Verifica se as dimensões do PictureBox são válidas
      if (pbMapa.Width <= 0 || pbMapa.Height <= 0)
      {
        MessageBox.Show("O mapa não está disponível ou não foi inicializado corretamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      
      // Calcula coordenadas proporcionais (0 a 1)
      double xProporcional = (double)e.X / pbMapa.Width;
      double yProporcional = (double)e.Y / pbMapa.Height;
      
      // Atualiza os campos de coordenadas
      udX.Value = (decimal)xProporcional;
      udY.Value = (decimal)yProporcional;
    }
    
    /// <summary>
    /// Inclui uma nova cidade na árvore.
    /// </summary>
    private void btnIncluirCidade_Click(object sender, EventArgs e)
    {
      string nomeCidade = txtNomeCidade.Text.Trim();
      
      if (string.IsNullOrEmpty(nomeCidade))
      {
        MessageBox.Show("Informe o nome da cidade.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtNomeCidade.Focus();
        return;
      }
      
      double x = (double)udX.Value;
      double y = (double)udY.Value;
      
      Cidade novaCidade = new Cidade(nomeCidade, x, y);
      
      if (arvore.IncluirNovoDado(novaCidade))
      {
        cidadeAtual = novaCidade;
        MessageBox.Show("Cidade incluída com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        AtualizarComboBoxDestino();
        AtualizarDataGridLigacoes();
        pnlArvore.Invalidate();
        pbMapa.Invalidate();
      }
      else
      {
        MessageBox.Show("Cidade já existe na árvore.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }
    
    /// <summary>
    /// Busca uma cidade na árvore pelo nome.
    /// </summary>
    private void btnBuscarCidade_Click(object sender, EventArgs e)
    {
      string nomeCidade = txtNomeCidade.Text.Trim();
      
      if (string.IsNullOrEmpty(nomeCidade))
      {
        MessageBox.Show("Informe o nome da cidade para buscar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtNomeCidade.Focus();
        return;
      }
      
      Cidade cidadeBusca = new Cidade(nomeCidade);
      
      if (arvore.Existe(cidadeBusca))
      {
        cidadeAtual = arvore.Atual.Info;
        ExibirCidade(cidadeAtual);
        MessageBox.Show("Cidade encontrada!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      else
      {
        cidadeAtual = null;
        MessageBox.Show("Cidade não encontrada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }
    
    /// <summary>
    /// Altera os dados de uma cidade existente.
    /// </summary>
    private void btnAlterarCidade_Click(object sender, EventArgs e)
    {
      if (cidadeAtual == null)
      {
        MessageBox.Show("Busque uma cidade primeiro para alterar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }
      
      // Atualiza as coordenadas da cidade atual
      cidadeAtual.X = (double)udX.Value;
      cidadeAtual.Y = (double)udY.Value;
      
      pnlArvore.Invalidate();
      pbMapa.Invalidate();
      MessageBox.Show("Cidade alterada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    
    /// <summary>
    /// Exclui uma cidade da árvore. Só permite exclusão se não houver ligações.
    /// </summary>
    private void btnExcluirCidade_Click(object sender, EventArgs e)
    {
      if (cidadeAtual == null)
      {
        MessageBox.Show("Busque uma cidade primeiro para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }
      
      // Verifica se a cidade possui ligações
      if (cidadeAtual.Ligacoes.QuantosNos > 0)
      {
        MessageBox.Show("Não é possível excluir uma cidade que possui ligações. Remova as ligações primeiro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }
      
      // Verifica se outras cidades possuem ligações para esta cidade
      if (ExistemLigacoesParaCidade(cidadeAtual.Nome.Trim()))
      {
        MessageBox.Show("Não é possível excluir esta cidade pois outras cidades possuem ligações para ela. Remova as ligações primeiro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }
      
      DialogResult resultado = MessageBox.Show($"Deseja realmente excluir a cidade '{cidadeAtual.Nome.Trim()}'?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      
      if (resultado == DialogResult.Yes)
      {
        if (arvore.Excluir(cidadeAtual))
        {
          MessageBox.Show("Cidade excluída com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
          cidadeAtual = null;
          LimparCamposCidade();
          AtualizarComboBoxDestino();
          pnlArvore.Invalidate();
          pbMapa.Invalidate();
        }
        else
        {
          MessageBox.Show("Erro ao excluir a cidade.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }
    
    /// <summary>
    /// Verifica se existem ligações de outras cidades para a cidade especificada.
    /// </summary>
    private bool ExistemLigacoesParaCidade(string nomeCidade)
    {
      List<Cidade> cidades = new List<Cidade>();
      arvore.VisitarEmOrdem(cidades);
      
      return cidades.Where(c => c.ExisteLigacao(nomeCidade)).Any();
    }
    
    /// <summary>
    /// Exibe os dados de uma cidade nos campos do formulário.
    /// </summary>
    private void ExibirCidade(Cidade cidade)
    {
      txtNomeCidade.Text = cidade.Nome.Trim();
      udX.Value = (decimal)cidade.X;
      udY.Value = (decimal)cidade.Y;
      AtualizarDataGridLigacoes();
    }
    
    /// <summary>
    /// Limpa os campos de cidade no formulário.
    /// </summary>
    private void LimparCamposCidade()
    {
      txtNomeCidade.Text = "";
      udX.Value = 0;
      udY.Value = 0;
      dgvLigacoes.Rows.Clear();
    }
    
    /// <summary>
    /// Atualiza o DataGridView de ligações com as ligações da cidade atual.
    /// </summary>
    private void AtualizarDataGridLigacoes()
    {
      dgvLigacoes.Rows.Clear();
      
      if (cidadeAtual == null)
        return;
      
      var ligacoes = cidadeAtual.Ligacoes.Listar();
      foreach (var ligacao in ligacoes)
      {
        dgvLigacoes.Rows.Add(ligacao.CidadeDestino, ligacao.Distancia);
      }
    }
    
    /// <summary>
    /// Atualiza o ComboBox de destino e origem com todas as cidades da árvore.
    /// </summary>
    private void AtualizarComboBoxDestino()
    {
      cbxCidadeDestino.Items.Clear();
      cbxCidadeOrigem.Items.Clear();
      
      List<Cidade> cidades = new List<Cidade>();
      arvore.VisitarEmOrdem(cidades);
      
      foreach (Cidade cidade in cidades)
      {
        string nomeCidade = cidade.Nome.Trim();
        cbxCidadeDestino.Items.Add(nomeCidade);
        cbxCidadeOrigem.Items.Add(nomeCidade);
      }
    }
    
    #endregion
    
    #region Manutenção de Ligações
    
    /// <summary>
    /// Adiciona uma nova ligação à cidade atual.
    /// </summary>
    private void btnIncluirCaminho_Click(object sender, EventArgs e)
    {
      if (cidadeAtual == null)
      {
        MessageBox.Show("Busque ou inclua uma cidade primeiro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }
      
      string cidadeDestino = txtNovoDestino.Text.Trim();
      int distancia = (int)nudDistancia.Value;
      
      if (string.IsNullOrEmpty(cidadeDestino))
      {
        MessageBox.Show("Informe a cidade de destino.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtNovoDestino.Focus();
        return;
      }
      
      if (distancia <= 0)
      {
        MessageBox.Show("A distância deve ser maior que zero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        nudDistancia.Focus();
        return;
      }
      
      // Verifica se a cidade de destino existe
      Cidade cidadeDestinoBusca = new Cidade(cidadeDestino);
      if (!arvore.Existe(cidadeDestinoBusca))
      {
        MessageBox.Show("A cidade de destino não existe.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }
      
      // Verifica se não está tentando criar ligação para a mesma cidade
      if (cidadeDestino.Equals(cidadeAtual.Nome.Trim()))
      {
        MessageBox.Show("Não é possível criar uma ligação de uma cidade para ela mesma.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }
      
      if (cidadeAtual.AdicionarLigacao(arvore.Atual.Info.Nome.Trim(), distancia))
      {
        MessageBox.Show("Ligação adicionada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        AtualizarDataGridLigacoes();
        txtNovoDestino.Text = "";
        nudDistancia.Value = 0;
        pnlArvore.Invalidate();
        pbMapa.Invalidate();
      }
      else
      {
        MessageBox.Show("Esta ligação já existe.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }
    
    /// <summary>
    /// Remove a ligação selecionada da cidade atual.
    /// </summary>
    private void btnExcluirCaminho_Click(object sender, EventArgs e)
    {
      if (cidadeAtual == null)
      {
        MessageBox.Show("Busque ou inclua uma cidade primeiro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }
      
      if (dgvLigacoes.SelectedRows.Count == 0)
      {
        MessageBox.Show("Selecione uma ligação para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }
      
      string cidadeDestino = dgvLigacoes.SelectedRows[0].Cells[0].Value?.ToString();
      
      if (string.IsNullOrEmpty(cidadeDestino))
      {
        MessageBox.Show("Selecione uma ligação válida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }
      
      DialogResult resultado = MessageBox.Show($"Deseja realmente excluir a ligação para '{cidadeDestino}'?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      
      if (resultado == DialogResult.Yes)
      {
        if (cidadeAtual.RemoverLigacao(cidadeDestino))
        {
          MessageBox.Show("Ligação excluída com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
          AtualizarDataGridLigacoes();
          pnlArvore.Invalidate();
          pbMapa.Invalidate();
        }
        else
        {
          MessageBox.Show("Erro ao excluir a ligação.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }
    
    #endregion
    
    #region Busca de Menor Caminho (Dijkstra)
    
    /// <summary>
    /// Busca o menor caminho entre a cidade de origem e a cidade de destino.
    /// </summary>
    private void btnBuscarCaminho_Click(object sender, EventArgs e)
    {
      // Limpa resultados anteriores
      dgvRotas.Rows.Clear();
      lbDistanciaTotal.Text = "Distância total: ";
      caminhoAtual = null;
      
      // Valida seleção das cidades
      if (cbxCidadeOrigem.SelectedItem == null || string.IsNullOrEmpty(cbxCidadeOrigem.SelectedItem.ToString()))
      {
        MessageBox.Show("Selecione a cidade de origem.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        cbxCidadeOrigem.Focus();
        return;
      }
      
      if (cbxCidadeDestino.SelectedItem == null || string.IsNullOrEmpty(cbxCidadeDestino.SelectedItem.ToString()))
      {
        MessageBox.Show("Selecione a cidade de destino.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        cbxCidadeDestino.Focus();
        return;
      }
      
      string nomeOrigem = cbxCidadeOrigem.SelectedItem.ToString();
      string nomeDestino = cbxCidadeDestino.SelectedItem.ToString();
      
      // Verifica se origem e destino são iguais
      if (nomeOrigem == nomeDestino)
      {
        MessageBox.Show("A cidade de origem e destino devem ser diferentes.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }
      
      // Executa o algoritmo de Dijkstra
      caminhoAtual = Dijkstra.BuscarMenorCaminho(arvore, nomeOrigem, nomeDestino);
      
      if (!caminhoAtual.CaminhoEncontrado)
      {
        MessageBox.Show($"Não existe caminho entre {nomeOrigem} e {nomeDestino}.", "Caminho Inexistente", MessageBoxButtons.OK, MessageBoxIcon.Information);
        pbMapa.Invalidate();
        return;
      }
      
      // Exibe o caminho encontrado no DataGridView
      ExibirCaminhoNoGrid(caminhoAtual);
      
      // Atualiza a distância total
      lbDistanciaTotal.Text = $"Distância total: {caminhoAtual.DistanciaTotal} km";
      
      // Redesenha o mapa para destacar o caminho
      pbMapa.Invalidate();
      
      MessageBox.Show($"Caminho encontrado com {caminhoAtual.Caminho.Count} cidades e distância total de {caminhoAtual.DistanciaTotal} km.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    
    /// <summary>
    /// Exibe o caminho no DataGridView dgvRotas.
    /// </summary>
    private void ExibirCaminhoNoGrid(ResultadoDijkstra resultado)
    {
      dgvRotas.Rows.Clear();
      
      if (!resultado.CaminhoEncontrado)
        return;
      
      // Obtém todas as cidades para calcular distâncias parciais
      Dictionary<string, Cidade> cidadesPorNome = new Dictionary<string, Cidade>();
      List<Cidade> todasCidades = new List<Cidade>();
      arvore.VisitarEmOrdem(todasCidades);
      foreach (Cidade c in todasCidades)
      {
        cidadesPorNome[c.Nome.Trim()] = c;
      }
      
      // Adiciona cada cidade do caminho com a distância acumulada
      int distanciaAcumulada = 0;
      
      for (int i = 0; i < resultado.Caminho.Count; i++)
      {
        string nomeCidade = resultado.Caminho[i];
        
        // Calcula a distância do trecho anterior (se não for a primeira cidade)
        if (i > 0)
        {
          string cidadeAnterior = resultado.Caminho[i - 1];
          if (cidadesPorNome.TryGetValue(cidadeAnterior, out Cidade cidadeOrigem))
          {
            var ligacoes = cidadeOrigem.Ligacoes.Listar();
            foreach (var ligacao in ligacoes)
            {
              if (ligacao.CidadeDestino.Trim() == nomeCidade)
              {
                distanciaAcumulada += ligacao.Distancia;
                break;
              }
            }
          }
        }
        
        dgvRotas.Rows.Add(nomeCidade, distanciaAcumulada);
      }
    }
    
    #endregion
    
    #region Desenho do Mapa
    
    // Recursos de desenho reutilizáveis para o mapa
    private readonly Pen penLigacao = new Pen(Color.Blue, 1);
    private readonly Pen penCaminho = new Pen(Color.Green, 3);
    private readonly SolidBrush brushCidade = new SolidBrush(Color.Red);
    private readonly SolidBrush brushCidadeCaminho = new SolidBrush(Color.Green);
    private readonly SolidBrush brushTexto = new SolidBrush(Color.Black);
    private readonly Font fontNome = new Font("Arial", 7, FontStyle.Regular);
    private const int raioCidade = 5;
    private const int raioCidadeCaminho = 7;
    
    /// <summary>
    /// Evento Paint do PictureBox do mapa - desenha cidades e ligações.
    /// </summary>
    private void pbMapa_Paint(object sender, PaintEventArgs e)
    {
      DesenharMapa(e.Graphics);
    }
    
    /// <summary>
    /// Desenha o mapa com todas as cidades e suas ligações.
    /// Destaca o caminho encontrado pelo algoritmo de Dijkstra em verde.
    /// </summary>
    private void DesenharMapa(Graphics g)
    {
      if (pbMapa.Width <= 0 || pbMapa.Height <= 0)
        return;
      
      // Obtém todas as cidades da árvore
      List<Cidade> cidades = new List<Cidade>();
      arvore.VisitarEmOrdem(cidades);
      
      if (cidades.Count == 0)
        return;
      
      // Cria um dicionário para acesso rápido às cidades por nome (normalizado)
      Dictionary<string, Cidade> cidadesPorNome = new Dictionary<string, Cidade>();
      foreach (Cidade cidade in cidades)
      {
        string nomeNormalizado = cidade.Nome.Trim();
        cidadesPorNome[nomeNormalizado] = cidade;
      }
      
      // Cria um conjunto com as cidades que fazem parte do caminho
      HashSet<string> cidadesNoCaminho = new HashSet<string>();
      if (caminhoAtual != null && caminhoAtual.CaminhoEncontrado)
      {
        foreach (string nome in caminhoAtual.Caminho)
        {
          cidadesNoCaminho.Add(nome);
        }
      }
      
      // Primeiro, desenha todas as ligações normais (linhas azuis)
      foreach (Cidade cidade in cidades)
      {
        int x1 = (int)(cidade.X * pbMapa.Width);
        int y1 = (int)(cidade.Y * pbMapa.Height);
        
        var ligacoes = cidade.Ligacoes.Listar();
        foreach (var ligacao in ligacoes)
        {
          string nomeDestino = ligacao.CidadeDestino.Trim();
          if (cidadesPorNome.TryGetValue(nomeDestino, out Cidade cidadeDestino))
          {
            int x2 = (int)(cidadeDestino.X * pbMapa.Width);
            int y2 = (int)(cidadeDestino.Y * pbMapa.Height);
            
            g.DrawLine(penLigacao, x1, y1, x2, y2);
          }
        }
      }
      
      // Desenha o caminho destacado (linhas verdes mais grossas)
      if (caminhoAtual != null && caminhoAtual.CaminhoEncontrado)
      {
        for (int i = 0; i < caminhoAtual.Caminho.Count - 1; i++)
        {
          string nomeOrigem = caminhoAtual.Caminho[i];
          string nomeDestino = caminhoAtual.Caminho[i + 1];
          
          if (cidadesPorNome.TryGetValue(nomeOrigem, out Cidade cidadeOrigem) &&
              cidadesPorNome.TryGetValue(nomeDestino, out Cidade cidadeDestinoObj))
          {
            int x1 = (int)(cidadeOrigem.X * pbMapa.Width);
            int y1 = (int)(cidadeOrigem.Y * pbMapa.Height);
            int x2 = (int)(cidadeDestinoObj.X * pbMapa.Width);
            int y2 = (int)(cidadeDestinoObj.Y * pbMapa.Height);
            
            g.DrawLine(penCaminho, x1, y1, x2, y2);
          }
        }
      }
      
      // Depois, desenha todas as cidades (círculos) e nomes
      foreach (Cidade cidade in cidades)
      {
        string nomeCidade = cidade.Nome.Trim();
        int x = (int)(cidade.X * pbMapa.Width);
        int y = (int)(cidade.Y * pbMapa.Height);
        
        // Verifica se a cidade faz parte do caminho
        bool fazParteDoCaminho = cidadesNoCaminho.Contains(nomeCidade);
        
        if (fazParteDoCaminho)
        {
          // Desenha o círculo da cidade em verde (maior) para cidades no caminho
          g.FillEllipse(brushCidadeCaminho, x - raioCidadeCaminho, y - raioCidadeCaminho, raioCidadeCaminho * 2, raioCidadeCaminho * 2);
        }
        else
        {
          // Desenha o círculo da cidade em vermelho
          g.FillEllipse(brushCidade, x - raioCidade, y - raioCidade, raioCidade * 2, raioCidade * 2);
        }
        
        // Desenha o nome da cidade próximo ao círculo
        g.DrawString(nomeCidade, fontNome, brushTexto, x + raioCidade + 2, y - raioCidade);
      }
    }
    
    #endregion
  }
}
