/*
 * 演示如何通过 Windows.System.Profile 命名空间下的类获取信息
 *
 * 主要可获取到设备类型，系统版本号等
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Windows.System.Profile;

namespace Windows10.Information
{
    public sealed partial class ProfileInfo : Page
    {
        public ProfileInfo()
        {
            this.InitializeComponent();

            this.Loaded += ProfileInfo_Loaded;
        }

        private void ProfileInfo_Loaded(object sender, RoutedEventArgs e)
        {
            // 获取设备类型，目前已知的返回字符串有：Windows.Mobile, Windows.Desktop, Windows.Xbox
            lblMsg.Text = string.Format("DeviceFamily: {0}", AnalyticsInfo.VersionInfo.DeviceFamily);
            lblMsg.Text += Environment.NewLine;

            // 获取系统版本号，一个长整型值
            lblMsg.Text += string.Format("DeviceFamilyVersion: {0}", AnalyticsInfo.VersionInfo.DeviceFamilyVersion);
            lblMsg.Text += Environment.NewLine;

            // 将长整型的系统版本号转换为 major.minor.revision.build 的方式
            string versionString = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong version = ulong.Parse(versionString);
            ulong v1 = (version & 0xFFFF000000000000L) >> 48;
            ulong v2 = (version & 0x0000FFFF00000000L) >> 32;
            ulong v3 = (version & 0x00000000FFFF0000L) >> 16;
            ulong v4 = (version & 0x000000000000FFFFL);
            string v = $"{v1}.{v2}.{v3}.{v4}";

            lblMsg.Text += string.Format("DeviceFamilyVersion(major.minor.revision.build): {0}", v);
            lblMsg.Text += Environment.NewLine;



            // 获取当前的“向 Microsoft 发送你的设备数据”的收集等级。在“设置”->“隐私”->“反馈和诊断”中配置（Security, Basic, Enhanced, Full）
            lblMsg.Text += string.Format("PlatformDiagnosticsAndUsageDataSettings.CollectionLevel: {0}", PlatformDiagnosticsAndUsageDataSettings.CollectionLevel);
            lblMsg.Text += Environment.NewLine;

            // 检查当前配置是否允许指定级别的信息收集
            lblMsg.Text += string.Format("PlatformDataCollectionLevel.Full: {0}", PlatformDiagnosticsAndUsageDataSettings.CanCollectDiagnostics(PlatformDataCollectionLevel.Full));
            lblMsg.Text += Environment.NewLine;

            // 在“设置”->“隐私”->“反馈和诊断”中配置的“向 Microsoft 发送你的设备数据”发生变化时触发的事件
            PlatformDiagnosticsAndUsageDataSettings.CollectionLevelChanged += PlatformDiagnosticsAndUsageDataSettings_CollectionLevelChanged;
        }

        private async void PlatformDiagnosticsAndUsageDataSettings_CollectionLevelChanged(object sender, object e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                lblMsg.Text += string.Format("PlatformDiagnosticsAndUsageDataSettings.CollectionLevel: {0}", PlatformDiagnosticsAndUsageDataSettings.CollectionLevel);
                lblMsg.Text += Environment.NewLine;

                lblMsg.Text += string.Format("PlatformDataCollectionLevel.Full: {0}", PlatformDiagnosticsAndUsageDataSettings.CanCollectDiagnostics(PlatformDataCollectionLevel.Full));
                lblMsg.Text += Environment.NewLine;
            });
        }
    }
}
