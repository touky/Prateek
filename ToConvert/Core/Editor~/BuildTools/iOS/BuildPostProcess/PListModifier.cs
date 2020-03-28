#if UNITY_IOS
namespace Mayfair.Core.Editor.BuildTools.iOS.BuildPostProcess
{
    using System.IO;
    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEditor.iOS.Xcode;
    using UnityEngine;

    public class PListModifier
    {
        [PostProcessBuild(1)]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuiltProject)
        {
            // Keys correspond to values expected in the Info.plist file
            const string EXITS_ON_SUSPEND_KEY = "UIApplicationExitsOnSuspend";
            const string ENCRYPT_KEY = "ITSAppUsesNonExcemptEncryption";

            Debug.Log("[PListModifier] Starting to modify Plist");

            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = ReadPlistFromFile(plistPath);

            PlistElementDict rootDictionary = plist.root;
            rootDictionary.SetBoolean(ENCRYPT_KEY, false);

            TryToRemoveValueFromPlist(rootDictionary, EXITS_ON_SUSPEND_KEY);

            File.WriteAllText(plistPath, plist.WriteToString());
        }

        public static void TryToRemoveValueFromPlist(PlistElementDict rootDictionary, string key)
        {
            if (rootDictionary.values.ContainsKey(key))
            {
                Debug.Log("[PostProcessBuild] Removing " + key + " from info.plist");
                rootDictionary.values.Remove(key);
            }
            else
            {
                Debug.LogWarning("[PostProcessBuild] Could not find " + key + " in info.plist");
            }
        }

        public static PlistDocument ReadPlistFromFile(string plistPath)
        {
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            return plist;
        }
    }
}
#endif