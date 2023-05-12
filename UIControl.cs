using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIControl : Singleton<UIControl>
{
    [SerializeField]
    private Text hexagonCountText;
    [SerializeField]
    private GameObject unlockG;
    public bool IsShowUnlock
    {
        get
        {
            return unlockG.activeSelf;
        }
    }
    [SerializeField]
    private List<Transform> unlockItemTsList;
    [SerializeField]
    private List<GameObject> unlockHexagonGsList;
    [SerializeField]
    private Button restartButton;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in unlockItemTsList)
        {
            int index = unlockItemTsList.IndexOf(item);
            Button button = item.Find("UnlockButton").GetComponent<Button>();
            GameObject unlockedLabelG = item.Find("UnlockedLabel").gameObject;
            button.onClick.AddListener(() =>
            {
                button.gameObject.SetActive(false);
                unlockedLabelG.SetActive(true);
                SceneControl.Instance.UnlockHexagon(unlockHexagonGsList[index]);
                SceneControl.Instance.enabled = true;
                unlockG.SetActive(false);
            });
        }

        restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
    }
    public void ShowHexagonCount(int hexagonCount)
    {
        hexagonCountText.text = $"Hexagon Count: {hexagonCount}";
    }
    public void ShowUnlock()
    {
        unlockG.SetActive(true);
    }
}
