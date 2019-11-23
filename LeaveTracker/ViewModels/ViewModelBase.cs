using System.Threading.Tasks;
using LeaveTracker.Interfaces;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using LeaveTracker.Models;

namespace LeaveTracker.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible, IConfirmNavigation
    {
        #region Fields
        private string _title;
        private bool _isBusy;
        private string _ActivityIndicatorTitle;
        #endregion

        #region Properties
        protected INavigationService NavigationService { get; private set; }
        protected IFirebaseService FirebaseService { get; private set; }
        protected IPageDialogService DialogService { get; private set; }
        protected User UserDetails { get; private set; }
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }
        public string ActivityIndicatorTitle
        {
            get { return _ActivityIndicatorTitle; }
            set { SetProperty(ref _ActivityIndicatorTitle, value); }
        }
        #endregion

        #region Constructors
        public ViewModelBase(INavigationService navigationService, IFirebaseService firebaseService, IPageDialogService dialogService)
        {
            NavigationService = navigationService;
            FirebaseService = firebaseService;
            DialogService = dialogService;
        }

        public ViewModelBase(INavigationService navigationService, IFirebaseService firebaseService)
        {
            NavigationService = navigationService;
            FirebaseService = firebaseService;
        }

        public ViewModelBase(INavigationService navigationService, IPageDialogService dialogService)
        {
            NavigationService = navigationService;
            DialogService = dialogService;
        }
        #endregion

        #region Methods
        public virtual void Initialize(INavigationParameters parameters) { }

        public virtual void OnNavigatedFrom(INavigationParameters parameters) { }

        public virtual void OnNavigatedTo(INavigationParameters parameters) { }

        public virtual void Destroy() { }

        public virtual bool CanNavigate(INavigationParameters parameters) { return true; }
        #endregion
    }
}
