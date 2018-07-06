/*
 * 演示与“改变语言”相关的一些应用
 * 
 * 1、演示如何改变当前的语言环境
 * 2、演示如何监测当前语言环境发生的变化
 * 3、演示如何获取指定语言环境下的资源
 */

using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Windows.Globalization;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Localization
{
    public sealed partial class Language : Page
    {
        public Language()
        {
            this.InitializeComponent();

            this.Loaded += Language_Loaded;
        }

        void Language_Loaded(object sender, RoutedEventArgs e)
        {
            // 获取当前的语言
            string currentLanguage;
            ResourceContext.GetForCurrentView().QualifierValues.TryGetValue("Language", out currentLanguage);
            lblMsg.Text = "current language: " + currentLanguage;
            lblMsg.Text += Environment.NewLine;


            // ApplicationLanguages.ManifestLanguages - 遍历 Package.appxmanifest 中的语言列表
            foreach (string strLang in ApplicationLanguages.ManifestLanguages)
            {
                // 关于 Language 的说明详见 GlobalizationDemo.xaml
                var lang = new Windows.Globalization.Language(strLang);
                cmbLanguage.Items.Add(string.Format("DisplayName:{0}, NativeName:{1}, LanguageTag:{2}, Script:{3}",
                    lang.DisplayName, lang.NativeName, lang.LanguageTag, lang.Script));
            }
            cmbLanguage.SelectionChanged += cmbLanguage_SelectionChanged;


            // 获取当前语言环境的指定资源（更多用法请参见 LocalizationDemo.xaml）
            lblMsg.Text += ResourceLoader.GetForViewIndependentUse().GetString("Hello");


            // 当前语言环境发生改变时所触发的事件（通过 API 更改或者通过“电脑设置 -> 常规 -> 语言首选项”更改都会触发此事件）
            // 注：当通过 API（ApplicationLanguages.PrimaryLanguageOverride）修改语言环境时，如果监听了 MapChanged 事件的话，则有很大的几率会导致崩溃，本例就是这样，原因未知
            ResourceContext.GetForCurrentView().QualifierValues.MapChanged += async (s, m) =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    lblMsg.Text += Environment.NewLine;
                    lblMsg.Text += ResourceLoader.GetForViewIndependentUse().GetString("Hello");
                });
            };
        }

        private void cmbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ApplicationLanguages.PrimaryLanguageOverride - 设置或获取首选语言（BCP-47 语言标记）
            if (cmbLanguage.SelectedValue.ToString().ToLower().Contains("en-us"))
                ApplicationLanguages.PrimaryLanguageOverride = "en-US";
            else if (cmbLanguage.SelectedValue.ToString().ToLower().Contains("zh-hans"))
                ApplicationLanguages.PrimaryLanguageOverride = "zh-Hans-CN";

            StringBuilder sb = new StringBuilder();
            // ApplicationLanguages.Languages - 按语言级别排序，获取语言列表
            foreach (string item in ApplicationLanguages.Languages)
            {
                sb.Append(item);
                sb.Append(",");
            }

            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "ApplicationLanguages.Languages: " + sb.ToString();
        }

        private void btnGetEnglish_Click(object sender, RoutedEventArgs e)
        {
            // 指定 ResourceContext 为 en-US 语言环境
            ResourceContext resourceContext = new ResourceContext();
            resourceContext.Languages = new List<string>() { "en-US" };

            // 获取 en-US 语言环境下的 Resources 映射
            ResourceMap resourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");
            lblMsg.Text += Environment.NewLine;
            // 获取指定的语言环境下的指定标识的资源
            lblMsg.Text += "英语的 Hello: " + resourceMap.GetValue("Hello", resourceContext).ValueAsString;
        }

        private void btnGetChinese_Click(object sender, RoutedEventArgs e)
        {
            // 指定 ResourceContext 为 zh-Hans-CN 语言环境
            ResourceContext resourceContext = new ResourceContext();
            resourceContext.Languages = new List<string>() { "zh-Hans-CN" };

            // 获取 zh-Hans 语言环境下的 Resources 映射
            ResourceMap resourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");
            lblMsg.Text += Environment.NewLine;
            // 获取指定的语言环境下的指定标识的资源
            lblMsg.Text += "简体中文的 Hello: " + resourceMap.GetValue("Hello", resourceContext).ValueAsString;
        }
    }
}
