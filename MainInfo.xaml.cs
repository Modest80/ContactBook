using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ContactBook
{
    /// <summary>
    /// Логика взаимодействия для MainInfo.xaml
    /// </summary>
    public partial class MainInfo : Window {
        public MainInfo() {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            SaveMyInfo();
        }
        public void SaveMyInfo() {
            if (tbName.Text == "") return;
            if (pbPassword.Password == "") return;
            DateTime birthday = DateTime.Parse("1970-1-1");
            try { birthday = DateTime.Parse(dtBirthday.Text); }
            catch { }
            MainWindow.me = new MyPerson {
                Id = 0,
                Fullname = tbName.Text,
                Birthday = birthday,
                MobilePhone = tbMobilePhone.Text,
                HomeAddress = tbHomeAddress.Text,
                Email = tbEmail.Text,
                Password = pbPassword.Password
            };
            using (StreamWriter write = new StreamWriter(MainWindow.fileMyInfo)) {
                string json = JsonConvert.SerializeObject
                        (MainWindow.me, Formatting.Indented);
                write.WriteLine(json);
            }
            Close();
                 
        }
    }
}
