using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] int damages;
    [SerializeField] float moveSpeed;

    public enum PlayerState
    {
        NORMAL, ATTACKING, PUSHED, KO
    }

    Rigidbody rBody;
    PlayerState state;

    // Start is called before the first frame update
    void Start()
    {
        state = PlayerState.NORMAL;
        rBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameStarted || state != PlayerState.NORMAL) return;

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal " + index), 0, Input.GetAxis("Vertical " + index)) * moveSpeed;
        rBody.AddForce(movement);
        if (rBody.velocity.magnitude > moveSpeed)
            rBody.velocity = rBody.velocity.normalized * moveSpeed;

        if (movement.magnitude > 0.1f)
        {
            transform.LookAt(transform.position+movement);
        }

        if (Input.GetButtonDown("Attack " + index))
        {
            StartCoroutine(AttackAnim());
        }
    }

    IEnumerator AttackAnim()
    {
        state = PlayerState.ATTACKING;
        rBody.velocity = Vector3.zero;
        yield return new WaitForSeconds(.2f);
        TestAttackPropultion();
        yield return new WaitForSeconds(.8f);
        state = PlayerState.NORMAL;
    }

    void TestAttackPropultion()
    {
        Collider[] colls = Physics.OverlapBox(transform.position + transform.forward, new Vector3(1, 1, 1));
        foreach(Collider coll in colls)
        {
            if (coll.tag == "Player")
            {
                Vector3 direction = (coll.transform.position - transform.position).normalized;
                coll.GetComponent<PlayerController>().Push(direction);
            }
        }
    }

    public void Push(Vector3 baseForce)
    {
        EventManager.onPlayerDamaged.Invoke();
        damages += 1000;
        rBody.AddForce(baseForce * damages);
        StartCoroutine(PushAnim());
    }

    IEnumerator PushAnim()
    {
        state = PlayerState.PUSHED;
        yield return new WaitForSeconds(1f);
        state = PlayerState.NORMAL;
    }

    public void Eliminate()
    {
        EventManager.onPlayerEliminated.Invoke();
    }
}
