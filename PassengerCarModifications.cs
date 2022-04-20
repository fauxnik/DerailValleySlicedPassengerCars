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

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SlicedPassengerCars
{
    public static class PassengerCarModifications
    {
        public static List<PrefabModder.MeshReplacement> MeshReplacements { get; private set; }
        public static List<PrefabModder.Transformation> Transformations { get => new PrefabModder.Transformation[]
        {
            new PrefabModder.Transformation("[colliders]", scale: new Vector3(1, 1, 0.745f)),
            new PrefabModder.MirroredTransformation("BogieR", translate: new Vector3(0, 0, -2.98f)),
            new PrefabModder.MirroredTransformation("BogieF", translate: new Vector3(0, 0, -2.98f)),
            new PrefabModder.MirroredTransformation("HookPlate_R", translate: new Vector3(0, 0, -2.98f)),
            new PrefabModder.MirroredTransformation("HookPlate_F", translate: new Vector3(0, 0, -2.98f)),
            new PrefabModder.MirroredTransformation("Buffer_RR", translate: new Vector3(0, 0, -2.98f)),
            new PrefabModder.MirroredTransformation("Buffer_RL", translate: new Vector3(0, 0, -2.98f)),
            new PrefabModder.MirroredTransformation("Buffer_FR", translate: new Vector3(0, 0, -2.98f)),
            new PrefabModder.MirroredTransformation("Buffer_FL", translate: new Vector3(0, 0, -2.98f)),
            new PrefabModder.MirroredTransformation("BuffersAndChainRig", translate: new Vector3(0, 0, -2.98f)),
            new PrefabModder.MirroredTransformation("[coupler rear]", translate: new Vector3(0, 0, -2.98f)),
            new PrefabModder.MirroredTransformation("[coupler front]", translate: new Vector3(0, 0, -2.98f)),
            new PrefabModder.MirroredTransformation("[car plate anchor2]", translate: new Vector3(0, 0, -1.49f)),
            new PrefabModder.MirroredTransformation("[car plate anchor1]", translate: new Vector3(0, 0, -1.49f)),
        }.ToList(); }

        private static string[] meshNames = new string[]
        {
            "car_passenger_interior_LOD1",
            "car_passenger_interior_LOD2",
            "car_passenger_LOD1",
            "car_passenger_LOD2",
            "car_passenger_LOD3",
            "exterior",
            "front doors",
            "interior",
            "side doors",
            "windows",
            "windows_int",
        };

        public static void Initialize(string modPath)
        {
            Main.Log("Initializing passenger car modifications...");

            MeshReplacements = new List<PrefabModder.MeshReplacement>();

            AssetBundle bundle = AssetBundle.LoadFromFile(modPath + "Resources/slicedpassengercar");

            foreach (var name in meshNames)
            {
                var go = bundle.LoadAsset<GameObject>($"Assets/Models/SlicedPax/{name}.fbx");
                var mf = go.GetComponent<MeshFilter>();
                var sm = mf.sharedMesh;
                MeshReplacements.Add(new PrefabModder.MeshReplacement(name, sm));
            }

            bundle.Unload(false);

            Main.Log("Done");
        }
    }
}
