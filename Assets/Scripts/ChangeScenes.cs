using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
	public void ChangesScene(string sceneName)
	{ 
		//this function is called by the button in unity; when called it simply causes the Application to load whichever sceen is entered within the button config in unity
		//sceneName is entered from unity, within the button.
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single); //single mode replaces current scene with the new one
	}
}
