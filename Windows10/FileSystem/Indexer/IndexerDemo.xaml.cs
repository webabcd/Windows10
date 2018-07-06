/*
 * 演示如何管理索引器，以及如何通过索引器获取数据
 * 
 * ContentIndexer - 索引器管理类
 */

using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.FileSystem.Indexer
{
    public sealed partial class IndexerDemo : Page
    {
        public IndexerDemo()
        {
            this.InitializeComponent();
        }

        // 添加数据到索引器
        private async void btnAddToIndexer_Click(object sender, RoutedEventArgs e)
        {
            // 获取一个索引器（可以指定索引器的名字，从而达到对索引器分类的目的）
            var indexer = ContentIndexer.GetIndexer();
            var content = new IndexableContent();
            for (int i = 0; i < 100; i++)
            {
                content.Properties[SystemProperties.Title] = "Title: " + i.ToString().PadLeft(2, '0');
                content.Properties[SystemProperties.Keywords] = "Keywords: " + i.ToString().PadLeft(2, '0'); // 多个用“;”隔开
                content.Properties[SystemProperties.Comment] = "Comment: " + i.ToString().PadLeft(2, '0');
                content.Id = "key" + i; // 标识，增加同标识的索引就是更新

                // 增加一个索引（另外还有 Update 和 Delete 操作）
                await indexer.AddAsync(content);
            }
        }

        // 获取索引器中的全部数据
        private void btnRetrieveAllItems_Click(object sender, RoutedEventArgs e)
        {
            ExecuteQueryHelper("*");
        }

        // 按指定的查询条件获取索引器中的数据
        private void btnRetrieveMatchingItems_Click(object sender, RoutedEventArgs e)
        {
            ExecuteQueryHelper("title:\"99\"");
        }


        // 按指定的 AQS 语法从索引器中查询数据
        private async void ExecuteQueryHelper(string queryString)
        {
            lblMsg.Text = "";
            var indexer = ContentIndexer.GetIndexer();

            // 需要检索的属性
            string[] propertyKeys =
            {
                SystemProperties.Title,
                SystemProperties.Keywords,
                SystemProperties.Comment
            };
            // 指定 AQS 字符串（Advanced Query Syntax），参见 http://msdn.microsoft.com/zh-cn/library/windows/apps/aa965711.aspx
            var query = indexer.CreateQuery(queryString, propertyKeys);

            // 执行查询，并获取结果
            var documents = await query.GetAsync();
            foreach (var document in documents)
            {
                string itemString = "Key: " + document.Id + "\n";
                foreach (var propertyKey in propertyKeys)
                {
                    itemString += propertyKey + ": " + StringifyProperty(document.Properties[propertyKey]) + "\n";
                }
                lblMsg.Text += itemString + "\n";
            }
        }

        // 如果对象是一个字符串集合则用“;”做分隔符，然后以字符串形式输出
        public string StringifyProperty(object property)
        {
            string propertyString = "";
            if (property != null)
            {
                var vectorProperty = property as IEnumerable<string>;
                if (vectorProperty != null)
                {
                    foreach (var prop in vectorProperty)
                    {
                        propertyString += prop + "; ";
                    }
                }
                else
                {
                    propertyString = property.ToString();
                }
            }
            return propertyString;
        }
    }
}
