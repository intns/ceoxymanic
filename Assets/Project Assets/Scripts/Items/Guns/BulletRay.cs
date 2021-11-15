/*
 * BulletRay.cs
 * Created by: intns
 * Created on: 5/2/2020 (dd/mm/yy)
 * Created for: Drawing bullet rays
 */
using System.Collections;
using UnityEngine;
public class BulletRay : MonoBehaviour
{
    LineRenderer _LineRenderer;
    [SerializeField] [Range(0, 1)] float _LerpSpeed;
    private void Awake()
    {
        _LineRenderer = GetComponent<LineRenderer>();
        _LineRenderer.enabled = false;
        _LineRenderer.useWorldSpace = true;
        StartCoroutine(WaitThenDestroy(1));
    }

    private void Update()
    {
        _LineRenderer.SetPosition(0, Vector3.Lerp(_LineRenderer.GetPosition(0), _LineRenderer.GetPosition(1), _LerpSpeed));
    }

    public void SetRendererPosition(Vector3 position)
    {
        _LineRenderer.enabled = true;
        _LineRenderer.SetPosition(0, transform.position);
        _LineRenderer.SetPosition(1, position);
    }

    //Todo: remove IEnumerator
    public IEnumerator WaitThenDestroy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}