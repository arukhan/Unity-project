# Three location maps / Три карты

Добавлены в существующий проект Dead Drop (Unity 6000.6.0f1).

## Что сделано

- Hospital: палата с кроватями, процедурная, приёмная, аптечные шкафы.
- Police station: камеры, хранение улик, стойка приёма, рабочие столы, шкафчики.
- Shop: склад, холодильники, продуктовые стеллажи, овощи/фрукты и касса.
- Общий набор из 16 спрайтов мебели, стены, окна, полы, названия зон и эвакуационная площадка.
- Стены и мебель используют общие прямоугольники для отрисовки и препятствий. Существующий поиск пути зомби учитывает их.
- Начальные точки появления зомби подбираются по новой планировке, в том числе при перезапуске после поимки.
- Сохранены миссии, управление, предметы и последовательное открытие уровней. Размер изображения ящиков немного уменьшен для читаемости мебели.

## Открыть и показать

1. Открыть проект в Unity 6000.6.0f1.
2. Открыть `Assets/Scenes/MainMenu.unity` и нажать Play для обычной игры.
3. Для быстрого просмотра любой карты: меню **Dead Drop > Maps > 1 Hospital / 2 Police station / 3 Supermarket**. Оно запускает выбранную карту и ставит редактор на паузу, не меняя сохранённые разблокировки.
4. Чтобы играть после просмотра, выключить кнопку Pause (две вертикальные полоски) в верхней панели Unity.
5. Повторная проверка: **Dead Drop > Maps > Validate all layouts**. В Play Mode также проверяются настоящие ящики с предметами и количество начальных зомби.

## Устройство

Планировки находятся в `Assets/Scripts/Levels/LocationMap.cs`, отрисовка — в `Assets/Scripts/Graphics/LocationRenderer.cs`, набор мебели — `Assets/Resources/Maps/FurnitureAtlas.png`. Координаты мира: 16 × 10. Как и в исходном проекте, уровни рисуются через OnGUI во время игры; это не отдельные Tilemap-сцены. Для изменения позиции мебели редактируйте вызовы Furniture в LocationMap.

Проверено в Unity: компиляция, отображение всех трёх карт, связность проходимой сетки с радиусом игрока 0.27, доступность SAFE, наличие всех необходимых предметов в 17 ящиках каждого уровня. Это проверка запуска и геометрии, а не полный игровой баланс или прохождение трёх миссий.

## Передать команде

Архив `DeadDrop-Maps.zip` содержит изменённые и новые файлы с путями относительно корня проекта. Основа: `arukhan/Unity-project`, commit `e3b20305a53fb0bae48eaddd6acc7825b531ecbc`. Перед объединением сохранить свою копию проекта; если эти скрипты уже менялись другими участниками, объединить изменения через Git. На GitHub ничего не опубликовано автоматически.

## Происхождение графики

FurnitureAtlas.png создан встроенным инструментом image_gen; это новый сгенерированный набор, а не нарезка присланной картинки. Ссылка-пример использована как ориентир атмосферы. Оригинальная графика персонажей и игровых ящиков осталась из репозитория.

### Полный запрос генерации

Use case: stylized-concept. Asset type: production pixel-art sprite atlas for a top-down 2D zombie survival game. Create ONE 1024x1024 square transparent PNG containing exactly a 4 by 4 regular grid of 16 isolated furniture sprites, each centered in its own equal 256x256 cell, with at least 24 pixels transparent padding in each cell. No grid lines, no labels, no text, no backdrop, no floor tiles, no drop shadow outside each object. Orthographic overhead RPG view with a little visible front face, consistent camera and lighting, crisp hand-drawn 32-bit pixel art, weathered materials, dark outlines, restrained realistic muted colours, like a detailed abandoned supermarket survival tileset. Each individual object completely inside its cell, no overlap, no unrelated loose objects. Row 1 left to right: hospital bed with teal mattress and white pillow oriented vertically; low medical supply cabinet with red first-aid cross; reception desk with computer; hospital waiting bench with three teal seats. Row 2: wooden police office desk with monitor and papers; blue steel locker bank; metal prison bars panel horizontal; evidence storage cabinet with brown boxes. Row 3: tall grocery shelf packed with colourful cans oriented vertically; produce display with red green orange fruit; supermarket checkout counter with register; glass-front drinks refrigerator. Row 4: wooden shipping crate; large potted plant in round pot; metal examination table; industrial generator with yellow hazard stripes. Every sprite should fill roughly 80 percent of its cell in its longest dimension. Consistent detailed pixel art across all sixteen sprites. Actual transparent background.
