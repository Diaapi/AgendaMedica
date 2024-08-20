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

namespace MedicalDate.View.MedicalRecord;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class UpdateHistorial : Page
{
    HistorialServices services = new HistorialServices();
    DoctorServices doctorServices = new DoctorServices();
    PatientServices patServices = new PatientServices();
    int id;
    public UpdateHistorial()
    {
        this.InitializeComponent();
        InitializeDoctor();
        InitializePatient();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var select = (HistorialModel)e.Parameter;
        txtdiagnosis.Text = select.Diagnosis;
        txtnote.Text = select.Notes;
        txttreatment.Text = select.Treatment;
        combodoc.Text = select.DoctorName;
        combopac.Text = select.PatientName;
        id = select.Id;
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
        combopac.ItemsSource = item;
    }

    private async void btnupdate_Click(object sender, RoutedEventArgs e)
    {
        var doc = (DoctorModel)combodoc.SelectedItem;
        var pac = (PatientModel)combopac.SelectedItem;
        var item = new Model.HistorialModel()
        {
            Id = id,
            DoctorName = doc.Name,
            PatientName = pac.Name,
            Diagnosis = txtdiagnosis.Text,
            Notes = txtnote.Text,
            Treatment = txttreatment.Text,
            VisitDate = date.Date.ToString(),
        };
        await services.Update(item);
        Frame frame = new Frame();
        frame.Navigate(typeof(Historial));
        this.Content = frame;
    }
}
