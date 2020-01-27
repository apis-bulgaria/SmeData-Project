using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using SmeData.Mobile.Data;

namespace SmeData.Mobile.Droid
{
    [Activity(Label = "GDPR in Your Pocket", Icon = "@drawable/SmeDataLogo", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            //var installPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            //System.Reflection.Assembly.LoadFile(installPath + @".\bg\SmeData.Mobile.resources.dll");
            //var dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "database.sqlite");
            RequestedOrientation = ScreenOrientation.Portrait;
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            var _ = new TouchTracking.Forms.Droid.TouchEffect();

            Xamarin.Forms.DependencyService.Register<AndroidLogging>();
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, cert, chain, errors) => true;
            LoadApplication(new App());
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}