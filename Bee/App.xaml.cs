using Bee.Data;
using Bee.Views;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bee
{
    public partial class App : Application
    {
        static BazaPanj podatkovnabazaPanjSingelton;
        public static BazaPanj BazaPanjSingelton
        {
            get
            {
                if (podatkovnabazaPanjSingelton == null)
                {
                    podatkovnabazaPanjSingelton = new BazaPanj(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bazaZaPanj"));
                }
                return podatkovnabazaPanjSingelton;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
