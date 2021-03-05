using UnityEngine.Events;

public class ErrorAction
{
    public string Title { get; protected set; }

    public string Message { get; protected set; }

    public UnityAction Action { get; protected set; }
}
