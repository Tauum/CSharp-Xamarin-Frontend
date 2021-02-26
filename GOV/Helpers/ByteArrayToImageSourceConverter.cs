using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace GOV.Helpers
{
    public class ByteArrayToImageSourceConverter : IValueConverter //this is uses to convert binary
    {
        #region IValueConverter implementation
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null) // null check
            {
                byte[] byteStream = (byte[])value; //generates a byte stream locally
                ImageSource retImageSource = ImageSource.FromStream(() => new MemoryStream(byteStream));
                return retImageSource;
            }
            if (parameter != null)// null check
            {
                string fillerIcon = (string)parameter;
                ImageSource retImageSource = ImageSource.FromFile(fillerIcon);
                return retImageSource;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

