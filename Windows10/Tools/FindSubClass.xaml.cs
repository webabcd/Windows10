/*
 * 用于查找指定类或接口的所在程序集的所有子类和子接口
 */

using System;
using System.Reflection;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Linq;

namespace Windows10.Tools
{
    public sealed partial class FindSubClass : Page
    {
        public FindSubClass()
        {
            this.InitializeComponent();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            // 这样不行
            // Type type = Type.GetType("Windows.UI.Xaml.Controls.Button");

            Type type = typeof(Windows.UI.Xaml.UIElement);

            List<Type> subTypes = GetSubTypes(type);
            AddWrapGrid(subTypes);
        }

        private void AddWrapGrid(List<Type> subTypes)
        {
            VariableSizedWrapGrid wrapGrid = CreateWrapGrid();
            root.Children.Add(wrapGrid);

            foreach (Type type in subTypes)
            {
                Button button = CreateButton(type);
                wrapGrid.Children.Add(button);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = sender as FrameworkElement;
            Type type = button.Tag as Type;

            int index = 0;
            // 删除被选中按钮的所属容器，以及此容器之后的所有控件
            if (button.Parent.GetType() == typeof(VariableSizedWrapGrid))
                index = root.Children.IndexOf(button.Parent as UIElement);
            // 删除被选中按钮，以及此按钮之后的所有控件
            else
                index = root.Children.IndexOf(button);
            while (root.Children.Count > index)
            {
                root.Children.RemoveAt(root.Children.Count - 1);
            }

            // 将被选中按钮添加到根容器
            Button buttonNew = CreateButton(type);
            root.Children.Add(buttonNew);
            root.Children.Add(new Grid() { Height = 20 });

            // 将被选中类的所有子类添加到根容器
            List<Type> subTypes = GetSubTypes(type);
            AddWrapGrid(subTypes);
        }

        private VariableSizedWrapGrid CreateWrapGrid()
        {
            VariableSizedWrapGrid wrapGrid = new VariableSizedWrapGrid();
            wrapGrid.Orientation = Orientation.Vertical;
            wrapGrid.ItemWidth = 9999;
            wrapGrid.HorizontalAlignment = HorizontalAlignment.Stretch;

            return wrapGrid;
        }

        private Button CreateButton(Type type)
        {
            Button button = new Button();
            button.Content = type.ToString();
            button.Margin = new Thickness(1);
            button.Tag = type;
            button.Click += Button_Click;

            return button;
        }

        // 获取儿子类，孙子及以下级别不会返回
        private List<Type> GetSubTypes(Type type)
        {
            List<Type> subTypes = new List<Type>();

            Type[] assemblyTypes = type.GetTypeInfo().Assembly.GetTypes();

            foreach (Type t in assemblyTypes)
            {
                if (type.GetTypeInfo().IsInterface)
                {
                    if (t.GetInterfaces().Contains(type))
                    {
                        subTypes.Add(t);
                    }
                }
                else 
                {
                    if (t.GetTypeInfo().BaseType == type)
                    {
                        subTypes.Add(t);
                    }
                }
            }

            subTypes = subTypes.OrderBy(p => p.FullName).ToList();

            return subTypes;
        }
    }
}
