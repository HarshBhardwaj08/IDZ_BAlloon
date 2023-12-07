using UnityEngine;
using System.IO;

namespace DoodleGlowColoring
{
    public class SaveScreenshot : MonoBehaviour
    {
        private static SaveScreenshot Instance;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }

        }

        public static SaveScreenshot GetInstance()
        {
            if (!Instance)
            {
                GameObject temp = new GameObject();
                temp.name = "SaveScreenShot";
                temp.AddComponent<SaveScreenshot>();
            }
            return Instance;
        }

        public Sprite Load(string folderName, string fileName, int width, int height)
        {
            var dir = $"{Application.persistentDataPath}/{folderName}";
            if (Directory.Exists(dir))
            {
                var filePath = $"{dir}/{fileName}.png";
                if (File.Exists(filePath))
                {
                    var bytes = File.ReadAllBytes(filePath);
                    var bufferTex2d = new Texture2D(width, height, TextureFormat.ARGB32, false);
                    bufferTex2d.LoadImage(bytes);
                    var rect = new Rect(0, y: 0, width, height);
                    var sprite = Sprite.Create(bufferTex2d, rect, Vector2.zero);
                    sprite.name = fileName;
                    return sprite;
                }

#if UNITY_EDITOR
                Debug.Log(dir);
#endif
            }

            return null;
        }

        public void Save(Camera screenShotCamera,string folderName, string fileName, Color backgroundColor, string[] layerNames, int width, int height, int finalWidth, int finalHeight, float offsetX = 0, float offsetY = 0f)
        {
            string dir = $"{Application.persistentDataPath}/{folderName}";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string filePath = $"{dir}/{fileName}.png";

            RenderTexture activeRT = RenderTexture.active;
            RenderTexture bufferRT = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
            bufferRT.name = "DoodleScreenShot";

            
            Camera camera = screenShotCamera;
            camera.CopyFrom(Camera.main);
            camera.orthographicSize = Camera.main.orthographicSize * 0.5f;
            Transform cameraTransform = camera.GetComponent<Transform>();
            cameraTransform.position = new Vector3(cameraTransform.position.x + offsetX, cameraTransform.position.y + offsetY, -10f);
            camera.enabled = false;
            camera.backgroundColor = backgroundColor;
            int cullingMaskValue = 1;
            cullingMaskValue = 1 << LayerMask.NameToLayer(layerNames[1]) | 1 << LayerMask.NameToLayer(layerNames[0]);
            camera.cullingMask = cullingMaskValue;
            camera.targetTexture = bufferRT;
            camera.Render();

            RenderTexture.active = bufferRT;
            Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
            texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);

            TextureScaler.scale(texture2D, finalWidth, finalWidth);
            File.WriteAllBytes(filePath, texture2D.EncodeToPNG());
            RenderTexture.active = activeRT;

            Object.Destroy(activeRT);
            Object.Destroy(bufferRT);
            Object.Destroy(texture2D);

#if UNITY_EDITOR
            Debug.Log(dir);
#endif
        }

        public void Delete(string folderName, string fileName)
        {
            var dir = $"{Application.persistentDataPath}/{folderName}";
            if (Directory.Exists(dir))
            {
                var filePath = $"{dir}/{fileName}.png";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }
    }
}
