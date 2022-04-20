/**
 * Copyright 2020 Niko Fox
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

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
