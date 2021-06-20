using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfPractice.WpfControlsUnleashed
{
    public class ProgressConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double progress = (double)values[0];
            ProgressBar progressBar = values[1] as ProgressBar;
            return 359.999 * (progress / (progressBar.Maximum - progressBar.Minimum));
            /*
            One thing that might look odd is the use of 359.9999 as the destination angle instead of
            360. This is a quirk of modulo arithmetic. When code, including WPF (but not limited to
            WPF, I’ve seen this in several animation frameworks), considers drawing from 0 to 360,
            since 360 is actually at the same location as 0, “optimizations” often decide to do no
            drawing at all. So to force the drawing to take place, we use “almost 360.” 
            */
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
