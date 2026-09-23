# Configurar GitHub (`gh` CLI) para jugar con la demo

Este es el repo original de la charla. Si vas a correr las demos vos mismo
(agentes creando branches, PRs, etc.), **no lo hagas apuntando a este repo** —
forkealo primero a tu cuenta.

## 0. Antes que nada: forkeá el repo

```bash
gh repo fork eze-etchecoin/beyond-the-prompt-tech-talk --clone
```

Esto crea `tu-usuario/beyond-the-prompt-tech-talk` y lo clona con `origin`
apuntando a tu fork y `upstream` al repo original. Trabajá siempre contra tu
fork:

```bash
cd beyond-the-prompt-tech-talk
git remote -v   # origin -> tu fork, upstream -> repo original
```

> ⚠️ Si clonaste el repo original directo, **no sigas**: un agente con
> `gh`/`git` autenticado va a intentar pushear branches y abrir PRs contra el
> repo original en vez de tu copia. Forkeá y volvé a clonar desde tu fork
> antes de dejar correr un agente suelto.

## 1. Instalar y autenticar `gh`

```bash
gh --version        # si no está: https://cli.github.com
gh auth login       # elegí GitHub.com, HTTPS o SSH, y logueate con tu cuenta
gh auth status      # confirmá la cuenta y los scopes (necesitás al menos 'repo')
```

## 2. Verificar que estás sobre tu fork, no sobre el original

```bash
gh repo set-default   # elegí TU fork como repo por defecto de gh
gh repo view --json nameWithOwner -q .nameWithOwner
# tiene que devolver tu-usuario/beyond-the-prompt-tech-talk
```

> En un fork, `gh pr create` propone por defecto abrir el PR contra el repo
> **original**. Fijar el default con `gh repo set-default` hace que los PRs
> del agente queden dentro de tu fork.

Si un agente te pregunta a qué repo pushear o abrir un PR, la respuesta
correcta es **tu fork**. Para traer cambios nuevos de la charla:

```bash
git fetch upstream
git merge upstream/main   # o rebase, a gusto
```

## 3. Qué puede hacer un agente con `gh` autenticado

Con `repo` scope, un agente puede crear branches, abrir/cerrar PRs, comentar,
mergear y (con permiso) hacer `push --force` o borrar branches remotas, en
cualquier repo donde tu usuario tenga permisos. Por eso conviene forkear
primero y fijar tu fork como default.

## 4. Limpieza al terminar de jugar

Si dejaste PRs o branches de prueba en tu fork, podés limpiarlos vos o
pedirle al agente:

```bash
gh pr list --state open --author "@me"
gh pr close <numero>
git push origin --delete <branch>
```
