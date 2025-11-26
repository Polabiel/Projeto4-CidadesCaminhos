// ============================================================
// Projeto 4 - Cadastro de Cidades e Caminhos
// Disciplina: Estruturas de Dados - 2o DSNot 2025
// ------------------------------------------------------------
// Alunos:
//   Andrew Douglas Nithack - RA: 23305
//   Gabriel Oliveira dos Santos - RA: 23600
// ============================================================

using System.IO;

public interface IRegistro
{
  void LerRegistro(BinaryReader arquivo, long qualRegistro);
  void GravarRegistro(BinaryWriter arquivo);
  int TamanhoRegistro { get; }
}
