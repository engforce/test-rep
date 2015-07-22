using UnityEngine;
using System.Collections;

public class LoadGameOnClick : MonoBehaviour {

	public void LoadScene(int level)
	{
		Application.LoadLevel(level);
	}
}
