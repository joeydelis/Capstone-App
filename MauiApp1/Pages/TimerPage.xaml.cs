namespace MauiApp1.Pages;

public partial class TimerPage : ContentPage
{
	public TimerPage()
	{
		InitializeComponent();
        InitializePickers();
	}
    private void InitializePickers()
    {
        // Populate the Minutes Picker
        List<string> minutes = new List<string>();
        for (int i = 0; i < 60; i++)
        {
            minutes.Add(i.ToString("D2"));
        }
        MinutesPicker.ItemsSource = minutes;

        // Populate the Seconds Picker
        List<string> seconds = new List<string>();
        for (int i = 0; i < 60; i++)
        {
            seconds.Add(i.ToString("D2"));
        }
        SecondsPicker.ItemsSource = seconds;
    }

    private void OnSetTimerClicked(object sender, EventArgs e)
    {
        string selectedMinutes = MinutesPicker.SelectedItem?.ToString();
        string selectedSeconds = SecondsPicker.SelectedItem?.ToString();

        if (selectedMinutes != null && selectedSeconds != null)
        {
            DisplayAlert("Timer Set", $"Timer set for {selectedMinutes} minutes and {selectedSeconds} seconds", "OK");
        }
        else
        {
            DisplayAlert("Error", "Please select both minutes and seconds.", "OK");
        }
    }
}