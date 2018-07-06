/*
 * 演示如何关联指定的文件类型（即用本程序打开指定类型的文件）
 * 
 * 1、在 Package.appxmanifest 中新增一个“文件类型关联”声明，并做相关配置
 *    本例中的这部分的配置如下
 *    <uap:Extension Category="windows.fileTypeAssociation">
 *      <uap:FileTypeAssociation Name=".webabcd">
 *        <uap:DisplayName>打开 .webabcd 文件</uap:DisplayName>
 *        <uap:Logo>Assets\StoreLogo.png</uap:Logo>
 *        <uap:InfoTip>用 win10 demo 打开 .webabcd 文件</uap:InfoTip>
 *        <uap:SupportedFileTypes>
 *          <uap:FileType>.webabcd</uap:FileType>
 *        </uap:SupportedFileTypes>
 *      </uap:FileTypeAssociation>
 *    </uap:Extension>
 * 2、在 App.xaml.cs 中 override void OnFileActivated(FileActivatedEventArgs args)，以获取相关的文件信息
 * 
 * FileActivatedEventArgs - 通过打开文件激活应用程序时的事件参数
 *     Files - 相关的文件信息
 *     Kind - 此 app 被激活的类型（ActivationKind 枚举）
 *         比如，如果是通过“打开文件”激活的话，则此值为 File
 *     PreviousExecutionState - 此 app 被激活前的状态（ApplicationExecutionState 枚举）
 *         比如，如果此 app 被激活前就是运行状态的或，则此值为 Running
 *     NeighboringFilesQuery - 获取当前文件的相邻文件
 *     SplashScreen - 获取此 app 的 SplashScreen 对象
 *     User - 获取激活了此 app 的 User 对象
 */

using System;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Windows10.AssociationLaunching
{
    public sealed partial class FileTypeAssociation : Page
    {
        private FileActivatedEventArgs _fileActivated;

        public FileTypeAssociation()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 获取 FileActivatedEventArgs 对象（从 App.xaml.cs 传来的）
            _fileActivated = e.Parameter as FileActivatedEventArgs;

            // 获取文件中的文本内容，并显示
            if (_fileActivated != null)
            {
                grid.Background = new SolidColorBrush(Colors.Blue);
                lblMsg.Foreground = new SolidColorBrush(Colors.White);

                IStorageFile isf = _fileActivated.Files[0] as IStorageFile;
                lblMsg.Text = $"激活程序的文件是“{isf.Name}”，其文本内容为：{await FileIO.ReadTextAsync(isf)}";
            }
        }
    }
}
