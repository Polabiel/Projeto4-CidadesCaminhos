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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proj4
{
  internal static class Program
  {
    /// <summary>
    /// Ponto de entrada principal para o aplicativo.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Form1());
    }
  }
}
