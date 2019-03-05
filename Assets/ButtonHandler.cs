using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour {

	// Use this for initialization
	public void changeScene(string scene) {
		UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        Window_Graph.inAwake = false;
	}
	
}
