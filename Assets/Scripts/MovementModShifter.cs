using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MovementModShifter : MonoBehaviour
{
	public GameObject movementUI;

	private Component[] components;

	private Text[] textObjects;
	private int textObjectsSize;
	private int currentPosition;

	// Use this for initialization
	void Start ()
	{
		components = movementUI.GetComponentsInChildren(typeof(Text));

		textObjects = new Text[components.Length];

		if(components != null)
		{
			int i = 0;
			foreach(Component c in components)
			{
				Text t = (Text)(c);

				t.CrossFadeAlpha(0.5f, 0f, true);

				textObjects[i++] = t;
			}

			currentPosition = 0;

			textObjects[currentPosition].CrossFadeAlpha(255, 0f, true);

			textObjectsSize = components.Length;
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetButtonDown("Cycle"))
		{
			textObjects[currentPosition].CrossFadeAlpha(0.5f, 0f, true);

			if(currentPosition % (textObjectsSize-1) != 0 || currentPosition == 0)
			{
				currentPosition++;
			}
			else
			{
				currentPosition = 0;
			}

			textObjects[currentPosition].CrossFadeAlpha(1f, 0f, true);
		}
	}

	public string GetActiveComponentName()
	{
		return textObjects[currentPosition].name;
	}

	public string[] GetAvailableComponentsNames()
	{
		string[] names = new string[textObjectsSize];

		int i = 0;
		foreach(Text t in textObjects)
		{
			names[i++] = (t.name);
		}

		return names;
	}
}
