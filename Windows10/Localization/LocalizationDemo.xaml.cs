/*
 * 演示本地化的基本应用
 * 
 * 
 * 注：建议使用多语言应用工具包 https://developer.microsoft.com/zh-cn/windows/develop/multilingual-app-toolkit
 */

using System;
using System.Resources;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.Localization
{
    public sealed partial class LocalizationDemo : Page
    {
        public LocalizationDemo()
        {
            this.InitializeComponent(); 
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            /*
             * ResourceLoader resourceLoader = ResourceLoader.GetForViewIndependentUse(); - 获取默认的 ResourceLoader（Resources.resw 中的资源）
             * ResourceLoader resourceLoader = ResourceLoader.GetForViewIndependentUse("MyResources"); - 获取指定的 ResourceLoader（MyResources.resw 中的资源）
             * ResourceLoader resourceLoader = ResourceLoader.GetForViewIndependentUse("ClassLibrary/MyResources"); - 获取指定类库的指定的 ResourceLoader（ClassLibrary 类库中的 MyResources.resw 中的资源）
             * resourceLoader.GetString(), resourceLoader.GetStringForUri() - 通过资源标识，获取当前语言环境的指定的资源
             * 
             * GetForCurrentView() 和 GetForViewIndependentUse() 的区别如下：
             * 1、GetForCurrentView() - 在 UI 线程上执行
             * 2、GetForViewIndependentUse() - 在非 UI 线程上执行（注：Independent 这个词在 uwp 中就时非 UI 线程的意思，比如 Independent Animation 就是不依赖 UI 线程的）
             */

            // 获取默认的 ResourceLoader（即 Resources.resw 中的资源）
            ResourceLoader resourceLoader = ResourceLoader.GetForViewIndependentUse();

            // 通过资源标识，获取当前语言环境的指定的资源（资源名：Hello）
            lblMsg1.Text = resourceLoader.GetString("Hello");

            // 通过资源标识，获取当前语言环境的指定的资源（资源名：HelloTextBlock.Text）
            lblMsg2.Text = resourceLoader.GetString("HelloTextBlock/Text");

            // 通过资源标识，获取当前语言环境的指定的资源（资源名：Hello）
            lblMsg3.Text = resourceLoader.GetStringForUri(new Uri("ms-resource:///Resources/Hello"));

            // 通过资源标识，获取当前语言环境的指定的资源（资源名：HelloTextBlock.Text）
            lblMsg4.Text = resourceLoader.GetStringForUri(new Uri("ms-resource:///Resources/HelloTextBlock/Text"));

            // 获取当前语言环境的指定的资源的另一种方式
            lblMsg5.Text = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap.GetValue("Resources/Hello", ResourceContext.GetForCurrentView()).ValueAsString;
        }
    }

    // 用于演示如何绑定本地化资源
    public class LocalizedStrings
    {
        public string this[string key]
        {
            get
            {
                return ResourceLoader.GetForCurrentView().GetString(key);
            }
        }
    }
}
