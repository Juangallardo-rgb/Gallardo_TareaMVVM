using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace JGNotas.JGViewModels;

internal class JGNoteViewModel : ObservableObject, IQueryAttributable
{
    private JGModels.JGNote _note;

    public string Text
    {
        get => _note.Text;
        set
        {
            if (_note.Text != value)
            {
                _note.Text = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime Date => _note.Date;

    public string Identifier => _note.Filename;

    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }

    public JGNoteViewModel()
    {
        _note = new JGModels.JGNote();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public JGNoteViewModel(JGModels.JGNote note)
    {
        _note = note;
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    private async Task Save()
    {
        _note.Date = DateTime.Now;
        _note.Save();
        await Shell.Current.GoToAsync($"..?saved={_note.Filename}");
    }

    private async Task Delete()
    {
        _note.Delete();
        await Shell.Current.GoToAsync($"..?deleted={_note.Filename}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _note = JGModels.JGNote.Load(query["load"].ToString());
            RefreshProperties();
        }
    }

    public void Reload()
    {
        _note = JGModels.JGNote.Load(_note.Filename);
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Text));
        OnPropertyChanged(nameof(Date));
    }
}