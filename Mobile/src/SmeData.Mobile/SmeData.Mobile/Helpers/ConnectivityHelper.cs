using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmeData.Mobile.Helpers
{
    public static class ConnectivityHelper
    {
        public static async Task<bool> CheckInternetConection(IPageDialogService dialogService)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(() => {
                    dialogService.DisplayAlertAsync(Translator.GetString("Warning"), Translator.GetString("No Internet Access"), Translator.GetString("Ok"));
                });

                return false;
            }
            return true;
        }

        public static async Task<bool> CheckForWifiConnection(IPageDialogService dialogService)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await dialogService.DisplayAlertAsync(Translator.GetString("Warning"), Translator.GetString("No Internet Access"), Translator.GetString("Ok"));
                return false;
            }
            else
            {
                var profiles = Connectivity.ConnectionProfiles;
                if (!profiles.Contains(ConnectionProfile.WiFi))
                {
                    await dialogService.DisplayAlertAsync(Translator.GetString("Warning"), Translator.GetString("No Wifi Access"), Translator.GetString("Ok"));
                    return false;
                }
            }

            return true;
        }

        public static bool IsWifiConnected()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return false;
            }
            else
            {
                var profiles = Connectivity.ConnectionProfiles;
                if (!profiles.Contains(ConnectionProfile.WiFi))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
