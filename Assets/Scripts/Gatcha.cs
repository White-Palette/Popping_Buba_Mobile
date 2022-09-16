using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public List<string> itemName;
    public List<bool> getIt;
}

public class Gatcha : MonoSingleton<Gatcha>
{
    public int gatchaPrice { get; private set; } = 500;

    [SerializeField]
    List<GameObject> gatchaList;
    [SerializeField]
    Item getItems = null;

    private void Start()
    {
        if(PlayerPrefs.HasKey("GetItems"))
            getItems = JsonUtility.FromJson<Item>(PlayerPrefs.GetString("GetItems", ""));
        for (int i = 0; i < gatchaList.Count; i++)
        {
            gatchaList[i].SetActive(getItems.getIt[i]);
        }
    }

    public void Shuffle()
    {
        SoundManager.Instance.PlaySound(Effect.Click);
        if (UserData.Coin<gatchaPrice)
        {
            return;
        }
        for(int i=0; i< gatchaList.Count; i++)
        {
            if (getItems.getIt[i] == false)
                break;
            if (getItems.getIt[i] == true && i == gatchaList.Count-1)
            {
                Debug.Log("이미 전부 뽑았습니다!");
                return;
            }
        }
        UserData.Coin -= gatchaPrice;
        TitleManager.Instance.resetCoinAmount();
        int rand = Random.Range(0, gatchaList.Count);
        while(getItems.getIt[rand])
        {
            rand = Random.Range(0, gatchaList.Count);
        }
        gatchaList[rand].SetActive(true);
        getItems.getIt[rand] = true;
        string getItem = JsonUtility.ToJson(getItems);
        PlayerPrefs.SetString("GetItems", getItem);
        Debug.Log(getItems.itemName[rand]);
        UserData.Save();
    }
}
