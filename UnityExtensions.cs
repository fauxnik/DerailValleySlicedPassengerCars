using System.Collections.Generic;
using UnityEngine;

namespace SlicedPassengerCars
{
	public static class UnityExtensions
	{
		public static GameObject Find(this GameObject parent, string name)
		{
			foreach(var transform in parent.GetComponentsInChildren<Transform>(true))
			{
				if (transform.name == name) { return transform.gameObject; }
			}
			return null;
		}

		public static List<GameObject> FindAll(this GameObject parent, string name)
		{
			var found = new List<GameObject>();
			foreach (var transform in parent.GetComponentsInChildren<Transform>(true))
			{
				if (transform.name == name) { found.Add(transform.gameObject); }
			}
			return found;
		}

		public static string GetPath(this Transform transform)
        {
			string path = transform.name;
			while (transform.parent != null)
            {
				transform = transform.parent;
				path = $"{transform.name}/{path}";
            }
			return path;
        }
	}
}
