using UnityEngine;

public class ButtonScroller : MonoBehaviour
{

    bool isMoving = false;
    [SerializeField] float trackSpeed = 3f;
    [SerializeField] Transform centerObj, parent;

    CustomScrollManager scroll;
    // Update is called once per frame
    private void Start()
    {
        parent = transform.parent;
        scroll = parent.GetComponent<CustomScrollManager>();
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(SetTrack);
    }
    void Update()
    {
        if (isMoving)
        {
            Vector3 _dir = centerObj.position - transform.position;
            _dir.x = 0;
            for (int i = 0; i < parent.childCount; i++)
            {
                parent.GetChild(i).transform.position += Time.deltaTime * trackSpeed * _dir;
                if (scroll.CheckChildPos(parent.GetChild(i).gameObject, _dir.y > 0))
                {
                    break;
                }
            }
            if (_dir.magnitude < 1f)
            {
                isMoving = false;
                scroll.CanScroll = true;
            }
        }
    }

    void SetTrack()
    {
        if (scroll.CanScroll)
        {
            scroll.CanScroll = false;
            isMoving = true;
        }
    }
}
