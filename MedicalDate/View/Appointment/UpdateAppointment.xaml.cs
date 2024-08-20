using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using MedicalDate.Services;
using MedicalDate.Model;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MedicalDate.View.Appointment;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class UpdateAppointment : Page
{
    AppointmentServices services = new AppointmentServices();
    DoctorServices doctorServices = new DoctorServices();
    PatientServices patServices = new PatientServices();
    List<string> status = new List<string>() { "Completado", "Programado", "Cancelado" };
    int id;
    public UpdateAppointment()
    {
        this.InitializeComponent();
        InitializePatient();
        InitializeDoctor();
        combostatus.ItemsSource = status;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var select = (AppointmentModel)e.Parameter;
        txtreason.Text = select.Reason;
        combodoc.Text = select.Doctor;
        combopac.Text = select.Patient;
        combostatus.Text = select.Status;
        id = select.Id;
        date.Date = Convert.ToDateTime(select.AppointmentDate);
        base.OnNavigatedTo(e);
    }

    private async void InitializeDoctor()
    {
        var item = await doctorServices.GetAll();
        combodoc.ItemsSource = item;
    }

    private async void InitializePatient()
    {
        var item = await patServices.GetAll();
        combopac.ItemsSource = item.ToList();
    }

    private async void btnupdate_Click(object sender, RoutedEventArgs e)
    {
        var doc = (DoctorModel)combodoc.SelectedItem;
        var pac = (PatientModel)combopac.SelectedItem;
        var item = new Model.AppointmentModel()
        {
            Id = id,
            Doctor = doc.Name,
            Patient = pac.Name,
            AppointmentDate = date.Date.ToString(),
            Status = combostatus.SelectedItem.ToString(),
            Reason = txtreason.Text,
        };
        await services.Update(item);
        Frame frame = new Frame();
        frame.Navigate(typeof(Appointment));
        this.Content = frame;
    }
}
