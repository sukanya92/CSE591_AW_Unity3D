﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class login : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void navigate()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Roll-a-ball");
    }
}
