# Projeto de Web API para CRUD de Contatos e Identificação da Região pelo DDD

## Visão Geral

Este projeto foi desenvolvido para fornecer uma plataforma de Web API escalável e segura, seguindo boas práticas de arquitetura em camadas. Ele oferece as seguintes funcionalidades principais:

- **CRUD (Create, Read, Update, Delete) para entidades principais**: Permite realizar operações de CRUD para entidades como Contatos, Estados, Regiões, etc.
- **Autenticação JWT**: Implementa autenticação JWT para garantir a segurança das APIs restritas, protegendo o acesso não autorizado.
- **Validação de entrada de dados utilizando Fluent Validator**: Utiliza Fluent Validator para validar os dados de entrada e garantir sua integridade antes de serem processados.
- **Conexão com um banco de dados SQL Server**: Estabelece conexão com um banco de dados SQL Server para persistência de dados, garantindo a confiabilidade e durabilidade das informações armazenadas.

## Funcionalidades Adicionais

Além das funcionalidades principais, este projeto oferece uma funcionalidade adicional:

- **Identificação da Região pelo DDD**: A partir do DDD do telefone de um contato, é possível identificar a região correspondente do cliente, auxiliando na segmentação geográfica.
- **Pesquisar pelo DDD**: É possivel buscar uma lista de contatos, a partir do DDD do telefone cadsatrado.

## Tecnologias Utilizadas

- **.NET 8**: Plataforma de desenvolvimento rápida, moderna e multiplataforma para construção de aplicativos.
- **Entity Framework Core**: ORM (Object-Relational Mapping) para mapear objetos .NET para um banco de dados relacional.
- **Fluent Validation**: Biblioteca para validação de modelos .NET de forma elegante e extensível.
- **JWT (JSON Web Tokens)**: Mecanismo de autenticação JSON compacto e seguro para autenticação de APIs.
- **SQL Server**: Sistema de gerenciamento de banco de dados relacional usado para armazenar dados.

## Estrutura do Projeto

O projeto está estruturado em camadas para separação de responsabilidades e facilitar a manutenção e extensibilidade:

- **API Controllers**: Controladores que definem as rotas da API e manipulam as solicitações HTTP.
- **Serviços**: Implementações dos serviços de negócios que contêm a lógica de aplicação.
- **Repositórios**: Implementações dos repositórios que lidam com operações de banco de dados.
- **DTOs (Data Transfer Objects)**: Objetos para transferência de dados entre a camada de serviço e a camada de API.
- **Entidades**: Definições das entidades do domínio que representam os dados do aplicativo.
- **Middleware**: Middleware personalizados, como autenticação JWT, manipulação de erros, etc.
- **Configurações**: Arquivos de configuração, como configuração de banco de dados, autenticação, etc.

## Configuração e Uso

1. **Clonando o Repositório**:
   ```shell
   git clone https://github.com/netiim/TechChallenge.git
   ```

2. **Instalando Dependências**:
   ```shell
   dotnet restore
   ```

3. **Configurando o Banco de Dados**:
   - Altere a string de conexão do banco de dados no arquivo `appsettings.json`.

4. **Executando o Projeto**:
   ```shell
   dotnet run
   ```

5. **Testando as APIs**:
   - Use uma ferramenta como Postman ou Swagger para testar as APIs fornecidas.

## Contribuição

Contribuições são bem-vindas! Se você deseja melhorar este projeto, sinta-se à vontade para enviar pull requests ou abrir problemas para discutir novos recursos ou correções.

## Licença

Este projeto está licenciado sob a [MIT License](LICENSE).
