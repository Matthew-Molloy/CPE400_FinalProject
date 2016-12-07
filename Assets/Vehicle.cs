using System;

namespace AssemblyCSharp
{
	public class Vehicle
	{
		public float Speed;
		public float Acceleration;
		public int radius;

		public Vehicle ()
		{
			Speed = 2.5f;
			Acceleration = 0.06f;
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
	}
}

