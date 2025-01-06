using CommunityToolkit.Mvvm.Input;
using JGNotas.JGModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace JGNotas.JGViewModels;

internal class JGNotesViewModel : IQueryAttributable
{
public ObservableCollection<JGViewModels.JGNoteViewModel> AllNotes { get; }
public ICommand NewCommand { get; }
public ICommand SelectNoteCommand { get; }

public JGNotesViewModel()
{
    AllNotes = new ObservableCollection<JGViewModels.JGNoteViewModel>(JGNotas.JGModels.JGNote.LoadAll().Select(n => new JGNoteViewModel(n)));
    NewCommand = new AsyncRelayCommand(NewNoteAsync);
    SelectNoteCommand = new AsyncRelayCommand<JGViewModels.JGNoteViewModel>(SelectNoteAsync);
}

private async Task NewNoteAsync()
{
    await Shell.Current.GoToAsync(nameof(JGViews.JGNotePage));
}

private async Task SelectNoteAsync(JGViewModels.JGNoteViewModel note)
{
    if (note != null)
        await Shell.Current.GoToAsync($"{nameof(JGViews.JGNotePage)}?load={note.Identifier}");
}

void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
{
    if (query.ContainsKey("deleted"))
    {
        string noteId = query["deleted"].ToString();
        JGNoteViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

        // If note exists, delete it
        if (matchedNote != null)
            AllNotes.Remove(matchedNote);
    }
    else if (query.ContainsKey("saved"))
    {
        string noteId = query["saved"].ToString();
        JGNoteViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

        // If note is found, update it
        if (matchedNote != null)
            {
                matchedNote.Reload();
                AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
            }

            // If note isn't found, it's new; add it.
            else
                AllNotes.Insert(0, new JGNoteViewModel(JGModels.JGNote.Load(noteId)));
        }
}
}