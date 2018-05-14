using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// Parse a numeric value into a short scale string representation.
/// </summary>
public class ShortScaleString : MonoBehaviour {

	/*
	 * The ShortScaleString script parses either a double, float, or int value 
	 * value type into a form that is easier to read. The list supports values up to
	 * the largest currently accepted value(one centillion). values given larger than
	 * 999 centillion will be represented as "1000 centillion" and so on.
	 * It is important to be aware of the limitations of each data type. for intParse, numbers higher than 1 billion
	 * will throw an overflow error. similarly a float value will also overflow after too large a number is reached.
	 * for applicatins where the needed use is well over 1+E100 use parseDouble as its maximun output value tested is
	 * 1000 centillion, thats 306 0's!
	 */


	/// <summary>
	/// The list containing all short scale values.
	/// </summary>
	static string[] ShortScaleReference= {"thousand","million","billion","trillion","quadrillion","quintillion",
	"sextillion","septillion","octillion","nonillion","decillion",
	"undecillion","duodecillion","tredecillion","quattuordecillion","quindecillion",
	"sexdecillion","septendecillion","octodecillion","novemdecillion","vigintillion",
	"unvigintillion", "duovigintillion","trevigintillion","quattuorvigintillion",
	"quinvigitillion","sexvigintillion","septenvigitillion","octovigintillion","novemvigitillion",
	"trigintillion","untrigintillion","duotrigintillion","tretrigintillion","quattuortrigintillion",
	"quintrigintillion","sextrigintillion","septentrigintillion","octotrigintillion",
	"novemtrigintillion","quadragintillion","unquadragintillion","duoquadragintillion",
	"trequadragintillion","quattuorquadragintillion","quinquadragintillion","sexquadragintillion",
	"septenquadragintillion","octoquadragintillion","novemquadragintillion","quinquagintillion","unquinquagintillion",
	"duoquinquagintillion","trequinquagintillion","quattuorquinquagintillion","quinquinquagintillion",
	"sexquinquagintillion","septenquinquagintillion","octoquinquagintillion","novemquinquagintillion","sexagintillion",
	"unsexagintillion","duosexagintillion","tresexagintillion","quattuorsexagintillion","quinsexagintillion",
	"sexsexagintillion","septensexagintillion","octosexagintillion","novemsexagintillion","septuagintillion",
	"unseptuagintillion","duoseptuagintillion","treseptuagintillion","quattuorseptuagintillion","quinseptuagintillion",
	"sexseptuagintillion","septenseptuagintillion","octoseptuagintillion","novemseptuagintillion",
	"octogintillion","unoctogintillion","duooctogintillion","treoctogintillion","quattuoroctogintillion",
	"quinoctogintillion","sexoctogintillion","septenoctogintillion","octooctogintillion","novemoctogintillion","nonagintillion",
	"unnonagintillion","duononagintillion","trenonagintillion","quattuornonagintillion","quinnonagintillion",
	"sexnonagintillion","septennonagintillion","octononagintillion","novemnonagintillion","centillion"};

	/// <summary>
	/// Parses the double value into short scale notation.
	/// </summary>
	/// <returns>The short scale string.</returns>
	/// <param name="value">The input value that will be parsed.</param>
	/// <param name="precision">(Optional) The decimal precision that should be represented (subject to data type round off).
	/// Default value is 3.</param> 
	/// <param name="startShortScale">(Optinal) Set the value to begin parsing to short scale. Default value is 1 million.</param>
	public static string parseDouble(double value,int precision = 3, double startShortScale = 1000000){
		int index = -1;
		int isNegative = 1;
		string addPrecision = new string ('#', precision);
		double precisionValue = Mathf.Pow (10, precision);

        Debug.Log(value.ToString("0"));

		if (value < 0) {
			isNegative = -1;
			value = value * isNegative;
		} else {
			isNegative = 1;
		}

		if (value < 1000d || value < startShortScale){
			return (Math.Floor(value * isNegative * precisionValue) / precisionValue).ToString ("#,#." + addPrecision);
		}

		while (value >= 1000d) {
			if (index >=  ShortScaleReference.Length -1 ){
				break;
			}
			value = value / 1000d;
			index++;
		}
		return (Math.Floor(value * isNegative * precisionValue ) / precisionValue ).ToString ("#,#." + addPrecision) + " " + ShortScaleReference [index];
	}

	/// <summary>
	/// Parses the float value into short scale notation.
	/// </summary>
	/// <returns>The short scale string.</returns>
	/// <param name="value">The input value that will be parsed.</param>
	/// <param name="precision">(Optional) The decimal precision that should be represented (subject to data type round off).
	/// Default value is 3.</param> 
	/// <param name="startShortScale">(Optional) Set the value to begin parsing to short scale. Default value is 1 million.</param>
	public static string parseFloat(float value, int precision = 3, float startShortScale = 1000000){
		int index = -1;
		int isNegative = 1;
		string addPrecision = new string ('#', precision);
		double precisionValue = Mathf.Pow (10, precision);

		if (value < 0) {
			isNegative = -1;
			value = value * isNegative;
		} else {
			isNegative = 1;
		}

		if (value < 1000 || value < startShortScale) {
			return (Math.Floor(value * isNegative * precisionValue) / precisionValue).ToString ("#,#." + addPrecision);
		}
		while (value >= 1000.0f) {
			if (index == ShortScaleReference.Length - 1){
				break;
			}
			value = value / 1000;
			index++;
		}
        
		return (Math.Floor(value * isNegative * precisionValue ) / precisionValue ).ToString ("#,#." + addPrecision) + " " + ShortScaleReference [index];
	}

	/// <summary>
	/// Parses the int value into short scale notation.
	/// </summary>
	/// <returns>The short scale string.</returns>
	/// <param name="value">The input value that will be parsed.</param>
	/// <param name="precision">(Optional) The decimal precision that should be represented (subject to data type round off).
	/// Default value is 3.</param> 
	/// <param name="startShortScale">(Optional) Set the value to begin parsing to short scale. Default value is 1 million.</param>
	public static string parseInt(int value, int precision = 3, int startShortScale = 1000000){
		int index = -1;
		int isNegative = 1;
		string addPrecision = new string ('#', precision);

		if (value < 0) {
			isNegative = -1;
			value = value * isNegative;
		} else {
			isNegative = 1;
		}

		if (value < 1000 || value < startShortScale) {
			return (value * isNegative).ToString ("#,#");
		}
		while (value >= 1000) {
			if (index == ShortScaleReference.Length - 1){
				break;
			}
			value = value / 1000;
			index++;
		}
		return (value * isNegative).ToString ("#,#." + addPrecision) + " " + ShortScaleReference [index];
	}
}
