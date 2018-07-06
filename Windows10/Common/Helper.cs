using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Windows10.Common
{
    public class Helper
    {
        /// <summary>
        /// 获取指定元素所占用的矩形区域，此矩形的位置相对于 app 原点
        /// </summary>
        public static Rect GetElementRect(FrameworkElement element)
        {
            GeneralTransform generalTransform = element.TransformToVisual(null);
            Point point = generalTransform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

        /// <summary>
        /// 获取指定元素内部的指定类型的 DependencyObject
        /// </summary>
        public static T GetVisualChild<T>(DependencyObject parent) 
            where T : DependencyObject
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                DependencyObject obj = (DependencyObject)VisualTreeHelper.GetChild(parent, i);
                child = obj as T;
                if (child == null)
                    child = GetVisualChild<T>(obj);
                if (child != null)
                    break;
            }
            return child;
        }


        /// <summary>
        /// 获取指定元素内部的指定名称的 FrameworkElement
        /// </summary>
        public static T GetVisualChild<T>(DependencyObject parent, string name)
            where T : FrameworkElement
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                DependencyObject obj = VisualTreeHelper.GetChild(parent, i);
                child = obj as T;

                if (child == null || child.Name != name)
                    child = GetVisualChild<T>(obj, name);
                if (child != null)
                    break;
            }
            return child;
        }
    }
}
