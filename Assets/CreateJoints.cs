using UnityEngine;

public class CreateJoints : MonoBehaviour
{
    public bool stationary,called;

    Vector2 prev_pos, cur_pos;
    private void Start()
    {
        prev_pos = transform.position;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!called)
            stationary = checkStationary();
        if (!called && stationary)
        {
            transform.parent.GetComponent<destroyChildren>().DestroyChild();
            called = true;
        }
    }

    bool checkStationary()
    {
        cur_pos = transform.position;
        cur_pos.x = (float)decimal.Round((decimal)cur_pos.x, 2);
        cur_pos.y = (float)decimal.Round((decimal)cur_pos.y, 2);
        if (prev_pos != cur_pos)
        {
            prev_pos = cur_pos;
            return false;
        }
        return true;
    }
}
