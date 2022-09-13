using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatcha : MonoBehaviour
{
    [SerializeField]
    List<GameObject> gatchaList;
    List<bool> getItems;

    private void Start()
    {
        for(int i=0; i<gatchaList.Count; i++)
        {
            gatchaList[i].SetActive(false);
            getItems[i] = false;
        }
    }

    public void Shuffle()
    {
        UserData.Coin -= 100;
        int rand = Random.Range(0, gatchaList.Count);
        while(getItems[rand])
        {
            rand = Random.Range(0, gatchaList.Count);
        }
        gatchaList[rand].SetActive(true);
        getItems[rand] = true;
        
        UserData.Save();
    }
}
