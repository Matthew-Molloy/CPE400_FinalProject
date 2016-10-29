using System;

namespace AssemblyCSharp
{
	public class Vehicle
	{

		int[] position;
		int speed;

		public Vehicle ()
		{
			position = new int[2];
			speed = 1;
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

