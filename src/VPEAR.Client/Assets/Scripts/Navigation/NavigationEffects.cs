using Fluxor;
using System.Threading.Tasks;

public class NavigateBackEffect : Effect<NavigateBackAction>
{
    private readonly NavigationService _navigationService;

    public NavigateBackEffect(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public override Task HandleAsync(NavigateBackAction action, IDispatcher dispatcher)
    {
        _navigationService.NavigateBack();
        dispatcher.Dispatch(new NavigateToAction(_navigationService.LocationName));

        return Task.CompletedTask;
    }
}

public class NavigateToEffect : Effect<NavigateToAction>
{
    private readonly NavigationService _navigationService;

    public NavigateToEffect(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public override Task HandleAsync(NavigateToAction action, IDispatcher dispatcher)
    {
        _navigationService.NavigateTo(action.NextView);

        return Task.CompletedTask;
    }
}

public class ChangeSceneEffect : Effect<ChangeSceneAction>
{
    private readonly NavigationService _navigationService;

    public ChangeSceneEffect(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public override Task HandleAsync(ChangeSceneAction action, IDispatcher dispatcher)
    {
        _navigationService.ChangeScene(action.SceneName);

        return Task.CompletedTask;
    }
}
