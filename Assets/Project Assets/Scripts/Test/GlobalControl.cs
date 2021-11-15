/*
 * GlobalControl.cs
 * Created by: Kman 
 * Created on: 1/3/2020 (dd/mm/yy)
 * Created for: Controlling the player's stats between scenes
 */

using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}