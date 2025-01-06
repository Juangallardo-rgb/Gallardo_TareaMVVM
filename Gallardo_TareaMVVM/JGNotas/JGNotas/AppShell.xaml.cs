namespace JGNotas
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(JGViews.JGNotePage), typeof(JGViews.JGNotePage));
        }
    }
}
