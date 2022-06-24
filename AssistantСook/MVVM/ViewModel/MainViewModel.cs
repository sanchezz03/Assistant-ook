using System;
using AssistantСook.Core;

namespace AssistantСook.MVVM.ViewModel
{
     class MainViewModel: ObservableObject
    {
        public RelayCommand StockViewCommand { get; set; }

        public RelayCommand CookBookViewCommand { get; set; }

        public RelayCommand OrdersViewCommand { get; set; }
        public StockViewModel StockVM { get; set; }
        public CookBookViewModel CookBookVM { get; set; }
        public OrdersViewModel OrdersVM { get; set; }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value;
                OnPropertyChanged();
            }

        }

        public MainViewModel()
        {
            StockVM = new StockViewModel();
            CookBookVM = new CookBookViewModel();
            OrdersVM = new OrdersViewModel();

            CurrentView = StockVM;

            StockViewCommand = new RelayCommand(o =>
            {
                CurrentView = StockVM;
            });

            CookBookViewCommand = new RelayCommand(o =>
            {
                CurrentView = CookBookVM;
            });

            OrdersViewCommand = new RelayCommand(o =>
            {
                CurrentView = OrdersVM;
            });
        }
    }
}
