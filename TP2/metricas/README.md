Verificador de Acessibilidade -readme sem vergonha só para os brothers 
Este projeto implementa uma ferramenta para verificar a acessibilidade de sites. Ele utiliza o HtmlAgilityPack para analisar o HTML de um site e o iText7 para gerar um relatório PDF detalhado contendo informações sobre as imagens, a estrutura semântica do site e a pontuação de acessibilidade com base nas diretrizes WCAG (Web Content Accessibility Guidelines).

Funcionalidades
Verificação dos atributos alt de todas as imagens.
Verificação da presença e uso correto de tags semânticas, como <header>, <footer>, <nav>, <main>, <article>, <section>, etc.
Cálculo de uma pontuação final de acessibilidade, normalizada entre 0 e 100.
Classificação da acessibilidade do site com base na pontuação:
0 a 25: Ruim
26 a 50: Regular
51 a 75: Bom
76 a 100: Ótimo
Geração de um relatório PDF detalhado com o nome do site incluído no nome do arquivo.
Como Funciona
O código utiliza HtmlAgilityPack para carregar e analisar a página HTML do site fornecido.
Ele verifica se todas as imagens possuem o atributo alt e se o site utiliza corretamente tags semânticas.
Penalizações são aplicadas quando imagens não possuem alt ou quando tags semânticas essenciais estão ausentes.
A pontuação final é calculada e normalizada para o intervalo de 0 a 100.
Um relatório em PDF é gerado, contendo a análise completa, a pontuação e a classificação final.
Requisitos
.NET SDK 8.0 ou superior
HtmlAgilityPack
iText7 para gerar PDFs
BouncyCastle (adaptador de criptografia usado pelo iText7)
Instalação
Clone o repositório:

bash
Copiar código
git clone https://github.com/seu-repositorio/verificador-acessibilidade.git
Instale as dependências do projeto:

bash
Copiar código
dotnet add package HtmlAgilityPack
dotnet add package itext7
dotnet add package itext7.bouncy-castle-adapter
Verifique se todas as dependências foram instaladas corretamente.

Como Usar
Execute o projeto no terminal fornecendo a URL do site que deseja analisar:

bash
Copiar código
dotnet run https://www.seu-site.com
O programa irá gerar um arquivo PDF com o nome Relatorio_Acessibilidade_<nome-do-site>.pdf, contendo o relatório completo de acessibilidade do site.

Lógica de Pontuação
A pontuação final do site é calculada com base nas seguintes verificações:

Verificação de Imagens (40% de peso):

Penalização para imagens que não possuem o atributo alt.
Pontuação adicional para imagens com alt adequado.
Verificação de Estrutura Semântica (30% de peso):

Penalização para ausência de tags semânticas como <nav>, <main>, <article>, etc.
Pontuação adicional para o uso correto de tags semânticas e hierarquia de cabeçalhos (<h1>, <h2>, etc.).
Verificação de Cabeçalhos (30% de peso):

Penalização para má utilização ou ausência de cabeçalhos.
Pontuação adicional para a hierarquia correta dos cabeçalhos.
Classificação da Pontuação
A pontuação final é normalizada para o intervalo de 0 a 100 e a classificação é feita da seguinte forma:

0 a 25: Ruim
26 a 50: Regular
51 a 75: Bom
76 a 100: Ótimo
Exemplo de Relatório Gerado
Total de imagens corretas
Total de imagens verificadas
Penalizações por ausência de alt em imagens
Tags semânticas encontradas e ausentes
Pontuação final e classificação (ruim, regular, bom ou ótimo)
Explicação da lógica de penalização e métrica de acessibilidade

