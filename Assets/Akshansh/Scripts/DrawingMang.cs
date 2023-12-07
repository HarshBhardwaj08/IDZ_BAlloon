using AkshanshKanojia.Inputs.Mobile;
using UnityEngine;

public class DrawingMang : MobileInputs
{
    [SerializeField] GameObject linePref;
    [SerializeField] float zDepth = 10f;

    GameObject active;
    public override void OnTapEnd(MobileInputManager.TouchData _data)
    {
    }

    public override void OnTapMove(MobileInputManager.TouchData _data)
    {
        var _pos = _data.TouchPosition;
        _pos.z = zDepth;
        _pos = Camera.main.ScreenToWorldPoint(_pos);
        active.transform.position =_pos;
    }

    public override void OnTapped(MobileInputManager.TouchData _data)
    {
        var _pos = _data.TouchPosition;
        _pos.z = zDepth;
        _pos = Camera.main.ScreenToWorldPoint(_pos);
        active = Instantiate(linePref, _pos,Quaternion.identity);
        active.GetComponent<TrailRenderer>().Clear();
    }

    public override void OnTapStay(MobileInputManager.TouchData _data)
    {
    }
}
