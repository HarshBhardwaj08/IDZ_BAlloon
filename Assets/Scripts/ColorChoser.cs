using UnityEngine;
using UnityEngine.UI;

public class ColorChoser : MonoBehaviour
{
    public GameObject ColorChoserPanel;
    public Slider slider_R, slider_G, slider_B;
    public Image preview;
    public bool visible = false;
    public LinesDrawer lineDrawer;

    public void onbuttonclicked()
    {
        visible = !visible;
        ColorChoserPanel.SetActive(visible);
        lineDrawer.chosingColor = visible;
    }

    public void oncolorchanged()
    {
        Color32 color = new Color32((byte)slider_R.value, (byte)slider_G.value, (byte)slider_B.value,255);
        preview.color = color;
        GradientColorKey[] colorkeys = lineDrawer.lineColor.colorKeys;
        for (int i = 0; i < colorkeys.Length; i++)
        {
            colorkeys[i].color = color;
        }
        lineDrawer.lineColor.colorKeys = colorkeys;
    }
}
