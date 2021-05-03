using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bee.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bee.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(PanjID), "UpdatePanjID")]
    public partial class UstvariPanj : ContentPage
    {
        private bool Is_new_panj;
        private Panj Created_panj;
        public string PanjID
        {
            set
            {
                if (value == "nov")
                {
                    Is_new_panj = true;
                    Create_new_hive();
                }
                else
                {
                    Is_new_panj = false;
                    LoadPanj(value);
                }
            }
        }

        public UstvariPanj()
        {
            InitializeComponent();
        }

        private void Create_new_hive()
        {
            Created_panj = new Panj();

            editor_StPanja.Text = "";
            DatePicker_Panj.Date = DateTime.Today;
        }

        async void LoadPanj(string panjID)
        {
            try
            {
                int id = Convert.ToInt32(panjID);
                Created_panj = await App.BazaPanjSingelton.GetPanjAsync(id);
                editor_StPanja.Text = Created_panj.StPanj.ToString();
            }
            catch (Exception)
            {
                Console.WriteLine("Faile to load panj");
            }
        }

        async void OnSaveButtonClick(object sender, EventArgs e)
        {
            Created_panj.StPanj = int.Parse(editor_StPanja.Text);
            Created_panj.Datum = DatePicker_Panj.Date;

            if (string.IsNullOrWhiteSpace(Created_panj.Matica)) Created_panj.Matica = "Prazno"; 
            await App.BazaPanjSingelton.SavePanjAsync(Created_panj);

            // Navigate backwards
            if (Is_new_panj) await Shell.Current.GoToAsync("..");
            else await Shell.Current.GoToAsync($"..?EvidencniPanj={Created_panj.ID}");
        }

        async void OnDeleteButtonClick(object sender, EventArgs e)
        {
            if (Is_new_panj)
            {
                await Shell.Current.GoToAsync(".."); 
            }
            else
            {
                bool Tudi_preglede = await DisplayAlert("", "Izbrišem ta panj in vse njegove preglede?", "DA", "NE");
                if (Tudi_preglede)
                {
                    await App.BazaPanjSingelton.DeletePanjAsync(Created_panj, true);
                    await Shell.Current.GoToAsync("../..");
                }
            }
            //bool Tudi_preglede = await DisplayAlert("", "Izbrišem tudi preglede tega panja?", "DA", "NE");
            //await App.BazaPanjSingelton.DeletePanjAsync(Created_panj, Tudi_preglede);
        }

        
    }
}