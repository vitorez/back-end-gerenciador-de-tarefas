# Front-end — Gerenciador de Tarefas

Front React (Vite) bem simples para a API em `..` (.NET 8).

## Rodando

1. Suba o backend (perfil http, porta 5215):

   ```
   dotnet run --launch-profile http
   ```

2. Em outro terminal:

   ```
   npm install
   npm run dev
   ```

Abre em http://localhost:4200 — essa porta é obrigatória, é a única
liberada na policy de CORS (`AllowAngular`) do `Program.cs`.

A URL da API fica em `.env` (`VITE_API_URL`).

## O que tem

- Listar tarefas (`GET /api/tasks`)
- Criar (`POST /api/tasks`)
- Editar (`PUT /api/tasks/{id}`)
- Marcar como concluída (PUT com `completed` invertido)
- Excluir (`DELETE /api/tasks/{id}`)

## Observação sobre datas

O `GET /api/tasks` devolve a data já formatada para exibição (`"31 ago"`),
não em ISO. Por isso, ao editar uma tarefa os campos de data/hora voltam
vazios — e o backend, ao receber `date` vazio, mantém a data original.
Para alterar a data, basta preencher os campos novamente.
