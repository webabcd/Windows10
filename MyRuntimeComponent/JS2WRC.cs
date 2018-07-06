/*
 * 用于演示 javascript 调用 windows runtime component 中定义的方法
 */

using Windows.Foundation.Metadata;

namespace MyRuntimeComponent
{
    [AllowForWeb] // 允许 js 调用
    public sealed class JS2WRC // 注意：在 winrc 中用 c# 写的类必须是 sealed 的
    {
        public string hello(string name)
        {
            return $"hello: {name}";
        }
    }
}
