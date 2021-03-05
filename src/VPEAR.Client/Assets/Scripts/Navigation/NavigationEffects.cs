using Fluxor;
using Serilog;
using System;
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
        dispatcher.Dispatch(new NavigateToAction(_navigationService.CurrentViewName));

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
