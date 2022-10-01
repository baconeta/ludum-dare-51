using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    Player.Player Player;

    [SerializeField]
    float FollowSpeed = 10f;

    [SerializeField]
    Vector2 posOffset = new Vector2(0.5f, 0.5f);

    [SerializeField]
    private float leftLimit = -25f;
    [SerializeField]
    private float RightLimit = 25f;
    [SerializeField]
    private float BottomLimit = -25f;
    [SerializeField]
    private float TopLimit = 25f;


    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<Player.Player>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Player != null)
        {
            Vector3 start = transform.position;
            Vector3 end = Player.transform.position;

            end.x += posOffset.x;
            end.y += posOffset.y;
            end.z = transform.position.z;

            transform.position = Vector3.Lerp(start, end, FollowSpeed * Time.fixedDeltaTime);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimit, RightLimit),
                                             Mathf.Clamp(transform.position.y, BottomLimit, TopLimit),
                                             transform.position.z);
        }
        else
        {
            Debug.LogWarning("Player Ref was null");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(leftLimit, TopLimit), new Vector2(RightLimit, TopLimit));
        Gizmos.DrawLine(new Vector2(RightLimit, TopLimit), new Vector2(RightLimit, BottomLimit));
        Gizmos.DrawLine(new Vector2(RightLimit, BottomLimit), new Vector2(leftLimit, BottomLimit));
        Gizmos.DrawLine(new Vector2(leftLimit, BottomLimit), new Vector2(leftLimit, TopLimit));
    }
}
