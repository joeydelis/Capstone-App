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

        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }

        public PresetsViewModel()
        {
            firebase = Firebase.Instance;
            DeleteCommand = new Command<Firebase.PresetData>(async (preset) => await DeletePreset(preset));
            AddCommand = new Command(async () => await AddPreset());

            LoadPresets();
        }

        private async void LoadPresets()
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
        }

        private async Task DeletePreset(Firebase.PresetData preset)
        {
            bool success = await firebase.DeleteUserPresetAsync(preset.Id);
            if (success)
            {
                Presets.Remove(preset);
            }
        }

        private async Task AddPreset()
        {
            string name = await Application.Current.MainPage.DisplayPromptAsync("New Preset", "Enter preset name:", "OK", "Cancel", "Preset Name");
            if (string.IsNullOrWhiteSpace(name))
                return;
            bool success = await firebase.AddUserSetting(name);
            if (success)
            {
                LoadPresets();
            }
        }
    }
}

