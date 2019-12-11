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
    /// ButtonControl.xaml 的交互逻辑
    /// </summary>
    public partial class ButtonControl : UserControl
    {
        public ButtonControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 方法
        /// </summary>
        /// <param name="sender">参数1</param>
        /// <param name="e">参数2</param>
        private void Popup_Opened(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 方法
        /// </summary>
        /// <param name="sender">参数1</param>
        /// <param name="e">参数2</param>
        private void Popup_Closed(object sender, EventArgs e)
        {
        }

        private void ToggleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.Popup.IsOpen)
            {
                this.Popup.IsOpen = false;
            }
            else
            {
                this.Popup.IsOpen = true;
            }
        }

        /// <summary>
        /// 安装指南
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InstallGuide_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.baidu.com");
            e.Handled = true;
        }
    }
}
