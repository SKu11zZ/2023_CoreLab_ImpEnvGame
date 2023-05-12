using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneControl : Singleton<SceneControl>
{
    [SerializeField]
    private List<GameObject> refHexagonGsList;
    [SerializeField]
    private List<GameObject> hexagonPsList;
    private class Hexagon
    {
        public GameObject Prefab;
        public bool Unlocked;
    }
    private List<Hexagon> hexagonsList = new List<Hexagon>();
    private Vector3[] offsets =
    {
        new Vector3(0.5f, 0f, 0.5f / Mathf.Sqrt(3f) * 3f),
        new Vector3(1f, 0f),
        new Vector3(0.5f, 0f, -0.5f / Mathf.Sqrt(3f) * 3f),
        new Vector3(-0.5f, 0f, -0.5f / Mathf.Sqrt(3f) * 3f),
        new Vector3(-1f, 0f),
        new Vector3(-0.5f, 0f, 0.5f / Mathf.Sqrt(3f) * 3f)
    };
    private List<GameObject> hexagonGsList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < hexagonPsList.Count; i++)
        {
            hexagonsList.Add(new Hexagon { Prefab = hexagonPsList[i], Unlocked = i < 5 });
        }

        CreateHexagon(Vector3.zero);
        foreach (var item in offsets)
        {
            CreateHexagon(item);
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit))
            {
                if (raycastHit.transform.name.Contains("RefHexagon"))
                {
                    CreateHexagon(raycastHit.transform.position);
                }
                if (IsActorAround(raycastHit.transform) && ActorControl.Instance.SetTargetPos(raycastHit.transform.position + new Vector3(0f, 0.2f)))
                {
                    HideRefHexagons();
                }
            }
        }
    }
    public void ShowRefHexagons(Vector3 hexagonPos)
    {
        for (int i = 0; i < offsets.Length; i++)
        {
            Vector3 pos = hexagonPos + offsets[i];
            bool isExist = false;
            foreach (var item in hexagonGsList)
            {
                if (Vector3.Distance(item.transform.position, pos) < 0.01f)
                {
                    isExist = true;
                }
            }
            if (!isExist)
            {
                refHexagonGsList[i].SetActive(true);
                refHexagonGsList[i].transform.position = pos;
            }
        }
    }
    public void HideRefHexagons()
    {
        foreach (var item in refHexagonGsList)
        {
            item.SetActive(false);
        }
    }
    public void UnlockHexagon(GameObject hexagon)
    {
        foreach (var item in hexagonsList)
        {
            if (item.Prefab == hexagon)
            {
                item.Unlocked = true;
                break;
            }
        }
    }
    private void CreateHexagon(Vector3 pos)
    {
        Hexagon[] hexagons = hexagonsList.Where(hexagon => hexagon.Unlocked).ToArray();
        GameObject prefab = hexagons[Random.Range(0, hexagons.Length)].Prefab;
        GameObject hexagonG = Instantiate<GameObject>(prefab, pos, Quaternion.identity, this.transform);
        hexagonG.SetActive(true);
        hexagonGsList.Add(hexagonG);
        UIControl.Instance.ShowHexagonCount(hexagonGsList.Count);
        if (hexagonGsList.Count <= 270 && hexagonGsList.Count % 15 == 0)
        {
            UIControl.Instance.ShowUnlock();
            this.enabled = false;
        }
    }
    private bool IsActorAround(Transform hexagon)
    {
        if (Physics.Raycast(new Ray(ActorControl.Instance.transform.position + Vector3.up, Vector3.down), out RaycastHit raycastHit))
        {
            foreach (var item in offsets)
            {
                if (Vector3.Distance(hexagon.position, raycastHit.transform.position + item) < 0.01f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
