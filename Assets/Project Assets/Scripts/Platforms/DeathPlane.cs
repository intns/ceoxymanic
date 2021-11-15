/*
 * DeathPlane.cs
 * Created by: intns & Kman
 * Created on: 20/2/2020 (dd/mm/yy)
 * Created for: Destroying objects that fall off the map
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPlane : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		else
		{
			Destroy(other.gameObject);
		}
	}
}
