using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Windows.Foundation;
using Windows.Foundation.Collections;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MedicalDate.View.MedicalRecord;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Historial : Page
{
    HistorialServices services = new HistorialServices();
    DoctorServices doctorServices = new DoctorServices();
    PatientServices patServices = new PatientServices();
    public Historial()
    {
        this.InitializeComponent();
        Initialize();
        InitializeDoctor();
        InitializePatient();
    }

    private async void Initialize()
    {
        var item = await services.GetAll();
        list.ItemsSource = item;
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

    private async void btnadd_Click(object sender, RoutedEventArgs e)
    {
        var doc = (DoctorModel)combodoc.SelectedItem;
        var pac = (PatientModel)combopac.SelectedItem;
        var item = new Model.HistorialModel()
        {
            DoctorName = doc.Name,
            PatientName = pac.Name,
            Diagnosis = txtdiagnosis.Text,
            Notes = txtnote.Text,
            Treatment = txttreatment.Text,
            VisitDate = date.Date.ToString(),
        };
        await services.Add(item);
        Initialize();
        btnopen.Flyout.Hide();
    }

    private async void btndelete_Click(object sender, RoutedEventArgs e)
    {
        var item = (MenuFlyoutItem)sender;
        var result = (HistorialModel)item.DataContext;
        await services.DeletePerson(result.Id);
        Initialize();
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

    private void btnupdate_Click(object sender, RoutedEventArgs e)
    {
        var item = (MenuFlyoutItem)sender;
        var result = (HistorialModel)item.DataContext;
        Frame frame = new Frame();
        frame.Navigate(typeof(UpdateHistorial), result);
        this.Content = frame;
    }
}
