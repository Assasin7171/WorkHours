# WorkHours – Lista TODO

## ✅ Zrobione
- [x] Dodawanie sesji pracy z opisem, datą, godzinami i miejscem
- [x] Obsługa relacji Place → Worksession (1:N)
- [x] Reset formularza po dodaniu
- [x] Usuwanie sesji z potwierdzeniem
- [x] Dynamiczne ładowanie danych (InitAsync)
- [x] ObservableCollection dla UI
- [x] Rozwijana sekcja z ikoną strzałki

## 🚧 Do zrobienia

### 🧠 Logika / Funkcjonalność
- [ ] Zabezpieczenie AddSessionToDatabase przed SelectedPlace == null
- [ ] Walidacja opisu i liczby godzin
- [ ] Sortowanie Worksessions po CreatedTime
- [ ] Możliwość edycji sesji pracy

### 📊 Statystyki / Wykresy
- [ ] Statystyki miesięczne – BarChart / DonutChart
- [ ] Obsługa przypisania do dnia z przeszłości
- [ ] Unikanie duplikatów dni w wykresie tygodniowym
- [ ] Roczne statystyki – sumowanie godzin per miesiąc

### 🎨 UI / UX
- [ ] Przewijanie do nowo dodanego elementu
- [ ] Poprawka pozycjonowania formularza (iOS, dół ekranu)
- [ ] Wyróżnianie dnia dzisiejszego na wykresie
- [ ] Blokowanie przycisków formularza przy brakujących danych

## 💡 Pomysły na przyszłość
- [ ] Eksport danych do CSV / PDF
- [ ] Edycja i usuwanie miejsc pracy
- [ ] Filtrowanie po dacie i miejscu
- [ ] Przełącznik motywu jasny/ciemny z poziomu aplikacji
- [ ] Onboarding dla nowych użytkowników
- [ ] Obsługa synchronizacji offline