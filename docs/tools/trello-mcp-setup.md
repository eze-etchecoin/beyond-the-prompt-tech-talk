# Conectar el MCP Server de Trello (en tu máquina)

El MCP **oficial de Atlassian para Trello** es un servidor **remoto (cloud-hosted)**
que se autentica por **OAuth 2.0** — **no requiere API key ni token**. Se instala a
nivel **usuario/máquina**, así que no hay credenciales ni config en el repositorio.

- URL del servidor: `https://mcp.trello.com/v1`
- Repo de referencia: <https://github.com/atlassian/trello-mcp-server>
- Directorio de conectores para Claude: <https://claude.ai/directory/connectors/trello>

> Ojo: el MCP de Atlassian *Rovo* (Jira/Confluence) es **otro** servidor distinto a
> este de Trello.

## 1. Registrar el servidor (ya hecho)

En este equipo ya quedó agregado a nivel usuario con:

```bash
claude mcp add --transport http --scope user trello https://mcp.trello.com/v1
```

Verificar:

```bash
claude mcp list        # 'trello ... (HTTP)'
claude mcp get trello  # detalle + scope
```

## 2. Autenticar (OAuth — paso interactivo)

El registro deja el server en estado **"Needs authentication"**. Para completar el
OAuth hay que hacerlo en una **sesión interactiva** de Claude Code (no funciona en
sesiones no interactivas):

1. Abrí Claude Code en cualquier proyecto.
2. Ejecutá `/mcp`.
3. Elegí **trello** y seguí el flujo de autorización en el navegador (consent de
   Trello). Al terminar, el estado pasa a *conectado*.

> En el lanzamiento, la conexión es de **un workspace de Trello por conexión**.

## 3. Quitarlo (si hace falta)

```bash
claude mcp remove trello --scope user
```

## 4. Preparar el tablero

Creá **tu propio** tablero de Trello para el desarrollo del Minesweeper (no
reutilices el tablero personal de quien dio la charla) con, al menos, las
listas: **Backlog**, **To Do**, **In Progress**, **In Review**, **Done**.
El backlog inicial de *cards* está en
[`../talk/minesweeper-demo.md`](../talk/minesweeper-demo.md).

> El OAuth queda atado a **tu cuenta de Trello**, así que el agente solo va a
> poder ver/editar tableros a los que vos tengas acceso — no hay forma de que
> toque el tablero de otra persona por error.

## Alternativa: skill `trello-use`

El repo también ofrece una *skill* de agente instalable con:

```bash
npx skills install atlassian/trello-mcp-server
```
