using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Bee.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bee.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeznamPanjev : ContentPage
    {
        public SeznamPanjev()
        {
            InitializeComponent();

            //ICommand hmm = new Command(Refresh_the_view);
            refreshViewOne.Command = new Command(Refresh_the_view);
            //refreshViewOne.Command.Execute(refreshViewOne);
            this.OnAppearing();
        }

        async void Refresh_the_view()
        {
            collectionViewPanji.ItemsSource = await App.BazaPanjSingelton.GetVsiPanjiAsync();
            refreshViewOne.IsRefreshing = false;
            //testerFooter.Text = "done";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Refresh_the_view();
        }

        async void Event_dodaj_panj(object sender, EventArgs e)
        {
            //go to page Ustvari panj
            await Shell.Current.GoToAsync($"UstvariAliUreduPanj?UpdatePanjID={"nov"}");
            Refresh_the_view();
        }

        async void Selected_item(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                Panj p = (Panj)e.CurrentSelection.FirstOrDefault();
                await Shell.Current.GoToAsync($"EvidencniListPanja?EvidencniPanj={p.ID.ToString()}");

            }
        } //when you click on existing hive in list go to panj info page

        async void Test_transiton(object sender, EventArgs e)
        {
            string a = "yes butr actually no";
            await Shell.Current.GoToAsync(
                $"EvidencniListPanja?nastaviEyee={a}"
                );
        }
    }
}