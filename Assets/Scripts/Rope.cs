using UnityEngine;

public class Rope : MonoBehaviour
{
    public Vector2 pointStart, pointEnd;
    public Color color;
    public Rigidbody2D hook_start, hook_end;
    public GameObject rope_prefab;
    public int links = 0;

    public void generateRope(Vector2 start, Vector2 end,Color ropeColor)//public void generateRope()
    {
        pointStart = start;
        pointEnd = end;
        color = ropeColor;
        float distance = Vector2.Distance(start, end);
        float ropeLength = rope_prefab.GetComponent<SpriteRenderer>().bounds.size.y;
        links = (int)(distance *2 / ropeLength);
        links++;
        if (links < 3)
            Destroy(gameObject);
        Rigidbody2D prev_rb = hook_start;
        for (int i = 0; i < links; i++)
        {
            GameObject newseg = Instantiate(rope_prefab);
            newseg.GetComponent<SpriteRenderer>().color = ropeColor;
            newseg.transform.parent = transform;
            newseg.transform.position = transform.position;
            HingeJoint2D hj = newseg.GetComponent<HingeJoint2D>();
            hj.connectedBody = prev_rb;
            prev_rb = newseg.GetComponent<Rigidbody2D>();
        }

        hook_end.transform.position = end;
        prev_rb.transform.position = end;
        hook_end.GetComponent<HingeJoint2D>().connectedBody = prev_rb;

        prev_rb.GetComponent<SpriteRenderer>().enabled = false;
        prev_rb.GetComponent<BoxCollider2D>().enabled = false;
    }
}
