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
using System.Text;
using System.Threading.Tasks;

namespace AgendaAlfabetica
{
    public class NoLista<Dado> 
        where Dado :   IComparable<Dado>
    {
        private Dado info;
        private NoLista<Dado> prox;

        public NoLista(Dado informacao, NoLista<Dado> ponteiro)
        {
            info = informacao;
            prox = ponteiro;
        }

        public Dado Info
        {
            get
            {
                return info;
            }

            set
            {
                info = value;
            }
        }

        public NoLista<Dado> Prox
        {
            get
            {
                return prox;
            }

            set
            {
                prox = value;
            }
        }
    }
}
