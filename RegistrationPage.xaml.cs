using Firebase.Auth;
using Firebase.Database;
using UporabniskiVmesnik.ViewModels;


namespace blitz_chat;

public partial class RegistrationPage : ContentPage
{
    public RegistrationPage()
	{
		InitializeComponent();
        BindingContext = new ViewModels.RegistrationViewModel(Navigation);
    }


}