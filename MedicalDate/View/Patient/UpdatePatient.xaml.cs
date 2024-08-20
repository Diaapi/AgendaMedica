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

namespace MedicalDate.View.Patient;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class UpdatePatient : Page
{
    PatientServices services = new PatientServices();
    int id;
    public UpdatePatient()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var select = (PatientModel)e.Parameter;
        txtaddress.Text = select.Address;
        txtname.Text = select.Name;
        txtphone.Text = select.Phone;
        txtemail.Text = select.Email;
        date.Date = Convert.ToDateTime(select.Dateofbirth);
        id = select.Id;
        base.OnNavigatedTo(e);
    }

    private async void btnupdate_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtaddress.Text) || !string.IsNullOrEmpty(txtemail.Text) || !string.IsNullOrEmpty(txtname.Text) || !string.IsNullOrEmpty(txtphone.Text))
        {
            var item = new Model.PatientModel()
            {
                Id = id,
                Name = txtname.Text,
                Email = txtemail.Text,
                Phone = txtphone.Text,
                Address = txtaddress.Text,
                Dateofbirth = date.Date.ToString()
            };
            await services.Update(item);
            Frame frame = new Frame();
            frame.Navigate(typeof(Patient));
            this.Content = frame;
        }
        else
        {
            ContentDialog dialog = new()
            {
                XamlRoot = XamlRoot,
                Title = "Alerta",
                Content = "Rellene los campos",
                PrimaryButtonText = "OK"
            };

            await dialog.ShowAsync();
        }
    }
}
