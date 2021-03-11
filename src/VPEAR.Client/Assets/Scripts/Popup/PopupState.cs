using UnityEngine.Events;

public class PopupState
{
    public PopupState(
        string title,
        string message,
        UnityAction action = default,
        bool isVisible = default)
    {
        Title = title;
        Message = message;
        Action = action;
        IsVisible = isVisible;
    }

    public bool IsVisible { get; }

    public string Title { get; }

    public string Message { get; }

    public UnityAction Action { get; }
}
