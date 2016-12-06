using System;

namespace AssemblyCSharp
{
	public class Vehicle
	{

		private int[] position;
		public float Speed = 3f;
		public float Acceleration = 0.06f;

		public Vehicle ()
		{
			position = new int[2];
		}

		public void stop()
		{

		}

		public void go()
		{

		}

		public bool checkLight()
		{
			return true;
		}

		public bool checkCar()
		{
			return true;
		}

		public int[] Position {
			get {
				return this.position;
			}
			set {
				position = value;
			}
		}

	}
}

