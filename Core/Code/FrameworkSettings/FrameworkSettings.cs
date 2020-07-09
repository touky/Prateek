using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for framework settings.
/// Use templated version
/// </summary>
public abstract class FrameworkSettings
{
    public const string DEFAULT_PATH = "Settings/";

    public abstract bool IsAvailable { get; }

    internal void InternalInit()
    {
        Init();
    }

    protected abstract void Init();
}

/// <summary>
/// Base class to derive from to create internal settings for you own systems
/// </summary>
public abstract class FrameworkSettings<TSettings> : FrameworkSettings
    where TSettings : FrameworkSettings, new()
{
    private static TSettings instance;

    public TSettings Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TSettings();
                instance.InternalInit();
            }

            return instance;
        }
    }

    protected TResource LoadResource<TResource>(string path)
        where TResource : ScriptableObject
    {
        var resource = Resources.Load<TResource>(path);
        if (resource == null)
        {
            Debug.LogError($"Couldn't load settings for {nameof(TSettings)} at path {path}");
        }

        return resource;
    }
}
