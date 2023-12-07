using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CategorySceneManager : MonoBehaviour
{
    [System.Serializable]
    struct ButtonDataHolder
    {
        public Button ButtonObj;
        public CategoryDataHolder.AvailableCategories ButtonCategory;
    }

    public string loadSceneName = "LevelSelection";

    [SerializeField] ButtonDataHolder[] ButtonsInScene;
    private void Start()
    {
        foreach (var v in ButtonsInScene)
        {
            v.ButtonObj.onClick.AddListener(() =>
            {
                LoadScene(loadSceneName);
                CategoryDataHolder.Instance.ActiveCategory = v.ButtonCategory;
            });
        }
    }
    public void LoadScene(string _name)
    {
        SceneManager.LoadScene(_name);
    }
}
