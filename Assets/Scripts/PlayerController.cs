using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameManager gameManager;
    Animator animator;
    Rigidbody rigidbody;
    Vector2 BorderLimit = new Vector2(-5f, 5f);

    private float xMovement;
    
    // Use this for initialization
    void Start ()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        rigidbody = transform.GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (gameManager.active)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Jump();
            HorizontalMovement(Input.GetAxis("Horizontal"));
        }
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
    }

    public void HorizontalMovement(float horizontalInput)
    {
        transform.position += Vector3.right * horizontalInput * 15f * Time.deltaTime;
        if (transform.position.x <= BorderLimit.x)
        {
            transform.position = new Vector3(BorderLimit.x, transform.position.y, transform.position.z);
            if (xMovement < 0f)
                xMovement += Time.deltaTime * 5;
            else
                xMovement = 0f;
            animator.SetFloat("Blend", xMovement);
        }
        else
        if (transform.position.x >= BorderLimit.y)
        {
            transform.position = new Vector3(BorderLimit.y, transform.position.y, transform.position.z);
            if (xMovement > 0f)
                xMovement -= Time.deltaTime * 5;
            else
                xMovement = 0f;
            animator.SetFloat("Blend", xMovement);
        }
        else
        {
            xMovement = Input.GetAxis("Horizontal");
            animator.SetFloat("Blend", xMovement);
        }
    }

    public IEnumerator DisablePlayer(Vector3 velocity)
    {
        animator.enabled = false;
        rigidbody.useGravity = true;
        rigidbody.freezeRotation = false;
        rigidbody.transform.localScale = Vector3.one;
        yield return null;
    }
    
}
