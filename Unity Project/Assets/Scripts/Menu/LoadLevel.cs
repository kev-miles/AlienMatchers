using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadLevel : MonoBehaviour {

	public void ChangeScene (int scene)
    {
        SceneManager.LoadScene(scene);
	}
}
