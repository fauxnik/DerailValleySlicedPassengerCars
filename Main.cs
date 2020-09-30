using Harmony12;
using System;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace SlicedPassengerCars
{
    public static class Main
    {
        public static Settings settings;
        
        public static bool OnLoad(UnityModManager.ModEntry modEntry)
        {
            try { settings = Settings.Load<Settings>(modEntry); } catch { }

            try { PassengerCarModifications.Initialize(modEntry.Path); }
            catch (Exception e)
            {
                LogError(e);
                return false;
            }

            var harmony = HarmonyInstance.Create(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;

            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry) { settings.Draw(modEntry); }
        static void OnSaveGUI(UnityModManager.ModEntry modEntry) { settings.Save(modEntry); }

        public static void Log(object message)
        {
            if (settings.verbose) { Debug.Log($"[SlicedPax] >>> {message}"); }
        }

        public static void LogWarning(object message)
        {
            Debug.LogWarning($"[SlicedPax] >>> {message}");
        }

        public static void LogError(object message)
        {
            Debug.LogError($"[SlicedPax] >>> {message}");
        }
    }
}
