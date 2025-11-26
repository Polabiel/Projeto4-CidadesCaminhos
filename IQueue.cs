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

public interface IQueue<Tipo>
{
  void Enfileirar(Tipo novoDado);
  Tipo Retirar();
  Tipo OInicio();
  Tipo OFim();
  bool EstaVazia { get; }
  int Tamanho    { get; }
  List<Tipo> Listar();
}
