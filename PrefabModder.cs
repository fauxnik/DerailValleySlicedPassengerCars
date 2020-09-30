using System.Collections.Generic;
using System.Linq;
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
            List<Transformation> transformations = null,
            Vector3? relativeAdjustment = null)
        {
            Main.Log($"Modifying prefab: {prefab}");
            var missing = new List<string>();

            missing.AddRange(meshReplacements?.Where(mr => !ReplaceMesh(prefab, mr))?.Select(mr => mr.name) ?? new string[0]);
            missing.AddRange(transformations?.Where(t => !TransformChild(prefab, t))?.Select(t => t.name) ?? new string[0]);
            if (relativeAdjustment != null) { AdjustChildren(prefab, relativeAdjustment.Value); }

            return missing;
        }

        public static void AdjustChildren(GameObject prefab, Vector3 ra)
        {
            // DFS?
            var stack = new List<Transform>();
            stack.AddRange(prefab.transform.OfType<Transform>());
            while (stack.Count > 0)
            {
                var transform = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
                if (transform.localPosition.sqrMagnitude < 0.0005f)
                {
                    Main.Log($"Treating {transform.GetPath()} as parent (sqrMagnitude = {transform.localPosition.sqrMagnitude})");
                    stack.AddRange(transform.OfType<Transform>());
                }
                else
                {
                    //Main.Log($"Adjusting transform {transform.GetPath()} (sqrMagnitude = {transform.localPosition.sqrMagnitude})");
                    var lp = transform.localPosition;
                    var av = new Vector3(
                        Mathf.Sign(lp.x) * ra.x,
                        Mathf.Sign(lp.y) * ra.y,
                        Mathf.Sign(lp.z) * ra.z);
                    var nlp = lp + av;
                    Main.Log($"Adjusting transform {transform.GetPath()} ({lp} >>{av}>> {nlp})");
                    transform.localPosition = nlp;
                }
            }
        }

        private static bool ReplaceMesh(GameObject prefab, MeshReplacement meshReplacement)
        {
            var replacees = prefab.FindAll(meshReplacement.name);

            foreach (var replacee in replacees)
            {
                // one of these two should exist
                var mf = replacee.GetComponent<MeshFilter>();
                if (mf != null) { mf.sharedMesh = meshReplacement.mesh; }
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
                        //replacee.transform.localPosition += transformation.translate;
                        var lp = replacee.transform.localPosition;
                        var av = new Vector3(
                            Mathf.Sign(lp.x) * transformation.translate.x,
                            Mathf.Sign(lp.y) * transformation.translate.y,
                            Mathf.Sign(lp.z) * transformation.translate.z);
                        var nlp = lp + av;
                        Main.Log($"Adjusting transform {replacee.transform.GetPath()} ({lp} >>{av}>> {nlp})");
                        replacee.transform.localPosition = nlp;
                    }
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
