# Windows10


#### C# 6.0 新特性
1. C# 6.0 示例 1
2. C# 6.0 示例 2
3. C# 6.0 示例 3

#### C# 7.0 新特性
1. C# 7.0 示例 1
2. C# 7.0 示例 2
3. C# 7.0 示例 3

#### UI
1. 概述
2. 启动屏幕
3. 屏幕方向
4. 窗口全屏
5. 窗口尺寸
6. 多窗口
7. 标题栏

#### 控件 UI
1. 字体的自动继承的特性
2. Style
3. ControlTemplate
4. VisualState 和 VisualStateManager
5. 控件的默认 Style, ControlTemplate, VisualState
6. StateTrigger

#### 资源
1. 资源限定符 - 概述
2. 资源限定符 - 示例
3. StaticResource
4. ThemeResource
5. CustomResource
6. ResourceDictionary
7. 加载外部的 ResourceDictionary 文件

#### 绘图
1. Shape
2. Path
3. Stroke
4. Brush

#### 动画（Storyboard）
1. 线性动画
2. 关键帧动画
3. 缓动动画

#### 动画（ThemeAnimation）
1. PopInThemeAnimation, PopOutThemeAnimation
2. FadeInThemeAnimation, FadeOutThemeAnimation
3. PointerDownThemeAnimation, PointerUpThemeAnimation
4. SwipeHintThemeAnimation, SwipeBackThemeAnimation
5. RepositionThemeAnimation
6. SplitOpenThemeAnimation, SplitCloseThemeAnimation
7. DrillInThemeAnimation, DrillOutThemeAnimation
8. DragItemThemeAnimation, DragOverThemeAnimation, DropTargetItemThemeAnimation

#### 动画（ThemeTransition）
1. 概述
2. EntranceThemeTransition
3. ContentThemeTransition
4. RepositionThemeTransition
5. PopupThemeTransition
6. AddDeleteThemeTransition
7. ReorderThemeTransition
8. PaneThemeTransition
9. EdgeUIThemeTransition
10. NavigationThemeTransition

#### 绑定
1. 与 Element 绑定
2. 与 Indexer 绑定
3. TargetNullValue, FallbackValue
4. TemplateBinding 绑定
5. 与 RelativeSource 绑定
6. 与 StaticResource 绑定
7. DataContextChanged
8. UpdateSourceTrigger
9. 对绑定的数据做自定义转换
10. x:Bind 绑定
11. x:Bind 绑定之 x:Phase
12. 使用绑定过程中的一些技巧
13. 通过 Binding 绑定对象
14. 通过 x:Bind 绑定对象
15. 通过 Binding 绑定集合
16. 通过 x:Bind 绑定集合
17. 通过 Binding 绑定字典表

#### MVVM（Model-View-ViewModel）
1. 通过 Binding 或 x:Bind 结合 Command 实现，通过 ButtonBase 触发命令
2. 通过 Binding 或 x:Bind 结合 Command 实现，通过非 ButtonBase 触发命令
3. 通过 x:Bind 实现 MVVM（不用 Command）

#### xaml 特性
1. x:DeferLoadStrategy 通过 FindName 加载
2. x:DeferLoadStrategy 通过绑定加载
3. x:DeferLoadStrategy 通过 Storyboard 加载
4. x:DeferLoadStrategy 通过 Setter 加载
5. x:DeferLoadStrategy 通过 GetTemplateChild 加载
6. x:Null

#### 文本类控件
1. TextBlock(1)
2. TextBlock(2)
3. 使用自定义字体
使用 Unicode 编码
4. TextBox(1)
5. TextBox(2)
6. PasswordBox
7. RichTextBlock
8. RichTextBlockOverflow
9. RichEditBox
10. AutoSuggestBox

#### 按钮类控件
1. ButtonBase(基类)
2. Button
3. HyperlinkButton
4. RepeatButton
5. ToggleButton
6. AppBarButton
7. AppBarToggleButton

#### 选择类控件
1. Selector(基类)
SelectorItem(基类)
2. ComboBox
3. ListBox
4. RadioButton
5. CheckBox
6. ToggleSwitch

#### 进度类控件
1. RangeBase(基类)
2. Slider
3. ProgressBar
4. ProgressRing

#### 弹出类控件
1. FlyoutBase(基类)
2. Flyout
3. MenuFlyout
4. ToolTip
5. Popup
6. PopupMenu
7. MessageDialog
8. ContentDialog

#### 布局类控件
1. Panel(基类)
2. Canvas
3. RelativePanel
4. StackPanel
5. Grid
6. VariableSizedWrapGrid
7. Border
8. Viewbox
9. SplitView

#### 导航类控件
1. AppBar
2. CommandBar
3. Frame

#### 日期类控件
1. CalendarView
2. DatePicker
3. TimePicker

#### 图标类控件
1. IconElement(基类)
2. SymbolIcon
3. FontIcon
4. PathIcon
5. BitmapIcon

#### ScrollViewer
1. ScrollViewer
2. ScrollBar
3. ScrollContentPresenter
4. Chaining - 锁链
5. Rail - 轨道
6. Inertia - 惯性
7. Snap - 对齐
8. Zoom - 缩放

#### 集合类控件 - FlipView, Pivot, Hub
1. FlipView
2. Pivot
3. Hub

#### 集合类控件 - ItemsControl
1. 基础知识
2. 数据绑定
3. ItemsPresenter
4. GridViewItemPresenter, ListViewItemPresenter
5. 项模板选择器
6. 数据分组
7. 自定义 ItemsControl（自定义 GirdView 使其每个 item 占用不同大小的空间）
8. 自定义 ContentPresenter 实现类似 GridViewItemPresenter 和 ListViewItemPresenter 的效果

#### 集合类控件 - ItemsControl 的布局控件
1. ItemsStackPanel
2. ItemsWrapGrid
3. OrientedVirtualizingPanel(基类)
4. VirtualizingStackPanel
5. WrapGrid

#### 集合类控件 - SemanticZoom
1. SemanticZoom
2. ISemanticZoomInformation

#### 集合类控件 - ListViewBase
1. 基础知识
2. 拖动项
3. 增量加载
4. 分步绘制（大数据量流畅滚动）
5. ListView
6. GridView

#### 媒体类控件
1. Image(1)
2. Image(2)
3. MediaElement
4. 通过处理 Pointer 相关事件实现一个简单的涂鸦板
5. InkCanvas 基础知识
6. InkCanvas 涂鸦编辑
7. InkCanvas 保存和加载
8. InkCanvas 手写识别

#### WebView
1. 基础知识
2. 加载 html, http, https, ms-appx-web:///, embedded resource, ms-appdata:///, ms-local-stream://
3. 加载指定 HttpMethod 的请求，自定义请求的 http header
4. app 与 js 的交互
5. 对 WebView 中的内容截图
6. 通过 Share Contract 分享 WebView 中的被选中的内容
7. 监听页面的进入全屏事件和退出全屏事件，监听导航至不支持 uri 协议的事件，监听导航至不支持类型的文件的事件
8. 监听用新窗口打开 uri 的事件，监听获取特殊权限的事件

#### 控件基类 - DependencyObject
1. CoreDispatcher
2. 依赖属性的设置与获取
3. 依赖属性的变化回调

#### 控件基类 - UIElement
1. Pointer 相关事件
2. Tap 相关事件
3. Key 相关事件
4. Focus 相关事件
5. Manipulate 手势处理
6. 路由事件的注册，路由事件的冒泡，命中测试的可见性
7. Transform3D（3D变换）
8. Projection（3D投影）
9. RenderTransform（2D变换）
10. Clip（剪裁）
11. 获取 UIElement 的位置
12. UIElement 的布局相关（Measure, Arrange）
13. UIElement 的非完整像素布局（UseLayoutRounding）
14. UIElement 的其他特性（Visibility, Opacity, CacheMode）
15. 拖放的基本应用
16. 手动开启 UIElement 的拖放操作
17. 与 CanDrag 相关的事件（DragStartingEventArgs, DropCompletedEventArgs）
18. 与 AllowDrop 相关的事件（DragEventArgs）

#### 控件基类 - FrameworkElement
1. 基础知识
2. 相关事件
3. HorizontalAlignment 和 VerticalAlignment

#### 控件基类 - Control
1. 基础知识
2. 焦点相关
3. 运行时获取 ControlTemplate 和 DataTemplate 中的元素

#### 控件基类 - ContentControl, UserControl, Page
1. ContentPresenter
2. ContentControl
3. UserControl
4. Page

#### 自定义控件
1. 自定义控件的基础知识，依赖属性和附加属性
2. 自定义控件的 Layout 系统
3. 自定义控件的控件模板和事件处理的相关知识点

#### 本地化和全球化
1. 本地化 - Demo
2. 本地化 - 改变语言
3. 全球化 - Demo
4. 全球化 - 格式化数字

#### 用户和账号
1. 获取用户的信息
2. 获取用户的同意
3. 数据账号的添加和管理
4. OAuth 2.0 验证
5. 微软账号的登录和注销

#### 文件系统
1. 获取文件夹和文件
2. 分组文件夹，排序过滤文件夹和文件，搜索文件
3. 获取文件夹的属性
4. 获取文件夹的缩略图
5. 获取文件的属性，修改文件的属性
6. 获取文件的缩略图
7. 创建文件夹，重命名文件夹，删除文件夹，在指定的文件夹中创建文件
8. 创建文件，复制文件，移动文件，重命名文件，删除文件
9. 打开文件，获取指定的本地 uri 的文件，通过 StreamedFileDataRequest 或远程 uri 创建文件或替换文件
10. 读写文本数据
11. 读写二进制数据
12. 读写流数据
13. 获取 Package 中的文件
14. 可移动存储中的文件操作
15. “库”管理
16. Application Data 中的文件操作
17. Application Data 中的“设置”操作
18. 通过 uri 引用 Application Data 中的媒体
19. 读写“最近访问列表”和“未来访问列表”
20. 管理以及使用索引

#### Picker（选取器）
1. FileOpenPicker（文件选取窗口）
2. FolderPicker（文件夹选取窗口）
3. FileSavePicker（文件保存窗口）
4. 自定义文件打开选取器
5. 自定义文件保存选取器
6. ContactPicker（联系人选取窗口）
7. 通过 ContactPicker 选取联系人，并获取其完整信息
8. CachedFileUpdater（缓存文件更新程序）

#### 关联启动
1. 使用外部程序打开一个文件
2. 使用外部程序打开一个 Uri
3. 关联指定的文件类型
4. 关联指定的协议

#### 应用间通信
1. 分享
2. 通过协议打开指定的 app 并传递数据以及获取返回数据
3. 将本 app 沙盒内的文件共享给其他 app 使用
4. 剪切板 - 基础, 复制/粘贴 text 内容
5. 剪切板 - 复制/粘贴 html 内容
6. 剪切板 - 复制/粘贴 bitmap 内容，延迟复制
7. 剪切板 - 复制/粘贴文件

#### 通知（Toast）
1. 基础
2. 按计划显示 toast 通知
3. 纯文本 toast, 短时 toast, 长时 toast
4. 图文 toast
5. 带按钮的 toast
6. 带输入的 toast（文本输入框，下拉选择框）
7. 通过 toast 打开协议
8. 通过 toast 选择在指定的时间之后延迟提醒或者取消延迟提醒
9. 提示音
10. 特定场景

#### 通知（Tile）
1. application tile 基础
2. secondary tile 基础
3. 按计划显示 tile 通知
4. 轮询服务端以更新 tile 通知
5. secondary tile 模板之基础
6. secondary tile 模板之文本
7. secondary tile 模板之图片
8. secondary tile 模板之分组

#### 通知（Badge）
1. application 的 badge 通知
2. secondary 的 badge 通知
3. 轮询服务端以更新 badge 通知

#### 锁屏
1. 将 Application 的 Badge 通知和 Tile 通知发送到锁屏
2. 将 secondary tile 的 Badge 通知和 Tile 通知发送到锁屏

#### 后台任务
1. 后台任务的 Demo（与 app 不同进程）
2. 后台任务的 Demo（与 app 相同进程）
3. 通过 toast 激活后台任务
4. 定时激活后台任务
5. 前台程序激活后台任务
6. 后台下载任务
7. 后台下载任务（任务分组，并行或串行执行，组完成后通知）
8. 后台下载任务（任务分组，组完成后触发后台任务）
9. 后台上传任务
10. 推送通知

#### 信息
1. ProfileInfo

#### 工具
1. 查找指定类或接口的所在程序集的所有子类和子接口
