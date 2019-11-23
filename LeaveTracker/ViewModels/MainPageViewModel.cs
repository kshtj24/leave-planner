using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace LeaveTracker.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Properties
        public DelegateCommand ApplyLeaveNavCommand { get; private set; }
        public DelegateCommand ViewLeavesNavCommand { get; private set; }
        public DelegateCommand AddUserNavCommand { get; private set; }
        #endregion

        public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            Title = "Main Page";

            ApplyLeaveNavCommand = new DelegateCommand(ApplyLeave);
            ViewLeavesNavCommand = new DelegateCommand(ViewLeaves);
            AddUserNavCommand = new DelegateCommand(AddUser);
        }

        #region Methods
        private async void ApplyLeave()
        {
            await NavigationService.NavigateAsync("LeaveApplyPage");
        }

        private async void ViewLeaves()
        {
            await NavigationService.NavigateAsync("LeaveViewerPage");
        }

        private async void AddUser()
        {
            await NavigationService.NavigateAsync("AddUserPage");
        }

        public override bool CanNavigate(INavigationParameters parameters)
        {
            if(parameters.GetNavigationMode() == NavigationMode.Back)
            {
                DialogService.DisplayAlertAsync("title", "message", "OK");
            }

            return true;
        }
        #endregion
    }
}
