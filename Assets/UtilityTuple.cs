using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class UtilityTuple
	{
		public float dist { get; set; }
		public GameObject vehicle { get; set; }

		public UtilityTuple(float d, GameObject v) {
			dist = d;
			vehicle = v;
		}
	}
}

