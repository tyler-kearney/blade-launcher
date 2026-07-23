using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using blade_launcher.Core;
using blade_launcher.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace blade_launcher.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly MacAppIndexer _indexer = new();
    private List<AppEntry> _allApps = new();
    
    [ObservableProperty]
    private string _searchQuery = string.Empty;

    public ObservableCollection<AppEntry> FilteredApps { get; } = new();

    public MainWindowViewModel()
    {
        LoadApps();
    }

    private void LoadApps()
    {
        _allApps = _indexer.ScanApplications();
        FilterApps(SearchQuery);
    }

    partial void OnSearchQueryChanged(string value)
    {
        FilterApps(value);
    }

    private void FilterApps(string query)
    {
        FilteredApps.Clear();
        
        var matches = string.IsNullOrWhiteSpace(query)
            ? _allApps
            : _allApps.Where(a => a.Name.Contains(query, StringComparison.OrdinalIgnoreCase));

        foreach (var app in matches)
        {
            FilteredApps.Add(app);
        }
    }

    [RelayCommand]
    public void LaunchSelected(AppEntry? app)
    {
        if (app == null) return;
        
        // Use macOS native `open` command to start the application
        System.Diagnostics.Process.Start("open", $"\"{app.ExecutablePath}\"");
    }
}