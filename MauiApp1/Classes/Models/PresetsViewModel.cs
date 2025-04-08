using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.Classes.Models
{
    public class PresetsViewModel : BindableObject
    {
        private readonly Firebase firebase;
        public ObservableCollection<Firebase.PresetData> Presets { get; set; } = new();

        private Firebase.PresetData _selectedPreset;
        public Firebase.PresetData SelectedPreset
        {
            get => _selectedPreset;
            set
            {
                _selectedPreset = value;
                OnPropertyChanged();
                if (_selectedPreset != null)
                {
                    LoadPresetCommand.Execute(_selectedPreset);
                    SelectedPreset = null;
                }
            }
        }

        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand LoadPresetCommand { get; }

        public PresetsViewModel(Firebase firebaseParam)
        {
            firebase = firebaseParam;
            DeleteCommand = new Command<Firebase.PresetData>(async (preset) => await DeletePreset(preset));
            AddCommand = new Command(async () => await AddPreset());
            LoadPresetCommand = new Command<Firebase.PresetData>(async (preset) => await LoadPreset(preset));

            LoadPresets();
        }

        private async void LoadPresets()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Application.Current.MainPage.DisplayAlert("No Internet", "Please check your internet connection and try again.", "OK");
            }
            else
            {
                var presets = await firebase.GetUserPresetsAsync();
                if (presets != null)
                {
                    Presets.Clear();
                    foreach (var preset in presets)
                    {
                        Presets.Add(preset);
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error!", "There was an error retrieving user settings.", "Ok");
                }
            }
            
        }

        private async Task DeletePreset(Firebase.PresetData preset)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Application.Current.MainPage.DisplayAlert("No Internet", "Please check your internet connection and try again.", "OK");
                return;
            }

            bool success = await firebase.DeleteUserPresetAsync(preset.Id);
            if (success)
            {
                Presets.Remove(preset);
                await Application.Current.MainPage.DisplayAlert("Preset deleted.", $"Preset {preset.Name} has been deleted.", "Ok");
            }
        }

        private async Task AddPreset()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Application.Current.MainPage.DisplayAlert("No Internet", "Please check your internet connection and try again.", "OK");
                return;
            }

            string name = await Application.Current.MainPage.DisplayPromptAsync("New Preset", "Enter preset name:", "OK", "Cancel", "Preset Name");
            if (string.IsNullOrWhiteSpace(name))
                return;
            bool success = await firebase.AddUserSetting(name);
            if (success)
            {
                LoadPresets();
                await Application.Current.MainPage.DisplayAlert("Preset added!", $"Preset {name} has been added", "Ok");
            }
        }

        private async Task LoadPreset(Firebase.PresetData preset)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Application.Current.MainPage.DisplayAlert("No Internet", "Please check your internet connection and try again.", "OK");
            }

            if (preset == null) return;

            firebase.LoadPreset(preset.Time, preset.Strength);

            await Application.Current.MainPage.DisplayAlert("Preset loaded!", $"Time: {(Globals.DeviceMinutes * 60) + Globals.DeviceSeconds} minutes\n Strength: {preset.Strength}", "Ok");

        }
    }
}

