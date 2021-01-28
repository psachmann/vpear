using System;
using UnityEngine;

public abstract class AbstractBase : MonoBehaviour, IDisposable
{
    static AbstractBase()
    {
        ConfigureLogger();
        ConfigureClient();
    }

    private static void ConfigureLogger()
    {
        // throw new NotImplementedException();
    }

    private static void ConfigureClient()
    {
        // throw new NotImplementedException();
    }


    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
