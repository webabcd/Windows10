/*
 * 用于演示自定义 StateTrigger
 *
 *
 * StateTriggerBase - 自定义 StateTrigger 需要继承此基类
 *     SetActive(Boolean IsActive) - 调用此方法，传递 true 则应用对应的 VisualState；传递 false 则取消对应的 VisualState
 *
 *
 * 此类的作用：当前的设备类型与指定的一致时，则触发对应的 VisualState
 * 注：如果 DeviceFamily 属性需要绑定的话，别忘了将其定义为依赖属性
 */

using Windows.UI.Xaml;

namespace Windows10.Controls.UI.VisualState
{
    public class MyDeviceFamilyStateTrigger : StateTriggerBase
    {
        private string _deviceFamily;

        public string DeviceFamily
        {
            get
            {
                return _deviceFamily;
            }
            set
            {
                _deviceFamily = value;

                // 获取当前的设备类型，目前已知的返回字符串有：Windows.Mobile, Windows.Desktop, Windows.Xbox
                string currentDeviceFamily = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;

                // 当前的设备类型与指定的一致则触发对应的 VisualState
                SetActive(_deviceFamily == currentDeviceFamily);
            }
        }
    }
}
