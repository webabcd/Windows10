/*
 * FlipView - 翻转视图控件（继承自 Selector, 请参见 /Controls/SelectionControl/SelectorDemo.xaml）
 */

using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows10.Common;

namespace Windows10.Controls.CollectionControl
{
    public sealed partial class FlipViewDemo : Page
    {
        public FlipViewDemo()
        {
            this.InitializeComponent();
        }

        //  FlipView 和 ListBox 的数据源
        public FlipViewDataSource Data { get; set; } = new FlipViewDataSource();
    }


    // 用于提供数据
    public sealed class FlipViewDataSource : BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public FlipViewDataSource(String title, String picture)
        {
            this._title = title;
            this._picture = picture;
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private Uri _image = null;
        private String _picture = null;
        public Uri Image
        {
            get
            {
                return new Uri(_baseUri, this._picture);
            }

            set
            {
                this._picture = null;
                this.SetProperty(ref this._image, value);
            }
        }

        private ObservableCollection<object> _items = new ObservableCollection<object>();
        public ObservableCollection<object> Items
        {
            get { return this._items; }
        }

        public FlipViewDataSource()
        {
            Items.Add(new FlipViewDataSource("aaa", "Assets/StoreLogo.png"));
            Items.Add(new FlipViewDataSource("bbb", "Assets/StoreLogo.png"));
            Items.Add(new FlipViewDataSource("ccc", "Assets/StoreLogo.png"));
            Items.Add(new FlipViewDataSource("ddd", "Assets/StoreLogo.png"));
            Items.Add(new FlipViewDataSource("eee", "Assets/StoreLogo.png"));
        }
    }
}