using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ContactBook {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public static bool isAuth = false;
        //List<Person> persons;
        ObservableCollection<Person> persons;
        bool isEditUser = false;
        public static int lastId = 1;
        public int editUserId = 0;
        public string filePersons = "persons.json";
        public static string fileMyInfo = "myinfo.json";
        internal static MyPerson me;
        public MainWindow() {
            StartForm();
            InitializeComponent();
            //persons = new List<Person>();
            persons = new ObservableCollection<Person>();
            LoadPersons();
            tvUserlist.ItemsSource = persons;
            Title = "Записная книга: " + me.Fullname + " " + me.MobilePhone;
        }
        public void StartForm() {
            if (File.Exists(fileMyInfo)) {
                using(StreamReader read = new StreamReader(fileMyInfo)) {
                    string json = read.ReadToEnd();
                    me = JsonConvert.DeserializeObject<MyPerson>(json);
                }
                Login loginForm = new Login();
                loginForm.ShowDialog();
                if (!isAuth) Close();
            } else {
                MainInfo infoForm = new MainInfo();
                infoForm.ShowDialog();
            }
        }
        public void LoadPersons() {
            if (!File.Exists(filePersons)) return;

            using (StreamReader read = new StreamReader(filePersons)) {
                string json = read.ReadToEnd();
                persons = JsonConvert.DeserializeObject<ObservableCollection<Person>>(json);
            }
                
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            if (tbFullname.Text == "") {
                lblFullname.Foreground = Brushes.Red;
                lblFullname.Content = "Полное имя (является обязательным)";
                return;
            }
            lblFullname.Foreground = Brushes.Black;
            lblFullname.Content = "Полное имя *";
            DateTime userBirthday = DateTime.Parse("1970-1-1");
            if (dpBirthday.Text != "") {
                userBirthday = DateTime.Parse(dpBirthday.Text);
            }
            string imagePath = "";
            if (lblPhotoFilename.Content != null) {
                imagePath = lblPhotoFilename.Content.ToString();
            }
            if (!isEditUser) {
                Person person = new Person() {
                    Id = lastId++,
                    Fullname = tbFullname.Text,
                    Birthday = userBirthday,
                    HomeAddress = tbHomeAddress.Text,
                    MobilePhone = tbMobilePhone.Text,
                    Email = tbEmail.Text,
                    ImagePath = imagePath
                };
                persons.Add(person);
            } else {
                for (int i = 0; i < persons.Count; i++) {
                    if (persons[i].Id == editUserId) {
                        /*
                        var updatePerson = persons[i];
                        updatePerson.Birthday = userBirthday;
                        updatePerson.Fullname = tbFullname.Text;
                        updatePerson.HomeAddress = tbHomeAddress.Text;
                        updatePerson.Email = tbEmail.Text;
                        updatePerson.MobilePhone = tbMobilePhone.Text;
                        persons.RemoveAt(i);
                        persons.Insert(i, updatePerson);
                        */
                        persons[i] = new Person() {
                            Id = editUserId,
                            Fullname = tbFullname.Text,
                            Birthday = userBirthday,
                            HomeAddress = tbHomeAddress.Text,
                            MobilePhone = tbMobilePhone.Text,
                            Email = tbEmail.Text,
                            ImagePath = imagePath
                        };
                        isEditUser = false;
                        editUserId = 0;
                        break;
                    }
                }
            }
            lblState.Content = "Пользователь сохранен";

            tbFullname.Text = "";
            tbEmail.Text = "";
            dpBirthday.Text = "";
            tbHomeAddress.Text = "";
            tbMobilePhone.Text = "";
            lblPhotoFilename.Content = "";
            imgPhoto.Source = null;
        }

        private void tvUserList_Changed(object sender, SelectionChangedEventArgs e) {
            if (tvUserlist.SelectedIndex == -1) return;
            if (tvUserlist.SelectedItem == null) return;

            var person = tvUserlist.SelectedItem as Person;
            lblCardFullname.Content = person.Fullname;
            lblCardBirthday.Content = person.Birthday;
            lblCardAddress.Content = person.HomeAddress;
            lblCardPhone.Content = person.MobilePhone;
            lblCardEmail.Content = person.Email;
            if (person.ImagePath != null && person.ImagePath != "") {
                imgCardPhoto.Source = new BitmapImage(
                                        new Uri(person.ImagePath));
            }
        }

        private void btnSelectImage_Click(object sender, RoutedEventArgs e) {
            LoadUserImage();
        }
        private void LoadUserImage() {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Картинки|*.jpg;*.bmp;*.png;*.gif";
            if (open.ShowDialog() == true) {
                imgPhoto.Source = new BitmapImage(
                                        new Uri(open.FileName));
                lblPhotoFilename.Content = open.FileName;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) {
            if (tvUserlist.SelectedIndex == -1) return;
            if (tvUserlist.SelectedItem == null) return;
            var result = MessageBox.Show("Вы уверены, что хотите удалить?", "Удаление",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) {
                //persons.Remove(tvUserlist.SelectedItem as Person);
                int index = tvUserlist.SelectedIndex;
                persons.RemoveAt(index);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e) {
            if (tvUserlist.SelectedIndex == -1) return;
            if (tvUserlist.SelectedItem == null) return;
            var person = tvUserlist.SelectedItem as Person;
            if (person == null) return;
            tbFullname.Text = person.Fullname;
            dpBirthday.Text = person.Birthday.ToString();
            tbHomeAddress.Text = person.HomeAddress;
            tbMobilePhone.Text = person.MobilePhone;
            tbEmail.Text = person.Email;
            lblPhotoFilename.Content = person.ImagePath;
            editUserId = person.Id;
            tabAddUser.IsSelected = true;
            isEditUser = true;
        }

        private void formClosed(object sender, EventArgs e) {
            SavePersons();
        }
        public void SavePersons() {
            using(StreamWriter write = new StreamWriter(filePersons)) {
                string json = JsonConvert.SerializeObject(persons, 
                    Formatting.Indented);
                write.WriteLine(json);
            }
        }

        private void showListItem_Click(object sender, RoutedEventArgs e) {
            //tabControl.Visibility = Visibility.Visible;
            //tabListUsers.Visibility = Visibility.Visible;
        }

        private void addUserItem_Click(object sender, RoutedEventArgs e) {
            //tabControl.Visibility = Visibility.Visible;
            //tabAddUser.Visibility = Visibility.Visible;
        }

        private void saveCardItem_Click(object sender, RoutedEventArgs e) {
            CreateVCard();
        }
        public void CreateVCard() {
            StringBuilder vcard = new StringBuilder();
            vcard.AppendLine("BEGIN:END");
            vcard.AppendLine("VERSION:4.0");
            vcard.AppendLine("N:" + me.Fullname);
            vcard.AppendLine("TEL;TYPE#home,voice;VALUE#uri:tel:" + me.MobilePhone);
            vcard.AppendLine("EMAIL:" + me.Email);
            vcard.AppendLine("END:VCARD");
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Визитная карточка|*.vcf";
            save.DefaultExt = ".vcf";
            if (save.ShowDialog() == true) {
                File.WriteAllText(save.FileName, vcard.ToString());
            }
        }
    }
}
