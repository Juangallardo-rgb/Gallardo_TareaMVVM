namespace JGNotas.JGViews;

public partial class JGAllNotesPage : ContentPage
{
    public JGAllNotesPage()
    {
        InitializeComponent();
    }
    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        notesCollection.SelectedItem = null;
    }
}