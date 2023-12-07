#region Older Code
/*using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class LevelScreenManager : MonoBehaviour
{
    public Transform buttons_Container;
    public string pathtofile, pathToImage;
    public GameObject extra_button, buttonprefab;
    int pagesCount;

    public void Start()
    {
        bool firsttime = true;
        int num = PlayerPrefs.GetInt("firsttime");
        if (num == 0)
        {
            firsttime = true;
            PlayerPrefs.SetInt("firsttime", 1);
        }
        else
        {
            firsttime = false;
        }
        Debug.Log("firsttimetext : " + firsttime);
        // string firsttimetext = Application.dataPath + "/Resources/firsttimefile.txt";
        //string firsttimetext = Application.persistentDataPath + "/firsttimefile.txt";
        //Debug.Log("firsttimetext : " + firsttimetext);
        //if (File.Exists(firsttimetext))
        //{
        //    Debug.Log("firsttimetext done");
        //    firsttime = false;
        //}
        //else
        //{
        //    File.WriteAllText(firsttimetext, "this file is generated for the first time only");
        //}

#if UNITY_EDITOR
        firsttime = true;
#endif
        string tempfilename;
        string tempfilename2;
        Transform temp;
        Sprite sprite;
        TextAsset texass;
        Texture2D tex2d;
        string json;

        //string path = Application.dataPath + "/Resources/levels.txt";
        string path = Application.persistentDataPath + "/levels.txt";
        Debug.Log("path : " + path);
        if (File.Exists(path))
        {
            pagesCount = int.Parse(File.ReadAllText(path));
            Debug.Log("pagesCount : " + pagesCount);
        }
        else
        {
            pagesCount = 10;
        }

        for (int i = 0; i < pagesCount; i++)
        {
            //pathtofile = Application.dataPath + "/Resources/savefile_" + i;
            pathtofile = Application.persistentDataPath + "/savefile_" + i;
            // pathtoImage = Application.dataPath + "/Resources/LevelIcon_" + i;
            pathToImage = Application.persistentDataPath + "/LevelIcon_" + i;
            if (i < buttons_Container.childCount - 1)
                temp = buttons_Container.GetChild(i);
            else
            {
                temp = Instantiate(buttonprefab, buttons_Container).transform;
                extra_button.transform.SetAsLastSibling();
                Button tempbtn = temp.GetComponent<Button>();
                tempbtn.onClick.RemoveAllListeners();
                int n = i;
                tempbtn.onClick.AddListener(() => { onbuttonclicked(n); });
            }

            if (firsttime)
            {
                Debug.Log("firsttime texass");
                tempfilename = "savefile_" + i;
                tempfilename2 = "LevelIcon_" + i;
                texass = Resources.Load<TextAsset>(tempfilename);
                tex2d = Resources.Load<Texture2D>(tempfilename2);
                Debug.Log("firsttime texass : " + texass);
                Debug.Log("firsttime tex2d : " + tex2d);
                if (!texass)
                    continue;
                json = texass.ToString();
                File.WriteAllText(pathtofile + ".json", json);
                if (!tex2d)
                    continue;
                File.WriteAllBytes(pathToImage + ".png", tex2d.EncodeToPNG());
            }

            Image img = temp.GetComponent<Image>();
            sprite = loadimage();
            if (sprite)
                img.sprite = sprite;
        }
    }

    Sprite loadimage()
    {
        string temppath = pathToImage + ".png";
        if (!File.Exists(temppath))
        {
            return null;
        }
        //print("loading image " + temppath);
        byte[] image = File.ReadAllBytes(temppath);
        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(image);
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        return sprite;
    }

    public void onbuttonclicked(int n)
    {
        print(n);
        DataManager.instance.LeveltoLoad = n;
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void onextrabuttonClicked()
    {
        GameObject temp = Instantiate(buttonprefab, buttons_Container);
        extra_button.transform.SetAsLastSibling();
        Button tempbtn = temp.GetComponent<Button>();
        tempbtn.onClick.RemoveAllListeners();
        int n = pagesCount;
        tempbtn.onClick.AddListener(() => { onbuttonclicked(n); });
        pagesCount++;
    }

    public void onbackbuttonclicked()
    {
        SceneManager.LoadSceneAsync("CategorySelectionScreen");
    }

    private void OnDestroy()
    {
        // string path = Application.dataPath + "/Resources/levels.txt";
        string path = Application.persistentDataPath + "/levels.txt";
        File.WriteAllText(path, pagesCount.ToString());
    }
}
*/
#endregion

#region Old Code
/*
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelScreenManager : MonoBehaviour
{
    public Transform buttonsContainer;
    public string saveFilePath, levelIconPath;
    public GameObject extraButton, buttonPrefab;
    private int pageCount;
    [System.Serializable]
    struct CategorySaveHolder
    {
        public CategoryDataHolder.AvailableCategories Category;
        public int[] LevelsInCategory;
    }
    [SerializeField] CategorySaveHolder[] categorySaveData;

    private const string levelsFileName = "levels.json";
    private const int defaultPageCount = 10;

    public async void Start()
    {
        // Check if it's the first time playing
        bool isFirstTime = IsFirstTimePlayer();

#if UNITY_EDITOR
        // For testing purposes, set isFirstTime to true in the editor
        isFirstTime = true;
#endif

        // Load the page count from file
        pageCount = await LoadPageCount();
        Debug.Log("Page Count: " + pageCount);

        for (int i = 0; i < pageCount; i++)
        {
            var _tempLevels = GetLevelsInCategory();
            if (_tempLevels == null)
            {
                print("No levels in current category!");
                break;
            }

            bool isValidLevel = false;
            foreach (var v in _tempLevels)
            {
                if (v == i)
                {
                    isValidLevel = true;
                    print("Contains level: " + i);
                }
            }
            if (!isValidLevel)
                continue;

            // Get the file paths for save data and level icon
            saveFilePath = GetSaveFilePath(i);
            levelIconPath = GetLevelIconPath(i);

            // Get or create the button transform for the current index
            Transform buttonTransform = GetOrCreateButtonTransform(i);

            if (isFirstTime)
            {
                // Load and save level data if it's the first time playing
                await LoadAndSaveLevelData(i);
                await LoadAndSaveLevelIcon(i);
            }

            // Load the level icon and set it to the button image
            if (buttonTransform)
            {
                Image imageComponent = buttonTransform.GetComponent<Image>();
                Sprite levelIconSprite = await LoadLevelIcon();

                if (levelIconSprite != null)
                    imageComponent.sprite = levelIconSprite;
            }
        }
    }

    int[] GetLevelsInCategory()
    {
        foreach (var v in categorySaveData)
        {
            if (v.Category == CategoryDataHolder.Instance.ActiveCategory)
            {
                return v.LevelsInCategory;
            }
        }
        return null;
    }

    // Check if it's the first time playing
    private bool IsFirstTimePlayer()
    {
        bool isFirstTime = false;
        int firstTimeValue = PlayerPrefs.GetInt("firsttime");

        if (firstTimeValue == 0)
        {
            isFirstTime = true;
            PlayerPrefs.SetInt("firsttime", 1);
        }

        Debug.Log("Is First Time: " + isFirstTime);
        return isFirstTime;
    }

    // Load the page count from file
    private async Task<int> LoadPageCount()
    {
        string levelsFilePath = Path.Combine(Application.persistentDataPath, levelsFileName);

        if (File.Exists(levelsFilePath))
        {
            print("found file: " + levelsFilePath);
            string pageCountText = await File.ReadAllTextAsync(levelsFilePath);
            if (int.TryParse(pageCountText, out int result))
                return result;
        }

        //save it after reading for first time
        File.WriteAllText(levelsFilePath, defaultPageCount.ToString());
        return defaultPageCount;
    }

    // Get or create the button transform for the current index
    private Transform GetOrCreateButtonTransform(int index)
    {
        if (index < buttonsContainer.childCount - 1)
        {
            // If the button transform already exists, return it
            return buttonsContainer.GetChild(index);
        }
        else
        {
            // If the button transform doesn't exist, create a new one
            Transform buttonTransform = Instantiate(buttonPrefab, buttonsContainer).transform;
            extraButton.transform.SetAsLastSibling();
            Button buttonComponent = buttonTransform.GetComponent<Button>();
            buttonComponent.onClick.RemoveAllListeners();
            buttonComponent.onClick.AddListener(() => { OnButtonClicked(index); });
            return buttonTransform;
        }
    }

    // Get the file path for save data based on the index
    private string GetSaveFilePath(int index)
    {
        return Path.Combine(Application.persistentDataPath, $"savefile_{index}");
    }

    // Get the file path for level icon based on the index
    private string GetLevelIconPath(int index)
    {
        return Path.Combine(Application.persistentDataPath, $"LevelIcon_{index}");
    }

    // Load and save level data if it's the first time playing
    private async Task LoadAndSaveLevelData(int index)
    {
        string saveDataFileName = $"savefile_{index}";
        TextAsset saveDataAsset = Resources.Load<TextAsset>(saveDataFileName);

        if (saveDataAsset == null)
            return;

        string saveDataJson = saveDataAsset.ToString();
        await File.WriteAllTextAsync(saveFilePath + ".json", saveDataJson);
    }

    // Load and save level icon if it's the first time playing
    private async Task LoadAndSaveLevelIcon(int index)
    {
        string iconFileName = $"LevelIcon_{index}";
        Texture2D levelIconTexture = Resources.Load<Texture2D>(iconFileName);

        if (levelIconTexture == null)
            return;

        byte[] levelIconData = levelIconTexture.EncodeToPNG();
        await File.WriteAllBytesAsync(levelIconPath + ".png", levelIconData);
    }

    // Load the level icon from file
    private async Task<Sprite> LoadLevelIcon()
    {
        string levelIconFilePath = levelIconPath + ".png";

        if (!File.Exists(levelIconFilePath))
        {
            return null;
        }

        byte[] imageData = await File.ReadAllBytesAsync(levelIconFilePath);
        Texture2D levelIconTexture = new Texture2D(0, 0);
        levelIconTexture.LoadImage(imageData);
        Sprite levelIconSprite = Sprite.Create(levelIconTexture, new Rect(0, 0, levelIconTexture.width, levelIconTexture.height), Vector2.zero);
        return levelIconSprite;
    }

    // Handle button click event
    public void OnButtonClicked(int index)
    {
        Debug.Log("Level Button Clicked: " + index);
        DataManager.instance.LevelToLoad = index;
        SceneManager.LoadSceneAsync("GameScene");
    }

    // Handle extra button click event
    public void OnExtraButtonClicked()
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonsContainer);
        extraButton.transform.SetAsLastSibling();
        Button buttonComponent = newButton.GetComponent<Button>();
        buttonComponent.onClick.RemoveAllListeners();
        int newPageIndex = pageCount;
        buttonComponent.onClick.AddListener(() => { OnButtonClicked(newPageIndex); });
        pageCount++;
    }

    // Handle back button click event
    public void OnBackButtonClicked()
    {
        SceneManager.LoadSceneAsync("CategorySelectionScreen");
    }
}
*/
#endregion

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelScreenManager : MonoBehaviour
{
    public Transform buttonsContainer;
    public string saveFilePath, levelIconPath;
    public GameObject extraButton, buttonPrefab;
    private int pageCount;
    private bool isFirstTime;
    [System.Serializable]
    public struct CategorySaveHolder
    {
        public CategorySaveHolder(CategoryDataHolder.AvailableCategories Category, List<int> LevelsInCategory)
        {
            this.Category = Category;
            this.LevelsInCategory = LevelsInCategory;
        }
        public CategoryDataHolder.AvailableCategories Category;
        public List<int> LevelsInCategory;
    }
    [SerializeField] private CategorySaveHolder categorySaveHolder;

    [System.Serializable]
    public struct LevelsHolder
    {
        public int levelCount;
        public List<CategorySaveHolder> categories;
    }
    public LevelsHolder levelsHolder;

    private const string levelsFileName = "levels.json";
    private const int defaultPageCount = 10;
    private CategoryDataHolder.AvailableCategories[] defaultLevelCategories = { CategoryDataHolder.AvailableCategories.PhysicsDraw,
                                                                                CategoryDataHolder.AvailableCategories.Quiggle,
                                                                                CategoryDataHolder.AvailableCategories.jellyCar,
                                                                                CategoryDataHolder.AvailableCategories.SoftBody,
                                                                                CategoryDataHolder.AvailableCategories.MarbleGame,
                                                                                CategoryDataHolder.AvailableCategories.Coloring3D,
                                                                                CategoryDataHolder.AvailableCategories.FoodMonster,
                                                                                CategoryDataHolder.AvailableCategories.Ragdoll};
    private List<List<int>> defeultLevelsInCategories = new List<List<int>> { new List<int> { 0, 3, 4, 6 },
                                                                              new List<int>(),
                                                                              new List<int>(),
                                                                              new List<int> { 7, 8 },
                                                                              new List<int>(),
                                                                              new List<int>(),
                                                                              new List<int> { 1, 2, 5, 9 },
                                                                              new List<int>()
                                                                            };

    public async void Start()
    {
        // Check if it's the first time playing
        //bool isFirstTime = IsFirstTimePlayer();
        isFirstTime = false;

#if UNITY_EDITOR
        // For testing purposes, set isFirstTime to true in the editor
        // isFirstTime = true;
#endif

        // Load the page count from file
        pageCount = await LoadPageCount();
        Debug.Log("Page Count: " + pageCount);

        for (int i = 0; i < pageCount; i++)
        {
            var _tempLevels = GetLevelsInCategory();
            if (_tempLevels == null)
            {
                print("No levels in current category!");
                break;
            }

            bool isValidLevel = false;
            foreach (var v in _tempLevels)
            {
                if (v == i)
                {
                    isValidLevel = true;
                    print("Contains level: " + i);
                }
            }

            // Get the file paths for save data and level icon
            saveFilePath = GetSaveFilePath(i);
            levelIconPath = GetLevelIconPath(i);

            if (isFirstTime)
            {
                // Load and save level data if it's the first time playing
                await LoadAndSaveLevelData(i);
                await LoadAndSaveLevelIcon(i);
            }

            if (!isValidLevel)
                continue;

            // Get or create the button transform for the current index
            Transform buttonTransform = GetOrCreateButtonTransform(i);

            // Load the level icon and set it to the button image
            if (buttonTransform)
            {
                Image imageComponent = buttonTransform.GetComponent<Image>();
                Sprite levelIconSprite = await LoadLevelIcon();

                if (levelIconSprite != null)
                    imageComponent.sprite = levelIconSprite;
            }
        }
    }

    // Load the page count from file
    private async Task<int> LoadPageCount()
    {
        string levelsFilePath = Path.Combine(Application.persistentDataPath, levelsFileName);

        if (File.Exists(levelsFilePath))
        {
            print("found file: " + levelsFilePath);
            string levelsFile = await File.ReadAllTextAsync(levelsFilePath);
            levelsHolder = JsonUtility.FromJson<LevelsHolder>(levelsFile);
            return levelsHolder.levelCount;
        }

        isFirstTime = true;

        levelsHolder = new LevelsHolder();
        levelsHolder.levelCount = defaultPageCount;
        levelsHolder.categories = new List<CategorySaveHolder>();
        for(int i = 0; i < defaultLevelCategories.Length; i++)
        {
            levelsHolder.categories.Add(new CategorySaveHolder(defaultLevelCategories[i], defeultLevelsInCategories[i]));
        }

        string strToWrite = JsonUtility.ToJson(levelsHolder);
        File.WriteAllText(levelsFilePath, strToWrite);

        return defaultPageCount;
    }

    List<int> GetLevelsInCategory()
    {
        foreach (var v in levelsHolder.categories)
        {
            if (v.Category == CategoryDataHolder.Instance.ActiveCategory)
            {
                return v.LevelsInCategory;
            }
        }
        return null;
    }

    // Get or create the button transform for the current index
    private Transform GetOrCreateButtonTransform(int index)
    {
        if (index < buttonsContainer.childCount - 1)
        {
            // If the button transform already exists, return it
            return buttonsContainer.GetChild(index);
        }
        else
        {
            // If the button transform doesn't exist, create a new one
            Transform buttonTransform = Instantiate(buttonPrefab, buttonsContainer).transform;
            extraButton.transform.SetAsLastSibling();
            Button buttonComponent = buttonTransform.GetComponent<Button>();
            buttonComponent.onClick.RemoveAllListeners();
            buttonComponent.onClick.AddListener(() => { OnButtonClicked(index); });
            return buttonTransform;
        }
    }

    // Get the file path for save data based on the index
    private string GetSaveFilePath(int index)
    {
        return Path.Combine(Application.persistentDataPath, $"savefile_{index}");
    }

    // Get the file path for level icon based on the index
    private string GetLevelIconPath(int index)
    {
        return Path.Combine(Application.persistentDataPath, $"LevelIcon_{index}");
    }

    // Load and save level data if it's the first time playing
    private async Task LoadAndSaveLevelData(int index)
    {
        string saveDataFileName = $"savefile_{index}";
        TextAsset saveDataAsset = Resources.Load<TextAsset>(saveDataFileName);

        if (saveDataAsset == null)
            return;

        string saveDataJson = saveDataAsset.ToString();
        await File.WriteAllTextAsync(saveFilePath + ".json", saveDataJson);
    }

    // Load and save level icon if it's the first time playing
    private async Task LoadAndSaveLevelIcon(int index)
    {
        string iconFileName = $"LevelIcon_{index}";
        Texture2D levelIconTexture = Resources.Load<Texture2D>(iconFileName);

        if (levelIconTexture == null)
            return;

        byte[] levelIconData = levelIconTexture.EncodeToPNG();
        await File.WriteAllBytesAsync(levelIconPath + ".png", levelIconData);
    }

    // Load the level icon from file
    private async Task<Sprite> LoadLevelIcon()
    {
        string levelIconFilePath = levelIconPath + ".png";

        if (!File.Exists(levelIconFilePath))
        {
            return null;
        }

        byte[] imageData = await File.ReadAllBytesAsync(levelIconFilePath);
        Texture2D levelIconTexture = new Texture2D(0, 0);
        levelIconTexture.LoadImage(imageData);
        Sprite levelIconSprite = Sprite.Create(levelIconTexture, new Rect(0, 0, levelIconTexture.width, levelIconTexture.height), Vector2.zero);
        return levelIconSprite;
    }

    // Handle button click event
    public void OnButtonClicked(int index)
    {
        Debug.Log("Level Button Clicked: " + index);
        DataManager.instance.LevelToLoad = index;
        SceneManager.LoadSceneAsync("GameScene");
    }

    // Handle extra button click event
    public void OnExtraButtonClicked()
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonsContainer);
        extraButton.transform.SetAsLastSibling();
        Button buttonComponent = newButton.GetComponent<Button>();
        buttonComponent.onClick.RemoveAllListeners();
        int newPageIndex = pageCount;
        buttonComponent.onClick.AddListener(() => { OnButtonClicked(newPageIndex); });
        pageCount++;

        string levelsFilePath = Path.Combine(Application.persistentDataPath, levelsFileName);
        levelsHolder.levelCount++;
        string strToWrite = JsonUtility.ToJson(levelsHolder);
        File.WriteAllText(levelsFilePath, strToWrite);
    }

    // Handle back button click event
    public void OnBackButtonClicked()
    {
        SceneManager.LoadSceneAsync("CategorySelectionScreen");
    }
}