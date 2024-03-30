using WorkHours.Services;

namespace WorkHours.Views;

public partial class LoadingView : ContentPage
{
    private readonly AuthUserService _authUserService;

    public LoadingView(AuthUserService authUserService)
    {
        _authUserService = authUserService;
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (_authUserService.CheckUserAuth())
            Shell.Current.GoToAsync("//MainApp");
        else
            Shell.Current.GoToAsync("//LoginView");
    }
}