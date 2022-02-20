using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpeedIt.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Speed : ContentPage
    {

        Color colorNormalSeed = (Color)App.Current.Resources["colorNormalSpeed"];
        Color colorNormalHighSpeed = (Color)App.Current.Resources["colorNormalHighSpeed"];
        Color colorMediumSpeed = (Color)App.Current.Resources["colorMediumSpeed"];
        Color colorFastSpeed = (Color)App.Current.Resources["colorFastSpeed"];
        Color colorToFastSpeed = (Color)App.Current.Resources["colorToFastSpeed"];
        

        public Speed()
        {
            InitializeComponent();

           
        }
        protected async override void OnAppearing()
        {
            try
            {
                var localizador = CrossGeolocator.Current;
                if (!localizador.IsListening)
                {
                    await localizador.StartListeningAsync(TimeSpan.FromMilliseconds(5), 0, true);
                }
                
                if (CrossGeolocator.IsSupported)
                {

                    localizador.DesiredAccuracy = 15;
                    localizador.PositionChanged += (obj, arg) =>
                    {
                        var position = arg.Position;
                        var speedkmh = Math.Round((position.Speed * 3.6), 0);
                        ColorizeText(speedkmh);
                        speed.Text = speedkmh < 10 ? $"0{speedkmh}" : $"{speedkmh}";
                    };
                }
                else
                {
                    await localizador.StopListeningAsync();
                    await DisplayAlert("Error :(", "Este dispositivo no cuenta con los requisitos para correr este programa.", "Ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error :(", $"Ha ocurrido un error al intentar dar seguimiento de velocidad.{ex.Message}", "Ok");
            }
            

            //CounterProof();


        }

        void CounterProof() {
            double counter = 0;
            Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
            {
                ColorizeText(counter);
                speed.Text = $"{counter}";
                counter+= 0.1;
                return true;
            });
        }

        void ColorizeText(double speedVal) {
            if (speedVal < 41) {
                speed.TextColor = colorNormalSeed;
                speedType.TextColor = colorNormalSeed;
            }else if (speedVal < 61)
            {
                speed.TextColor = colorNormalHighSpeed;
                speedType.TextColor = colorNormalHighSpeed;
            }
            else if (speedVal < 81)
            {
                speed.TextColor = colorMediumSpeed;
                speedType.TextColor = colorMediumSpeed;
            }
            else if (speedVal < 100)
            {
                speed.TextColor = colorFastSpeed;
                speedType.TextColor = colorFastSpeed;
            }
            else if (speedVal >= 100)
            {
                speed.TextColor = colorToFastSpeed;
                speedType.TextColor = colorToFastSpeed;
            }
        }
    }
}