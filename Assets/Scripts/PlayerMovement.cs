using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   

    private float playerSpeed = 5.0f;
    private Rigidbody rb;

    void Start(){
        
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){
    
    }

    void FixedUpdate(){
        movePlayer();
    }

    private void movePlayer() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rb.MovePosition(transform.position + movement * Time.deltaTime * playerSpeed);
    }
}
