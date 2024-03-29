﻿using System;

namespace AssemblyCSharp
{
	public class Stoplight
	{
		public enum Light
		{
			Red,
			Yellow,
			Green
		}

		private int[] position;
		public Light status;

		public Stoplight ()
		{
			position = new int[2];
			status = Light.Red;
		}

		public int[] Position {
			get {
				return this.position;
			}
			set {
				position = value;
			}
		}

		public Light Status {
			get {
				return this.status;
			}
			set {
				status = value;
			}
		}
	}
}

