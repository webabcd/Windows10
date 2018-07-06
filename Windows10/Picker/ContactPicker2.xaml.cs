/* 演示如何通过 ContactPicker 选取联系人，并获取其完整信息
 *
 * ContactPicker - 联系人选择窗口（有好多 api 在 uwp 中废弃了）
 *     DesiredFieldsWithContactFieldType - 需要获取的联系人的字段（可以添加 ContactFieldType 类型的枚举）
 *     PickContactAsync() - 弹出联系人选取器（只能选取一个），返回 Contact 类型的对象
 *     PickContactsAsync() - 弹出联系人选取器（可以选取多个），返回 Contact 类型的对象
 *     
 * Contact - 联系人对象
 *     有一堆属性，看文档吧
 *     
 *     
 * 注：
 * 1、通过 ContactPicker 选取的联系人，可以获取的信息有限，但是可以通过 ContactStore 获取联系人的完整信息
 * 2、通过 ContactStore 是可以获取到全部联系人的完整信息的，这部分知识点以后再写
 * 3、通过 ContactStore 获取数据的话，需要在 Package.appxmanifest 中配置 <Capability Name = "contacts" />
 */

using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Contacts;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Windows10.Picker
{
    public sealed partial class ContactPicker2 : Page
    {
        public ContactPicker2()
        {
            this.InitializeComponent();
        }

        private async void btnPickContact_Click(object sender, RoutedEventArgs e)
        {
            ContactPicker contactPicker = new ContactPicker();
            // 指定需要选取的联系人的字段
            contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.Email);
            contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);

            // 启动联系人选取器，以选择一个联系人
            Contact contact = await contactPicker.PickContactAsync();

            if (contact != null)
            {
                lblMsg.Text += string.Format("name:{0}", contact.Name);
                lblMsg.Text += Environment.NewLine;
                lblMsg.Text += string.Format("id:{0}", contact.Id);
                lblMsg.Text += Environment.NewLine;

                foreach (ContactEmail email in contact.Emails)
                {
                    lblMsg.Text += string.Format("email kind:{0}, email address:{1}", email.Kind, email.Address);
                    lblMsg.Text += Environment.NewLine;
                }

                foreach (ContactPhone phone in contact.Phones)
                {
                    lblMsg.Text += string.Format("phone kind:{0}, phone number:{1}, phone description:{2}", phone.Kind, phone.Number, phone.Description);
                    lblMsg.Text += Environment.NewLine;
                }
                

                ContactStore contactStore = await ContactManager.RequestStoreAsync(ContactStoreAccessType.AllContactsReadOnly);
                // 通过 ContactStore 和联系人 id 可以获取到联系人的完整信息（需要配置 <Capability Name = "contacts" />）
                Contact realContact = await contactStore.GetContactAsync(contact.Id);

                // 通过 ContactStore 也是可以拿到全部联系人信息的，这部分知识点以后再写
                // IReadOnlyList<Contact> contacts = await contactStore.FindContactsAsync();

                // 显示联系人的图片（只通过 ContactPicker 是获取不到的）
                IRandomAccessStreamReference imageStreamRef = realContact.SmallDisplayPicture;
                if (imageStreamRef != null)
                {
                    IRandomAccessStream imageStream = await imageStreamRef.OpenReadAsync();
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(imageStream);
                    imgThumbnail.Source = bitmapImage;
                }
            }
            else
            {
                lblMsg.Text += "取消了";
                lblMsg.Text += Environment.NewLine;
            }
        }
    }
}
