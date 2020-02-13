using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float time = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Vanishing());
    }

    IEnumerator Vanishing()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
