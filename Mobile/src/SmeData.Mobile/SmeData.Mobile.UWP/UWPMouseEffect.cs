using SmeData.Mobile.Effects;
using SmeData.Mobile.UWP;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ResolutionGroupName("Microsoft")]
[assembly: ExportEffect(typeof(UWPMouseEffect), nameof(MouseOverEffect))]
namespace SmeData.Mobile.UWP
{
    public class UWPMouseEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var control = Control ?? Container;

            if (control is ToolbarItem)
            {
                control.PointerEntered += Control_PointerEntered;
            }
        }

        private void Control_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var action = MouseOverEffect.GetMouseOver(Element);
            action();
        }

        protected override void OnDetached()
        {
        }
    }
}
