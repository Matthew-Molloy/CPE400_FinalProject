using System;

namespace AssemblyCSharp
{
	public class Vehicle
	{

		int[] position;
		int speed;
		int topSpeed;
		double acceleration;

		public Vehicle ()
		{
			position = new int[2];
			speed = 0;
			acceleration = 0.1;
			topSpeed = 1;
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


	}
}

