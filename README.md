# Projeto 4 - Cadastro de Cidades e Caminhos

Aplicação Windows Forms para cadastro e manutenção de cidades e caminhos, desenvolvida em C# (.NET Framework 4.8).

## 📚 Disciplina

**Estruturas de Dados** - 2º DSNot 2025

## 👥 Autores

| Nome | RA |
|------|-----|
| Gabriel da Silva Nascimento | 24.01266-2 |
| Claudio Correa Gorza Filho | 24.01214-0 |

## 📋 Descrição do Projeto

Este projeto implementa um sistema de cadastro de cidades do estado de São Paulo com suas respectivas ligações (caminhos). O sistema utiliza:

- **Árvore AVL** para armazenamento balanceado das cidades
- **Lista Ligada** para armazenamento das ligações de cada cidade
- **Algoritmo de Dijkstra** para encontrar o menor caminho entre duas cidades

### Funcionalidades

- ✅ Inclusão, busca, alteração e exclusão de cidades
- ✅ Gerenciamento de ligações entre cidades (adicionar/remover)
- ✅ Visualização da árvore AVL em aba separada
- ✅ Mapa interativo do estado de São Paulo com visualização das cidades e ligações
- ✅ Busca do menor caminho entre duas cidades (Dijkstra)
- ✅ Persistência de dados em arquivos (binário para cidades, texto para ligações)

## 🗂️ Estrutura do Projeto

```
Projeto4-CidadesCaminhos/
├── Dados/                    # Arquivos de dados
│   ├── CidadesSaoPaulo.dat   # Arquivo binário com informações das cidades
│   └── GrafoOnibusSaoPaulo.txt  # Arquivo texto com as ligações entre cidades
├── Properties/               # Configurações do assembly
├── Resources/                # Recursos (imagem do mapa)
├── ArquivoHelper.cs          # Classe para leitura/gravação de arquivos
├── Arvore.cs                 # Implementação da Árvore AVL genérica
├── Cidade.cs                 # Classe que representa uma cidade
├── Dijkstra.cs               # Implementação do algoritmo de Dijkstra
├── FilaLista.cs              # Implementação de fila usando lista ligada
├── Form1.cs                  # Formulário principal (interface gráfica)
├── Form1.Designer.cs         # Designer do formulário
├── IQueue.cs                 # Interface para fila
├── IRegistro.cs              # Interface para registros serializáveis
├── Ligacao.cs                # Classe que representa uma ligação entre cidades
├── ListaSimples.cs           # Implementação de lista ligada simples
├── NoArvore.cs               # Nó da árvore AVL
├── NoLista.cs                # Nó da lista ligada
├── Program.cs                # Ponto de entrada da aplicação
├── Proj4.csproj              # Arquivo de projeto
└── Proj4.sln                 # Arquivo de solução
```

## 🛠️ Pré-requisitos

- **Windows 10/11**
- **.NET Framework 4.8** (geralmente já instalado no Windows)
- **Visual Studio 2019/2022** (para desenvolvimento/compilação)

## 🚀 Instruções de Execução

### Opção 1: Usando Visual Studio

1. Abra o arquivo `Proj4.sln` no Visual Studio
2. Pressione `F5` para compilar e executar em modo debug
3. Ou pressione `Ctrl+F5` para executar sem debug

### Opção 2: Executando o binário compilado

1. Navegue até a pasta `bin/Debug/` ou `bin/Release/`
2. Execute o arquivo `Proj4.exe`

### Opção 3: Compilação via linha de comando

```bash
# Usando MSBuild
msbuild Proj4.sln /p:Configuration=Release

# O executável será gerado em bin/Release/Proj4.exe
```

## 📖 Como Usar

### Aba "Cidades e Caminhos"

1. **Incluir Cidade**: Digite o nome da cidade, clique no mapa para definir as coordenadas e clique em "Incluir"
2. **Buscar Cidade**: Digite o nome e clique em "Buscar" para exibir os dados
3. **Alterar Cidade**: Após buscar, modifique as coordenadas e clique em "Alterar"
4. **Excluir Cidade**: Após buscar, clique em "Excluir" (somente se não houver ligações)

### Gerenciamento de Ligações

1. Busque uma cidade existente
2. Para adicionar uma ligação: digite a cidade de destino, a distância e clique em "+"
3. Para remover uma ligação: selecione na lista e clique em "-"

### Busca de Menor Caminho

1. Selecione a cidade de origem no primeiro combo
2. Selecione a cidade de destino no segundo combo
3. Clique em "Buscar caminhos"
4. O caminho será exibido na lista e destacado no mapa em verde

### Aba "Árvore Balanceada"

Visualiza a estrutura da árvore AVL com todas as cidades cadastradas.

## 📦 Instruções para Entrega

Para compactar o projeto conforme o padrão solicitado:

1. Feche o Visual Studio
2. Delete as pastas `bin/`, `obj/` e `.vs/` para reduzir o tamanho
3. Compacte a pasta do projeto em um arquivo ZIP
4. Renomeie para o padrão: `raMenor_raMaior_Proj4ED.zip`

Exemplo: `24012140_24012662_Proj4ED.zip`

## 📄 Licença

Este projeto foi desenvolvido para fins educacionais como parte da disciplina de Estruturas de Dados.
