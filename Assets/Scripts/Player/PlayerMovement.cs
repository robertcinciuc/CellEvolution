using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   

    public float playerSpeed = 8.0f;

    private Rigidbody rigidBody;

    void Start(){
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update(){
    
    }

    void FixedUpdate(){
        if (!PlayerState.isActive) {
            return;
        }

        translatePlayerToMouse();
        rotateOnForwardMovement();
    }

    private void translatePlayerToMouse() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        Vector3 moveVector = this.transform.TransformDirection(movement) * playerSpeed;
        rigidBody.velocity = new Vector3(moveVector.x, rigidBody.velocity.y, moveVector.z);
    }

    private void rotatePlayer() {
        Vector2 posOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = AngleBetweenTwoPoints(posOnScreen, mouseOnScreen);
        Quaternion rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));

        rigidBody.rotation = Quaternion.Slerp(rigidBody.transform.rotation, rotation, 7 * Time.deltaTime);
    }

    private void rotateOnForwardMovement(){
        if (Input.GetKey(KeyCode.W)) {
            rotatePlayer();
        }
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(b.x - a.x, b.y - a.y) * Mathf.Rad2Deg;
    }
}
