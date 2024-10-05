using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private bool followX = false;
    [SerializeField]
    private bool followY = false;
    [SerializeField]
    private bool followZ = false;
    [SerializeField]
    private Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if (followX)
        {
            pos.x = target.position.x;
        }
        if (followY)
        {
            pos.y = target.position.y;
        }
        if (followZ)
        {
            pos.z = target.position.z;
        }
        transform.position = pos;
    }
}
