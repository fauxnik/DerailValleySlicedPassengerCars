using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SlicedPassengerCars
{
    public static class PassengerCarModifications
    {
        public static List<PrefabModder.MeshReplacement> MeshReplacements { get; private set; }
        public static Vector3 ChildrenTranslationAroundParent { get => new Vector3(0, 0, -0.298f); }

        private static string[] meshNames = new string[]
        {
            "car_passenger_interior_LOD1",
            "car_passenger_interior_LOD2",
            "car_passenger_LOD1",
            "car_passenger_LOD2",
            "car_passenger_LOD3",
            "exterior",
            "front doors",
            //"interior",
            "side doors",
            "windows",
            "windows_int",
        };

        public static void Initialize(string modPath)
        {
            Main.Log("Initializing passenger car modifications...");

            MeshReplacements = new List<PrefabModder.MeshReplacement>();

            //Main.Log($"modPath={modPath}");
            AssetBundle bundle = AssetBundle.LoadFromFile(modPath + "Resources/slicedpassengercar");

            //Main.Log($"meshNames=[ {string.Join(", ", meshNames)} ]");
            foreach (var name in meshNames)
            {
                //Main.Log($"A name={name}");
                var go = bundle.LoadAsset<GameObject>($"Assets/Models/SlicedPax/{name}.fbx");
                //Main.Log($"B go={go}");
                var mf = go.GetComponent<MeshFilter>();
                var smr = go.GetComponent<SkinnedMeshRenderer>();
                //Main.Log($"C mf={mf} smr={smr}");
                var sm = mf.sharedMesh;
                //Main.Log($"D sm={sm}");
                MeshReplacements.Add(new PrefabModder.MeshReplacement(name, sm));
            }

            // TODO: wtf is wrong with the interior export?
            MeshReplacements.Add(
                new PrefabModder.MeshReplacement(
                    "interior",
                    bundle.LoadAsset<GameObject>($"Assets/Models/SlicedPax/car_passenger_interior_LOD1.fbx")
                        .GetComponent<MeshFilter>()
                        .sharedMesh));

            bundle.Unload(false);

            Main.Log("Done");
        }
    }
}
