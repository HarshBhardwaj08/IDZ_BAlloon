using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public enum TypeOfPen
{
    Create,
    Magnet,
    Box,
    Food,
    Monster,
    Water,
    Rope,
    Destroy,
    Ragdoll,
    SBCircle,
    SBSquare,
    SBRect,
    SBTri,
    SBPent,
    SBPlus,
    SBCar,
    SBWater
}
public class LinesDrawer : MonoBehaviour
{
    public static LinesDrawer Instance;

    public bool isMouseOverUI;

    public GraphicRaycaster graphicraycaster;
    public SaveGameObject savegameobject;
    public TypeOfPen penType;
    public GameObject linePrefab, MagnetPrefab, boxprefab, foodprefab, monsterprefab, waterprefab, ragdollPrefab, Ropeprefab, SBCirlceprefab, SBSquareprefab, SBRectprefab, SBTriprefab, Eraserprefab, SBPentprefab, SBPlusprefab, SBCarprefab, SBWaterprefab;
    public LayerMask cantDrawOverLayer, DestroyLayer;
    int cantDrawOverLayerIndex;

    [Space(30f)]
    public Gradient lineColor;
    public float linePointsMinDistance, BoxandFoodspawn;
    public float lineWidth;
    public Line currentLine;
    public bool chosingColor, iscreating;
    GameObject currentMagnet, currentEraser;
    Vehicle currentCar;
    Vector2 updatemousepos, updatetouchpos;
    Camera cam;
    float boxfoodspawntime, magnetSize, boxSize, foodSize, monsterSize, waterSize, ropeSize, ragdollSize, SBCSize, SBSSize, SBRSize, SBTSize, SBPenSize, SBPlusSize, SBCarSize, SBWaterSize;


    List<RaycastResult> results;
    PointerEventData ped;
    private List<GameObject> AllObjects = new List<GameObject>();
    void Start()
    {
        if (Instance == null) Instance = this;
        else return;

        cam = Camera.main;
        savegameobject = GetComponent<SaveGameObject>();
        cantDrawOverLayerIndex = LayerMask.NameToLayer("CantDrawOver");
        boxfoodspawntime = BoxandFoodspawn;

        magnetSize = MagnetPrefab.transform.localScale.x;
        boxSize = boxprefab.transform.localScale.x * 2;
        foodSize = foodprefab.transform.localScale.x * 2;
        monsterSize = monsterprefab.transform.localScale.x;
        waterSize = waterprefab.transform.localScale.x / 2;
        ragdollSize = ragdollPrefab.transform.localScale.x;
        ropeSize = 0.5f;

        SBCSize = SBCirlceprefab.transform.localScale.x;
        SBSSize = SBSquareprefab.transform.localScale.x;
        SBRSize = SBRectprefab.transform.localScale.x;
        SBTSize = SBTriprefab.transform.localScale.x;
        SBPenSize = SBPentprefab.transform.localScale.x;
        SBPlusSize = SBPlusprefab.transform.localScale.x;
        SBCarSize = SBCarprefab.transform.localScale.x;
        SBWaterSize = 1;
        //eraserSize = Eraserprefab.transform.localScale.x;

        currentMagnet = null;
        currentEraser = null;

        results = new List<RaycastResult>();
        ped = new PointerEventData(null);

        if (savegameobject == null)
            return;
        savegameobject.LevelNumber = DataManager.instance.LevelToLoad;
        savegameobject.LoadChildren();
    }

    void Update()
    {
        boxfoodspawntime += Time.deltaTime;
        if (chosingColor)
            return;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            updatetouchpos = cam.ScreenToWorldPoint(touch.position);

            results.Clear();
            //Set required parameters, in this case, mouse position
            ped.position = touch.position;
            //Raycast ui items
            graphicraycaster.Raycast(ped, results);

            if (results.Count > 0)
            {
                EndDraw(updatetouchpos, true);
                return;
            }

            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    // Record initial touch position.
                    BeginDraw(updatetouchpos);
                    break;

                case TouchPhase.Stationary:
                    BeginDraw(updatetouchpos, true);
                    break;
                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    Draw(updatetouchpos);
                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    EndDraw(updatetouchpos);
                    break;
            }
        } else
        {
            isMouseOverUI = false;
        }

        /*updatemousepos = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
            BeginDraw(updatemousepos);

        if (Input.GetMouseButton(0))
            Draw(updatemousepos);

        if (Input.GetMouseButtonUp(0))
            EndDraw(updatemousepos);*/
    }

    // Begin Draw ----------------------------------------------
    void BeginDraw(Vector2 position, bool stationary = false)
    {
        if (penType == TypeOfPen.Destroy)
            raycastDestroy(position);
        else if (penType == TypeOfPen.Magnet && currentMagnet == null)
            raycastMagnet(MagnetPrefab, magnetSize, position);
        else if (penType == TypeOfPen.Box)
            raycastMagnet(boxprefab, boxSize, position, stationary);
        else if (penType == TypeOfPen.Food)
            raycastMagnet(foodprefab, foodSize, position, stationary);
        else if (penType == TypeOfPen.Monster)
            raycastMagnet(monsterprefab, monsterSize, position);
        else if (penType == TypeOfPen.Water)
            raycastMagnet(waterprefab, waterSize, position, stationary);
        else if (penType == TypeOfPen.Ragdoll)
            raycastMagnet(ragdollPrefab, ragdollSize, position);
        else if (penType == TypeOfPen.Rope && !stationary) 
            raycastMagnet(Ropeprefab, ropeSize, position);
        else if (penType == TypeOfPen.SBCircle)
            raycastMagnet(SBCirlceprefab, SBCSize, position, stationary, true);
        else if (penType == TypeOfPen.SBSquare)
            raycastMagnet(SBSquareprefab, SBSSize, position, stationary, true);
        else if (penType == TypeOfPen.SBRect)
            raycastMagnet(SBRectprefab, SBRSize, position, stationary, true);
        else if (penType == TypeOfPen.SBTri)
            raycastMagnet(SBTriprefab, SBTSize, position, stationary, true);
        else if (penType == TypeOfPen.SBPent)
            raycastMagnet(SBPentprefab, SBPenSize, position, stationary, true);
        else if (penType == TypeOfPen.SBPlus)
            raycastMagnet(SBPlusprefab, SBPlusSize, position, stationary, true);
        else if (penType == TypeOfPen.SBCar)
            raycastMagnet(SBCarprefab, SBCarSize, position, stationary);
        else if (penType == TypeOfPen.SBWater)
            raycastMagnet(SBWaterprefab, SBWaterSize, position, stationary);
        if (linePrefab == null || isMouseOverUI || iscreating)
            return;
        iscreating = true;
        currentLine = Instantiate(linePrefab, transform).GetComponent<Line>();
        AllObjects.Add(currentLine.gameObject);
        //Set line properties
        currentLine.UsePhysics(false);
        currentLine.SetLineColor(lineColor);
        currentLine.SetPointsMinDistance(linePointsMinDistance);
        currentLine.SetLineWidth(lineWidth);

    }

    private void raycastMagnet(GameObject prefab, float prefabSize, Vector2 position, bool stationary = false, bool cancolorchange = false)
    {
        if (stationary)
        {
            if (boxfoodspawntime < BoxandFoodspawn) return;
        }
        if (boxfoodspawntime >= BoxandFoodspawn)
        {
            boxfoodspawntime = 0f;
        }

        if(penType == TypeOfPen.SBCar && currentCar)
        {
            Destroy(currentCar.gameObject);
        }

        Vector2 mousePosition = position; //cam.ScreenToWorldPoint(Input.mousePosition)

        //Check if mousePos hits any collider with layer "CantDrawOver", if true cut the line by calling EndDraw( )
        RaycastHit2D hit = Physics2D.CircleCast(mousePosition, prefabSize, Vector2.zero, 1f, DestroyLayer);

        if (!hit && !isMouseOverUI)
        {
            GameObject temp = Instantiate(prefab, mousePosition, Quaternion.identity, transform);
            AllObjects.Add(temp);
            if (cancolorchange)
                temp.GetComponent<SpriteRenderer>().color = lineColor.colorKeys[0].color;
            if (penType == TypeOfPen.Magnet)
                currentMagnet = temp;
            if (penType == TypeOfPen.SBCar)
                currentCar = temp.GetComponent<Vehicle>();
        } else
        {
            EndDraw(position);
        }
    }

    private void raycastDestroy(Vector2 position)
    {
        Vector2 mousePosition = position; //cam.ScreenToWorldPoint(Input.mousePosition)

        if (!currentEraser)
            currentEraser = Instantiate(Eraserprefab, mousePosition, Quaternion.identity, transform);

        if (!isMouseOverUI)
        {
            currentEraser.SetActive(true);
            currentEraser.transform.position = mousePosition;
        }
        else
            currentEraser.SetActive(false);

        mousePosition = currentEraser.transform.Find("Raycast_origin").position;
        //Check if mousePos hits any collider with layer "CantDrawOver", if true cut the line by calling EndDraw( )
        RaycastHit2D hit = Physics2D.CircleCast(mousePosition, lineWidth / 3f, Vector2.zero, 1f, DestroyLayer);

        if (hit)
        {
            currentEraser.SetActive(true);
            if (hit.collider.gameObject.CompareTag("SoftBody") || hit.collider.gameObject.CompareTag("Ragdoll"))
            {
                Destroy(hit.transform.parent.gameObject);
            }
            else if(hit.collider.gameObject.CompareTag("Vehicle"))
            {
                Destroy(hit.transform.parent.gameObject);
                currentCar = null;
            }
            else Destroy(hit.collider.gameObject);
        }
    }

    // Draw ----------------------------------------------------
    void Draw(Vector2 position)
    {
        if (penType == TypeOfPen.Destroy)
        {
            raycastDestroy(position);
            return;
        }
        else if (penType == TypeOfPen.Water)
            raycastMagnet(waterprefab, waterSize, position, true);
        else if (penType == TypeOfPen.SBWater)
            raycastMagnet(SBWaterprefab, SBWaterSize, position, true);
        else if (penType == TypeOfPen.Magnet && currentMagnet != null)
        {
            currentMagnet.transform.position = position;
        }
        else if (penType == TypeOfPen.Box)
            raycastMagnet(boxprefab, boxSize, position, true);
        else if (penType == TypeOfPen.Food)
            raycastMagnet(foodprefab, foodSize, position, true);
        else if (penType == TypeOfPen.SBCar)
            raycastMagnet(SBCarprefab, SBCarSize, position, true);
        else if (penType == TypeOfPen.Rope)
            return;
        if (currentLine == null)
            return;

        //Check if mousePos hits any collider with layer "CantDrawOver", if true cut the line by calling EndDraw( )
        RaycastHit2D hit = Physics2D.CircleCast(position, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer);

        // if (hit || isMouseOverUI)
        //  EndDraw(position);
        //else
        currentLine.AddPoint(position);
    }
    // End Draw ------------------------------------------------
    void EndDraw(Vector2 position, bool calledthroughraycast = false)
    {
        if (penType == TypeOfPen.Destroy)
        {
            raycastDestroy(position);
            Destroy(currentEraser);
            return;
        }
        else if (penType == TypeOfPen.Magnet && currentMagnet != null)
        {
            Destroy(currentMagnet);
            return;
        }
        else if (calledthroughraycast == false && penType == TypeOfPen.Rope)
            return;

        if (currentLine != null)
        {
            if (currentLine.pointsCount < 2)
            {
                //If line has one point
                Destroy(currentLine.gameObject);
                currentLine = null;
            }
            else
            {
                //Add the line to "CantDrawOver" layer
                currentLine.gameObject.layer = cantDrawOverLayerIndex;

                //Activate Physics on the line
                currentLine.UsePhysics(true);

                currentLine = null;
            }
            iscreating = false;
        }
    }

    public void OnCar_LeftButtonDown()
    {
        if(currentCar) currentCar.MoveLeft();
    }

    public void OnCar_RightButtonDown()
    {
        if (currentCar) currentCar.MoveRight();
    }

    public void OnCar_MoveButtonUp()
    {
        if (currentCar) currentCar.StopMovement();
    }

}