using MedicalDate.View;
using MedicalDate.View.Appointment;
using MedicalDate.View.Doctor;
using MedicalDate.View.MedicalRecord;
using MedicalDate.View.Patient;
using MedicalDate.View.Prescription;
using Windows.System.Threading;

namespace MedicalDate;

public sealed partial class MainPage : Page
{
    private ThreadPoolTimer? _timer;
    public MainPage()
    {
        this.InitializeComponent();
        StartTimer();
    }

    private void StartTimer()
    {
        // Configura el temporizador para que se dispare después de 5 minutos (300 segundos)
        _timer = ThreadPoolTimer.CreateTimer(TimerElapsedHandler, TimeSpan.FromHours(5));
    }

    private async void TimerElapsedHandler(ThreadPoolTimer timer)
    {
        // Detener el temporizador
        _timer.Cancel();

        // Realizar la acción deseada (cerrar la aplicación o deshabilitarla)
        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
        {
            var dialog = new ContentDialog
            {
                Title = "Tiempo Expirado",
                Content = "El período de uso ha expirado. La aplicación se desactivará.",
                XamlRoot = XamlRoot,
                CloseButtonText = "Cancelar",
            };

            _ = dialog.ShowAsync();
            nav.IsEnabled = false;
            frame.IsEnabled = false;
        });
    }

    private void nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        var selected = (NavigationViewItem)args.SelectedItem;
        if(selected.Name == doctor.Name)
        {
            frame.Navigate(typeof(Doctor));
        }else if(selected.Name == paciente.Name)
        {
            frame.Navigate(typeof(Patient));
        }else if(selected.Name == cita.Name)
        {
            frame.Navigate(typeof(Appointment));
        }else if(selected.Name == historial.Name)
        {
            frame.Navigate(typeof(Historial));
        }else if(selected.Name == receta.Name)
        {
            frame.Navigate(typeof(Prescription));
        }else if(selected.Name == logout.Name)
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values.Remove("Token");
            Frame frames = new Frame();
            frames.Navigate(typeof(Login));
            this.Content = frames;
        }
    }
}
