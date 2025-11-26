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
