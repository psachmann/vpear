using Fluxor;
using System.Threading.Tasks;

public class LoginEffects : Effect<LoginAction>
{
    public override Task HandleAsync(LoginAction action, IDispatcher dispatcher)
    {
        throw new System.NotImplementedException();
    }
}
