namespace Calculator
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());

#if WINDOWS
            // iOS calculator aspect ratio about 0.56
            const int width = 380;
            const int height = 680;

            window.Width = width;
            window.Height = height;

            window.MinimumWidth = width;
            window.MinimumHeight = height;

            window.MaximumWidth = width;
            window.MaximumHeight = height;
#endif

            return window;
        }
    }
}
