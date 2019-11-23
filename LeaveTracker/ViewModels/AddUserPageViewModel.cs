using LeaveTracker.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace LeaveTracker.ViewModels
{
    public class AddUserPageViewModel : ViewModelBase
    {
        #region Fields        
        private string _userName;
        private string _password;
        private string _userId;
        #endregion

        #region Properties        
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); RaisePropertyChanged(); }
        }
        public string UserID
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); RaisePropertyChanged(); }
        }
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); RaisePropertyChanged(); }
        }

        public DelegateCommand AddUserCommand { get; private set; }
        #endregion

        public AddUserPageViewModel(INavigationService navigationService, IFirebaseService firebaseService, IPageDialogService dialogService)
            : base(navigationService, firebaseService, dialogService)
        {
            Title = "Add new user";
            AddUserCommand = new DelegateCommand(AddUser, CanAddUser);
        }

        #region Methods
        private async void AddUser()
        {
            try
            {
                ActivityIndicatorTitle = "Adding user. Please wait.";
                IsBusy = true;

                var result = await FirebaseService.AddUserAsync(_userName, _userId, _password);

                if (result)
                    await DialogService.DisplayAlertAsync("Success!", "User successfully added.", "OK");
                else
                    await DialogService.DisplayAlertAsync("Error", "Error occurred while adding user. Please try again.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanAddUser()
        {
            return true;
        }
        #endregion
    }
}