/*
 * ScalePreserver.cs
 * Created by: Kman
 * Created on: 10/2/2020 (dd/mm/yy)
 * Created for: Nullifying the parent's scale
 */
using UnityEngine;

public class ScalePreserver : MonoBehaviour
{
    void Update()
    {
        transform.localScale = new Vector3(1 / transform.parent.localScale.x, 1 / transform.parent.localScale.y, 1 / transform.parent.localScale.z);
    }
}
