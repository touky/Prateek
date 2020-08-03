namespace DearImGui
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public static class DearImGuiIntegration
    {
        static DearImGuiIntegration() // static Constructor
        {
            var currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
#if UNITY_EDITOR
            var dllPath = Path.Combine(Application.dataPath, 
                "Library\\PackageCache\\com.realgames.dear-imgui@561339c07f", "Plugins", "x64");
#else // Player
            var dllPath = Path.Combine(Application.dataPath, "Plugins");
#endif
            Debug.Log(dllPath);
            if (currentPath != null && currentPath.Contains(dllPath) == false)
            {
                Debug.Log("Added to PATH");
                Environment.SetEnvironmentVariable("PATH", currentPath + Path.PathSeparator
                                                                       + dllPath, EnvironmentVariableTarget.Process);
            }
        }
    }
}