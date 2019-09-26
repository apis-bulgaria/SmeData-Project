using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace SmeData.Mobile.UITests
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp.Android.DeviceSerial("5043a0fb")// 5043a0fb | emulator-5554
                    .ApkFile(@"C:\VS PROJECTS 2015\EuProjects\SmeData\SmeData.Mobile\SmeData.Mobile.Android\bin\Release\com.ApisEurope.SmeData.Mobile.apk")
                    .EnableLocalScreenshots()
                    .StartApp();
            }
            else
            {
                return ConfigureApp.iOS.AppBundle(@"C:\VS PROJECTS 2015\EuProjects\SmeData\SmeData.Mobile\SmeData.Mobile.iOS\bin\iPhoneSimulator\Release\SmeData.Mobile.iOS.app").StartApp();
            }
        }
    }
}