using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Settings;
using CombatAnalysis.Core.ViewModels;
using Microsoft.Win32;
using MvvmCross.Platforms.Wpf.Views;
using System;
using System.IO;
using System.Text.Json;

namespace CombatAnalysis.App.Views;

public partial class CombatLogInformationView : MvxWpfView
{
    public CombatLogInformationView()
    {
        InitializeComponent();
    }

    private void SelectCmbatLogFiles(object sender, System.Windows.RoutedEventArgs e)
    {
        var viewModel = (CombatLogInformationViewModel)ViewModel;

        var userSettings = ReadUserSettings("user.json");

        var fileDialog = new OpenFileDialog
        {
            InitialDirectory = userSettings?.Location
        };
        fileDialog.ShowDialog();

        if (viewModel != null && !string.IsNullOrEmpty(fileDialog.FileName))
        {
            viewModel.CombatLogPaths.Clear();
            viewModel.CombatLogNames.Clear();

            viewModel.CombatLogPaths.Add(fileDialog.FileName);
            AppStaticData.SelectedCombatLogFilePaths = [.. viewModel.CombatLogPaths];
        }
    }

    private void SelectMoreCmbatLogFiles(object sender, System.Windows.RoutedEventArgs e)
    {
        var viewModel = (CombatLogInformationViewModel)ViewModel;

        var userSettings = ReadUserSettings("user.json");

        var fileDialog = new OpenFileDialog
        {
            InitialDirectory = userSettings?.Location
        };
        fileDialog.ShowDialog();

        if (viewModel != null && !string.IsNullOrEmpty(fileDialog.FileName))
        {
            viewModel.CombatLogPaths.Add(fileDialog.FileName);
            AppStaticData.SelectedCombatLogFilePaths = [.. viewModel.CombatLogPaths];
        }
    }

    private static UserSettings? ReadUserSettings(string settingsName)
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        using var fs = new FileStream($"{baseDirectory}{settingsName}", FileMode.OpenOrCreate);
        var userSettings = JsonSerializer.Deserialize<UserSettings>(fs);

        return userSettings;
    }
}
