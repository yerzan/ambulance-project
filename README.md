# LAST CALL: Emergency Transfer

Кооперативна first-person dark-comedy гра про хаотичне транспортування пацієнтів швидкою допомогою.
2–4 медики онлайн проти NPC-пацієнтів • Windows PC / Steam • Unity 6.6 + URP + Netcode for GameObjects.

> Робоча назва тимчасова. Дизайн: [`Docs/GDD`](Docs/GDD/). Рішення про рушій: [`ADR-001`](Docs/Architecture/ADR-001-engine.md). Правила для AI-агентів: [`CLAUDE.md`](CLAUDE.md).

## Вимоги

| Інструмент | Версія |
|---|---|
| Unity Editor | **6000.6.10f1** (точно ця версія, модуль *Windows Build Support (Mono/IL2CPP)*) |
| Git | 2.40+ |
| Git LFS | 3.x (`git lfs install` один раз на машині) |
| IDE | Rider або Visual Studio 2022 / VS Code з C# Dev Kit |

## Перший запуск (чиста машина)

```powershell
git lfs install
git clone https://github.com/yerzan/ambulance-project.git C:\Dev\ambulance-project
cd C:\Dev\ambulance-project
git checkout develop

# (рекомендовано) Unity Smart Merge для сцен/префабів
git config merge.unityyamlmerge.name "Unity SmartMerge"
git config merge.unityyamlmerge.driver "'C:/Program Files/Unity/Hub/Editor/6000.6.10f1/Editor/Data/Tools/UnityYAMLMerge.exe' merge -p %O %B %A %A"
```

Далі: Unity Hub → **Add project from disk** → обрати папку → відкрити. Перше відкриття імпортує пакети (кілька хвилин). Стартова сцена: `Assets/_Game/Scenes/Boot/Boot.unity`.

> ⚠️ Не тримайте проєкт у OneDrive/Dropbox/Google Drive — синхронізація `Library/` ламає Unity.

## Збірка та тести з командного рядка

```powershell
$UNITY = "C:\Program Files\Unity\Hub\Editor\6000.6.10f1\Editor\Unity.exe"

# EditMode-тести
& $UNITY -batchmode -projectPath . -runTests -testPlatform EditMode -testResults TestResults\editmode.xml -logFile Logs\tests-editmode.log

# Windows-збірка → Builds\Windows\LastCall.exe
& $UNITY -batchmode -quit -projectPath . -executeMethod LastCall.Editor.BuildScript.BuildWindows -logFile Logs\build.log
```

Редактор має бути закритий перед batchmode-командами на тому самому проєкті.

## Гілки та коміти

```
main      ← стабільна, тільки PR з develop після gate
develop   ← інтеграція
feature/LC-xxx-short-name, fix/LC-xxx-..., chore/...
```

- Коміт: `LC-xxx: короткий опис у наказовому способі`.
- PR у `develop` з заповненим чеклістом Definition of Done.
- Задачі: [GitHub Issues](https://github.com/yerzan/ambulance-project/issues), дзеркало — [`Docs/Tasks/BACKLOG.md`](Docs/Tasks/BACKLOG.md).

## Структура

```
Assets/_Game/   Art Audio Prefabs Scenes Scripts Data Settings Tests
Packages/       manifest.json + packages-lock.json (не змінювати без погодження)
ProjectSettings/
Docs/           GDD Architecture Networking TestPlans Tasks
CLAUDE.md       правила для AI-агентів
```
