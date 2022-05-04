using VikingRejser.ViewModels;
using VikingRejser.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingRejser
{
    internal class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            NavCommand = new MyICommand<string>(OnNav);
        }

        private readonly KunderViewModel kundViewModel = new KunderViewModel();
        private readonly TransportViewModel transportViewModel = new TransportViewModel();
        private readonly ArrangementerViewModel arrangementerViewModel = new ArrangementerViewModel();

        private BindableBase _CurrentViewModel;
        public BindableBase CurrentViewModel
        {
            get => _CurrentViewModel;
            set => SetProperty(ref _CurrentViewModel, value);
        }

        //NavCommand for Calling OnNav
        public MyICommand<string> NavCommand { get; private set; }

        /// <summary>
        /// Sets CurrentViewModel to specific view model based on destination does nothing if destination is unacounted for
        /// </summary>
        /// <param name="destination"></param>
        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "kunder":
                    CurrentViewModel = kundViewModel;
                    break;
                case "trans":
                    CurrentViewModel = transportViewModel;
                    break;
                case "arrang":
                    CurrentViewModel = arrangementerViewModel;
                    break;
                default:
                    break;
            }
        }
    }
}
