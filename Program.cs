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
    static void Main(string[] args)
    {
      // Se passado o argumento "test", executa os testes em modo console
      if (args.Length > 0 && args[0].ToLower() == "test")
      {
        Console.WriteLine("Executando testes das estruturas de dados...\n");
        TestDataStructures.ExecutarTodosTestes();
        Console.WriteLine("\nPressione qualquer tecla para sair...");
        Console.ReadKey();
        return;
      }

      // Caso contrário, executa a aplicação Windows Forms normalmente
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Form1());
    }
  }
}
