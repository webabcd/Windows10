dgml - 定向关系图文档（Directed Graph Markup Language）

StateTriggerBase.dgml - 显示了 StateTriggerBase 的所有派生类（新建 dgml 文件后，从对象浏览器中把 StateTriggerBase 拖进来，然后右键选择“显示所有派生类”）
ButtonBase.dgml - 以 ButtonBase 为中心，显示了其所有派生类和基类




使用 dgml 的技巧
1、新建 dgml 文件后，选择从“类视图”中添加类会比较方便
2、但是“类视图”中的类“不全”，比如我怎么也找不到 Windows.UI.Xaml.Controls.Primitives
那怎么办？可以新建一个类，让他继承 Windows.UI.Xaml.Controls.Primitives，然后在“类视图”中选择这个新建的类，在其“基类型”中就会发现 Windows.UI.Xaml.Controls.Primitives
3、在 dgml 编辑界面，选中某一个类，然后其右键菜单里有很多功能，比如：“显示所有基类型”，“显示所有派生类型”，“显示引用此项的类型”等
4、在 dgml 编辑界面里有很多设置，比如在“筛选器”中可以选择显示哪些元素，我的习惯是禁止显示“返回”
5、通过右键可以添加组或删除组


注：到了 visual studio 2017 时，其 dgml 编辑器中没有“显示所有基类型”，“显示所有派生类型”，“显示引用此项的类型”等选项了，怎么办？