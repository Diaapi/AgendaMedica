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
using Newtonsoft.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MedicalDate.View;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Login : Page
{
    public Login()
    {
        this.InitializeComponent();
    }

    private async void btnlogin_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtemail.Text) || !string.IsNullOrEmpty(txtpass.Password))
        {
            //Debe de agregar la Clave de API web de firebase donde dice apikey
            var authprovider = new FirebaseAuthProvider(new FirebaseConfig("apikey"));
            try
            {
                var auth = await authprovider.SignInWithEmailAndPasswordAsync(txtemail.Text, txtpass.Password);
                var refresh = await auth.GetFreshAuthAsync();
                var serialized = JsonConvert.SerializeObject(refresh);
                ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["Token"] = serialized;
                Frame frame = new Frame();
                frame.Navigate(typeof(MainPage));
                this.Content = frame;
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

    private void btnforgot_Click(object sender, RoutedEventArgs e)
    {
        Frame frame = new Frame();
        frame.Navigate(typeof(ForgotPage));
        this.Content = frame;
    }

    private void btnregister_Click(object sender, RoutedEventArgs e)
    {
        Frame frame = new Frame();
        frame.Navigate(typeof(Register));
        this.Content = frame;
    }
}
