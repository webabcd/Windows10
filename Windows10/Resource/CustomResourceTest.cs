/*
 * 本例是一个自定义 CustomXamlResourceLoader，用于演示 CustomResource 的使用
 */

using Windows.UI.Xaml.Resources;

namespace Windows10.Resource
{
    // 如果要在 xaml 中使用 CustomResource，那么需要在 C# 端自定义一个 CustomXamlResourceLoader
    public class CustomResourceTest : CustomXamlResourceLoader
    {
        /// <summary>
        /// 返回 xaml 中的 CustomResource 请求的资源
        /// </summary>
        /// <param name="resourceId">xaml 端的 CustomResource 中的 ResourceKey</param>
        /// <param name="objectType">使用了 CustomResource 的对象类型</param>
        /// <param name="propertyName">使用了 CustomResource 的属性名称</param>
        /// <param name="propertyType">使用了 CustomResource 的属性类型</param>
        /// <returns>返回指定的资源</returns>
        protected override object GetResource(string resourceId, string objectType, string propertyName, string propertyType)
        {
            return $"resourceId: {resourceId}, objectType: {objectType}, propertyName: {propertyName}, propertyType: {propertyType}";
        }
    }
}