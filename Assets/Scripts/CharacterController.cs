using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public GameObject head;
    public float mouseSensitivity;
    public float speed;
    public int jumpHeight = 5;
    private float originSpeed;
    private Rigidbody rb;
    float mouseX = 0;
    float mouseY = 0;
    private bool canJump;
    //private Ray ray;
    //private RaycastHit rayHit;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        originSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal") * speed;
        float zAxis = Input.GetAxis("Vertical") * speed;

        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        mouseY = Mathf.Clamp(mouseY, -90, 90);

        Vector3 movePos = transform.right * xAxis + transform.forward * zAxis;
        Vector3 newMovePos = new Vector3(movePos.x, rb.velocity.y, movePos.z);

        rb.velocity = newMovePos;

        head.transform.localEulerAngles = new Vector3(mouseY, 0, 0);
        transform.localEulerAngles = new Vector3(0, mouseX, 0);

        //if (Physics.Raycast(transform.position, Vector3.down, out rayHit, 1.1f))
        //{
        //    if (rayHit.transform.gameObject.tag == "Ground") { canJump = true; }
        //    else if (rayHit.transform.gameObject.tag != "Ground") { canJump = false; }
        //}

        if (Input.GetKeyDown(KeyCode.LeftShift)) { speed = speed * 2; }
        if (Input.GetKeyUp(KeyCode.LeftShift)) { speed = originSpeed; }
        //find a way to make this smoother!!!
        //if (Input.GetKeyDown(KeyCode.Space) && canJump == true) { rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + jumpHeight, rb.velocity.z); }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + jumpHeight, rb.velocity.z);
            //Debug.Log
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            canJump = true;
        }
        //Debug.Log("canjumpppp");
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            canJump = false;
        }
        //Debug.Log("cannotjumpppp");
    }
}
