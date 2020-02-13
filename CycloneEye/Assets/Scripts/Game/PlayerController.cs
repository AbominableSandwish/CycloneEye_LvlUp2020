using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] float moveSpeed;
    [SerializeField] GameObject attackAnim;

    public enum PlayerState
    {
        NORMAL, ATTACKING, PUSHED, KO
    }

    float damages;
    Rigidbody rBody;
    PlayerState state;
    float chargingAttack;
    bool charging = false;

    // Start is called before the first frame update
    void Start()
    {
        charging = false;
        state = PlayerState.NORMAL;
        rBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Start " + index))
            GameManager.Instance.Pause(index);

        if (GameManager.State != GameState.PLAYING || state != PlayerState.NORMAL) return;

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal " + index), 0, Input.GetAxis("Vertical " + index)) * moveSpeed;
        if (charging) movement /= 3;
        rBody.AddForce(movement);
        if (rBody.velocity.magnitude > moveSpeed)
            rBody.velocity = rBody.velocity.normalized * moveSpeed;

        if (movement.magnitude > 0.1f)
        {
            transform.LookAt(transform.position+movement);
        }

        if (Input.GetButtonDown("Attack " + index))
        {
            charging = true;
            chargingAttack = 0;
        }
        if (Input.GetButton("Attack " + index) && charging)
        {
            chargingAttack = Mathf.Min(1f, chargingAttack+=Time.deltaTime);
        }
        if (Input.GetButtonUp("Attack " + index) && charging)
        {
            charging = false;
            StartCoroutine(AttackAnim());
        }
    }

    IEnumerator AttackAnim()
    {
        Instantiate(attackAnim, transform.position + transform.forward, transform.rotation);
        state = PlayerState.ATTACKING;
        rBody.velocity = Vector3.zero;
        yield return new WaitForSeconds(.2f);
        TestAttackPropultion();
        yield return new WaitForSeconds(.8f);
        state = PlayerState.NORMAL;
    }

    void TestAttackPropultion()
    {
        Collider[] colls = Physics.OverlapBox(transform.position + transform.forward, new Vector3(.5f, .5f, .5f));
        foreach(Collider coll in colls)
        {
            if (coll.tag == "Player" && coll.gameObject != this.gameObject)
            {
                Vector3 direction = (coll.transform.position - transform.position).normalized;
                coll.GetComponent<PlayerController>().Push(direction, 1+chargingAttack);
            }
        }
    }

    public void Push(Vector3 baseForce, float power)
    {
        charging = false;
        EventManager.onPlayerDamaged.Invoke();
        damages += 1000 * power;
        rBody.AddForce(baseForce * damages);
        StartCoroutine(PushAnim());
    }

    IEnumerator PushAnim()
    {
        state = PlayerState.PUSHED;
        yield return new WaitForSeconds(0.4f);
        state = PlayerState.NORMAL;
    }

    public void Eliminate()
    {
        EventManager.onPlayerEliminated.Invoke();
    }
}
