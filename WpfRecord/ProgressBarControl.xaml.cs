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
    /// ProgressBar.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressBarControl : UserControl
    {


        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private string _Value;

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(ProgressBarControl), new PropertyMetadata(-1, new PropertyChangedCallback(OnValuetChanged)));

        /// <summary>
        /// 代理定义，可以在Invoke时传入相应的参数
        /// </summary>
        /// <param name="nValue">参数</param> 
        private delegate void LoadDataHandle(int nValue);
        /// <summary>
        /// 加载数据处理器
        /// </summary>
        private LoadDataHandle myHandle = null;

        /// <summary>
        /// 进度控制线程
        /// </summary>
        System.Threading.Thread thread = null;

        public ProgressBarControl()
        {
            InitializeComponent();

            thread = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadFun));
            //thread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void OnValuetChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ((ProgressBarControl)sender).OnChanged(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void OnChanged(DependencyPropertyChangedEventArgs e)
        {
            _Value = e.NewValue.ToString();

            if (Convert.ToInt32(_Value) >= 0)
            {
                thread.Start();
            }
        }

        /// <summary>
        /// 线程函数，用于处理调用
        /// </summary>
        private void ThreadFun()
        {
            try
            {
                System.Windows.Forms.MethodInvoker mi = new System.Windows.Forms.MethodInvoker(ShowProgressBar);
                this.Dispatcher.BeginInvoke(mi);
                while (this.myHandle == null)
                {
                    System.Threading.Thread.Sleep(1000); //sleep to show window                    
                }

                for (int i = 0; i < 100; ++i)
                {
                    System.Threading.Thread.Sleep(100);
                    // 这里直接调用代理
                    this.Dispatcher.Invoke(this.myHandle, new object[] { i });
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ShowProgressBar()
        {
            try
            {
                myHandle = new LoadDataHandle(this.SetProgressValue);
            }
            catch (Exception ex)
            {
            }
        }

        private void SetProgressValue(int value)
        {
            try
            {
                this.proBar.Value = value;
                this.txtPercent.Text = value.ToString() + "%";

                // 99%
                if (value == this.proBar.Maximum - 1)
                {
                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}
