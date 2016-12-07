using System;

namespace AssemblyCSharp
{
	public class Vehicle
	{

		private int[] position;
		private float speed;
		private int topSpeed;
		private double acceleration;
		public int radius;

		public Vehicle ()
		{
			position = new int[2];
			speed = 2f;
			acceleration = 0.1;
			topSpeed = 1;
			radius = 5;
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

		public float getSpeed() {
			return speed;
		}


	}
}

