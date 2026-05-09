# DONET // SYSTEM

> **Ecossistema de Gestão e Análise de Fluxos Acadêmicos**

O projeto **Donet** é uma plataforma de Atendimento Educacional Especializado (AEE), desenvolvida com foco em escalabilidade, organização arquitetural e ergonomia visual (Dark UI). O sistema integra processos administrativos e pedagógicos para otimizar o monitoramento de dados institucionais.

## 🛠️ Tecnologias Utilizadas

*   **Framework:** ASP.NET Core 8.0 (MVC)
*   **Linguagem:** C# / TypeScript
*   **Persistência de Dados:** Entity Framework Core
*   **Banco de Dados:** MySQL / MariaDB
*   **Estilização:** Bootstrap 5 & Custom Dark High-Contrast CSS
*   **Ambiente de Desenvolvimento:** Linux Mint / VS Code

## 🏗️ Arquitetura

A aplicação fundamenta-se no padrão **Repository Pattern**, garantindo o desacoplamento entre a camada de acesso a dados (DAL) e as regras de negócio. 
- **Injeção de Dependência:** Utilizada para gerenciamento de serviços e repositórios.
- **Autenticação:** Sistema baseado em `Cookie Authentication` para controle de níveis de acesso (ADM/Professor).

## 🌑 Interface (Dark Batman Style)

O design foi projetado sob a estética de alto contraste, visando:
- Redução da fadiga ocular em monitoramento prolongado.
- Eficiência energética em displays modernos.
- Interface minimalista com efeitos de *Glow Text* e *Bento Grid*.

## 🚀 Como Executar o Projeto

1.  **Clonar o repositório:**
    ```bash
    git clone [https://github.com/seu-usuario/donet-system.git](https://github.com/seu-usuario/donet-system.git)

1. Configurar o Banco de Dados:
Ajuste a DefaultConnection no arquivo appsettings.json com suas credenciais do MySQL.


3. Executar Migrations:
dotnet ef database update

4. Rodar a aplicação:
dotnet run


## ⚙️ Configuração Local

Por questões de segurança, os arquivos de configuração que contêm credenciais de banco de dados e senhas de administrador não são enviados ao repositório.

**Para rodar o projeto, siga estes passos:**

1. Na raiz do projeto, crie um arquivo chamado `appsettings.json`.
2. Cole o seguinte conteúdo, ajustando com suas credenciais do MySQL:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AcademicoDB;Uid=seu_usuario;Pwd=sua_senha;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}


---
**Desenvolvido por:** Natanael F. Rodrigues  
**Instituição:** IFMT - Campus Campo Verde  
**Ano:** 2026