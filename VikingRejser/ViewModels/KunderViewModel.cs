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
    internal class KunderViewModel : BindableBase
    {
        private bool all = true;
        private readonly VikingDBEntities _db = new VikingDBEntities();
        private string _nvnText;
        public string NvnText
        {
            get => _nvnText;
            set => SetProperty(ref _nvnText, value);
        }
        private string _adrText;
        public string AdrText
        {
            get => _adrText;
            set => SetProperty(ref _adrText, value);
        }
        private string _tlfText;
        public string TlfText
        {
            get => _tlfText;
            set => SetProperty(ref _tlfText, value);
        }
        private Kunder _selectedItem;
        public Kunder SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedItem, value);
                    NvnText = value.Navn;
                    TlfText = value.Telefon;
                    AdrText = value.Adresse;
                }
            }
        }

        public MyICommand<Kunder> UpCommand { get; private set; }
        /// <summary>
        /// Command for updating an old customer
        /// </summary>
        /// <param name="kunder"></param>
        private void Update(Kunder kunder)
        {
            if (SelectedItem == null)
            {
                _ = MessageBox.Show("Du skal vælge en person før du kan opdaterer", "Ingen person valgt", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(NvnText))
            {
                _ = MessageBox.Show("Der skal verre navn", "ingen navn", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(AdrText))
            {
                _ = MessageBox.Show("Der skal verre Adresse", "ingen Adresse", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(TlfText))
            {
                _ = MessageBox.Show("Der skal verre Telefon", "ingen Telefon", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (_db.Kunders.Where(m => m.Telefon == TlfText.Trim()).Count() > 0 && SelectedItem.Telefon != TlfText.Trim())
            {
                _ = MessageBox.Show("To personer kan ikke have det samme nummer", "Ikke Unikt nummer", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Kunder kunde = (from m in _db.Kunders
                            where m.Id == SelectedItem.Id
                            select m).Single();
            kunde.Adresse = AdrText.Trim();
            kunde.Navn = NvnText.Trim();
            kunde.Telefon = TlfText.Trim();
            _ = _db.SaveChanges();
            SetSource();
        }

        public MyICommand<string> CrCommand { get; private set; }
        /// <summary>
        /// Command for add a new customer
        /// </summary>
        /// <param name="s"></param>
        private void Create(string s)
        {
            if (string.IsNullOrWhiteSpace(NvnText))
            {
                _ = MessageBox.Show("Der skal verre navn", "ingen navn", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(AdrText))
            {
                _ = MessageBox.Show("Der skal verre Adresse", "ingen Adresse", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(TlfText))
            {
                _ = MessageBox.Show("Der skal verre Telefon", "ingen Telefon", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (_db.Kunders.Where(m => m.Telefon == TlfText.Trim()).Count() > 0)
            {
                _ = MessageBox.Show("To personer kan ikke have det samme nummer", "Ikke Unikt nummer", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _ = _db.Kunders.Add(new Kunder()
            {
                Navn = NvnText.Trim(),
                Adresse = AdrText.Trim(),
                Telefon = TlfText.Trim(),
                RejsearrangementId = "0"
            });
            _ = _db.SaveChanges();
            SetSource();
        }

        public MyICommand<string> MrOneCommand { get; private set; }
        /// <summary>
        /// Command for making it more than one
        /// </summary>
        /// <param name="s"></param>
        private void MrOne(string s)
        {
            if (all)
            {
                all = false;
                SetSource();
                return;
            }
            all = true;
            SetSource();
        }
        private List<Kunder> _sourceDG;
        public List<Kunder> SourceDG
        {
            get => _sourceDG;
            set => SetProperty(ref _sourceDG, value);
        }

        public KunderViewModel()
        {
            SetSource();
            UpCommand = new MyICommand<Kunder>(Update);
            CrCommand = new MyICommand<string>(Create);
            MrOneCommand = new MyICommand<string>(MrOne);
        }

        /// <summary>
        /// Method for updating the source
        /// </summary>
        private void SetSource()
        {
            SourceDG = all ? _db.Kunders.ToList() : _db.Kunders.Where(m => m.Tilmeldingers.Count > 1).ToList();
        }
    }
}
