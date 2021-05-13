using UnityEngine.Events;

public class ShowPopupAction
{
    public ShowPopupAction(string title, string message, UnityAction action = default)
    {
        Title = title;
        Message = message;
        Action = action;
    }

    public string Title { get; }

    public string Message { get; }

    public UnityAction Action { get; }
}

public class ClosePopupAction
{
}
