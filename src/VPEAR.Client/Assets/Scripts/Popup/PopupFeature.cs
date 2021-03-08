using Fluxor;

public class PopupFeature : Feature<PopupState>
{
    public override string GetName()
    {
        return nameof(PopupState);
    }

    protected override PopupState GetInitialState()
    {
        return new PopupState("Title", "Message");
    }
}
