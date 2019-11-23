using LeaveTracker.Interfaces;
using LeaveTracker.Models;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.ObjectModel;

namespace LeaveTracker.ViewModels
{
    public class LeaveViewerPageViewModel : ViewModelBase
    {
        #region Fields
        private string _month;
        private string _year;
        private bool _isVisible;
        private ObservableCollection<CalendarItem> _listItems;
        #endregion

        #region Properties
        public string Month
        {
            get { return _month; }
            set
            {
                SetProperty(ref _month, value);
                ViewCommand.RaiseCanExecuteChanged();
            }
        }
        public string Year
        {
            get { return _year; }
            set
            {
                SetProperty(ref _year, value);
                ViewCommand.RaiseCanExecuteChanged();
            }
        }
      
        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }

        public DelegateCommand ViewCommand { get; private set; }
        public DelegateCommand<CalendarItem> ItemTappedCommand { get; private set; }

        public ObservableCollection<CalendarItem> ListItems
        {
            get { return _listItems; }
            set { SetProperty(ref _listItems, value); }
        }
        #endregion

        public LeaveViewerPageViewModel(INavigationService navigationService, IFirebaseService firebaseService, IPageDialogService dialogService)
            : base(navigationService, firebaseService, dialogService)
        {
            Title = "View Leaves";

            ViewCommand = new DelegateCommand(ViewLeaves, CanView);
            ItemTappedCommand = new DelegateCommand<CalendarItem>(DisplayLeaveDetails);
            IsVisible = true;
        }

        #region Methods
        private bool CanView()
        {
            if (string.IsNullOrEmpty(_month) || string.IsNullOrEmpty(_year))
                return false;
            return true;
        }

        private async void ViewLeaves()
        {
            try
            {
                //Todo----Optimize below code to run in lesser time.
                //todo----add suitable catch statement for this try.
                IsBusy = true;
                IsVisible = false;
                ActivityIndicatorTitle = "Please wait while data is being loaded.";

                ListItems = new ObservableCollection<CalendarItem>();

                var x = await FirebaseService.GetAll(_month, _year);

                foreach (var item in x)
                {
                    ListItems.Add(item);
                }
            }
            finally
            {
                IsBusy = false;
                Title = string.Join("\t", Title, string.Concat("(", _month, ' ', _year, ")"));
            }
        }

        private async void DisplayLeaveDetails(CalendarItem calendarItem)
        {
            INavigationParameters navParams = new NavigationParameters();
            navParams.Add("details", calendarItem);
            await NavigationService.NavigateAsync("LeaveDetailPage", navParams);
        }
        #endregion
    }
}
