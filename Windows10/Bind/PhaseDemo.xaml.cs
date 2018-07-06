/*
 * 演示 x:Bind 绑定之 x:Phase 的相关知识点
 */

using System.Collections.ObjectModel;
using System.Threading;
using Windows.UI.Xaml.Controls;
using Windows10.Common;

namespace Windows10.Bind
{
    public sealed partial class PhaseDemo : Page
    {
        public PhaseDemo()
        {
            this.InitializeComponent();
        }

        // 用于人为减慢每阶段的显示速度，以便演示
        private void gridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            AutoResetEvent h = new AutoResetEvent(false);
            h.WaitOne(1);

            uint phase = args.Phase;
            if (phase < 10)
                args.RegisterUpdateCallback(gridView_ContainerContentChanging);
        }

        // 数据源
        public ObservableCollection<Employee> AllEmployees { get; set; } = TestData.GetEmployees(1000);
    }
}
