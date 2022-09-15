using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public List<string> itemName;
    public List<bool> getIt;
}

public class Gatcha : MonoBehaviour
{
    [SerializeField]
    int gatchaPrice;

    [SerializeField]
    List<GameObject> gatchaList;
    [SerializeField]
    Item getItems = null;

    private void Start()
    {
        if(PlayerPrefs.HasKey("GetItems"))
            getItems = JsonUtility.FromJson<Item>(PlayerPrefs.GetString("GetItems", ""));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Shuffle();
        }
    }

    public void Shuffle()
    {
        if(UserData.Coin<gatchaPrice)
        {
            return;
        }
        for(int i=0; i< gatchaList.Count; i++)
        {
            if (getItems.getIt[i] == false)
                break;
            if (getItems.getIt[i] == true && i == gatchaList.Count-1)
            {
                Debug.Log("이미 전부 뽑았음미다!");
                return;
            }

        }
        UserData.Coin -= gatchaPrice;
        int rand = Random.Range(0, gatchaList.Count);
        while(getItems.getIt[rand])
        {
            rand = Random.Range(0, gatchaList.Count);
        }
        gatchaList[rand].SetActive(true);
        getItems.getIt[rand] = true;
        string getItem = JsonUtility.ToJson(getItems);
        PlayerPrefs.SetString("GetItems", getItem);
        Debug.Log(getItem);
        UserData.Save();
    }
}
