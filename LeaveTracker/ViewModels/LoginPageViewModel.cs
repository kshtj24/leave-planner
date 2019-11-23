using LeaveTracker.Interfaces;
using LeaveTracker.Models;
using Prism;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace LeaveTracker.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        #region Fields
        private string _userId;
        private string _password;
        #endregion

        #region Properties
        public DelegateCommand LoginCommand { get; private set; }

        public string UserID
        {
            get { return _userId; }
            set
            {
                SetProperty(ref _userId, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        public LoginPageViewModel(INavigationService navigationService, IFirebaseService firebaseService, IPageDialogService dialogService)
            : base(navigationService, firebaseService, dialogService)
        {
            Title = "Log In";
            LoginCommand = new DelegateCommand(Login, CanLogin);
        }

        #region Methods
        private async void Login()
        {
            try
            {
                ActivityIndicatorTitle = "Logging In. Please wait.";
                IsBusy = true;

                User userObject = PrismApplicationBase.Current.Container.Resolve(typeof(User)) as User;

                userObject.UserID = _userId;
                userObject.Password = _password;
                userObject.UserName = await FirebaseService.GetUserNameAsync(_userId);

                if (!string.IsNullOrEmpty(await FirebaseService.LoginAsync(userObject)))
                {
                    await NavigationService.NavigateAsync("MainPage");
                }
                else
                {
                    await DialogService.DisplayAlertAsync("Log in failed !", "Incorrect ID or password. Please try again.", "OK");
                }
            }

            finally
            {
                IsBusy = false;
            }
        }

        private bool CanLogin()
        {
            if (string.IsNullOrEmpty(_userId) || string.IsNullOrEmpty(_password))
                return false;
            return true;
        }
        #endregion
    }
}
