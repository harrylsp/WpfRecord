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
    /// WebCalendarControl.xaml 的交互逻辑
    /// </summary>
    public partial class WebCalendarControl : UserControl
    {
        /// <summary>
        /// 当前月
        /// </summary>
        public string CurrentMonthTag = "CurrentMonth";

        /// <summary>
        /// 日历按钮 Name
        /// </summary>
        public string ButtonIdTag = "day";

        /// <summary>
        /// 日历按钮个数
        /// </summary>
        public int ButtonCount = 42;

        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime Now = DateTime.Now;

        /// <summary>
        /// 选择的时间
        /// </summary>
        public DateTime SelectTime = DateTime.Now;

        public WebCalendarControl()
        {
            InitializeComponent();

            // 初始今天时间
            InitTime();

            // 初始化数据
            InitDayData();
        }

        public void InitDayData()
        {
            // 清除样式
            ClearStyle();

            this.tbCurrentMonth.Text = SelectTime.ToString("yyyy-MM");

            // 当前月第一天
            var firstDay = Convert.ToDateTime(SelectTime.ToString("yyyy-MM-01"));
            // 当前月最后一天
            var lastDay = firstDay.AddMonths(1).AddDays(-1);
            // 获取当前月第一天是星期几
            int firstDayOfWeek = (int)firstDay.DayOfWeek;

            // 应显示当前月第一天之前的几天
            for (int i = 0; i < firstDayOfWeek; i++)
            {
                var btn = FindName(ButtonIdTag + i) as Button;
                btn.Content = firstDay.AddDays(i - firstDayOfWeek).Day.ToString();
                btn.Visibility = Visibility.Visible;
                btn.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF999999"));
            }

            // 当前月
            for (int i = 0; i < lastDay.Day; i++)
            {
                var btn = FindName(ButtonIdTag + (firstDayOfWeek + i)) as Button;
                btn.Content = (i + 1).ToString();
                btn.Tag = CurrentMonthTag;

                // 当天按钮显示样式
                if (Now.Year == SelectTime.Year && Now.Month == SelectTime.Month && Now.Day == (i + 1))
                {
                    btn.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFC6DDF3"));
                    btn.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF0C6BCD"));
                }

                btn.Visibility = Visibility.Visible;
            }

            // 下个月
            var rowCount = (firstDayOfWeek + lastDay.Day) / 7;
            if ((firstDayOfWeek + lastDay.Day) % 7 > 0)
            {
                rowCount += 1;
            }

            for (int i = 0; i < (ButtonCount - firstDayOfWeek - lastDay.Day); i++)
            {
                var btnIndex = firstDayOfWeek + lastDay.Day + i;

                var btn = FindName(ButtonIdTag + btnIndex) as Button;
                btn.Content = (i + 1).ToString();
                btn.Visibility = (rowCount <= 5 && btnIndex >= 35) ? Visibility.Hidden : Visibility.Visible;
                btn.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#999999"));
            }
        }

        /// <summary>
        /// 获取今天的时间
        /// </summary>
        public void InitTime()
        {
            Now = DateTime.Now;
            SelectTime = Now;
        }

        /// <summary>
        /// 还原初始样式
        /// </summary>
        public void ClearStyle()
        {
            for (int i = 0; i < ButtonCount; i++)
            {
                var btn = FindName(ButtonIdTag + i) as Button;

                btn.Background = new SolidColorBrush(Colors.White);
                btn.Foreground = new SolidColorBrush(Colors.Black);
                //if (btn.Tag?.ToString() == CurrentMonthTag)
                //{
                //    btn.Background = new SolidColorBrush(Colors.White);
                //    btn.Foreground = new SolidColorBrush(Colors.Black);
                //}

                // 置空Tag
                btn.Tag = "";
            }
        }

        /// <summary>
        /// 日历选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Day_Click(object sender, RoutedEventArgs e)
        {
            var clickBtn = sender as Button;

            if (clickBtn.Tag?.ToString() == CurrentMonthTag)
            {
                // 取消之前的点击效果
                for (int i = 0; i < ButtonCount; i++)
                {
                    var btn = FindName(ButtonIdTag + i) as Button;

                    // 非本月
                    if (btn.Tag?.ToString() != CurrentMonthTag)
                    {
                        continue;
                    }

                    // 非当天
                    if (btn.Content.ToString() == DateTime.Now.Day.ToString())
                    {
                        continue;
                    }

                    btn.Background = new SolidColorBrush(Colors.White);
                    btn.Foreground = new SolidColorBrush(Colors.Black);
                }

                // 点击非当天
                if (clickBtn.Content.ToString() != DateTime.Now.Day.ToString())
                {
                    clickBtn.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF0C6BCD"));
                    clickBtn.Foreground = new SolidColorBrush(Colors.White);
                }
            }
        }

        /// <summary>
        /// 上一年
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgPreYear_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.SelectTime = SelectTime.AddYears(-1);

            // 重新加载日历
            InitDayData();
        }

        /// <summary>
        /// 上一月
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgPreMonth_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.SelectTime = SelectTime.AddMonths(-1);

            // 重新加载日历
            InitDayData();
        }

        /// <summary>
        /// 下一月
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgNextMonth_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.SelectTime = SelectTime.AddMonths(1);

            // 重新加载日历
            InitDayData();
        }

        /// <summary>
        /// 下一年
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgNextYear_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.SelectTime = SelectTime.AddYears(1);

            // 重新加载日历
            InitDayData();
        }

        /// <summary>
        /// 定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Location_Click(object sender, RoutedEventArgs e)
        {
            this.Popup.IsOpen = true;

        }
        
        private void Area0_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Popup.IsOpen = false;

            var tb = sender as TextBlock;
            this.Location.Content = tb.Text;
        }
    }
}
