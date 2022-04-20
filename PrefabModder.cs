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
// using System.Text.RegularExpressions;
using UnityEngine;

namespace SlicedPassengerCars
{
    public static class PrefabModder
    {
        public class MeshReplacement
        {
            public readonly string name;
            public readonly Mesh mesh;

            public MeshReplacement(string name, Mesh mesh)
            {
                this.name = name;
                this.mesh = mesh;
            }
        }

        public class Transformation
        {
            public readonly string name;
            public readonly Vector3 translate;
            public readonly Quaternion rotate;
            public readonly Vector3 scale;

            public Transformation(string name, Vector3? translate = null, Quaternion? rotate = null, Vector3? scale = null)
            {
                this.name = name;
                this.translate = translate ?? Vector3.zero;
                this.rotate = rotate ?? Quaternion.identity;
                this.scale = scale ?? Vector3.one;
            }
        }

        public class MirroredTransformation : Transformation
        {
            public MirroredTransformation(string name, Vector3? translate = null, Quaternion? rotate = null, Vector3? scale = null) : base(name, translate, rotate, scale) { }
        }

        public static List<string> Modify(
            GameObject prefab,
            List<MeshReplacement> meshReplacements = null,
            List<Transformation> transformations = null)
        {
            Main.Log($"Modifying prefab: {prefab}");
            var missing = new List<string>();

            missing.AddRange(meshReplacements?.Where(mr => !ReplaceMesh(prefab, mr))?.Select(mr => mr.name) ?? new string[0]);
            missing.AddRange(transformations?.Where(t => !TransformChild(prefab, t))?.Select(t => t.name) ?? new string[0]);

            return missing;
        }

        private static bool ReplaceMesh(GameObject prefab, MeshReplacement meshReplacement)
        {
            var replacees = prefab.FindAll(meshReplacement.name);

            foreach (var replacee in replacees)
            {
                // one of these two should exist
                var mf = replacee.GetComponent<MeshFilter>();
                if (mf != null) {
                    // if (new Regex("interior").IsMatch(meshReplacement.name))
                    // {
                    //     var renderer = replacee.GetComponent<Renderer>();
                    //     Debug.Log($"{meshReplacement.name} textures are {string.Join(", ", renderer.sharedMaterials.Select(m => $"[{string.Join(", ", new string[] { m?.GetTexture("_t1")?.name, m?.GetTexture("_t3")?.name, m?.GetTexture("_t4")?.name })}]"))}");
                    // }
                    mf.sharedMesh = meshReplacement.mesh; 
                }
                var smr = replacee.GetComponent<SkinnedMeshRenderer>();
                if (smr != null) { smr.sharedMesh = meshReplacement.mesh; }
            }

            return replacees.Count > 0;
        }

        private static bool TransformChild(GameObject prefab, Transformation transformation)
        {
            var replacees = prefab.FindAll(transformation.name);

            foreach (var replacee in replacees)
            {
                if (transformation.GetType() == typeof(MirroredTransformation))
                {
                    if (transformation.translate != null)
                    {
                        var lp = replacee.transform.localPosition;
                        var av = new Vector3(
                            Mathf.Sign(lp.x) * transformation.translate.x,
                            Mathf.Sign(lp.y) * transformation.translate.y,
                            Mathf.Sign(lp.z) * transformation.translate.z);
                        var nlp = lp + av;
                        Main.Log($"Adjusting transform {replacee.transform.GetPath()} ({lp} >>{av}>> {nlp})");
                        replacee.transform.localPosition = nlp;
                    }
                    if (transformation.rotate != null) { Main.LogError("Unimplemented: MirroredTransformation.rotate handling"); }
                    if (transformation.scale != null) { Main.LogError("Unimplemented: MirroredTransformation.scale handling"); }
                }
                else
                {
                    if (transformation.translate != null) { replacee.transform.localPosition += transformation.translate; }
                    if (transformation.rotate != null) { replacee.transform.localRotation *= transformation.rotate; }
                    if (transformation.scale != null)
                    {
                        replacee.transform.localScale = new Vector3(
                            replacee.transform.localScale.x * transformation.scale.x,
                            replacee.transform.localScale.y * transformation.scale.y,
                            replacee.transform.localScale.z * transformation.scale.z);
                    }
                }
            }

            return replacees.Count > 0;
        }
    }
}
