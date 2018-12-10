using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewLoader : MonoBehaviour {
    private string coreSystemScene = "CoreSystem";

	// Use this for initialization
	void Start () {
        SceneManager.LoadScene(coreSystemScene, LoadSceneMode.Additive);
    }
	
	// Update is called once per frame
	void Update () {
	}
}
