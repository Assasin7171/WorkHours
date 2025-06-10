using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using SkiaSharp;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public partial class DataViewModel : ObservableObject
{
    private readonly DataStoreService _dataStoreService;
    [ObservableProperty] private Chart _chart;

    public DataViewModel(DataStoreService dataStoreService)
    {
        _dataStoreService = dataStoreService;

        var entries = new[]
        {
            new ChartEntry(4) { Label = "Pon", ValueLabel = "4h", Color = SKColor.Parse("#2ecc71") },
            new ChartEntry(6) { Label = "Wt",  ValueLabel = "6h", Color = SKColor.Parse("#3498db") },
            new ChartEntry(8) { Label = "Śr",  ValueLabel = "8h", Color = SKColor.Parse("#e74c3c") },
            new ChartEntry(5) { Label = "Czw", ValueLabel = "5h", Color = SKColor.Parse("#f1c40f") },
            new ChartEntry(7) { Label = "Pt",  ValueLabel = "7h", Color = SKColor.Parse("#9b59b6") }
        };
        
        Chart = new LineChart()
        {
            Entries = entries,
            LabelTextSize = 30,
        };
    }
}