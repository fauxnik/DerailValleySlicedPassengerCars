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
