using MaterialDesignThemes.Wpf;
using Mv.Shell.Views.Authentication;
using Mv.Ui.Mvvm;
using System.Windows.Controls;
using Unity;

namespace Mv.Shell.ViewModels.Authentication
{
    public class AuthenticationWindowViewModel : ViewModelBase, IViewLoadedAndUnloadedAware<AuthenticationWindow>, INotificable
    {
        private bool _isLoading;
        private TabItem _signInTabItem;


        public AuthenticationWindowViewModel(IUnityContainer container) : base(container)
        {
            this.GlobalMessageQueue = container.Resolve<ISnackbarMessageQueue>();
            EventAggregator.GetEvent<MainWindowLoadingEvent>().Subscribe(e => IsLoading = e);
            EventAggregator.GetEvent<SignUpSuccessEvent>().Subscribe(signUpInfo => _signInTabItem.IsSelected = true); // It cannot be done by binding the IsSelected property, it will cause an animation error.

        }


        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public void OnLoaded(AuthenticationWindow view)
        {
            _signInTabItem = view.SignInTabItem;
        }

        public void OnUnloaded(AuthenticationWindow view) { }
        public ISnackbarMessageQueue GlobalMessageQueue { get; set; }
    }
}
