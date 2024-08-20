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

namespace MedicalDate.View.Doctor;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class UpdateDoctor : Page
{
    DoctorServices services = new DoctorServices();
    int id;
    public UpdateDoctor()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var select = (DoctorModel)e.Parameter;
        txtemail.Text = select.Email;
        txtname.Text = select.Name;
        txtphone.Text = select.Phone;
        txtspecial.Text = select.Specialty;
        id = select.Id;
        base.OnNavigatedTo(e);
    }

    private async void btnupdate_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtspecial.Text) || !string.IsNullOrEmpty(txtemail.Text) || !string.IsNullOrEmpty(txtname.Text) || !string.IsNullOrEmpty(txtphone.Text))
        {
            var item = new DoctorModel()
            {
                Id = id,
                Name = txtname.Text,
                Email = txtemail.Text,
                Phone = txtphone.Text,
                Specialty = txtspecial.Text,
            };
            await services.Update(item);
            Frame frame = new Frame();
            frame.Navigate(typeof(Doctor));
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
