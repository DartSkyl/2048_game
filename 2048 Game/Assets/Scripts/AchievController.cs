using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievController : MonoBehaviour
{
    public string[] arrayTitles;
    public Sprite[] arraySprites;
    public string[] arrayUrl;
    public GameObject button;
    public GameObject content;

    private List<GameObject> list = new List<GameObject>();
    private VerticalLayoutGroup _group;
    
    void Start()
    {
        RectTransform rectT = content.GetComponent<RectTransform>();
        rectT.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        _group = GetComponent<VerticalLayoutGroup>();
        setAchievs();
    }

    private void RemovedList()
    {
        foreach (var elem in list)
        {
            Destroy(elem);
        }
        list.Clear();
    }

    void setAchievs()
    {
        RectTransform rectT = content.GetComponent<RectTransform>();
        rectT.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        RemovedList();
        if (arrayTitles.Length > 0)
        {
            var pr1 = Instantiate(button, transform);
            var h = pr1.GetComponent<RectTransform>().rect.height;
            var tr = GetComponent<RectTransform>();
            tr.sizeDelta = new Vector2(tr.rect.width, h * arrayTitles.Length);
            Destroy(pr1);
            for (var i = 0; i < PlayerPrefs.GetInt("maxCell"); i++)
            {
                var pr = Instantiate(button, transform);
                pr.GetComponentInChildren<Text>().text = arrayTitles[i];
                pr.GetComponentsInChildren<Image>()[1].sprite = arraySprites[i];
                var i1 = i;
                pr.GetComponent<Button>().onClick.AddListener(() => GetAchievment(i1));
                list.Add(pr);
            }
        }
    }


    void GetAchievment(int id)
    {
        Application.OpenURL(arrayUrl[id]);
    }
}
