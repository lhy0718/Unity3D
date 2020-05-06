using UnityEngine;

namespace Util_LHY
{
	public static class Util
	{
		/// <summary>
		///   <para>Determine whether two float variables are equal within the range of threshold.</para>
		/// </summary>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <param name="threshold"></param>
		public static bool FloatEquals(float value1, float value2, float threshold = 0.1f) => Mathf.Abs(value1 - value2) < threshold;

		/// <summary>
		///   <para>Prints multiple parameters.</para>
		/// </summary>
		public static void Print(params object[] message)
		{
			string s = "";
			foreach (object m in message)
			{
				s += m.ToString() + ", ";
			}
			MonoBehaviour.print(s.Remove(s.Length - 2));
		}

		public static float NormalizeAngle(float angle)
		{
			while (angle > 180) angle -= 360;
			while (angle < -180) angle += 360;
			return angle;
		}
	}
}
