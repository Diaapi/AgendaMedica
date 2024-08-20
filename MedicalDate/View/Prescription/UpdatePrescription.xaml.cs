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

namespace MedicalDate.View.Prescription;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class UpdatePrescription : Page
{
    PrescriptionServices services = new PrescriptionServices();
    PatientServices patientServices = new PatientServices();
    int id;
    public UpdatePrescription()
    {
        this.InitializeComponent();
        InitializePaciente();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var select = (PrescriptionModel)e.Parameter;
        txtdosage.Text = select.Dosage;
        txtinstruction.Text = select.Instructions;
        txtmedication.Text = select.Medication;
        combopac.Text = select.PatientName;
        id = select.Id;
        base.OnNavigatedTo(e);
    }

    private async void InitializePaciente()
    {
        var item = await patientServices.GetAll();
        combopac.ItemsSource = item;
    }

    private async void btnupdate_Click(object sender, RoutedEventArgs e)
    {
        var pac = (PatientModel)combopac.SelectedItem;
        var item = new Model.PrescriptionModel()
        {
            Id = id,
            Dosage = txtdosage.Text,
            Instructions = txtinstruction.Text,
            Medication = txtmedication.Text,
            PatientName = pac.Name,
        };
        await services.Update(item);
        Frame frame = new Frame();
        frame.Navigate(typeof(Prescription));
        this.Content = frame;
    }
}
