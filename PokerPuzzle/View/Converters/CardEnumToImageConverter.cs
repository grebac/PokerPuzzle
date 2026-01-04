using PokerPuzzleData.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace PokerPuzzle.View.Converters
{
    public class CardEnumToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not CardsEnum card)
                return "";

            var uriString = CardHelper.ToImagePath(card);
            if (string.IsNullOrWhiteSpace(uriString))
                return "";

            return new BitmapImage(new Uri(uriString, UriKind.Absolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
