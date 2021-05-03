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
    [QueryProperty(nameof(Set_pregled_nov), "panjpregledNOV")]
    [QueryProperty(nameof(Set_panj_ID), "panjpregledID")]
    [QueryProperty(nameof(Set_panj_NAME), "panjpregledNAME")]
    [QueryProperty(nameof(Eyaa), "eyoo")]
    public partial class PagePregledPanja : ContentPage
    {
        int panj_ID;
        string panj_NAME;
        bool Is_new_pregled;
        List<SimboliZaHrano> Eyaa;
        //DateTime DatePicker_datum;
        PregledPanja panj_pregled;

        public string Set_pregled_nov
        {
            set
            {
                if (value == "nov")
                {
                    Is_new_pregled = true;
                    //DatePicker_datum = DateTime.Today;
                }
                else
                {
                    Is_new_pregled = false;
                    Load_existing_pregled(value);
                }
            }
        }

        public string Set_panj_ID
        {
            set
            {
                panj_ID = int.Parse(value);
                //Load_simbols();
            }
        }

        public string Set_panj_NAME
        {
            set
            {
                panj_NAME = value;
                contentPage_forPanj.Title = "Panj: " + panj_NAME;
            }
        }

        public PagePregledPanja()
        {
            InitializeComponent();
            Load_simbols();
            //OnAppearing();
        } //Useless constructor

        void Load_new_pregled()
        {
 
        }

        void AA(object sender, EventArgs e)
        {
            Picker_selected_item();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!Is_new_pregled && panj_pregled != null)
            {
                Picker_zalega.SelectedIndex = panj_pregled.Zalega_ob_pregledu;
                Picker_hrana.SelectedIndex = panj_pregled.Hrana_ob_pregledu - 1;
                Picker_moc.SelectedIndex = panj_pregled.Moc_ob_pregledu - 1;
            }
        }

        async void Load_existing_pregled(string ID)
        {
            try
            {
                panj_pregled = await App.BazaPanjSingelton.Get_pregled_async(Convert.ToInt32(ID));
                DatePicker_one.Date = panj_pregled.Datum_pregleda;
                Entry_zapiski.Text = panj_pregled.Dodatni_zapiski;

                Picker_zalega.SelectedIndex = panj_pregled.Zalega_ob_pregledu;
                Picker_hrana.SelectedIndex = panj_pregled.Hrana_ob_pregledu - 1;
                Picker_moc.SelectedIndex = panj_pregled.Moc_ob_pregledu - 1;

                //Picker_hrana.BindingContext = panj_pregled;
                //Picker_hrana.SetBinding(Picker.SelectedIndexProperty, "Hrana_ob_pregledu");


                

                //Picker_selected_item();
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load pregled");
            }

            //OnAppearing();
        }

        void Picker_selected_item()
        {
            foreach (SimboliZaHrano h in Picker_hrana.ItemsSource)
            {
                if (h.Simbol_hrana_ID == panj_pregled.Hrana_ob_pregledu)
                {
                    Picker_hrana.SelectedItem = h;
                    break;
                }
            }

            foreach (SimboliZaMoc m in Picker_moc.ItemsSource)
            {
                if (m.Simbol_moc_ID == panj_pregled.Moc_ob_pregledu)
                {
                    Picker_moc.SelectedItem = m;
                    break;
                }
            }
        }

        async void Load_simbols()
        {
            Picker_zalega.ItemsSource = Enumerable.Range(0, 11).ToList();
            Picker_moc.ItemsSource = await App.BazaPanjSingelton.Get_vsi_simboli_moc_async();
            Picker_hrana.ItemsSource = await App.BazaPanjSingelton.Get_vsi_simboli_hrana_async();

            //testc binding
            //Eyaa = await App.BazaPanjSingelton.Get_vsi_simboli_hrana_async();
            //Picker_hrana.SetBinding(Picker.ItemsSourceProperty, "eyoo");
            //Picker_hrana.ItemDisplayBinding = new Binding("Simbol_hrana_opis");
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            if (Is_new_pregled)
            {
                panj_pregled = new PregledPanja();
                if (panj_ID != 0) panj_pregled.Pripada_panju = panj_ID;
                else panj_pregled.Pripada_panju = -1;   
            }

            if (Picker_hrana.SelectedItem != null && Picker_moc.SelectedItem != null && Picker_zalega.SelectedItem != null)
            {
                panj_pregled.Datum_pregleda = DatePicker_one.Date;
                panj_pregled.Dodatni_zapiski = Entry_zapiski.Text;

                panj_pregled.Moc_ob_pregledu = ((SimboliZaMoc)Picker_moc.SelectedItem).Simbol_moc_ID;
                panj_pregled.Hrana_ob_pregledu = ((SimboliZaHrano)Picker_hrana.SelectedItem).Simbol_hrana_ID;
                panj_pregled.Zalega_ob_pregledu = (int)Picker_zalega.SelectedItem;

                await App.BazaPanjSingelton.Save_pregled(panj_pregled);
            }

            // Navigate backwards
            await Shell.Current.GoToAsync($"..?EvidencniPanj={panj_ID}");
        }

        private async void Button_izbrisi_pregled(object sender, EventArgs e)
        {
            if (!Is_new_pregled) await App.BazaPanjSingelton.Delete_pregled_Async(panj_pregled);

            //navigate back
            await Shell.Current.GoToAsync($"..?EvidencniPanj={panj_ID}");
        }

    }
}