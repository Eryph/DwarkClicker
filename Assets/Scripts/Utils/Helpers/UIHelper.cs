namespace DwarkClicker.Helper
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public static class UIHelper
	{

		public static string FormatIntegerString(float value)
		{
			string ret = "";
			char unitCharacter = '\0';
			float tmpValue = 0.0f;

			if (value >= 1000000)
			{
				tmpValue = value / 1000000;
				unitCharacter = 'm';
			}
			else if (value >= 1000)
			{
				tmpValue = value / 1000;
				unitCharacter = 'k';
			}
			else
			{
				tmpValue = value;
			}

			if (unitCharacter != '\0')
			{
				tmpValue = (float)Math.Truncate(tmpValue * 10) / 10;
				ret = tmpValue.ToString("#.#") + unitCharacter;
			}
			else
			{
				ret = tmpValue.ToString();
			}

			if (ret == string.Empty)
			{
				ret = "0";
			}
			return ret;
		}
	}
}