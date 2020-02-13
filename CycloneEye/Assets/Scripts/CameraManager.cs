using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    void Start()
    {
        EventManager.onPlayerDamaged.AddListener(Chake);
    }

    void Chake()
    {
        StartCoroutine(ChakeAnim());
    }

    IEnumerator ChakeAnim()
    {
        Vector3 originPos = transform.position;
        for(int i = 0; i < 10; i++)
        {
            Vector3 startPos = transform.position;
            Vector3 target = new Vector3(Random.Range(-1f, 1f), originPos.y, Random.Range(-1f, 1f));
            for(float t = 0; t < 0.05f; t += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(startPos, target, t * 20);
                yield return null;
            }
        }
        Vector3 startPos2 = transform.position;
        for (float t = 0; t < 0.05f; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startPos2, originPos, t * 20);
            yield return null;
        }
    }
}
