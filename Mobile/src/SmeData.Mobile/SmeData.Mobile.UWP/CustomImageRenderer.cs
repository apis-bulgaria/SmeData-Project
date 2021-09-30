using SmeData.Mobile.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

//[assembly: ExportRenderer(typeof(Image), typeof(CustomImageRenderer))]
namespace SmeData.Mobile.UWP
{    
    public class CustomImageRenderer : ImageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            if (e.NewElement != null)
            {
                var source = e.NewElement.Source;
                if (source is FileImageSource fileImageSource)
                {
                    fileImageSource.File = $"Assets/{fileImageSource.File}";
                }
            }

            base.OnElementChanged(e);
        }
    }
}
