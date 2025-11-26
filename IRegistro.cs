// ============================================================
// Projeto 4 - Cadastro de Cidades e Caminhos
// Disciplina: Estruturas de Dados - 2o DSNot 2025
// ------------------------------------------------------------
// Alunos:
//   Gabriel da Silva Nascimento - RA: 24.01266-2
//   Claudio Correa Gorza Filho - RA: 24.01214-0
// ============================================================

using System.IO;

public interface IRegistro
{
  void LerRegistro(BinaryReader arquivo, long qualRegistro);
  void GravarRegistro(BinaryWriter arquivo);
  int TamanhoRegistro { get; }
}
