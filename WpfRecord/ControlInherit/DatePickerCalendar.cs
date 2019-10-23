using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace WpfRecord.ControlInherit
{
    /// <summary>
    /// 显示年月
    /// </summary>
    public class DatePickerCalendar
    {
        /// <summary>
        /// 显示年月
        /// </summary>
        public static readonly DependencyProperty IsMonthYearProperty =
            DependencyProperty.RegisterAttached("IsMonthYear", typeof(bool), typeof(DatePickerCalendar), new PropertyMetadata(OnIsMonthYearChanged));
        /// <summary>
        /// documentation header
        /// </summary>
        /// <param name="dobj">documentation</param>
        /// <returns>documentation111</returns>
        public static bool GetIsMonthYear(DependencyObject dobj)
        {
            return (bool)dobj.GetValue(IsMonthYearProperty);
        }
        /// <summary>
        /// documentation header
        /// </summary>
        /// <param name="dobj">documentation</param>
        /// <param name="value">documentation1</param>
        public static void SetIsMonthYear(DependencyObject dobj, bool value)
        {
            dobj.SetValue(IsMonthYearProperty, value);
        }

        /// <summary>
        /// 方法
        /// </summary>
        /// <param name="dobj">参数1</param>
        /// <param name="e">参数2</param>
        private static void OnIsMonthYearChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            var datePicker = (DatePicker)dobj;

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action<DatePicker, DependencyPropertyChangedEventArgs>(SetCalendarEventHandlers), datePicker, e);
        }
        /// <summary>
        /// 方法
        /// </summary>
        /// <param name="datePicker">参数1</param>
        /// <param name="e">参数2</param>
        private static void SetCalendarEventHandlers(DatePicker datePicker, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;

            if ((bool)e.NewValue)
            {
                datePicker.CalendarOpened += DatePickerOnCalendarOpened;
                datePicker.CalendarClosed += DatePickerOnCalendarClosed;
            }
            else
            {
                datePicker.CalendarOpened -= DatePickerOnCalendarOpened;
                datePicker.CalendarClosed -= DatePickerOnCalendarClosed;
            }
        }
        /// <summary>
        /// 方法
        /// </summary>
        /// <param name="sender">参数1</param>
        /// <param name="routedEventArgs">参数2</param>
        private static void DatePickerOnCalendarOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            var calendar = GetDatePickerCalendar(sender);
            calendar.DisplayMode = CalendarMode.Year;

            calendar.DisplayModeChanged += CalendarOnDisplayModeChanged;
        }
        /// <summary>
        /// 方法
        /// </summary>
        /// <param name="sender">参数1</param>
        /// <param name="routedEventArgs">参数2</param>
        private static void DatePickerOnCalendarClosed(object sender, RoutedEventArgs routedEventArgs)
        {
            var datePicker = (DatePicker)sender;
            var calendar = GetDatePickerCalendar(sender);
            datePicker.SelectedDate = calendar.SelectedDate;

            calendar.DisplayModeChanged -= CalendarOnDisplayModeChanged;
        }
        /// <summary>
        /// 方法
        /// </summary>
        /// <param name="sender">参数1</param>
        /// <param name="e">参数2</param>
        private static void CalendarOnDisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            var calendar = (Calendar)sender;
            if (calendar.DisplayMode != CalendarMode.Month)
                return;

            calendar.SelectedDate = GetSelectedCalendarDate(calendar.DisplayDate);

            var datePicker = GetCalendarsDatePicker(calendar);
            datePicker.IsDropDownOpen = false;
        }
        /// <summary>
        /// 方法
        /// </summary>
        /// <param name="sender">参数1</param>
        /// <returns>返回值</returns>
        private static Calendar GetDatePickerCalendar(object sender)
        {
            var datePicker = (DatePicker)sender;
            var popup = (Popup)datePicker.Template.FindName("PART_Popup", datePicker);
            return (Calendar)popup.Child;
        }

        /// <summary>
        /// 方法
        /// </summary>
        /// <param name="child">canshu </param>
        /// <returns>返回值</returns>
        private static DatePicker GetCalendarsDatePicker(FrameworkElement child)
        {
            var parent = (FrameworkElement)child.Parent;
            if (parent.Name == "PART_Root")
                return (DatePicker)parent.TemplatedParent;
            return GetCalendarsDatePicker(parent);
        }
        /// <summary>
        /// 方法
        /// </summary>
        /// <param name="selectedDate">参数</param>
        /// <returns>返回值</returns>
        private static DateTime? GetSelectedCalendarDate(DateTime? selectedDate)
        {
            if (!selectedDate.HasValue)
                return null;
            return new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, 1);
        }
    }
}
