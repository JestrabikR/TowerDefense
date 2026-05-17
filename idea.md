# Název projektu: Minimalist Tower Defense

## 1. Koncept hry
Jedná se o minimalistickou 2D strategickou hru typu Tower Defense zjednodušenou pro rychlou hratelnost. Hráč brání svou základnu před vlnami nepřátel, kteří se pohybují po pevně dané trase. Obranu zajišťuje strategickým umisťováním obranných věží na předem určená místa (sloty) na mapě.

## 2. Core Gameplay (Základní hratelnost)
*   **Pohyb nepřátel:** Nepřátelé se automaticky rodí (spawnují) na začátku trasy a putují k základně. Pokud dorazí do cíle, hráč ztrácí život.
*   **Obrana:** Hráč kliknutím na volný stavební slot může postavit obrannou věž, pokud má dostatek zlata.
*   **Útok věží:** Věž automaticky detekuje nejbližšího nepřítele ve svém dosahu a v pravidelných intervalech na něj střílí / uděluje mu poškození.
*   **Konec hry:** Hra končí porážkou (Game Over), pokud životy hráče klesnou na nulu.

## 3. Strategický systém (Požadavek zadání)
Hra obsahuje provázaný ekonomický a balanční systém založený na správě zdrojů:
*   **Zdroje (Zlato):** Hráč začíná s fixním množstvím zlata. Další zlato získává jako odměnu za každého zničeného nepřítele.
*   **Investice vs. Riziko:** Věže mají svou cenu. Hráč musí strategicky rozhodovat, kdy zlato pošetřit a kdy ho okamžitě investovat do obrany, aby ho nepřátelé nepřemohli.
*   **Gradace obtížnosti:** Každá další vlna nepřátel je o něco silnější (např. nepřátelé mají více životů), což nutí hráče neustále rozšiřovat svou síť věží a efektivně hospodařit s příjmy.

## 4. Technický rozsah:
*   **Grafika:** Použití základních 2D tvarů (ColorRect / Sprite2D) z volně dostupných balíčků.
*   **Úrovně:** Pouze 1 fixní mapa s 1 trasou.
*   **Typy jednotek:** 1 typ věže (Základní střílna) a 1 typ nepřítele (Základní voják).
*   **Uživatelské rozhraní (UI):** Horní lišta zobrazující aktuální Zlato a Životy, jednoduché tlačítko v herním světě pro stavbu a finální text "Game Over".
