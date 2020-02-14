using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEliminator : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.Instance.RemovePlayer(other.GetComponent<PlayerController>());
        }
    }
}
