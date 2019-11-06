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
    /// ListBoxControl.xaml 的交互逻辑
    /// </summary>
    public partial class ListBoxControl : UserControl
    {
        public ListBoxControl()
        {
            InitializeComponent();

            List<M> list = new List<M>
            {
                new M{ A = "01日-31日", B="我是标题", C="这是一个还有梦想的小伙子这是一个还有梦想的小伙子这是一个还有梦想的小伙子这是一个还有梦想的小伙子这是一个还有梦想的小伙子"},
                new M{ A = "01日-31日", B="我是标题", C="这是一个还有梦想的小伙子这是一个还有梦想的小伙子这是一个还有梦想的小伙子这是一个还有梦想的小伙子这是一个还有梦想的小伙子"},
                new M{ A = "01日-31日", B="我是标题", C="这是一个还有梦想的小伙子这是一个还有梦想的小伙子这是一个还有梦想的小伙子这是一个还有梦想的小伙子这是一个还有梦想的小伙子"}
            };

            var dt = Convert.ToDateTime("2019-11-1");

            var ss = "";

            //this.listBox.ItemsSource = list;
        }
    }

    public class M
    {
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
    }
}
