/*
 * LoadScene.cs
 * Created by: Kman 
 * Created on: 5/3/2020 (dd/mm/yy)
 * Created for: Changing Scenes
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
	[SerializeField] string _Scene;

	public void Load()
	{
		SceneManager.LoadScene(_Scene);
	}
}
