using System;

namespace AssemblyCSharp
{
	public class Vehicle
	{

		private int[] position;
		private float speed;

		public Vehicle ()
		{
			position = new int[2];
			speed = 2f;
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

		public float Speed {
			get {
				return this.speed;
			}
			set {
				speed = value;
			}
		}

	}
}

