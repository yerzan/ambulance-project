# ADR-001: Вибір рушія та базового технічного стеку

| Поле | Значення |
|---|---|
| Статус | **Accepted** |
| Дата | 2026-11-19 |
| Задача | LC-001 |
| Автори | Команда LAST CALL |
| Замінює | GDD v1.1 §13.1 («рушій ще не обрано») |

## Контекст

У GDD v1.1 є суперечність: у змісті вказано «рушій Unity», а в розділі 13 написано, що рушій не обрано і рішення треба ухвалити після порівняльного прототипу Unity vs Unreal. Без зафіксованого рушія неможливо стартувати LC-002…LC-011, налаштувати репозиторій (LFS, .gitignore), правила для Claude Code та мережевий стек.

Вимоги, які впливають на вибір (з GDD):

- Тільки first-person камера, зокрема в автомобілі (§3, §7.1, §11.1).
- Фізичні ноші, двері, автомобіль, обмежений ragdoll (§7.2, §13.4).
- NPC-пацієнти на навігаційній сітці + власні state machines (§8, §13.1).
- Online co-op 1–4 медики, host-authoritative, приватні лобі (§3, §13.4).
- Команда з двох розробників, активне використання Claude Code (§17, §20).
- Windows PC / Steam як перша платформа.

## Рішення

| Компонент | Обрано | Версія |
|---|---|---|
| Рушій | **Unity 6.6** | `6000.6.10f1` (зафіксовано в `ProjectSettings/ProjectVersion.txt`) |
| Render pipeline | Universal Render Pipeline (URP) | 17.x (з шаблону `urp-blank`) |
| Мова | C# | — |
| Мережа | Netcode for GameObjects | 2.x |
| Sessions / Lobby / Relay | Unity Multiplayer Services SDK (`com.unity.services.multiplayer`) | 1.x / 2.x |
| Локальні мультиплеєр-тести | Multiplayer Play Mode | — |
| Ввід | Input System | 1.x |
| Навігація NPC | AI Navigation (NavMesh) | 2.x |
| Тести | Unity Test Framework (EditMode/PlayMode) | — |
| Голос | Vivox — **відкладено** до Phase 3+ | — |
| VCS | Git + Git LFS, GitHub | — |

Точні версії пакетів фіксуються в `Packages/manifest.json` і `Packages/packages-lock.json`.

### Чому Unity, а не Unreal

1. **C#** — швидша ітерація і краща продуктивність Claude Code, ніж на C++/Blueprints; малий поріг входу для команди з 2 людей.
2. **NGO** розрахований саме на невеликі GameObject-based кооперативні ігри (R1); є офіційна модель server-authoritative фізики через `NetworkRigidbody` (R3).
3. **Multiplayer Services SDK** об'єднує Sessions/Lobby/Relay в одному API (R2) — приватні лобі без власних серверів у MVP.
4. **Multiplayer Play Mode** дає змогу тестувати 2–4 гравців на одному ПК (R5).
5. Stylized-арт і first-person не потребують фотореалізму Unreal.

### Чому Unity 6.6, а не 6.3 LTS / 6.7

- 6.6 (`6000.6.10f1`) уже встановлена на робочій машині та є стабільним (не beta) релізом.
- 6.7 на момент рішення — beta, не придатна для production-фундаменту.
- 6.3 LTS — стабільніша, але відстає за функціями; при потребі LTS-міграції оформити окремий ADR.

## Критерії валідації (spike, 3–5 днів, у межах LC-001 → LC-003/006/009)

Рішення вважається остаточним, якщо в тестовій сцені `TestYard` за 3–5 днів вдається показати:

| # | Критерій | Як перевіряємо |
|---|---|---|
| V1 | First-person контролер (рух, біг, присід, interact) | Ручний тест у редакторі, 60+ FPS |
| V2 | Ноші-фізика: 2 «ручки», перенесення, зіткнення з дверима без застрягань | Ручний тест + 1 PlayMode-тест |
| V3 | NPC на NavMesh: idle → follow → evade, без застрягань | Ручний тест, unstuck-таймер |
| V4 | Мережа на 2 гравців: host + client через Multiplayer Play Mode, синхронні ноші й NPC, фізика тільки на хості | MPPM, штучний lag 150 мс |

**Якщо V2 або V4 провалюються без зрозумілого шляху виправлення** — ADR переглядається (варіанти: інший мережевий підхід у Unity, наприклад client-anticipation / Netcode for Entities, або повернення до порівняння з Unreal).

## Наслідки

**Позитивні**
- Розблоковано LC-002…LC-011; архітектура GDD §13.3/§14 застосовується без змін.
- Єдиний стек (Unity Gaming Services) для лобі, relay і в майбутньому голосу.

**Негативні / ризики**
- Мережева фізика нош/NPC — найбільший технічний ризик (GDD §18.2); NGO не дає вбудованої client-side prediction для фізики.
- Залежність від Unity Gaming Services (ціна/ліміти при масштабуванні).
- Unity 6.6 — не LTS: оновлення патчів робимо свідомо, одним комітом, лише після погодження.

## Правила, що випливають з рішення

- Версію редактора, пакетів та `ProjectSettings/` **не змінювати без явного погодження** (див. `CLAUDE.md`).
- Оновлення версії Unity — окрема гілка `chore/unity-upgrade-<версія>` + новий ADR.
- Розділ 13 GDD вважається застарілим у частині «рушій не обрано»; джерело правди — цей ADR.

## Джерела

- [R1] Unity Netcode packages — https://docs.unity.com/en-us/multiplayer/netcode/netcode
- [R2] Multiplayer Services SDK — https://docs.unity.com/en-us/mps-sdk/install-and-upgrade
- [R3] NGO physics — https://mp-docs.dl.it.unity3d.com/netcode/current/advanced-topics/physics/
- [R5] Multiplayer Play Mode — https://mp-docs.dl.it.unity3d.com/mppm/current/about/
