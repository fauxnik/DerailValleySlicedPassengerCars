using Harmony12;
using UnityEngine;

namespace SlicedPassengerCars
{
    public static class Patches
    {
        private static int patchedMask = 0;
        public static bool IsPatched(TrainCarType type) => (patchedMask & (1 << (int)type)) > 0;
        public static void SetPatched(TrainCarType type) => patchedMask |= 1 << (int)type;

        static bool IsPassengerCar(TrainCar car)
        {
            switch (car.carType)
            {
                case TrainCarType.PassengerBlue:
                case TrainCarType.PassengerGreen:
                case TrainCarType.PassengerRed:
                    return true;
                default:
                    return false;
            }
        }

        [HarmonyPatch(typeof(CarTypes), "GetCarPrefab")]
        public static class GetCarPrefabPatch
        {
            public static void Postfix(ref GameObject __result)
            {
                if (__result == null)
                    return;
                var car = __result.GetComponent<TrainCar>();
                if (IsPassengerCar(car) && !IsPatched(car.carType))
                {
                    SetPatched(car.carType);
                    PrefabModder.Modify(
                        __result,
                        meshReplacements: PassengerCarModifications.MeshReplacements,
                        transformations: PassengerCarModifications.Transformations);
                }
            }
        }
    }
}
