using PM2E11542.Models;
using System.IO;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Path = System.IO.Path;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using PM2E11542.Controllers;

namespace PM2E11542.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageMaps : ContentPage
    {

        Sitios Sitios = null;
        public PageMaps(Sitios sitios)
        {
            InitializeComponent();
            Sitios = sitios;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {

                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();


                if (status == PermissionStatus.Granted)
                {

                    var localizacion = await Geolocation.GetLocationAsync();

                    if (localizacion != null)
                    {

                        var pin = new Pin()
                        {
                            Type = PinType.SavedPin,
                            Position = new Position(Sitios.latitud, Sitios.longitud),
                            Label = "Descripcion",
                            Address = Sitios.descripcion

                        };


                        mapa.Pins.Add(pin);

                        //mapa.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(localizacion.Latitude, localizacion.Longitude), Distance.FromMeters(100)));


                        mapa.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Sitios.latitud, Sitios.longitud), Distance.FromMeters(100)));
                    }

                }
                else
                {
                    await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }
            }
            catch (Exception e)
            {
                if (e.Message.Equals("Location services are not enabled on device."))
                {

                    Message("Error", "Servicio de localizacion no encendido");
                }
                else
                {
                    Message("Error", e.Message);

                }
            }

        }

        private async void Message(string title, string message)
        {
            await DisplayAlert(title, message, "OK");
        }

        private void btnAdd_Clicked(object sender, EventArgs e)
        {

        }

        private async void bbtnCompartir_Clicke(object sender, EventArgs e)
        {
            if (Sitios.image == null || Sitios.pathImage== null)
            {
                await DisplayAlert("Advertencia", "Debes seleccionar una ubicacion con foto", "Ok");
            }
            else
            {
                try
                {
                    await Share.RequestAsync(new ShareFileRequest
                    {
                        Title = Sitios.descripcion,
                        File = new ShareFile(Sitios.pathImage)
                    });
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Advertencia", "Se produjo un error al compartir la fotografia", "Ok");
                }

            }
        }


    }
}