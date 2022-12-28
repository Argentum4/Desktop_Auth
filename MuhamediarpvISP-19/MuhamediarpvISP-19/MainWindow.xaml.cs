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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MuhamediarpvISP_19
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Pass.IsEnabled=false;
            Cod.IsEnabled=false;
            Code.IsEnabled=false;
            Vhod.IsEnabled=false;
            Global.Cod = null;
            Nomer.Focus();
        }

        public static class Global
        {
            public static string Cod;
        }

        private void Nomer_KeyUp(object sender, KeyEventArgs e)                 //Настройки для поля ввода номера пользователя и проверка совпадений с информацией из базы данных
        {
            if (e.Key == Key.Enter)
            {
                using (var db = new BazaEkzamenEntities())
                {
                    var logi = db.Tablica_Ekzamen.FirstOrDefault(u => u.Number == Nomer.Text);
                    if(logi == null)
                    {
                        MessageBox.Show("Пользователь не найден");
                    }
                    else
                        Pass.IsEnabled = true;
                        Pass.Focus();
                        Nomer.IsEnabled = false;
                }
            }
        }
        private void Pass_KeyUp(object sender, KeyEventArgs e)                  //Настройки для поля ввода пароля пользователя и проверка совпадений с информацией из базы данных
        {
            if (e.Key == Key.Enter)
            {
                using (var db = new BazaEkzamenEntities())
                {
                    var pass = db.Tablica_Ekzamen.FirstOrDefault(u => u.Password == Pass.Password);
                    if (pass == null)
                    {
                        MessageBox.Show("Неверный пароль");
                    }
                    else
                        Cod.IsEnabled = true;
                        Code.IsEnabled = true;
                        Code.Focus();
                        Pass.IsEnabled = false;
                }
            }
        }

        DispatcherTimer timer = new DispatcherTimer();      
        private void Code_Click(object sender, RoutedEventArgs e)              //Настройки для кнопки получения кода пользоватеем и запуск таймера
        {
            Random random = new Random();
            Global.Cod = random.Next(0, 10000).ToString();
            MessageBox.Show("Ваш код " + Global.Cod);

            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)                 //Функция остановки таймера и удаление кода
        {
            Cod = null;
            MessageBox.Show("Время вышло. Снова запросите код");
            timer.Stop();
        }

        private void Cod_KeyUp(object sender, KeyEventArgs e)               //Проверка введённого кода пользователем
        {
            if (e.Key == Key.Enter)
            {
                if (Global.Cod != Cod.Text)
                {
                    MessageBox.Show("Неверный код");
                }
                else
                    Vhod.IsEnabled = true;
            }
        }
       
        private void Otmena_Click(object sender, RoutedEventArgs e)             //Функция кнопки отмены, очищающей поля воода
        {
            Nomer.Clear();
            Pass.Clear();
            Cod.Clear();
            Pass.IsEnabled = false;
            Cod.IsEnabled = false;
            Code.IsEnabled = false;
            Vhod.IsEnabled = false;
            Nomer.IsEnabled = true;
        }

        private void Vhod_Click(object sender, RoutedEventArgs e)               //Функция кнопки входа, открывающей другое окно
        {
            Window1 window1 = new Window1();
            window1.Show();
            Application.Current.MainWindow.Close();
        }
    }
}
