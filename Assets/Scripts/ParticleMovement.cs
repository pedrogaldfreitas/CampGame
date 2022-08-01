using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    public float height; //Height of the particle.
    [SerializeField]
    private float fallSpeed; //Falling speed of the object.

    public float yval;
    private Vector2 heightpos;
    private Renderer particleRenderer;


    // Start is called before the first frame update
    void Start()
    {


        particleRenderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody2D>();

        heightpos = new Vector2(transform.position.x, transform.position.y + height);

        yval = rb.transform.position.y;

        //To set the actual position to the particle's height:
        rb.MovePosition(heightpos);
    }

    // Update is called once per frame
    void Update()
    {
        heightpos.y = yval + height;
    }

    private void FixedUpdate()
    {
        height = height - (fallSpeed*Time.deltaTime);
        heightpos.y = yval + height;
        if (height >= 0)
        {
            fallSpeed = fallSpeed + 1;
        } else if (height < 0)
        {
            height = 0;
            fallSpeed = -fallSpeed;
        }
        rb.MovePosition(heightpos);

    }

}
