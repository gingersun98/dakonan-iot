using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityToTarget : MonoBehaviour
{
    public Transform gravityCenter;
    public float gravityStrength = 10f;

    private Rigidbody2D rb;

    public Vector2 externalForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // =========================
    // PERGANTIAN POINT GRAVITASI
    // =========================
    public void Initialize(Transform newTarget)
    {
        gravityCenter = newTarget;

        rb.AddForce(Vector2.one * 5);
    }

    // =========================
    // SIMULASI GRAVITASI
    // =========================
    private void FixedUpdate()
    {
        if (gravityCenter == null)
            return;

        Vector3 targetPos =
            gravityCenter.position +
            GameManager.Instance.phoneOffset;

        Vector2 direction =
            (targetPos - transform.position).normalized;

        rb.AddForce(direction * gravityStrength);
    }
}