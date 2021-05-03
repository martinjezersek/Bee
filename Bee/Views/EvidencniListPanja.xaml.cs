using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bee.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bee.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty( nameof(Set_trenutni_panj), "EvidencniPanj")]
    public partial class EvidencniListPanja : ContentPage
    {
        Panj Trenutni_panj;

        public string Set_trenutni_panj 
        { set 
            {
                int id_panja = Convert.ToInt32(value);
                Set_trenutni_panj_async(id_panja); //preps the page fo viewing
            }
        }

        public EvidencniListPanja()
        {
            InitializeComponent();
            RefreshView_two.Command = new Command(Seznam_pregledov);
        } //useless constructor

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Seznam_pregledov();
            //CollectionView_Seznam.SelectedItem = null;
        }

        async void Set_trenutni_panj_async(int id)
        {
            try { Trenutni_panj = await App.BazaPanjSingelton.GetPanjAsync(id); }
            catch (Exception) { Console.WriteLine("Failed to load panj."); }

            if (Trenutni_panj != null) contentPage_EvidencaPanj.Title = "Panj:  " + Trenutni_panj.StPanj;
            else contentPage_EvidencaPanj.Title = "Woops";

            Seznam_pregledov();
        }

        async void Seznam_pregledov()
        {
            List<ForDisplayMeritev> a = new List<ForDisplayMeritev>();

            if (Trenutni_panj != null)
            {
                List<PregledPanja> list_pregledov = await App.BazaPanjSingelton.Get_vsi_pregledi_panja(Trenutni_panj.ID);

                foreach (PregledPanja p in list_pregledov)
                {
                    a.Add(await App.BazaPanjSingelton.Get_display_meritev(p));
                }
            }

            CollectionView_Seznam.ItemsSource = a;
            RefreshView_two.IsRefreshing = false;
        } //dobi vse preglede panja iz Trenutni_panj

        async void Button_click_nov_pregled(object sender, EventArgs e)
        {
            if (Trenutni_panj != null)
            {
                string a = Trenutni_panj.ID.ToString();
                string b = Trenutni_panj.StPanj.ToString();
                await Shell.Current.GoToAsync($"{nameof(PagePregledPanja)}?panjpregledNOV={"nov"}&panjpregledID={a}&panjpregledNAME={b}");

            }
        }

        async void Izbrisi_izbrani_panj(object sender, EventArgs e)
        {
            string a = Trenutni_panj.ID.ToString();
            await Shell.Current.GoToAsync($"UstvariAliUreduPanj?UpdatePanjID={a}");
        }

        private async void CollectionView_Seznam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                string a = Trenutni_panj.ID.ToString();
                string b = Trenutni_panj.StPanj.ToString();
                string c = ((ForDisplayMeritev) e.CurrentSelection.FirstOrDefault()).Meritev__pregled_ID; //pregled

                RefreshView_two.IsRefreshing = true;
                await Shell.Current.GoToAsync($"{nameof(PagePregledPanja)}?panjpregledID={a}&panjpregledNAME={b}&panjpregledNOV={c}");
                RefreshView_two.IsRefreshing = false;
            }
        }
    }
}