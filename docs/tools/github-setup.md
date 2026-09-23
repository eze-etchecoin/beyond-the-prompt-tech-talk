# Configurar GitHub (`gh` CLI) para jugar con la demo

Este repo es el **fork/repo de Avenga** usado en la charla. Si vas a correr las
demos vos mismo (agentes creando branches, PRs, etc.), **no lo hagas apuntando
a este repo directamente** — forkealo primero a tu cuenta personal.

## 0. Antes que nada: forkeá el repo

```bash
gh repo fork <org-o-usuario>/beyond-the-prompt-tech-talk --clone
```

Esto crea `tu-usuario/beyond-the-prompt-tech-talk` y lo clona apuntando `origin`
a tu fork (con `upstream` apuntando al repo original). Trabajá siempre contra
tu fork:

```bash
cd beyond-the-prompt-tech-talk
git remote -v   # origin -> tu fork, upstream -> repo de Avenga
```

> ⚠️ Si ya clonaste el repo de Avenga directo, **no sigas**: cualquier agente
> con `gh`/`git` autenticado va a poder abrir PRs, pushear branches y comentar
> issues **en la org**, no en tu copia. Forkeá y volvé a clonar desde tu fork
> antes de dejar correr un agente suelto.

## 1. Instalar y autenticar `gh`

```bash
gh --version        # si no está: https://cli.github.com
gh auth login        # elegí GitHub.com, HTTPS o SSH, y logueate con tu cuenta personal
gh auth status        # confirmá la cuenta y los scopes (necesitás al menos 'repo')
```

Usá **tu cuenta personal**, no una cuenta de servicio de la compañía, salvo que
te lo indiquen explícitamente.

## 2. Verificar que estás sobre tu fork, no sobre el original

```bash
gh repo view --json nameWithOwner -q .nameWithOwner
# tiene que devolver tu-usuario/beyond-the-prompt-tech-talk, NO la org de Avenga
```

Si un agente te pregunta a qué repo pushear/abrir un PR, la respuesta correcta
es **tu fork**. Actualizalo desde `upstream` cuando quieras traer cambios
nuevos de la charla:

```bash
git fetch upstream
git merge upstream/main   # o rebase, a gusto
```

## 3. Qué puede hacer un agente con `gh` autenticado

Con `repo` scope, un agente puede: crear branches, abrir/cerrar PRs, comentar,
mergear, y (con permiso) hacer `push --force` o borrar branches remotas. Todo
eso queda scopeado a los repos donde tu usuario tiene permisos — por eso
forkear primero es lo que evita que termine tocando el repo de la compañía.

## 4. Limpieza al terminar de jugar

Si dejaste PRs o branches de prueba en tu fork, podés limpiarlos vos o
pedirle al agente:

```bash
gh pr list --state open --author "@me"
gh pr close <numero>
git push origin --delete <branch>
```

Esto es sobre **tu fork** — no afecta el repo de Avenga.
