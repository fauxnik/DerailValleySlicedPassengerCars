using Harmony12;
using UnityEngine;

namespace SlicedPassengerCars
{
    public static class Patches
    {
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
                if (IsPassengerCar(car) && __result.Find("interior")?.GetComponent<MeshFilter>()?.sharedMesh?.name?.Contains("LOD") != true)
                {
                    /*PrefabModder.Modify(
                        __result,
                        meshReplacements: PassengerCarModifications.MeshReplacements,
                        relativeAdjustment: PassengerCarModifications.ChildrenTranslationAroundParent);*/
                    PrefabModder.Modify(
                        __result,
                        meshReplacements: PassengerCarModifications.MeshReplacements,
                        transformations: PassengerCarModifications.Transformations);
                }
            }
        }
    }
}
