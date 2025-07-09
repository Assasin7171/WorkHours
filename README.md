# 📱 WorkHours

**WorkHours** to mobilna aplikacja napisana w .NET MAUI, która umożliwia śledzenie przepracowanych godzin, zarobków i generowanie statystyk tygodniowych, miesięcznych oraz rocznych. Idealna dla freelancerów, pracowników z elastycznym czasem pracy lub osób chcących lepiej zarządzać swoim czasem.

---

## ✨ Funkcje

- ⏱️ Rejestracja sesji pracy z lokalizacją  
- 📈 Wykresy godzin i zarobków (tydzień / miesiąc / rok)  
- 📊 Statystyki dzienne, średnie, sumaryczne  
- 🌙 Tryb jasny i ciemny z dynamicznym dostosowaniem kolorów  
- 📱 Obsługa Android i iOS  
- 💾 Lokalne przechowywanie danych w SQLite

---

## 📷 Zrzuty ekranu

![ss1](screenshots/Simulator%20Screenshot%20-%20iPhone%2016%20Pro%20-%202025-07-09%20at%2015.38.54.png)
![ss2](screenshots/Simulator%20Screenshot%20-%20iPhone%2016%20Pro%20-%202025-07-09%20at%2015.39.04.png)
![ss2](screenshots/Simulator%20Screenshot%20-%20iPhone%2016%20Pro%20-%202025-07-09%20at%2015.39.10.png)
![ss2](screenshots/Simulator%20Screenshot%20-%20iPhone%2016%20Pro%20-%202025-07-09%20at%2015.39.21.png)
![ss2](screenshots/Simulator%20Screenshot%20-%20iPhone%2016%20Pro%20-%202025-07-09%20at%2015.39.25.png)
![ss2](screenshots/Simulator%20Screenshot%20-%20iPhone%2016%20Pro%20-%202025-07-09%20at%2015.39.32.png)
---

## 🛠️ Technologie

- [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/)
- `sqlite-net-pcl` do lokalnej bazy danych
- `Microcharts.Maui` do tworzenia wykresów
- `CommunityToolkit.Mvvm` i `CommunityToolkit.Maui`

---

## 🚀 Uruchamianie projektu

### Wymagania
- [.NET 8 SDK](https://dotnet.microsoft.com/)
- Android/iOS emulator lub fizyczne urządzenie
- Visual Studio lub JetBrains Rider
  
---

## 📂 Struktura projektu

```
WorkHours/
├── Models/           // Encje bazy danych
├── ViewModels/       // Logika i dane
├── Views/            // Strony XAML
├── Handlers/         // Własne Handlery np. dla efektów
├── Resources/
│   ├── Fonts/
│   ├── Images/
│   ├── Splash/
│   └── AppIcon/
└── App.xaml.cs       // Start aplikacji
```

---

## 🧠 Przyszłe funkcje

- ☁️ Synchronizacja z chmurą (opcjonalnie)
- 🔔 Przypomnienia i powiadomienia
- 💼 Eksport danych do CSV / PDF
- 🧩 Widgety na ekranie głównym

---

## 📜 Licencja

MIT © 2025 Konrad Bojanecki  

---

## 💬 Kontakt

Masz pytanie lub sugestię?  
Skontaktuj się na GitHubie.
