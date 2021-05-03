using Bee.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Bee
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SeznamPanjev), typeof(SeznamPanjev));
            Routing.RegisterRoute("UstvariAliUreduPanj", typeof(UstvariPanj));
            //Routing.RegisterRoute(nameof(UpdatePanjPage), typeof(UpdatePanjPage));
            Routing.RegisterRoute("EvidencniListPanja", typeof(EvidencniListPanja));
            Routing.RegisterRoute(nameof(PagePregledPanja), typeof(PagePregledPanja));

            //shellHmm.Items = App.BazaPanjSingelton.GetVsiPanjiAsync();
        }
    }
}
