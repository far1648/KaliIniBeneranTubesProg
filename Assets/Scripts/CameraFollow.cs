using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 1.5f;

    public Vector3 offset;
    private Vector3 targetPos;
    PlayerMovement player;
    void Start()
    {
        if (target == null) return;

        offset = transform.position - target.position;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        targetPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);

    }
}
