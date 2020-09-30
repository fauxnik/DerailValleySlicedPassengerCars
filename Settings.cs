using UnityEngine;
using UnityModManagerNet;

namespace SlicedPassengerCars
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw(Label = "Verbose Logging", Type = DrawType.Toggle)]
        public bool verbose =
#if DEBUG
            true;
#else
            false;
#endif

        override public void Save(UnityModManager.ModEntry entry)
        {
            Save<Settings>(this, entry);
        }

        public void OnChange() { }
    }
}