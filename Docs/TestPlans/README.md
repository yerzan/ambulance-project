# Test Plans

## Автоматичні тести

| Тип | Де | Запуск |
|---|---|---|
| EditMode | `Assets/_Game/Tests/EditMode` (`LastCall.Tests.EditMode`) | `-runTests -testPlatform EditMode` |
| PlayMode | `Assets/_Game/Tests/PlayMode` (`LastCall.Tests.PlayMode`) | `-runTests -testPlatform PlayMode` |
| Build smoke | `LastCall.Editor.BuildScript.BuildWindows` | `-executeMethod …` |

## Мінімальний мережевий тест-набір (GDD §18.1)

| # | Сценарій | Що перевіряємо | З якої фази |
|---|---|---|---|
| T1 | 1 медик + NPC | Базовий контракт проходиться мінімальною кількістю гравців; адаптація складності | Phase 2 (локально) |
| T2 | 2 медики + NPC | Ролі, фізика, ноші, доставка, умови завершення | Phase 3 |
| T3 | 4 медики + NPC | Трафік, читабельність HUD, взаємодії | Phase 3 |
| T4 | Поганий ping / packet loss (150 мс / 5%) | Розсинхрон нош, пацієнта, дверей, авто | Phase 3 |
| T5 | Від'єднання хоста | Матч коректно завершується з повідомленням | Phase 3 |
| T6 | NPC тікає під час затримки | Сигнал втечі, реакція медиків, залежність від архетипу | Phase 4 |
| T7 | NPC застряг у геометрії | Unstuck / safe recovery без розсинхрону | Phase 4 |
| T8 | Spam RPC / повторна взаємодія | Хост відхиляє невалідні та повторні дії | Phase 3 |
| T9 | Різний FPS (30 / 60 / 144) | Фізика й таймери не залежать від FPS | Phase 1+ |

## Gate-перевірки

- **Phase 0:** чистий `git clone` → batchmode-збірка Windows без помилок + EditMode smoke-тест зелений.
