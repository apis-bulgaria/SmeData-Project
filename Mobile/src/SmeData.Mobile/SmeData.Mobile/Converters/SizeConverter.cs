using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SmeData.Mobile.Converters
{
    public class SizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string[] parts = ((string)value).Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 1)
                {
                    return int.Parse(parts[0]);
                }

                string pointerSource = parts[0];
                double incomingFontSize = double.Parse(parts[1]);
                int screenWidth = int.Parse(parts[2]);

                double outgoingFontSize = incomingFontSize;

                if (pointerSource == "i")
                {
                    if (screenWidth > 2400)
                    {
                        outgoingFontSize *= 2.6;
                    }
                    else if (screenWidth > 2000)
                    {
                        outgoingFontSize *= 2.3;
                    }
                    else if (screenWidth > 1440)
                    {
                        outgoingFontSize *= 2;
                    }
                    else if (screenWidth > 1242)
                    {
                        outgoingFontSize *= 1.5;
                    }
                    else if (screenWidth > 1080)
                    {
                        outgoingFontSize *= 1.3;
                    }
                    else if (screenWidth < 800)
                    {
                        outgoingFontSize *= 0.8;
                    }
                }
                else if (pointerSource == "t")
                {
                    if (screenWidth > 2400)
                    {
                        outgoingFontSize += 10;
                    }
                    else if (screenWidth > 2000)
                    {
                        outgoingFontSize += 8;
                    }
                    else if (screenWidth > 1440)
                    {
                        outgoingFontSize += 6;
                    }
                    else if (screenWidth > 1242)
                    {
                        outgoingFontSize += 4;
                    }
                    else if (screenWidth > 1080)
                    {
                        outgoingFontSize += 2;
                    }
                    else if (screenWidth < 800)
                    {
                        outgoingFontSize -= 3;
                    }
                }

                return (int)Math.Round(outgoingFontSize);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GET Error {ex.Message}");
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
