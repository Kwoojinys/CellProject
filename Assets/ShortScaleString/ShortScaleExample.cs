using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShortScaleExample : MonoBehaviour {

	public InputField inputTarget;
	double inputValue;
	public Text outputTarget;

	void Update () {

		inputValue = double.Parse (inputTarget.text);
		outputTarget.text = ShortScaleString.parseDouble (inputValue);

	}
}

