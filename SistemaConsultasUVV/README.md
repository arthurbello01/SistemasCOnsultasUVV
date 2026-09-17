## Configuração do banco de dados

> **Atenção:** os arquivos do projeto estão dentro da subpasta `SistemaConsultasUVV/`. Entre nela antes de rodar os comandos abaixo:
> ```bash
> cd SistemaConsultasUVV
> ```

1. Restaure os pacotes do projeto:
```bash
   dotnet restore
```
...

## Executando a aplicação

```bash
dotnet run
```
> **Importante:** o código-fonte está dentro da subpasta `SistemaConsultasUVV/`. Rode `cd SistemaConsultasUVV` antes dos comandos abaixo.

cd SistemaConsultasUVV
dotnet restore
dotnet ef database update
dotnet run

# Sistema de Gestão de Consultas UVV

Trabalho prático da disciplina **Desenvolvimento Web Back-end** (UVV).
Aplicação ASP.NET Core MVC com EF Core (Code First), SQL Server, autenticação por cookie e autorização de rotas.

## Participantes do grupo

- Arthur Bello Medina 


## Tecnologias

- ASP.NET Core 8.0 (MVC)
- Entity Framework Core 8 (Code First + Migrations)
- SQL Server (LocalDB por padrão)
- Autenticação por Cookie + `[Authorize]`
- Bootstrap 5 (via CDN)

## Estrutura do projeto

```
SistemaConsultasUVV/
├── Controllers/       # HomeController, ContaController, ConsultasController
├── Models/            # Usuario, Consulta
├── ViewModels/         # LoginViewModel, RegistroViewModel
├── Data/              # ApplicationDbContext
├── Views/             # Razor Views (Home, Conta, Consultas, Shared)
├── wwwroot/           # Arquivos estáticos (css)
├── appsettings.json   # Connection string
└── Program.cs         # DI, autenticação e pipeline de middlewares
```

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (já vem com o Visual Studio) **ou** uma instância própria do SQL Server
- Ferramenta `dotnet-ef`:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

## Configuração do banco de dados

1. Restaure os pacotes do projeto:
   ```bash
   dotnet restore
   ```

2. Ajuste, se necessário, a connection string em `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SistemaConsultasUVV;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   }
   ```

3. Crie a migration inicial (gera as tabelas `Usuarios` e `Consultas` a partir das classes em `Models/`):
   ```bash
   dotnet ef migrations add InitialCreate
   ```

4. Aplique a migration no banco de dados:
   ```bash
   dotnet ef database update
   ```

   > No Visual Studio, os passos 3 e 4 equivalem a rodar `Add-Migration InitialCreate` e `Update-Database` no Package Manager Console.

## Executando a aplicação

```bash
dotnet run
```

A aplicação estará disponível em `https://localhost:5001` (ou na porta exibida no terminal).

## Fluxo de uso

1. Acesse `/Conta/Registro` e crie uma conta (nome, e-mail e senha).
2. Faça login em `/Conta/Login`.
3. Após autenticado, acesse "Minhas Consultas" para cadastrar, editar, visualizar ou excluir suas consultas.
4. As rotas do `ConsultasController` são protegidas por `[Authorize]`: sem login, o usuário é redirecionado para a tela de login.

## Segurança implementada

- Senhas nunca são armazenadas em texto puro: são convertidas em hash com `IPasswordHasher<Usuario>` antes de serem persistidas.
- Validação de entrada no servidor com Data Annotations (`[Required]`, `[EmailAddress]`, `[StringLength]`, `[Compare]`).
- Autenticação via cookie (`app.UseAuthentication()` configurado **antes** de `app.UseAuthorization()` em `Program.cs`).
- Rotas de consultas protegidas com `[Authorize]`, e cada usuário só visualiza/edita/exclui as próprias consultas (filtro por `UsuarioId` extraído do cookie de autenticação).
- Tokens antifalsificação (`[ValidateAntiForgeryToken]`) em todos os formulários POST.

## Vídeo demonstrativo

🔗 [Inserir aqui o link do vídeo demonstrativo (Loom/YouTube) mostrando cadastro, login e registro de consulta]

## Repositório

🔗 [Inserir aqui o link do repositório GitHub]
