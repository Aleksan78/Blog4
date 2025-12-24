using Blog2.BLL.ViewModels.User;


namespace Blog2.BLL.ViewModels
{
    public class MainViewModel
    {
        public UserRegisterViewModel RegisterView { get; set; } = new();

        public UserLoginViewModel LoginView { get; set; } = new();

    }
}
