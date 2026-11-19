# Backlog — LAST CALL

Джерело правди — [GitHub Issues](https://github.com/yerzan/ambulance-project/issues). Цей файл — дзеркало для офлайн-контексту та AI-агентів. Оновлюйте статус разом із PR.

Статуси: `Backlog` → `Ready` → `In Progress` → `Review` → `Done`.

| ID | Issue | Задача | Етап (milestone) | Пріоритет | Залежності | Статус |
|---|---|---|---|---|---|---|
| LC-001 | [#1](https://github.com/yerzan/ambulance-project/issues/1) | Рушій, базовий проєкт, Git repo | Phase 0 | P0 | — | In Progress → Done після Gate |
| LC-002 | [#2](https://github.com/yerzan/ambulance-project/issues/2) | Greybox Apartment scene | Phase 0 | P0 | LC-001 | Ready |
| LC-003 | [#3](https://github.com/yerzan/ambulance-project/issues/3) | First-person MedicController | Phase 1 | P0 | LC-001 | Ready |
| LC-005 | [#5](https://github.com/yerzan/ambulance-project/issues/5) | Interactable interface | Phase 1 | P0 | LC-003 | Backlog |
| LC-004 | [#4](https://github.com/yerzan/ambulance-project/issues/4) | NPCPatientController + state machine | Phase 2 | P0 | LC-002, LC-005 | Backlog |
| LC-006 | [#6](https://github.com/yerzan/ambulance-project/issues/6) | Stretcher prototype | Phase 2 | P0 | LC-005 | Backlog |
| LC-007 | [#7](https://github.com/yerzan/ambulance-project/issues/7) | Ambulance prototype | Phase 2 | P0 | LC-005, LC-006 | Backlog |
| LC-008 | [#8](https://github.com/yerzan/ambulance-project/issues/8) | Локальний PatientTransferFlow | Phase 2 | P0 | LC-004, LC-006, LC-007 | Backlog |
| LC-009 | [#9](https://github.com/yerzan/ambulance-project/issues/9) | Мережева сесія, синхронізація медиків і NPC | Phase 3 | P0 | LC-008 | Backlog |
| LC-010 | [#10](https://github.com/yerzan/ambulance-project/issues/10) | Перша мережева NPC-місія | Phase 3 | P0 | LC-009 | Backlog |
| LC-011 | [#11](https://github.com/yerzan/ambulance-project/issues/11) | Несправність швидкої + NPC escape | Phase 6 | P1 | LC-004, LC-007 | Backlog |

Повні приймальні критерії — в issues та в [`backlog.json`](backlog.json) (використовувався для створення issues).

## Порядок роботи (критичний шлях)

```
LC-001 ─┬─ LC-002 ───────────────┐
        └─ LC-003 ─ LC-005 ─┬─ LC-004 ─┐
                            ├─ LC-006 ─┼─ LC-007 ─ LC-008 ─ LC-009 ─ LC-010
                            │          │
                            └──────────┴─ LC-011 (Phase 6)
```

Spike-критерії ADR-001 (V1–V4) перевіряються в LC-003, LC-006, LC-004, LC-009 відповідно.
