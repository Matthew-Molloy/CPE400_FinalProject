using System;
using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	/// <summary>
	/// Code for a CircleRenderer, retrieved from http://gamedev.stackexchange.com/a/126429
	/// </summary>
	[RequireComponent(typeof(LineRenderer))]
	public class CircleRenderer : MonoBehaviour {
		[Range(0.1f, 100f)]
		public float radius = 1.0f;

		[Range(3, 256)]
		public int numSegments = 128;

		[Range(0.1f, 3f)]
		public float lineWidth = 0.5f;

		void Start ( ) {
			DoRenderer();
		}

		public void DoRenderer ( ) {
			LineRenderer lr = gameObject.GetComponent<LineRenderer>();
			Color c1 = new Color(0.5f, 0.5f, 0.5f, 1);
			lr.material = new Material(Shader.Find("Particles/Additive"));
			lr.SetColors(c1, c1);
			lr.SetWidth(lineWidth, lineWidth);
			lr.SetVertexCount(numSegments + 1);
			lr.useWorldSpace = false;

			float deltaTheta = (float) (2.0 * Mathf.PI) / numSegments;
			float theta = 0f;

			for (int i = 0 ; i < numSegments + 1 ; i++) {
				float x = radius * Mathf.Cos(theta);
				float y = radius * Mathf.Sin(theta);
				Vector3 pos = new Vector3(x, y, 0);
				lr.SetPosition(i, pos);
				theta += deltaTheta;
			}
		}
	}
}

