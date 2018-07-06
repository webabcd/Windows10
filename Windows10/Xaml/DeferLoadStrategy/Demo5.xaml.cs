/*
 * 演示 x:DeferLoadStrategy 的相关知识点
 *
 * 本例演示通过“GetTemplateChild”加载延迟加载元素
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Windows10.Xaml.DeferLoadStrategy
{
    public sealed partial class Demo5 : Page
    {
        public Demo5()
        {
            this.InitializeComponent();
        }
    }



    // 自定义控件（一个可显示 Title 的 Image）
    public class TitledImage : Control
    {
        // 此自定义控件用于显示 Title 的 ContentPresenter
        private ContentPresenter _titlePresenter;

        public TitledImage()
        {
            // 使用 TargetType 为 TitledImage 的样式，即 <Style xmlns:local="using:TitledImage" TargetType="local:TitledImage" />
            this.DefaultStyleKey = typeof(TitledImage);

            // 注册一个回调，当 Title 发生变化时触发
            this.RegisterPropertyChangedCallback(TitleProperty, TitleChanged);
        }

        // 定义一个依赖属性 Title（用于显示标题）
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(TitledImage), new PropertyMetadata(null));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // 定义一个依赖属性 Source（用于显示图片）
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(TitledImage), new PropertyMetadata(null));
        public ImageSource Source
        {
            get { return (BitmapSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        void TitleChanged(DependencyObject sender, DependencyProperty prop)
        {
            string title = (string)sender.GetValue(prop);
            
            if (!string.IsNullOrEmpty(Title) && _titlePresenter == null)
            {
                // TitlePresenter 是控件模板中的一个延迟加载元素，通过 GetTemplateChild 调用后，它将被加载
                _titlePresenter = (ContentPresenter)GetTemplateChild("TitlePresenter"); 
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (!string.IsNullOrEmpty(Title))
            {
                // TitlePresenter 是控件模板中的一个延迟加载元素，通过 GetTemplateChild 调用后，它将被加载
                _titlePresenter = (ContentPresenter)GetTemplateChild("TitlePresenter");
            }
        }
    }
}
