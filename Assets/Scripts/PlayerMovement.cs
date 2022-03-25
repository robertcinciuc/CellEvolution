using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   

    private float playerSpeed = 5.0f;
    private Rigidbody rb;
    private CharacterController characterController;
    private Vector3 playerVelocity;

    // Start is called before the first frame update
    void Start(){
        
        rb = GetComponent<Rigidbody> ();
        characterController = GetComponent<CharacterController> ();
    }

    // Update is called once per frame
    void Update(){
    
    }


    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        //Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        //rb.AddForce(movement * playerSpeed);



        if (characterController != null) {
            Vector3 move = new Vector3(moveHorizontal, 0, moveVertical);
            characterController.Move(move * playerSpeed * Time.deltaTime);
            characterController.Move(playerVelocity * Time.deltaTime);
        }

    }
}
