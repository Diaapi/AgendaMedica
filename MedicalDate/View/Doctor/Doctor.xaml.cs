using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.InteropServices.WindowsRuntime;
using MedicalDate.Model;
using MedicalDate.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.ApplicationModel.Contacts;
using Windows.ApplicationModel.Email;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MedicalDate.View.Doctor;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Doctor : Page
{
    DoctorServices services = new DoctorServices();
    public Doctor()
    {
        this.InitializeComponent();
        Initialize();
    }

    private async void Initialize()
    {
        var item = await services.GetAll();
        list.ItemsSource = item;
        txtsearch.ItemsSource = item;
    }

    private async void btnadd_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtspecial.Text) || !string.IsNullOrEmpty(txtemail.Text) || !string.IsNullOrEmpty(txtname.Text) || !string.IsNullOrEmpty(txtphone.Text))
        {
            var item = new DoctorModel()
            {
                Name = txtname.Text,
                Email = txtemail.Text,
                Phone = txtphone.Text,
                Specialty = txtspecial.Text,
            };
            await services.Add(item);
            Initialize();
            btnopen.Flyout.Hide();
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

    private async void btndelete_Click(object sender, RoutedEventArgs e)
    {
        var item = (MenuFlyoutItem)sender;
        var result = (DoctorModel)item.DataContext;
        await services.DeletePerson(result.Id);
        Initialize();
    }

    private async void btnshare_Click(object sender, RoutedEventArgs e)
    {
        var item = (MenuFlyoutItem)sender;
        var result = (DoctorModel)item.DataContext;
        var mailtoUri = new Uri($"whatsapp://send?phone={result.Phone}");

        // Utiliza Windows.System.Launcher para abrir el cliente de correo
        _ = await Windows.System.Launcher.LaunchUriAsync(mailtoUri);
    }

    private async void txtsearch_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (string.IsNullOrEmpty(txtsearch.Text))
        {
            Initialize();
        }
        else
        {
            var item = await services.Search(txtsearch.Text);
            list.ItemsSource = item;
        }
    }

    private async void txtsearch_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        var item = (string)args.SelectedItem;
        if (string.IsNullOrEmpty(item))
        {
            Initialize();
        }
        else
        {
            var items = await services.Search(item);
            list.ItemsSource = items;
        }
    }

    private async void btnemail_Click(object sender, RoutedEventArgs e)
    {
        var item = (MenuFlyoutItem)sender;
        var result = (DoctorModel)item.DataContext;
        var mailtoUri = new Uri($"mailto:{result.Email}");

        // Utiliza Windows.System.Launcher para abrir el cliente de correo
        _ = await Windows.System.Launcher.LaunchUriAsync(mailtoUri);
    }

    private async void btnsms_Click(object sender, RoutedEventArgs e)
    {
        var item = (MenuFlyoutItem)sender;
        var result = (DoctorModel)item.DataContext;
        var mailtoUri = new Uri($"sms:{result.Phone}");

        // Utiliza Windows.System.Launcher para abrir el cliente de correo
        _ = await Windows.System.Launcher.LaunchUriAsync(mailtoUri);
    }

    private void btnupdate_Click(object sender, RoutedEventArgs e)
    {
        var item = (MenuFlyoutItem)sender;
        var result = (DoctorModel)item.DataContext;
        Frame frame = new Frame();
        frame.Navigate(typeof(UpdateDoctor),result);
        this.Content = frame;
    }
}
