using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfRecord.ControlInherit
{
    public class DatePickerDateFormat
    {
        /// <summary>
        /// 控件显示格式 入 yyyyMMdd 或者 yyyy-MM-dd HH:mm:ss等
        /// </summary>
        public static readonly DependencyProperty DateFormatProperty = DependencyProperty.RegisterAttached("DateFormat", typeof(string), typeof(DatePickerDateFormat), new PropertyMetadata(OnDateFormatChanged));
        /// <summary>
        /// documentation header
        /// </summary>
        /// <param name="dobj">documentation</param>
        /// <returns>documentation111</returns>
        public static string GetDateFormat(DependencyObject dobj)
        {
            return (string)dobj.GetValue(DateFormatProperty);
        }

        /// <summary>
        /// documentation header
        /// </summary>
        /// <param name="dobj">documentation</param>
        /// <param name="value">documentation1</param>
        public static void SetDateFormat(DependencyObject dobj, string value)
        {
            dobj.SetValue(DateFormatProperty, value);
        }
        /// <summary>
        /// 方法
        /// </summary>
        /// <param name="dobj">参数1</param>
        /// <param name="e">参数2</param>
        private static void OnDateFormatChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            var datePicker = (DatePicker)dobj;

            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Loaded, new Action<DatePicker>(ApplyDateFormat), datePicker);
        }

        /// <summary>
        /// 方法
        /// </summary>
        /// <param name="datePicker">参数</param>
        private static void ApplyDateFormat(DatePicker datePicker)
        {
            var binding = new Binding("SelectedDate")
            {
                RelativeSource = new RelativeSource { AncestorType = typeof(DatePicker) },
                Converter = new DatePickerDateTimeConverter(),
                ConverterParameter = new Tuple<DatePicker, string>(datePicker, GetDateFormat(datePicker))
            };
            var textBox = GetTemplateTextBox(datePicker);
            textBox.SetBinding(TextBox.TextProperty, binding);

            textBox.PreviewKeyDown -= TextBoxOnPreviewKeyDown;
            textBox.PreviewKeyDown += TextBoxOnPreviewKeyDown;

            datePicker.CalendarOpened -= DatePickerOnCalendarOpened;
            datePicker.CalendarOpened += DatePickerOnCalendarOpened;
        }

        /// <summary>
        /// 方法
        /// </summary>
        /// <param name="control">参数</param>
        /// <returns>返回值</returns>
        private static TextBox GetTemplateTextBox(Control control)
        {
            control.ApplyTemplate();
            return (TextBox)control.Template.FindName("PART_TextBox", control);
        }

        /// <summary>
        /// 方法
        /// </summary>
        /// <param name="sender">参数1</param>
        /// <param name="e">参数2</param>
        private static void TextBoxOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
                return;

            /* DatePicker subscribes to its TextBox's KeyDown event to set its SelectedDate if Key.Return was
             * pressed. When this happens its text will be the result of its internal date parsing until it
             * loses focus or another date is selected. A workaround is to stop the KeyDown event bubbling up
             * and handling setting the DatePicker.SelectedDate. */

            e.Handled = true;

            var textBox = (TextBox)sender;
            var datePicker = (DatePicker)textBox.TemplatedParent;
            var dateStr = textBox.Text;
            var formatStr = GetDateFormat(datePicker);
            datePicker.SelectedDate = DatePickerDateTimeConverter.StringToDateTime(datePicker, formatStr, dateStr);
        }
        /// <summary>
        /// 方法
        /// </summary>
        /// <param name="sender">参数1</param>
        /// <param name="e">参数2</param>
        private static void DatePickerOnCalendarOpened(object sender, RoutedEventArgs e)
        {
            /* When DatePicker's TextBox is not focused and its Calendar is opened by clicking its calendar button
             * its text will be the result of its internal date parsing until its TextBox is focused and another
             * date is selected. A workaround is to set this string when it is opened. */

            var datePicker = (DatePicker)sender;
            var textBox = GetTemplateTextBox(datePicker);
            var formatStr = GetDateFormat(datePicker);
            textBox.Text = DatePickerDateTimeConverter.DateTimeToString(formatStr, datePicker.SelectedDate);
        }

        /// <summary>
        /// 方法
        /// </summary>
        private class DatePickerDateTimeConverter : IValueConverter
        {
            /// <summary>
            /// 方法
            /// </summary>
            /// <param name="value">参数1</param>
            /// <param name="targetType">参数2</param>
            /// <param name="parameter">参数3</param>
            /// <param name="culture">参数4</param>
            /// <returns>返回值</returns>
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var formatStr = ((Tuple<DatePicker, string>)parameter).Item2;
                var selectedDate = (DateTime?)value;
                return DateTimeToString(formatStr, selectedDate);
            }
            /// <summary>
            /// 方法
            /// </summary>
            /// <param name="value">参数1</param>
            /// <param name="targetType">参数2</param>
            /// <param name="parameter">参数3</param>
            /// <param name="culture">参数4</param>
            /// <returns>返回值</returns>
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var tupleParam = (Tuple<DatePicker, string>)parameter;
                var dateStr = (string)value;
                return StringToDateTime(tupleParam.Item1, tupleParam.Item2, dateStr);
            }
            /// <summary>
            /// 方法
            /// </summary>
            /// <param name="formatStr">参数1</param>
            /// <param name="selectedDate">参数2</param>
            /// <returns>返回值</returns>
            public static string DateTimeToString(string formatStr, DateTime? selectedDate)
            {
                return selectedDate.HasValue ? selectedDate.Value.ToString(formatStr) : null;
            }
            /// <summary>
            /// 方法
            /// </summary>
            /// <param name="datePicker">参数1</param>
            /// <param name="formatStr">参数2</param>
            /// <param name="dateStr">参数3</param>
            /// <returns>返回值</returns>
            public static DateTime? StringToDateTime(DatePicker datePicker, string formatStr, string dateStr)
            {
                DateTime date;
                var canParse = DateTime.TryParseExact(dateStr, formatStr, CultureInfo.CurrentCulture, DateTimeStyles.None, out date);

                if (!canParse)
                    canParse = DateTime.TryParse(dateStr, CultureInfo.CurrentCulture, DateTimeStyles.None, out date);

                return canParse ? date : datePicker.SelectedDate;
            }
        }
    }
}
