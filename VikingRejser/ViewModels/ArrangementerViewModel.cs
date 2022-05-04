using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VikingRejser.ViewModels
{
    internal class ArrangementerViewModel : BindableBase
    {
        #region fields
        private string mode = "all";
        private List<Rejsearrangementer> sourceDG;
        private Rejsearrangementer selectedItem;
        private string ttlText;
        private string byText;
        private DateTime strDt;
        private DateTime sltDt;
        private string prText;
        private string mxText;
        private string beText;
        private string trTlf;
        private readonly VikingDBEntities _db = new VikingDBEntities();
        #endregion

        #region properties
        public MyICommand<string> UCCommand { get; private set; }
        public MyICommand<string> CrCommand { get; private set; }
        public MyICommand<string> ModeCommand { get; private set; }
        public List<Rejsearrangementer> SourceDG { get => sourceDG; set => SetProperty(ref sourceDG, value); }
        public Rejsearrangementer SelectedItem
        {
            get => selectedItem;
            set
            {
                if (value != null)
                {
                    SetProperty(ref selectedItem, value);
                    TtlText = value.Titel;
                    ByText = value.By;
                    StrDt = value.Startdato;
                    SltDt = value.Slutdato;
                    PrText = value.Pris.ToString();
                    MxText = value.MaxAntal.ToString();
                    BeText = value.Beskrivelse;
                    if (value.Transportør != null)
                    {
                        TrTlf = _db.Transportører.Where(m => m.Id == value.Transportør).Single().Telefon;
                    }
                }
            }
        }
        public string TtlText { get => ttlText; set => SetProperty(ref ttlText, value); }
        public string ByText { get => byText; set => SetProperty(ref byText, value); }
        public DateTime StrDt { get => strDt; set => SetProperty(ref strDt, value); }
        public DateTime SltDt { get => sltDt; set => SetProperty(ref sltDt, value); }
        public string PrText { get => prText; set => SetProperty(ref prText, value); }
        public string MxText { get => mxText; set => SetProperty(ref mxText, value); }
        public string BeText { get => beText; set => SetProperty(ref beText, value); }
        public string TrTlf { get => trTlf; set => SetProperty(ref trTlf, value); }
        #endregion

        public ArrangementerViewModel()
        {
            SetSource(mode);
            UCCommand = new MyICommand<string>(UpCre);
            ModeCommand = new MyICommand<string>(SetSource);
        }

        private void SetSource(string mod)
        {
            mode = mod;
            switch (mod)
            {
                case "all":
                    SourceDG = _db.Rejsearrangementers.ToList();
                    break;
                case "slut":
                    SourceDG = _db.Rejsearrangementers.Where(m => m.Slutdato.CompareTo(DateTime.Now) <= 0).ToList();
                    break;
                case "start":
                    SourceDG = _db.Rejsearrangementers.Where(m => m.Startdato.CompareTo(DateTime.Now) <= 0 && m.Slutdato.CompareTo(DateTime.Now) > 0).ToList();
                    break;
                case "ustart":
                    SourceDG = _db.Rejsearrangementers.Where(m => m.Startdato.CompareTo(DateTime.Now) > 0).ToList();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Method for updating a old arrangement or creating a new arrangement
        /// </summary>
        /// <param name="rejs">If add makes this create else it updates</param>
        private void UpCre(string rejs)
        {
            //Checks to see if input is valid
            if (SelectedItem == null && rejs != "add")
            {
                _ = MessageBox.Show("Du skal vælge et arrangement før du kan opdaterer", "Igen arrangement valgt", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(TtlText))
            {
                _ = MessageBox.Show("Der skal værre en titel", "ingen titel", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(ByText))
            {
                _ = MessageBox.Show("Der skal værre en by", "ingen by", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (StrDt == null)
            {
                _ = MessageBox.Show("Der skal værre en start dato", "ingen start dato", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (SltDt == null)
            {
                _ = MessageBox.Show("Der skal værre en slut dato", "ingen slut dato", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(PrText))
            {
                _ = MessageBox.Show("Der skal værre en pris", "ingen pris", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                _ = int.Parse(PrText);
            }
            catch (Exception)
            {
                _ = MessageBox.Show("Pris kan kun værre tal", "pris ikke kun tal", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!string.IsNullOrWhiteSpace(MxText))
            {
                try
                {
                    _ = int.Parse(MxText);
                }
                catch (Exception)
                {
                    _ = MessageBox.Show("Max antal kan kun værre tal", "max antal ikke kun tal", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            if (string.IsNullOrWhiteSpace(BeText))
            {
                _ = MessageBox.Show("Der skal værre en beskrivelse", "ingen beskrivelse", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (rejs == "add")
            {
                _ = _db.Rejsearrangementers.Add(new Rejsearrangementer()
                {
                    Titel = TtlText,
                    By = ByText,
                    Startdato = StrDt,
                    Slutdato = SltDt,
                    Pris = int.Parse(PrText),
                    MaxAntal = !string.IsNullOrWhiteSpace(MxText) ? int.Parse(MxText) : (int?)null,
                    Beskrivelse = BeText,
                    Transportør = !string.IsNullOrWhiteSpace(TrTlf) ? _db.Transportører.Where(m => m.Telefon == TrTlf).Single().Id : (int?)null
                });
                _ = _db.SaveChanges();
                SetSource(mode);
                return;
            }
            Rejsearrangementer rejser = (from m in _db.Rejsearrangementers
                                         where m.Id == SelectedItem.Id
                                         select m).Single();
            rejser.Titel = TtlText;
            rejser.By = ByText;
            rejser.Startdato = StrDt;
            rejser.Slutdato = SltDt;
            rejser.Pris = int.Parse(PrText);
            rejser.MaxAntal = !string.IsNullOrWhiteSpace(MxText) ? int.Parse(MxText) : rejser.MaxAntal;
            rejser.Beskrivelse = BeText;
            rejser.Transportør = !string.IsNullOrWhiteSpace(TrTlf) ? _db.Transportører.Where(m => m.Telefon == TrTlf).Single().Id : rejser.Transportør;
            _ = _db.SaveChanges();
            SetSource(mode);
        }
    }
}
