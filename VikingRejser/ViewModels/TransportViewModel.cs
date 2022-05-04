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
    internal class TransportViewModel : BindableBase
    {
        //Value for showing all or those with more than 4
        private bool all = true;
        private readonly VikingDBEntities _db = new VikingDBEntities();
        public TransportViewModel()
        {
            SetSource();
            UpCommand = new MyICommand<Transportører>(Update);
            CrCommand = new MyICommand<string>(Create);
            MrOneCommand = new MyICommand<string>(MrOne);
        }

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

        private string _bemText;
        public string BemText
        {
            get => _bemText;
            set => SetProperty(ref _bemText, value);
        }

        private Transportører _selectedItem;
        public Transportører SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedItem, value);
                    NvnText = value.Navn;
                    AdrText = value.Adresse;
                    TlfText = value.Telefon;
                    BemText = value.Bemærkninger;
                }
            }
        }

        private List<Transportører> _sourceDG;
        public List<Transportører> SourceDG
        {
            get => _sourceDG;
            set => SetProperty(ref _sourceDG, value);
        }

        /// <summary>
        /// Sets the Source to all or only those that have more than 4 arangements
        /// </summary>
        private void SetSource()
        {
            SourceDG = all ? _db.Transportører.ToList() : _db.Transportører.Where(m => m.Rejsearrangementers.Count > 4).ToList();
        }

        public MyICommand<string> MrOneCommand { get; private set; }
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

        public MyICommand<string> CrCommand { get; private set; }
        /// <summary>
        /// Command for creating a new transport
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
            if (string.IsNullOrWhiteSpace(BemText))
            {
                _ = MessageBox.Show("Der skal verre en bemærkning", "Ingen bemærkning", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (_db.Transportører.Where(m => m.Telefon == TlfText.Trim()).Count() > 0)
            {
                _ = MessageBox.Show("To Transportører kan ikke have det samme nummer", "Ikke Unikt nummer", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _ = _db.Transportører.Add(new Transportører()
            {
                Navn = NvnText.Trim(),
                Adresse = AdrText.Trim(),
                Telefon = TlfText.Trim(),
                Bemærkninger = BemText.Trim()
            });
            _ = _db.SaveChanges();
            SetSource();
        }

        public MyICommand<Transportører> UpCommand { get; private set; }
        /// <summary>
        /// Command for updating an old transport
        /// </summary>
        /// <param name="transportører"></param>
        private void Update(Transportører transportører)
        {
            if (SelectedItem == null)
            {
                _ = MessageBox.Show("Du skal vælge en transportør før du kan opdaterer", "Ingen transportør valgt", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (string.IsNullOrWhiteSpace(BemText))
            {
                _ = MessageBox.Show("Der skal verre en bemærkning", "Ingen bemærkning", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (_db.Transportører.Where(m => m.Telefon == TlfText.Trim()).Count() > 0 && SelectedItem.Telefon != TlfText.Trim())
            {
                _ = MessageBox.Show("To Transportører kan ikke have det samme nummer", "Ikke Unikt nummer", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Transportører transportører1 = (from m in _db.Transportører
                                            where m.Id == SelectedItem.Id
                                            select m).Single();
            transportører1.Adresse = AdrText.Trim();
            transportører1.Navn = NvnText.Trim();
            transportører1.Telefon = TlfText.Trim();
            transportører1.Bemærkninger = BemText.Trim();
            _ = _db.SaveChanges();
            SetSource();
        }
    }
}
