using System;
using System.Collections.Generic;
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

namespace ContactBook {
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window {
        //string login = "Admin";
        //string password = "1234";
        public Login() {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e) {
            if (tbUserlogin.Text == "") return;
            if (tbUserpassword.Password == "") return;

            if(tbUserlogin.Text == MainWindow.me.Fullname && 
               tbUserpassword.Password == MainWindow.me.Password) {
                //Проверка пройдена
                MainWindow.isAuth = true;
                Close();
            }
                
        }
    }
}
