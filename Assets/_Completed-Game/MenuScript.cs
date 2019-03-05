using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackScript : MonoBehaviour {

	public void ChangeScene(string screneName){
        Window_Graph.flag = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(screneName);
    }
}
