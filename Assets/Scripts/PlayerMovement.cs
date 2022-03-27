using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   

    private float playerSpeed = 5.0f;
    private Rigidbody rb;


    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    void Update(){
    
    }

    void FixedUpdate(){
        translatePlayer();
        rotatePlayer();
    }

    private void translatePlayer() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rb.MovePosition(this.transform.position + movement * Time.deltaTime * playerSpeed);
    }

    private void rotatePlayer() {
        Vector2 posOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = AngleBetweenTwoPoints(posOnScreen, mouseOnScreen);
        Quaternion rotation = Quaternion.Euler(new Vector3(0f, -angle, 0f));

        rb.rotation = Quaternion.Slerp(rb.transform.rotation, rotation, 5 * Time.deltaTime);
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
