using UnityEngine;

// Lecture 06, Part 1: a rate, every physics step.
// Put on the Sphere. Tag it "Player". It needs a Rigidbody + Sphere Collider.
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveForce = 10f;
    [SerializeField] ForceMode mode = ForceMode.Force;

    Rigidbody rb;
    Vector3 input;

    void Awake() => rb = GetComponent<Rigidbody>();

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        input = Vector3.ClampMagnitude(new Vector3(h, 0f, v), 1f);
    }

    void FixedUpdate() => rb.AddForce(input * moveForce, mode);
}
