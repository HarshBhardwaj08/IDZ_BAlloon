using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.IO;
using static LevelScreenManager;

public class SaveGameObject : MonoBehaviour
{
    LinesDrawer LinesDrawer;
    public GameObject wallprefab, gravitywallprefab, floatwallprefab, vehicleprefab, SBC_prefab, SBS_prefab, SBR_prefab, SBT_prefab, SBPen_prefab, SBPlus_prefab, SBCar_prefab, box_prefab, food_prefab, water_prefab, monster_prefab, rope_prefab;
    public string pathtofile;
    public int LevelNumber;
    public int layerint;

    private void Start()
    {
        //".json";
        LinesDrawer = GetComponent<LinesDrawer>();
    }

    public void SaveChildren()
    {
        //Start();
        //print("Saving...");
        StartCoroutine(SaveChildrenIenum());
    }

    public void LoadChildren()
    {
        //Start();
        //print("Loading...");
        StartCoroutine(loadChildrenIenum());
    }

    public IEnumerator SaveChildrenIenum()
    {
        Time.timeScale = 0;
        if (LinesDrawer == null)
            LinesDrawer = GetComponent<LinesDrawer>();
        DrawnLineRenderer Dlrs = new DrawnLineRenderer();

        yield return null;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.name.Contains("Wall"))
                Dlrs.Walls.Add(saveWall(child));
            if (child.name.Contains("Gravity"))
                Dlrs.GravityWalls.Add(saveWall(child));
            if (child.name.Contains("Float"))
                Dlrs.FloatWalls.Add(saveWall(child));
            if (child.name.Contains("Vehicle"))
                Dlrs.VehicleWalls.Add(saveWall(child));
            if (child.name.Contains("CircleStiff"))
                Dlrs.Softbodies.Add(saveSB(child, "CircleStiff"));
            if (child.name.Contains("SquareSmall"))
                Dlrs.Softbodies.Add(saveSB(child, "SquareSmall"));
            if (child.name.Contains("Rectangle"))
                Dlrs.Softbodies.Add(saveSB(child, "Rectangle"));
            if (child.name.Contains("Triangle"))
                Dlrs.Softbodies.Add(saveSB(child, "Triangle"));
            if (child.name.Contains("Pentagon"))
                Dlrs.Softbodies.Add(saveSB(child, "Pentagon"));
            if (child.name.Contains("Plus"))
                Dlrs.Softbodies.Add(saveSB(child, "Plus"));
            if (child.name.Contains("car"))
                Dlrs.Softbodies.Add(saveSB(child, "car"));
            if (child.name.Contains("Water"))
                Dlrs.box_food_Monster.Add(savebox(child, "Water"));
            if (child.name.Contains("Box"))
                Dlrs.box_food_Monster.Add(savebox(child, "Box"));
            if (child.name.Contains("Food"))
                Dlrs.box_food_Monster.Add(savebox(child, "Food"));
            if (child.name.Contains("Monster"))
                Dlrs.box_food_Monster.Add(savebox(child, "Monster"));
            if (child.name.Contains("Rope"))
            {
                saveRope rope = saveRope(child);
                if(rope != null) Dlrs.Savedropes.Add(rope);
            }

            yield return null;
        }

        yield return null;
        string strtosave = JsonUtility.ToJson(Dlrs);
        pathtofile = Application.persistentDataPath + "/savefile_";
        pathtofile += LevelNumber + ".json";
        File.WriteAllText(pathtofile, strtosave);
        saveLevelFile();
        yield return null;
        Time.timeScale = 1;
        //print("Saved Level to " + pathtofile);
    }

    public void saveLevelFile()
    {
        string levelsFilePath = Path.Combine(Application.persistentDataPath, "levels.json");
        string levelsFile = File.ReadAllText(levelsFilePath);
        LevelScreenManager.LevelsHolder levelsHolder = JsonUtility.FromJson<LevelScreenManager.LevelsHolder>(levelsFile);

        for (int i = 0; i < levelsHolder.categories.Count; i++)
        {
            if (levelsHolder.categories[i].Category == CategoryDataHolder.Instance.ActiveCategory)
                if (!levelsHolder.categories[i].LevelsInCategory.Contains(LevelNumber))
                    levelsHolder.categories[i].LevelsInCategory.Add(LevelNumber);
        }

        string strToWrite = JsonUtility.ToJson(levelsHolder);
        File.WriteAllText(levelsFilePath, strToWrite);
    }

    public IEnumerator loadChildrenIenum()
    {
        Time.timeScale = 0;
        pathtofile = Application.persistentDataPath + "/savefile_";
        pathtofile += LevelNumber + ".json";
        string json;
        if (!File.Exists(pathtofile))
        {
            //print("Unable to load Level from " + pathtofile);
            Time.timeScale = 1;
            yield break;
        }
        json = File.ReadAllText(pathtofile);
        //print("Loading Level from " + pathtofile);
        DrawnLineRenderer dlrs = JsonUtility.FromJson<DrawnLineRenderer>(json);

        yield return null;

        foreach (saveRope sr in dlrs.Savedropes)
        {
            loadropes(sr);
            yield return null;
        }

        foreach (LineRendererSave lrs in dlrs.Walls)
        {
            GameObject wall = Instantiate(wallprefab, transform);
            loadWall(wall, lrs);
            yield return null;
        }
        foreach (LineRendererSave lrs in dlrs.GravityWalls)
        {
            GameObject wall = Instantiate(gravitywallprefab, transform);
            Line linecom = wall.GetComponent<Line>();
            linecom.UsePhysics(false);
            loadWall(wall, lrs);
            linecom.UsePhysics(true);
            yield return null;
        }
        foreach (LineRendererSave lrs in dlrs.FloatWalls)
        {
            GameObject wall = Instantiate(floatwallprefab, transform);
            Line linecom = wall.GetComponent<Line>();
            linecom.UsePhysics(false);
            loadWall(wall, lrs);
            linecom.UsePhysics(true);
            yield return null;
        }
        foreach (LineRendererSave lrs in dlrs.VehicleWalls)
        {
            GameObject wall = Instantiate(vehicleprefab, transform);
            Line linecom = wall.GetComponent<Line>();
            linecom.UsePhysics(false);
            loadWall(wall, lrs);
            linecom.UsePhysics(true);
            yield return null;
        }
        foreach (Softbody sb in dlrs.Softbodies)
        {
            loadsoftbodies(sb);
            yield return null;
        }
        foreach (box bx in dlrs.box_food_Monster)
        {
            loadbox(bx);
            yield return null;
        }
        yield return null;
        Time.timeScale = 1;
        //print("Loaded Level from " + pathtofile);
    }

    public saveRope saveRope(Transform rope)
    {
        saveRope saverope = new saveRope();

        RopeBridge rope_comp = rope.gameObject.GetComponent<RopeBridge>();
        if (!rope_comp) return null;

        saverope.startposition = rope_comp.m_StartPoint.position;
        saverope.endposition = rope_comp.m_EndPoint.position;

        return saverope;
    }

    public box savebox(Transform box, string name)
    {
        box bx = new box();

        bx.name = name;

        bx.position = box.position;
        bx.rotation = box.rotation;

        return bx;
    }

    public Softbody saveSB(Transform softbody, string name)
    {
        Softbody sb = new Softbody();

        sb.name = name;
        sb.position = softbody.position;
        sb.rotation = softbody.rotation;

        for (int i = 0; i < softbody.childCount; i++)
        {
            Transform child = softbody.GetChild(i);
            sb.boneposition.Add(child.position);
            sb.bonerotation.Add(child.rotation);
        }

        sb.color = softbody.GetComponent<SpriteRenderer>().color;

        return sb;
    }

    public LineRendererSave saveWall(Transform wall)
    {
        LineRenderer lr = wall.GetComponent<LineRenderer>();
        EdgeCollider2D edgcol = wall.GetComponent<EdgeCollider2D>();

        LineRendererSave lrs = new LineRendererSave();
        lrs.position = wall.transform.position;
        lrs.rotation = wall.transform.rotation;
        lrs.scale = wall.transform.localScale;
        for (int i = 0; i < lr.positionCount; i++)
        {
            lrs.LineRendpoints.Add(lr.GetPosition(i));
        }
        lrs.lineColor = lr.colorGradient;
        lrs.EdgeCollpoints.AddRange(edgcol.points);
        foreach (CircleCollider2D circleCol in wall.GetComponents<CircleCollider2D>())
        {
            lrs.circleColsOffset.Add(circleCol.offset);
        }
        return lrs;
    }

    public void loadropes(saveRope sr)
    {
        GameObject rope = Instantiate(rope_prefab, sr.startposition, Quaternion.identity, transform);
        rope.GetComponent<RopeBridge>().GenerateRope(sr.startposition, sr.endposition);
    }

    public void loadbox(box bx)
    {
        GameObject box;

        if (bx.name.Contains("Box"))
            box = Instantiate(box_prefab, transform);
        else if (bx.name.Contains("Food"))
            box = Instantiate(food_prefab, transform);
        else if(bx.name.Contains("Water"))
            box = Instantiate(water_prefab, transform);
        else
            box = Instantiate(monster_prefab, transform);

        box.transform.SetPositionAndRotation(bx.position, bx.rotation);
    }

    public void loadsoftbodies(Softbody sb)
    {
        GameObject sb_ins;
        if (sb.name.Contains("CircleStiff"))
        {
            sb_ins = Instantiate(SBC_prefab, transform);
        }
        else if (sb.name.Contains("SquareSmall"))
            sb_ins = Instantiate(SBS_prefab, transform);
        else if (sb.name.Contains("Triangle"))
            sb_ins = Instantiate(SBT_prefab, transform);
        else if (sb.name.Contains("Pentagon"))
            sb_ins = Instantiate(SBPen_prefab, transform);
        else if (sb.name.Contains("Plus"))
            sb_ins = Instantiate(SBPlus_prefab, transform);
        else if (sb.name.Contains("car"))
            sb_ins = Instantiate(SBCar_prefab, transform);
        //else if (sb.name.Contains("Water"))
        //{
        //    LoadWater(sb);
        //    return;
        //}
        //sb_ins = Instantiate(water_prefab, transform);
        else if (sb.name.Contains("Rectangle"))
            sb_ins = Instantiate(SBR_prefab, transform);
        else return;
        sb_ins.transform.SetPositionAndRotation(sb.position, sb.rotation);

        for (int i = 0; i < sb_ins.transform.childCount; i++)
        {
            Transform child = sb_ins.transform.GetChild(i);
            child.SetPositionAndRotation(sb.boneposition[i], sb.bonerotation[i]);
        }

        sb_ins.GetComponent<SpriteRenderer>().color = sb.color;
    }

    //public void LoadWater(Softbody sb)
    //{
    //    GameObject waterParticle = Instantiate(water_prefab, transform);
    //    waterParticle.transform.SetPositionAndRotation(sb.position, sb.rotation);
    //}

    public void loadWall(GameObject wall, LineRendererSave lrs)
    {
        LineRenderer lr = wall.GetComponent<LineRenderer>();
        EdgeCollider2D edgcol = wall.GetComponent<EdgeCollider2D>();

        for (int i = 0; i < lrs.LineRendpoints.Count; i++)
        {
            lr.positionCount++;
            lr.SetPosition(i, lrs.LineRendpoints[i]);
        }
        edgcol.points = lrs.EdgeCollpoints.ToArray();

        foreach (Vector2 offsets in lrs.circleColsOffset)
        {
            CircleCollider2D circlecol = wall.AddComponent<CircleCollider2D>();
            circlecol.offset = offsets;
            circlecol.radius = .1f;
        }
        lr.colorGradient = lrs.lineColor;
        lr.material.color = lrs.lineColor.colorKeys[0].color;

        wall.transform.SetPositionAndRotation(lrs.position, lrs.rotation);
        wall.transform.localScale = lrs.scale;

        wall.layer = layerint;
    }
}

[Serializable]
public class DrawnLineRenderer
{
    public List<LineRendererSave> Walls;
    public List<LineRendererSave> GravityWalls;
    public List<LineRendererSave> FloatWalls;
    public List<LineRendererSave> VehicleWalls;
    public List<Softbody> Softbodies;
    public List<box> box_food_Monster;
    public List<saveRope> Savedropes;
    public DrawnLineRenderer()
    {
        Walls = new List<LineRendererSave>();
        GravityWalls = new List<LineRendererSave>();
        FloatWalls = new List<LineRendererSave>();
        VehicleWalls = new List<LineRendererSave>();
        Softbodies = new List<Softbody>();
        box_food_Monster = new List<box>();
        Savedropes = new List<saveRope>();
    }
}

[Serializable]
public class LineRendererSave
{
    public Vector3 position, scale;
    public Quaternion rotation;
    public List<Vector2> LineRendpoints;
    public List<Vector2> EdgeCollpoints;
    public List<Vector2> circleColsOffset;
    public Gradient lineColor = default;

    public LineRendererSave()
    {
        LineRendpoints = new List<Vector2>();
        EdgeCollpoints = new List<Vector2>();
        circleColsOffset = new List<Vector2>();
    }
}

[Serializable]
public class Softbody
{
    public string name;
    public Vector3 position;
    public Quaternion rotation;
    public List<Vector3> boneposition;
    public List<Quaternion> bonerotation;
    public Color color;
    public Softbody()
    {
        boneposition = new List<Vector3>();
        bonerotation = new List<Quaternion>();
    }
}

[Serializable]
public class box
{
    public string name;
    public Vector3 position;
    public Quaternion rotation;
}

[Serializable]
public class saveRope
{
    public Vector2 startposition, endposition;
}