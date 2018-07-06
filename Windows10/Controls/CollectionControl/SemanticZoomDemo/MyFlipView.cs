/*
 * 开发一个实现了 ISemanticZoomInformation 接口的自定义 FlipView
 * 由于本例仅用于 SemanticZoom 中 ZoomedInView 的演示，所以并没有实现 ISemanticZoomInformation 的全部逻辑
 * 两个 ISemanticZoomInformation 对象之间交互的核心逻辑就是通过 SemanticZoomLocation source 获取数据，通过 SemanticZoomLocation destination 设置数据
 * 
 * 
 * ISemanticZoomInformation - 用于 SemanticZoom 的 ZoomedInView 和 ZoomedOutView（说明详见本例中的注释）
 * SemanticZoomLocation - 用于设置或获取 ISemanticZoomInformation 在 SemanticZoom 中的状态（说明详见本例中的注释）
 */

using Windows.UI.Xaml.Controls;
using Windows10.Common;

namespace Windows10.Controls.CollectionControl.SemanticZoomDemo
{
    public class MyFlipView : FlipView, ISemanticZoomInformation
    {
        public MyFlipView()
            : base()
        {

        }

        /// <summary>
        /// 视图完成了变化后调用，比如视图完成了显示或隐藏之后都会调用这个（与 InitializeViewChange() 是一对）
        /// </summary>
        public void CompleteViewChange() { }

        /// <summary>
        /// 完成 ZoomedInView -> ZoomedOutView 的切换时调用（与 StartViewChangeFrom() 是一对）
        /// </summary>
        public void CompleteViewChangeFrom(SemanticZoomLocation source, SemanticZoomLocation destination) { }

        /// <summary>
        /// 完成 ZoomedOutView -> ZoomedInView 的切换时调用（与 StartViewChangeTo() 是一对）
        /// </summary>
        public void CompleteViewChangeTo(SemanticZoomLocation source, SemanticZoomLocation destination) { }

        /// <summary>
        /// 视图将要发生变化时调用，比如视图将要被显示或将要被隐藏之前都会先调用这个（与 CompleteViewChange() 是一对）
        /// </summary>
        public void InitializeViewChange() { }

        /// <summary>
        /// 是否为活动视图
        /// </summary>
        public bool IsActiveView { get; set; }

        /// <summary>
        /// 是否为 ZoomedInView 视图
        /// </summary>
        public bool IsZoomedInView { get; set; }

        /// <summary>
        /// 所属的 SemanticZoom
        /// </summary>
        public SemanticZoom SemanticZoomOwner { get; set; }

        /// <summary>
        /// 开始 ZoomedInView -> ZoomedOutView 的切换时调用（与 CompleteViewChangeFrom() 是一对）
        /// </summary>
        public void StartViewChangeFrom(SemanticZoomLocation source, SemanticZoomLocation destination) { }

        /// <summary>
        /// 开始 ZoomedOutView -> ZoomedInView 的切换时调用（与 CompleteViewChangeTo() 是一对）
        /// </summary>
        /// <param name="source">在 ZoomedOutView 时被选中的数据</param>
        /// <param name="destination">需要传递给 ZoomedInView 的数据</param>
        public void StartViewChangeTo(SemanticZoomLocation source, SemanticZoomLocation destination)
        {
            /*
             * 注：
             * 1、ListViewBase 实现了 ISemanticZoomInformation 接口，其通过绑定 CollectionViewSource 数据即可使 SemanticZoom 中的两个视图进行有关联地切换，参见 /Controls/CollectionControl/SemanticZoomDemo/SemanticZoomDemo.xaml
             * 2、对于 ListViewBase 来说，它运行到这里时，通过 source.Item 获取到的是一个 ICollectionViewGroup 类型的数据，其有两个属性：Group 和 GroupItems
             */


            // 获取在 ZoomedOutView 中被选中的项，即被选中的父亲
            NavigationModel model = source.Item as NavigationModel;

            // 将此父亲的所有子数据传递给 ZoomedInView，接下来会执行 MakeVisible() 方法
            destination.Item = model.Items;
        }

        /// <summary>
        /// 开始 ZoomedOutView -> ZoomedInView 之后，会调用此方法
        /// 一般在此处重整 ZoomedInView 的数据源，或者滚动 ZoomedInView 中的内容到指定的项以对应 ZoomedOutView 中被选中的数据
        /// </summary>
        /// <param name="item">由 StartViewChangeTo() 方法传递给 ZoomedInView 的数据</param>
        public void MakeVisible(SemanticZoomLocation item)
        {
            // 将 FlipView 的数据源指定为被选中的父亲的所有子数据
            this.ItemsSource = item.Item;
        }
    }
}