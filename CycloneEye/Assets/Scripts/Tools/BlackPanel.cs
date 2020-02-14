using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackPanel : MonoBehaviour
{
    public void Show()
    {
        StartCoroutine(ShowAnim());
    }
    public void Hide()
    {
        StartCoroutine(HideAnim());
    }

    public IEnumerator ShowAnim()
    {
        Image panel = GetComponent<Image>();
        panel.raycastTarget = true;
        for (float t = 0; t <= 1f; t+= Time.deltaTime)
        {
            panel.color = new Color(0, 0, 0, t);
            yield return null;
        }
    }

    public IEnumerator HideAnim()
    {
        Image panel = GetComponent<Image>();
        for (float t = 0; t <= 1f; t += Time.deltaTime)
        {
            panel.color = new Color(0, 0, 0, 1f-t);
            yield return null;
        }
        panel.raycastTarget = false;
    }
}
