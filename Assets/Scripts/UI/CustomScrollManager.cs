using UnityEngine;
using UnityEngine.EventSystems;

public class CustomScrollManager : MonoBehaviour,  IPointerDownHandler
{
    [SerializeField] float endingOffsetMultiplier = 3f, ObjectSpacing = 10f, dragSpeed = 3f, dragDetectionSenstivity = 2f;//increase offset
    [SerializeField] float upPos, downPos;
    public bool IsDragging = false;
    public bool CanScroll = true;
    int childCount;
    Vector3 tempTouchPos;
    private void Start()
    {
        childCount = transform.childCount;
        ArrangeChildern();
        upPos = transform.GetChild(0).position.y + transform.GetChild(0).GetComponent<RectTransform>().rect.height * endingOffsetMultiplier;
        downPos = transform.GetChild(childCount-1).position.y - transform.GetChild(childCount-1).GetComponent<RectTransform>().rect.height * endingOffsetMultiplier;
    }
    void ArrangeChildern()
    {
        for (int i = 0; i < childCount - 1; i++)
        {
            Transform _obj = transform.GetChild(i + 1);
            RectTransform _ref = transform.GetChild(i).GetComponent<RectTransform>();
            _obj.localPosition = _ref.localPosition;
            _obj.localPosition -= new Vector3(0, _ref.rect.height + ObjectSpacing, 0);
        }
    }
    private void Update()
    {
        if (IsDragging)
        {
            if (Input.GetMouseButton(0))
            {
                var _Dir = Input.mousePosition.y - tempTouchPos.y;
                if (Mathf.Abs(_Dir) > dragDetectionSenstivity)
                {
                    _Dir = _Dir < 0 ? -1 : 1;
                    for (int i = 0; i < childCount; i++)
                    {
                        transform.GetChild(i).position += _Dir * dragSpeed *  Vector3.up;
                        if(CheckChildPos(transform.GetChild(i).gameObject,_Dir>0))
                        {
                            break;
                        }
                    }
                    tempTouchPos = Input.mousePosition;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                IsDragging = false;
                tempTouchPos = Input.mousePosition;
            }
        }
    }

    public bool CheckChildPos(GameObject _obj, bool isUp)
    {
        if (_obj.transform.position.y < upPos && _obj.transform.position.y > downPos)
        {
            return false;
        }
        if (isUp)
        {
            _obj.transform.position =  transform.GetChild(childCount - 1).position;
            _obj.transform.SetAsLastSibling();
            _obj.transform.localPosition += new Vector3(0, -transform.GetChild(childCount - 1).GetComponent<RectTransform>().rect.height - ObjectSpacing, 0);
        }
        else
        {
            _obj.transform.position = transform.GetChild(0).position;
            _obj.transform.SetAsFirstSibling();
            _obj.transform.localPosition += new Vector3(0, transform.GetChild(childCount - 1).GetComponent<RectTransform>().rect.height + ObjectSpacing, 0);
        }
        return true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetDragging();
    }

    public void SetDragging()
    {
        if (CanScroll)
            IsDragging = true;
    }
}
