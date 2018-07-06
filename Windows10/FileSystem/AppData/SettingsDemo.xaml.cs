/*
 * 演示如何在 Application Data（应用程序数据存储）中对“设置”进行操作
 * 
 * ApplicationData - 操作 Application Data 中数据的类
 *     Current - 返回当前的 ApplicationData 对象
 *     LocalSettings - 返回 ApplicationDataContainer 对象。本地存储，永久保存
 *         保存路径：%USERPROFILE%\AppData\Local\Packages\{PackageId}\Settings
 *     RoamingSettings - 返回 ApplicationDataContainer 对象。漫游存储，同一微软账户同一应用程序在不同设备间会自动同步
 *         保存路径：%USERPROFILE%\AppData\Local\Packages\{PackageId}\Settings
 *     Version - 获取当前 Application Data 的版本号，默认值为 0，只读（用于本地“设置”数据的版本控制）
 *     SetVersionAsync() - 指定当前 Application Data 的版本号（用于本地“设置”数据的版本控制）
 * 
 * ApplicationDataContainer - 操作“设置”数据的类
 *     Name - 容器的名称，默认为空
 *     CreateContainer(string name, ApplicationDataCreateDisposition disposition) - 激活一个用于保存“设置”数据的容器，即分组“设置”数据
 *         name - 容器的名称
 *         disposition - 容器的激活方式：Always - 始终激活；Existing - 容器存在才激活
 *     Containers - 容器集合
 *     DeleteContainer() - 删除指定的容器
 *     Values - 保存“设置”数据，一个字典表
 *         其数据可以是一个 ApplicationDataCompositeValue 类型的数据，ApplicationDataCompositeValue 也是一个字典表，这样可将多个“设置”数据放到一个 key 里
 *       
 * 
 * 备注：
 * 当 key 为 HighPriority 时，系统会以最快的速度在多个设备间同步 HighPriority 所对应的数据（支持 ApplicationDataCompositeValue 数据）
 * 示例如下：
 * ApplicationDataContainer.Values["HighPriority"] = "此处的值将会以系统最快的速度在多个设备间同步";
 */

using System;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.FileSystem.AppData
{
    public sealed partial class SettingsDemo : Page
    {
        public SettingsDemo()
        {
            this.InitializeComponent();
        }

        // 操作“设置”数据的 demo
        private void btnReadWrite_Click(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            // 保存“设置”数据
            localSettings.Values["key"] = "I am webabcd";

            // 如果 key 为 HighPriority，则系统将会以最高优先级的速度在多个设备间同步 HighPriority 的值
            // localSettings.Values["HighPriority"] = "I am webabcd";

            // 删除指定的“设置”数据
            // localSettings.Values.Remove("key");

            // 获取“设置”数据
            lblMsg.Text = (string)localSettings.Values["key"];
        }

        // 分组“设置”数据，即在不同的容器中保存不同的数据
        private void btnReadWriteWithContainer_Click(object sender, RoutedEventArgs e)
        {
            // 在 LocalSettings 中激活名为 groupName 的容器
            ApplicationDataContainer container = ApplicationData.Current.LocalSettings;
            ApplicationDataContainer localSettings = container.CreateContainer("groupName", ApplicationDataCreateDisposition.Always);

            // 删除指定的容器
            // container.DeleteContainer("groupName");

            // 在容器内保存“设置”数据
            if (container.Containers.ContainsKey("groupName"))
            {
                container.Containers["groupName"].Values["key"] = "I am webabcd";
            }

            // 从指定的容器内获取“设置”数据
            lblMsg.Text = (string)container.Containers["groupName"].Values["key"];
            lblMsg.Text += Environment.NewLine;
            // 从指定的容器内获取“设置”数据
            lblMsg.Text += (string)localSettings.Values["key"];
        }

        // 父子“设置”数据，即 key 中的数据是一个 ApplicationDataCompositeValue 对象，而 ApplicationDataCompositeValue 也是一个字典表
        private void btnReadWriteWithComposite_Click(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            // 父子“设置”数据的保存
            ApplicationDataCompositeValue parent1 = new ApplicationDataCompositeValue();
            parent1["child1"] = "abc";
            parent1["child2"] = "xyz";
            localSettings.Values["parent1"] = parent1;

            // 父子“设置”数据的获取
            lblMsg.Text = (string)((ApplicationDataCompositeValue)localSettings.Values["parent1"])["child1"];
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += (string)((ApplicationDataCompositeValue)localSettings.Values["parent1"])["child2"];
        }


        private async void btnSetVersion0_Click(object sender, RoutedEventArgs e)
        {
            // 将 Application Data 的版本号设置为 0，并执行指定的方法
            await ApplicationData.Current.SetVersionAsync(0, new ApplicationDataSetVersionHandler(SetVersionHandler));
        }

        private async void btnSetVersion1_Click(object sender, RoutedEventArgs e)
        {
            // 将 Application Data 的版本号设置为 1，并执行指定的方法
            await ApplicationData.Current.SetVersionAsync(1, new ApplicationDataSetVersionHandler(SetVersionHandler));
        }

        // 根据当前版本号和将要设置成的版本号，将“设置”数据升级到最新版本
        private async void SetVersionHandler(SetVersionRequest request)
        {
            // 异步操作
            SetVersionDeferral deferral = request.GetDeferral();

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                lblMsg.Text = "CurrentVersion: " + request.CurrentVersion; // 当前版本号
                lblMsg.Text += Environment.NewLine;
                lblMsg.Text += "DesiredVersion: " + request.DesiredVersion; // 将要设置成的版本号
                lblMsg.Text += Environment.NewLine;
                lblMsg.Text += "ApplicationData.Current.Version: " + ApplicationData.Current.Version; // 当前版本号
            });

            // 完成异步操作
            deferral.Complete();
        }
    }
}
