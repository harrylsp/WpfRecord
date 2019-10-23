using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfRecord
{
    /// <summary>
    /// DatePickerControl.xaml 的交互逻辑
    /// </summary>
    public partial class DatePickerControl : UserControl
    {
        public DatePickerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置选择后的值的字体颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ksrq_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // 设置字体颜色
            var datePicker = (DatePicker)sender;
            datePicker.ApplyTemplate();
            var textBox = (TextBox)datePicker.Template.FindName("PART_TextBox", datePicker);
            textBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            textBox.Opacity = 1;
        }
    }
}
