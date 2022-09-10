using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SchoolComputerControl.Client.Models.Settings;

public class FiredTasksSettings : Dictionary<Guid, DateTime>, INotifyPropertyChanged
{
    public new void Add(Guid key, DateTime value)
    {
        base.Add(key, value);
        OnPropertyChanged();
    }

    public new DateTime this[Guid key]
    {
        get => base[key];
        set
        {
            base[key] = value;
            OnPropertyChanged();
        }
    }

    public new void Remove(Guid key)
    {
        base.Remove(key);
        OnPropertyChanged();
    }
    
    public new void Clear()
    {
        base.Clear();
        OnPropertyChanged();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}