using System;
using System.Linq;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bee.Models;

namespace Bee.Data
{
    public class BazaPanj
    {
        readonly SQLiteAsyncConnection podatkovanbaza;
        public BazaPanj(string dbPath)
        {
            podatkovanbaza = new SQLiteAsyncConnection(dbPath);
            podatkovanbaza.CreateTableAsync<Panj>().Wait();
            podatkovanbaza.CreateTableAsync<PregledPanja>().Wait();

            podatkovanbaza.CreateTableAsync<SimboliZaMoc>().Wait();
            podatkovanbaza.CreateTableAsync<SimboliZaHrano>().Wait();
            Populate_Simbols();

            //podatkovanbaza.DropTableAsync<SimboliZaZalego>();
            //podatkovanbaza.DropTableAsync<SimboliZaHrano>();
        }

        private async void Populate_Simbols()
        {
            /*
            if (await podatkovanbaza.Table<SimboliZaZalego>().FirstOrDefaultAsync() == null)
            {
                await podatkovanbaza.InsertAsync(new SimboliZaZalego { Simbol_zalega_ID = 0, Simbol_zalega_icona = "N", Simbol_zalega_opis = "Normalna moč" });
                await podatkovanbaza.InsertAsync(new SimboliZaZalego { Simbol_zalega_ID = 1, Simbol_zalega_icona = "N-", Simbol_zalega_opis = "Slaba družina, potrebna pomoči" });
                await podatkovanbaza.InsertAsync(new SimboliZaZalego { Simbol_zalega_ID = 2, Simbol_zalega_icona = "N+", Simbol_zalega_opis = "Srednje močna družina" });
                await podatkovanbaza.InsertAsync(new SimboliZaZalego { Simbol_zalega_ID = 3, Simbol_zalega_icona = "N++", Simbol_zalega_opis = "Nadpovprečna družina" });
                await podatkovanbaza.InsertAsync(new SimboliZaZalego { Simbol_zalega_ID = 4, Simbol_zalega_icona = "N+++", Simbol_zalega_opis = "Zelo močna družina" });
            }
            */

            if (await podatkovanbaza.Table<SimboliZaMoc>().FirstOrDefaultAsync() == null)
            {
                await podatkovanbaza.InsertAsync(new SimboliZaMoc { Simbol_moc_ID = 0, Simbol_moc_icona = "M", Simbol_moc_opis = "Normalna moč" });
                await podatkovanbaza.InsertAsync(new SimboliZaMoc { Simbol_moc_ID = 1, Simbol_moc_icona = "M-", Simbol_moc_opis = "Slaba družina, potrebna pomoči" });
                await podatkovanbaza.InsertAsync(new SimboliZaMoc { Simbol_moc_ID = 2, Simbol_moc_icona = "M+", Simbol_moc_opis = "Srednje močna družina" });
                await podatkovanbaza.InsertAsync(new SimboliZaMoc { Simbol_moc_ID = 3, Simbol_moc_icona = "M++", Simbol_moc_opis = "Nadpovprečna družina" });
                await podatkovanbaza.InsertAsync(new SimboliZaMoc { Simbol_moc_ID = 4, Simbol_moc_icona = "M+++", Simbol_moc_opis = "Zelo močna družina" });
            }

            if (await podatkovanbaza.Table<SimboliZaHrano>().FirstOrDefaultAsync() == null)
            {
                await podatkovanbaza.InsertAsync(new SimboliZaHrano { Simbol_hrana_ID = 0, Simbol_hrana_icona = "H", Simbol_hrana_opis = "Hrana brez rezerve" });
                await podatkovanbaza.InsertAsync(new SimboliZaHrano { Simbol_hrana_ID = 1, Simbol_hrana_icona = "H-", Simbol_hrana_opis = "Premalo hrane" });
                await podatkovanbaza.InsertAsync(new SimboliZaHrano { Simbol_hrana_ID = 2, Simbol_hrana_icona = "H+", Simbol_hrana_opis = "Dovolj hrane" });
                await podatkovanbaza.InsertAsync(new SimboliZaHrano { Simbol_hrana_ID = 3, Simbol_hrana_icona = "H++", Simbol_hrana_opis = "Hrane nad potrebo" });
                await podatkovanbaza.InsertAsync(new SimboliZaHrano { Simbol_hrana_ID = 4, Simbol_hrana_icona = "H+++", Simbol_hrana_opis = "Hrana se lahko odvzame" });
            }
        }

        public Task<List<SimboliZaMoc>> Get_vsi_simboli_moc_async()
        {
            return podatkovanbaza.Table<SimboliZaMoc>().OrderBy(i => i.Simbol_moc_ID).ToListAsync();
        }

        public Task<List<SimboliZaHrano>> Get_vsi_simboli_hrana_async()
        {
            return podatkovanbaza.Table<SimboliZaHrano>().OrderBy(i => i.Simbol_hrana_ID).ToListAsync();
        }

        public Task<List<Panj>> GetVsiPanjiAsync()
        {
            //vrni vse panje
            return podatkovanbaza.Table<Panj>().OrderBy(i => i.StPanj).ToListAsync();
        }

        public Task<List<PregledPanja>> Get_vsi_pregledi_panja(int id_panja)
        {
            return podatkovanbaza.Table<PregledPanja>().Where(i => i.Pripada_panju == id_panja).ToListAsync();
        }

        public Task<Panj> GetPanjAsync(int idPanja)
        {
            return podatkovanbaza.Table<Panj>().Where(i => i.ID == idPanja).FirstOrDefaultAsync();
        } //vrni dolocen panj

        public Task<PregledPanja> Get_pregled_async(int idPregleda)
        {
            return podatkovanbaza.Table<PregledPanja>().Where(i => i.ID_pregleda == idPregleda).FirstOrDefaultAsync();
        }

        public async Task<ForDisplayMeritev> Get_display_meritev(PregledPanja p)
        {
            ForDisplayMeritev a = new ForDisplayMeritev();
            SimboliZaMoc moc = await podatkovanbaza.Table<SimboliZaMoc>().Where(i => i.Simbol_moc_ID == p.Moc_ob_pregledu).FirstOrDefaultAsync();
            SimboliZaHrano hrana = await podatkovanbaza.Table<SimboliZaHrano>().Where(i => i.Simbol_hrana_ID == p.Hrana_ob_pregledu).FirstOrDefaultAsync();

            if (moc != null && hrana != null)
            {
                a.Meritev_datum = p.Datum_pregleda;
                a.Meritev_zalega = p.Zalega_ob_pregledu.ToString();
                a.Meritev_moc = moc.Simbol_moc_icona;
                a.Meritev_hrana = hrana.Simbol_hrana_icona;
                a.Meritev_opis = p.Dodatni_zapiski;
                a.Meritev__pregled_ID = p.ID_pregleda.ToString();
            }
            
            return a;
        }

        public Task<int> SavePanjAsync(Panj p)
        {
            if (p.ID != 0)
            {
                //update panj
                return podatkovanbaza.UpdateAsync(p);
            }
            else
            {
                //ustvari nov panj
                return podatkovanbaza.InsertAsync(p);
            }
        }

        public Task<int> Save_pregled(PregledPanja p)
        {
            if (p.ID_pregleda != 0)
            {
                //update pregled
                return podatkovanbaza.UpdateAsync(p);
            }
            else
            {
                //ustvari nov pregled
                return podatkovanbaza.InsertAsync(p);
            }
        }

        public Task<int> DeletePanjAsync(Panj p, bool b)
        {
            if (b) podatkovanbaza.Table<PregledPanja>().Where(i => i.Pripada_panju == p.ID).DeleteAsync();

            //izbrisi panj
            return podatkovanbaza.DeleteAsync(p);
        }
        
        public Task<int> Delete_pregled_Async(PregledPanja p)
        {
            return podatkovanbaza.DeleteAsync(p);
        } //izbrisi pregled

    }
}
