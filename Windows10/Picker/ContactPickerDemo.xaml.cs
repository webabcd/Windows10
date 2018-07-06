/* 演示如何通过 ContactPicker 选择一个或多个联系人
 *
 * ContactPicker - 联系人选择窗口（有好多 api 在 uwp 中废弃了）
 *     DesiredFieldsWithContactFieldType - 需要获取的联系人的字段（可以添加 ContactFieldType 类型的枚举）
 *     PickContactAsync() - 弹出联系人选取器（只能选取一个），返回 Contact 类型的对象
 *     PickContactsAsync() - 弹出联系人选取器（可以选取多个），返回 Contact 类型的对象
 *     
 * Contact - 联系人对象
 *     有一堆属性，看文档吧
 */

using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Contacts;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Picker
{
    public sealed partial class ContactPickerDemo : Page
    {
        public ContactPickerDemo()
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
            }
            else
            {
                lblMsg.Text += "取消了";
                lblMsg.Text += Environment.NewLine;
            }
        }

        private async void btnPickContacts_Click(object sender, RoutedEventArgs e)
        {
            ContactPicker contactPicker = new ContactPicker();
            // 指定需要选取的联系人的字段
            contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.Email);
            contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);

            // 启动联系人选取器，以选择多个联系人
            IList<Contact> contacts = await contactPicker.PickContactsAsync();

            if (contacts != null && contacts.Count > 0)
            {
                foreach (Contact contact in contacts)
                {
                    lblMsg.Text += string.Format("name:{0}", contact.Name);
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
