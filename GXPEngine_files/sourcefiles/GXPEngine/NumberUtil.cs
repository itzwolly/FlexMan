using System;

class NumberUtil {

	private static Random _rnd = new Random ();

	static public int GetRandomNumber (int pMaxNumber)
	{
		return _rnd.Next(1, pMaxNumber + 1);
	}
}