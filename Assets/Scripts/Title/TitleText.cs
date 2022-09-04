using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleText : MonoBehaviour
{
    TMP_Text textMesh;

    Mesh mesh;

    Vector3[] vertices;

    //List<int> wordIndexes;
    //List<int> wordLengths;

    private void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        SoundManager.Instance.PlaySound(Music.Wonderful_Story_Alt_Version);
        //wordIndexes = new List<int> { 0 };
        //wordLengths = new List<int> {};

        //string s = textMesh.text;
        //for (int index = s.IndexOf(' '); index > -1; index = s.IndexOf(' ', index + 1))
        //{
        //    wordLengths.Add(index - wordIndexes[wordIndexes.Count - 1]);
        //    wordIndexes.Add(index + 1);
        //}
        //wordLengths.Add(s.Length - wordIndexes[wordIndexes.Count - 1]);
    }

    private void Update()
    {
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;

        //for(int i = 0; i < wordIndexes.Count; i++)
        //{
        //    int wordIndex = wordIndexes[i];
        //    Vector3 offset = Wobble(Time.time + i);

        //    for (int j = 0; j < wordLengths[i]; j++)
        //    {
        //        TMP_CharacterInfo c = textMesh.textInfo.characterInfo[wordIndex + j];

        //        int index = c.vertexIndex;

        //        vertices[index] += offset;
        //        vertices[index + 1] += offset;
        //        vertices[index + 2] += offset;
        //        vertices[index + 3] += offset;
        //    }
        //}

        for (int i = 0; i < textMesh.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo c = textMesh.textInfo.characterInfo[i];

            int index = c.vertexIndex;

            Vector3 offset = Wobble(Time.time + i) * 7f;
            vertices[index] += offset;
            vertices[index + 1] += offset;
            vertices[index + 2] += offset;
            vertices[index + 3] += offset;
        }

        mesh.vertices = vertices;
        textMesh.canvasRenderer.SetMesh(mesh);

    }

    Vector2 Wobble(float time)
    {
        return new Vector2(/*Mathf.Sin(time * 3.3f)*/ 0, Mathf.Cos(time * 4f));
    }
}
