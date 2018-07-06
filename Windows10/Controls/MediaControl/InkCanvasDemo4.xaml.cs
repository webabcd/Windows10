/*
 * InkCanvas - 涂鸦板控件（继承自 FrameworkElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     InkPresenter - 获取 InkPresenter 对象
 *        
 * InkRecognizerContainer - 用于管理手写识别
 *     GetRecognizers() - 获取 InkRecognizer 对象集合
 *     SetDefaultRecognizer(InkRecognizer recognizer) - 将指定的 InkRecognizer 设置为默认的手写识别器
 *     RecognizeAsync(InkStrokeContainer strokeCollection, InkRecognitionTarget recognitionTarget) - 识别，返回 InkRecognitionResult 对象集合
 *         InkStrokeContainer strokeCollection - 需要识别的 InkStrokeContainer 对象
 *         InkRecognitionTarget recognitionTarget - 需要识别的目标
 *             All - 识别全部涂鸦数据
 *             Selected - 识别被选中的涂鸦数据
 *             Recent - 识别 InkStroke 的 Recognized 为 false 的涂鸦数据
 *             
 * InkRecognizer - 手写识别器
 *     Name - 手写识别器的名字（只读）
 * 
 * InkRecognitionResult - 手写识别结果
 *     BoundingRect - 获取用于识别的涂鸦的 Rect 区域
 *     GetStrokes() - 获取用于识别的 InkStroke 对象集合
 *     GetTextCandidates() - 获取识别结果，这是一个候选结果列表
 * 
 * InkPresenter - 涂鸦板
 *     StrokeContainer - 返回 InkStrokeContainer 类型的对象
 *     
 * InkStrokeContainer - 用于管理涂鸦
 *     UpdateRecognitionResults(IReadOnlyList<InkRecognitionResult> recognitionResults) - 将指定的识别结果通知给 InkStrokeContainer（此时 InkStrokeContainer 中被识别的 InkStroke 的 Recognized 将被标记为 true）
 *         如果使用的是 InkRecognitionTarget.All 则 InkStrokeContainer 中的所有的 InkStroke 的 Recognized 将被标记为 true
 *         如果使用的是 InkRecognitionTarget.Selected 则 InkStrokeContainer 中的被选中的 InkStroke 的 Recognized 将被标记为 true
 *     GetRecognitionResults() - 返回之前通过 UpdateRecognitionResults 方法设置的数据
 *     
 * InkStroke - 涂鸦对象（这是一次的涂鸦对象，即鼠标按下后移动然后再抬起后所绘制出的涂鸦）
 *     Recognized - 此 InkStroke 是否被识别了
 *     Selected - 此 InkStroke 是否被选中了
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.MediaControl
{
    public sealed partial class InkCanvasDemo4 : Page
    {
        public InkCanvasDemo4()
        {
            this.InitializeComponent();

            inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen | CoreInputDeviceTypes.Touch;

            InkDrawingAttributes drawingAttributes = inkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.IgnorePressure = true;
            drawingAttributes.Color = Colors.Red;
            drawingAttributes.Size = new Size(4, 4);
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private async void recognize_Click(object sender, RoutedEventArgs e)
        {
            if (inkCanvas.InkPresenter.StrokeContainer.GetStrokes().Count == 0)
                return;

            InkRecognizerContainer container = new InkRecognizerContainer();

            lblMsg.Text = "手写识别器: ";
            lblMsg.Text += Environment.NewLine;
            // 获取当前支持的手写识别器列表，如果有多个的话可以通过 SetDefaultRecognizer 方法来指定默认的手写识别器
            IReadOnlyList<InkRecognizer> recognizers = container.GetRecognizers();
            foreach (InkRecognizer ir in recognizers)
            {
                lblMsg.Text += ir.Name;
                lblMsg.Text += Environment.NewLine;
            }

            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "识别结果: ";
            lblMsg.Text += Environment.NewLine;
            IReadOnlyList<InkRecognitionResult> result = await container.RecognizeAsync(inkCanvas.InkPresenter.StrokeContainer, InkRecognitionTarget.All);
            foreach (string textCandidate in result[0].GetTextCandidates())
            {
                lblMsg.Text += textCandidate;
                lblMsg.Text += Environment.NewLine;
            }

            // 将识别结果通知给 InkStrokeContainer
            inkCanvas.InkPresenter.StrokeContainer.UpdateRecognitionResults(result);

            // 识别结果通知给 InkStrokeContainer 后，被识别的 InkStroke 的 Recognized 将被标记为 true
            // 如果在识别的时候使用的是 InkRecognitionTarget.All 则所有的 InkStroke 的 Recognized 将被标记为 true
            // 如果在识别的时候使用的是 InkRecognitionTarget.Selected 则被选中的 InkStroke 的 Recognized 将被标记为 true
            IReadOnlyList<InkStroke> strokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();
            foreach (InkStroke stroke in strokes)
            {
                Debug.WriteLine("stroke.Recognized: " + stroke.Recognized);
            }

            // 这个获取到的就是之前通过 InkStrokeContainer 方式设置的数据
            result = inkCanvas.InkPresenter.StrokeContainer.GetRecognitionResults();
        }
    }
}
