# A11Y Analyzer

A11Y Analyzer é uma ferramenta projetada para analisar arquivos HTML em busca de problemas de acessibilidade. Ele verifica vários atributos de acessibilidade e fornece um relatório sobre as descobertas.

## Funcionalidades

- Verifica a ausência ou vazio de atributos `alt` em tags `img`.
- Verifica a presença de tags HTML semânticas.
- Identifica campos de entrada com atributos `type` e `autocomplete` ausentes ou vazios.
- Gera um relatório detalhado da análise.

## Requisitos

- .NET 8.0 ou superior
- JinianNet.JNTemplate

## Instalação

1. Clone o repositório:
    ```sh
    git clone https://github.com/seuusuario/a11y-analyzer.git
    ```
2. Navegue até o diretório do projeto:
    ```sh
    cd a11y-analyzer
    ```
3. Restaure as dependências:
    ```sh
    dotnet restore
    ```

## Uso

Para executar o A11Y Analyzer, use o seguinte comando:

```sh
dotnet run --project metricas.csproj "<caminho-para-pasta-html>"