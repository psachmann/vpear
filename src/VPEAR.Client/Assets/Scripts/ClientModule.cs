using Autofac;
using System.Net.Http;
using VPEAR.Core;
using VPEAR.Core.Abstractions;

internal class ClientModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.Register(_ => new HttpClient())
            .AsSelf()
            .SingleInstance();

        builder.Register(context => new VPEARClient(
                Constants.ServerBaseAddress,
                context.Resolve<HttpClient>()))
            .As<IVPEARClient>()
            .SingleInstance();
    }
}
