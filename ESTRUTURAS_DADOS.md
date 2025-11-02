# Estruturas de Dados - Documentação

Este documento descreve as estruturas de dados implementadas no projeto Cidades e Caminhos.

## 1. Lista Simplesmente Encadeada (ListaSimples)

### Descrição
Implementação de uma lista simplesmente encadeada ordenada que armazena elementos de forma sequencial.

### Classe: `NoLista<Dado>`
- **Propriedades:**
  - `Info`: Dado armazenado no nó
  - `Prox`: Referência para o próximo nó

### Classe: `ListaSimples<Dado>`
- **Operações Principais:**
  - `InserirEmOrdem(dado)`: Insere elemento mantendo a ordem
  - `RemoverDado(dado)`: Remove elemento específico
  - `ExisteDado(dado)`: Verifica se elemento existe
  - `IniciarPercursoSequencial()`: Inicia iteração
  - `PodePercorrer()`: Avança para próximo elemento
  - `Listar()`: Retorna todos elementos como List<>
  
- **Complexidade:**
  - Inserção: O(n) - precisa encontrar posição correta
  - Remoção: O(n) - precisa buscar o elemento
  - Busca: O(n) - percorre até encontrar
  
## 2. Árvore AVL (Arvore)

### Descrição
Implementação de uma árvore binária de busca auto-balanceada (AVL) que garante altura logarítmica.

### Classe: `NoArvore<Dado>`
- **Propriedades:**
  - `Info`: Dado armazenado no nó
  - `Esq`: Referência para filho esquerdo
  - `Dir`: Referência para filho direito
  - `Altura`: Altura do nó na árvore

### Classe: `Arvore<Dado>`
- **Operações Principais:**
  - `IncluirNovoDado(dado)`: Insere com balanceamento AVL
  - `Excluir(dado)`: Remove com rebalanceamento
  - `Existe(dado)`: Busca elemento na árvore
  - `VisitarEmOrdem(lista)`: Percurso em ordem (produz lista ordenada)
  - `Desenhar(tela)`: Desenha árvore graficamente

- **Métodos de Balanceamento:**
  - `RotacaoDireita(no)`: Rotação simples à direita
  - `RotacaoEsquerda(no)`: Rotação simples à esquerda
  - `Balancear(no)`: Aplica rotações conforme fator de balanceamento
  - `FatorBalanceamento(no)`: Calcula diferença de altura entre subárvores
  
- **Complexidade:**
  - Inserção: O(log n) - árvore balanceada
  - Remoção: O(log n) - árvore balanceada
  - Busca: O(log n) - árvore balanceada
  
- **Balanceamento AVL:**
  - Fator de balanceamento = Altura(esq) - Altura(dir)
  - Mantém |fator| ≤ 1 através de rotações
  - 4 casos de rotação:
    1. Esquerda-Esquerda: Rotação simples à direita
    2. Direita-Direita: Rotação simples à esquerda
    3. Esquerda-Direita: Rotação dupla (esq → dir)
    4. Direita-Esquerda: Rotação dupla (dir → esq)

## 3. Fila Encadeada (FilaLista)

### Descrição
Implementação de fila (FIFO - First In, First Out) baseada em `ListaSimples`.

### Interface: `IQueue<Tipo>`
Define operações padrão de fila.

### Classe: `FilaLista<Tipo>`
- **Operações Principais:**
  - `Enfileirar(dado)`: Adiciona no fim da fila
  - `Retirar()`: Remove do início da fila
  - `OInicio()`: Consulta primeiro elemento sem remover
  - `OFim()`: Consulta último elemento sem remover
  - `EstaVazia`: Verifica se fila está vazia
  - `Tamanho`: Retorna número de elementos
  
- **Complexidade:**
  - Enfileirar: O(1) - insere no fim
  - Retirar: O(1) - remove do início
  - Consulta: O(1)

## 4. Interface IRegistro

### Descrição
Interface base para objetos que podem ser persistidos em arquivo binário.

### Métodos:
- `LerRegistro(arquivo, qualRegistro)`: Lê registro de posição específica
- `GravarRegistro(arquivo)`: Grava registro no arquivo
- `TamanhoRegistro`: Retorna tamanho fixo do registro em bytes

## Testes

### Como executar os testes:

1. **Via Windows Forms:** Os testes são executados automaticamente ao carregar a aplicação (Form1_Load)

2. **Via Console:** Execute o programa com argumento "test":
   ```
   Proj4.exe test
   ```

### Testes Implementados:

- **ListaSimples**: Inserção ordenada, busca, remoção, percurso
- **Árvore AVL**: Inserção com rotações, busca, remoção com rebalanceamento, percurso em ordem
- **FilaLista**: Enfileirar, desenfileirar, consultas, exceções

Os testes validam:
- ✓ Operações básicas funcionam corretamente
- ✓ Estruturas mantêm invariantes (ordem, balanceamento, FIFO)
- ✓ Exceções são lançadas adequadamente
- ✓ Integração entre as estruturas

## Utilização no Projeto

- **Cidade**: Armazenada em `Arvore<Cidade>` para acesso rápido ordenado
- **Ligacao**: Armazenada em `ListaSimples<Ligacao>` dentro de cada cidade
- **FilaLista**: Usada em algoritmos de busca (BFS) para percorrer grafos

## Autores
Projeto desenvolvido para a disciplina de Estrutura de Dados.
