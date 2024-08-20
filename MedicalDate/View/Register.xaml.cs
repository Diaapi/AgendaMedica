using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Firebase.Auth;
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

namespace MedicalDate.View;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Register : Page
{
    public Register()
    {
        this.InitializeComponent();
    }

    private async void btnregister_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtemail.Text) || !string.IsNullOrEmpty(txtpass.Password) || !string.IsNullOrEmpty(txtuser.Text))
        {
            //Debe de agregar la Clave de API web de firebase donde dice apikey
            var authprovider = new FirebaseAuthProvider(new FirebaseConfig("apikey"));
            try
            {
                var auth = await authprovider.CreateUserWithEmailAndPasswordAsync(txtemail.Text, txtpass.Password, txtuser.Text);
                ContentDialog message = new()
                {
                    Title = "Alerta",
                    XamlRoot = XamlRoot,
                    Content = "Se creo correctamente",
                    PrimaryButtonText = "OK"
                };
                var result = await message.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    Frame frame = new Frame();
                    frame.Navigate(typeof(MainPage));
                    this.Content = frame;
                }
            }
            catch (Exception ex)
            {
                ContentDialog message = new()
                {
                    Title = "Alerta",
                    XamlRoot = XamlRoot,
                    Content = ex.Message,
                    PrimaryButtonText = "OK"
                };
                await message.ShowAsync();
            }
        }
        else
        {
            ContentDialog message = new()
            {
                Title = "Alerta",
                XamlRoot = XamlRoot,
                Content = "Se requiere completar los campos",
                PrimaryButtonText = "OK"
            };
            await message.ShowAsync();
        }
    }

    private void btnback_Click(object sender, RoutedEventArgs e)
    {
        Frame frame = new Frame();
        frame.GoBack();
        this.Content = frame;
    }
}
