using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemWall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController controller = other.gameObject.GetComponent<PlayerController>();
            if (controller.State == PlayerState.PUSHED)
            {
                print("BOUM");
                GetComponent<Animator>().SetTrigger("Destroy");
                EventManager.onWallDestroyed.Invoke();
            }
        }
    }

    public void DestroyWall()
    {
        Destroy(this.gameObject);
    }
}
