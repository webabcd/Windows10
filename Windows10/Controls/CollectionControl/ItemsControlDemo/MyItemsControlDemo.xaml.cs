/*
 * ItemsControl - 集合控件（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 *     protected virtual void PrepareContainerForItemOverride(DependencyObject element, object item); - 为 item 准备 container 时
 *         element - item 的 container
 *         item - item
 *         
 * 
 * 本例用于演示如何使 GirdView 中的每个 item 占用不同大小的空间
 * 1、布局控件要使用 VariableSizedWrapGrid（利用其 RowSpan 和 ColumnSpan 来实现 item 占用不同大小的空间），需要注意的是其并非是虚拟化布局控件
 * 2、自定义 GridView，并重写 ItemsControl 的 protected override void PrepareContainerForItemOverride(DependencyObject element, object item) 方法
 *    然后设置每个 item 的 VariableSizedWrapGrid.RowSpan 和 VariableSizedWrapGrid.ColumnSpan
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Reflection;

namespace Windows10.Controls.CollectionControl.ItemsControlDemo
{
    public sealed partial class MyItemsControlDemo : Page
    {
        public MyItemsControlDemo()
        {
            this.InitializeComponent();
   
            BindData();
        }

        private void BindData()
        {
            Random random = new Random();

            // 获取 Windows.UI.Colors 的全部数据
            Type type = typeof(Colors);
            List<ColorModel> colors = type.GetRuntimeProperties() // GetRuntimeProperties() 在 System.Reflection 命名空间下
                .Select(c => new ColorModel
                {
                    ColorName = c.Name,
                    ColorValue = new SolidColorBrush((Color)c.GetValue(null)),
                    ColSpan = random.Next(1, 3), // 此对象所占网格的列合并数
                    RowSpan = random.Next(1, 3) // 此对象所占网格的行合并数
                })
                .ToList();

            // 绑定数据
            gridView.ItemsSource = colors;
        }
    }


    /// <summary>
    /// 用于数据绑定的对象
    /// </summary>
    public class ColorModel
    {
        public string ColorName { get; set; }
        public SolidColorBrush ColorValue { get; set; }

        // 此对象所占的网格的列合并数
        public int ColSpan { get; set; }
        // 此对象所占的网格的行合并数
        public int RowSpan { get; set; }
    }


    /// <summary>
    /// 自定义 GridView，重写 ItemsControl 的 protected override void PrepareContainerForItemOverride(DependencyObject element, object item) 方法
    /// 用于指定 GridView 的每个 item 所占网格的列合并数和行合并数
    /// </summary>
    public class MyGridView : GridView
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            try
            {
                // 设置每个 item 的 VariableSizedWrapGrid.RowSpan 和 VariableSizedWrapGrid.ColumnSpan, 从而实现每个 item 占用不同大小的空间
                // 仅为演示用，由于这里的 ColSpan 和 RowSpan 都是随机计算的，所以可能会出现空白空间

                dynamic dynamicItem = item;
                element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, dynamicItem.ColSpan);
                element.SetValue(VariableSizedWrapGrid.RowSpanProperty, dynamicItem.RowSpan);
            }
            catch (Exception ex)
            {
                var ignore = ex;

                // 当有异常情况发生时（比如：item 没有 ColSpan 属性或 RowSpan 属性）

                element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 1);
                element.SetValue(VariableSizedWrapGrid.RowSpanProperty, 1);
            }
            finally
            {
                base.PrepareContainerForItemOverride(element, item);
            }
        }
    }
}
