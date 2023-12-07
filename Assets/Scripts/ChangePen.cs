using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangePen : MonoBehaviour
{
    [SerializeField] LinesDrawer lineDrawer;
    [SerializeField] Image highlightColorImage, highlightBrushImage;
    [SerializeField] Transform colorchangeAffected;

    private void Start()
    {
        if (lineDrawer == null)
            lineDrawer = FindObjectOfType<LinesDrawer>();
    }

    public void changePen(GameObject penPrefab)
    {
        lineDrawer.linePrefab = penPrefab;
        lineDrawer.penType = TypeOfPen.Create;
        LinesDrawer.Instance.isMouseOverUI = false;
        highlightBrushImage.transform.SetParent(EventSystem.current.currentSelectedGameObject.transform, false);
    }
    public void changePen(string pen)
    {
        bool temp = true;
        switch (pen)
        {
            case "eraser":
                lineDrawer.penType = TypeOfPen.Destroy;
                break;
            case "magnet":
                lineDrawer.penType = TypeOfPen.Magnet;
                break;
            case "box":
                lineDrawer.penType = TypeOfPen.Box;
                break;
            case "food":
                lineDrawer.penType = TypeOfPen.Food;
                break;
            case "monster":
                lineDrawer.penType = TypeOfPen.Monster;
                break;
            case "water":
                lineDrawer.penType = TypeOfPen.Water;
                break;
            case "ragdoll":
                lineDrawer.penType = TypeOfPen.Ragdoll;
                break;
            case "rope":
                lineDrawer.penType = TypeOfPen.Rope;
                break;
            case "circle":
                lineDrawer.penType = TypeOfPen.SBCircle;
                break;
            case "square":
                lineDrawer.penType = TypeOfPen.SBSquare;
                break;
            case "rect":
                lineDrawer.penType = TypeOfPen.SBRect;
                break;
            case "tri":
                lineDrawer.penType = TypeOfPen.SBTri;
                break;
            case "penta":
                lineDrawer.penType = TypeOfPen.SBPent;
                break;
            case "plus":
                lineDrawer.penType = TypeOfPen.SBPlus;
                break;
            case "car":
                lineDrawer.penType = TypeOfPen.SBCar;
                break;
            case "water_jelly":
                lineDrawer.penType = TypeOfPen.SBWater;
                break;
            default:
                temp = false;
                break;
        }
        if (temp)
        {
            lineDrawer.linePrefab = null;
            LinesDrawer.Instance.isMouseOverUI = false;
            highlightBrushImage.transform.SetParent(EventSystem.current.currentSelectedGameObject.transform, false);
        }
    }

    public void changeColor(Image img)
    {
        if (img == null)
        {
            return;
        }
        highlightColorImage.transform.SetParent(img.transform, false);
        Color color = img.color;
        if (lineDrawer == null)
            return;
        GradientColorKey[] colorkeys = lineDrawer.lineColor.colorKeys;
        for (int i = 0; i < colorkeys.Length; i++)
        {
            colorkeys[i].color = color;
        }
        lineDrawer.lineColor.colorKeys = colorkeys;
        LinesDrawer.Instance.isMouseOverUI = false;

        Transform tempchild;

        for (int i = 0; i < colorchangeAffected.childCount; i++)
        {
            tempchild = colorchangeAffected.GetChild(i).Find("BG");
            if (tempchild)
                tempchild.GetComponent<Image>().color = color;
        }
    }

    public void onclickdustbin()
    {
        //show popupHere
        if(lineDrawer.transform.childCount <= 0)
        {
            return;
        }
        lineDrawer.savegameobject.LevelNumber = DataManager.instance.LevelToLoad;
        for (int i = 0; i < lineDrawer.transform.childCount; i++)
            Destroy(lineDrawer.transform.GetChild(i).gameObject);
        changePen((GameObject) null);

        highlightBrushImage.transform.SetParent(null, false);
        highlightBrushImage.transform.SetParent(null, false);
    }

    public void onclickbackbutton()
    {
        lineDrawer.savegameobject.LevelNumber = DataManager.instance.LevelToLoad;
        GameSceneManager.instance.backbuttonpressed(lineDrawer);
    }
}
