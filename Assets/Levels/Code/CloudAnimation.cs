using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloudAnimation : MonoBehaviour
{

    public float speedMin = 5;
    public float speedMax = 7;
    public float padding = 0;
    public float offsetYRnd = 2;
    public Vector3 direction = Vector3.right;

    private float speed;
    private float cloudY;

    private float camWidth;
    private float camX;
    private float cloudWidth;
    private float cloudYStart;

    // Start is called before the first frame update
    void Start()
    {
        Camera cam = Camera.main;
        camX = cam.transform.position.x;
        camWidth = 2f * cam.orthographicSize * cam.aspect;
        cloudWidth = GetComponent<SpriteRenderer>().size.x;
        cloudYStart = gameObject.transform.position.y;
        reset();
        cloudY = cloudYStart;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = gameObject.transform.position;
        pos.y = cloudY;
        bool movesRight = direction.x > 0;
        if (movesRight && pos.x - camWidth/2 > camX + camWidth/2 + padding )
        {
            pos.x = camX - camWidth / 2 - cloudWidth - padding;
            reset();
        } else if (!movesRight && pos.x + camWidth / 2 < camX - camWidth / 2 - padding)
        {
            pos.x = camX + camWidth / 2 + cloudWidth + padding;
            reset();
        }
        pos += direction * speed * Time.deltaTime;
        gameObject.transform.position = pos;
    }

    private void reset()
    {
        speed = speedMin + Random.value * (speedMax - speedMin);
        cloudY = cloudYStart + (Random.value * 2.0f - 1.0f) * offsetYRnd;
    }
}
