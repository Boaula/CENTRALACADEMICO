# CentralAcademico

CentralAcademico é um sistema web ASP.NET Core MVC para gestão acadêmica (alunos, professores e disciplinas), com suporte a autenticação por cookies e administração por role `Admin`.

**Este README foca em como configurar o banco de dados e executar as migrations.**

**Requisitos**
- .NET SDK 8.0+ instalado
- `dotnet-ef` (ferramenta CLI do EF Core) — opcionalmente instalada globalmente

**Instalar a ferramenta EF (opcional)**
Use este comando se ainda não tiver o `dotnet-ef` disponível:

```
dotnet tool install --global dotnet-ef
```

Ou rode localmente no projeto:

```
dotnet add package Microsoft.EntityFrameworkCore.Design
```

**Configurar a conexão (appsettings.json)**
Edite o arquivo [appsettings.json](appsettings.json) na raiz do projeto e adicione a sua connection string em `ConnectionStrings:DefaultConnection`. Exemplos:

- SQL Server:
```
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=AcademicoDB;User Id=sa;Password=SuaSenha;TrustServerCertificate=True;"
}
```

- SQLite (para testes locais):
```
"ConnectionStrings": {
  "DefaultConnection": "Data Source=academico.db"
}
```

Após ajustar a connection string, verifique que o `DbContext` (`AcademicoContext`) está registrado em `Program.cs`/`Startup` usando `options.UseSqlServer(...)` ou `options.UseSqlite(...)` conforme sua escolha.

**Criar e aplicar migrations**
1. Abra um terminal na pasta do projeto (`CentralAcademico`), onde está o arquivo `.csproj`.
2. Para criar uma migration (exemplo solicitado):

```
dotnet ef migrations add CriandoDisciplina
```

3. Para aplicar as migrations e criar/atualizar o banco de dados:

```
dotnet ef database update
```

Observações úteis:
- Se estiver em uma solução com múltiplos projetos, especifique o projeto de inicialização com `--startup-project` e o projeto onde estão as migrations com `--project`.
  Exemplo:
  ```
  dotnet ef migrations add CriandoDisciplina --project Academico --startup-project Academico
  dotnet ef database update --project Academico --startup-project Academico
  ```
- Se receber erro sobre o provedor (ex.: `No database provider has been configured`), confirme que `UseSqlServer` ou `UseSqlite` foi chamado no `Program.cs` e que a connection string existe.
- Para remover a última migration (se ainda não foi aplicada):
  ```
  dotnet ef migrations remove
  ```

**Executando a aplicação**
Após aplicar as migrations, rode a aplicação:

```
dotnet run
```

ou, durante desenvolvimento com recarregamento automático:

```
dotnet watch run
```

**Arquivos importantes**
- `Program.cs`: registra `AcademicoContext` e configura serviços.
- `AcademicoContext.cs` (em `Models/`): definição do DbContext e DbSets.
- `Migrations/`: pasta onde o EF cria os arquivos de migrations.

Se quiser, posso também gerar um exemplo de `appsettings.json` já preenchido para o seu ambiente (SQL Server ou SQLite). Caso queira, diga qual banco você prefere.

---
Desenvolvido por: Natanael F. Rodrigues — IFMT (2026)
