using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMeshPro_Animation : MonoBehaviour
{

    public AnimationType animation;

    private TMP_Text textMesh;

    private Mesh mesh;
    private Vector3[] vertices;

    #region WordWobble
    private List<int> wordIndexes;
    private List<int> wordLengths;
    #endregion

    public enum AnimationType
    {
        WOBBLE,
        CHARACTER_WOBBLE,
        WORD_WOBBLE
    }

    [Header("Wobble")]
    public float sinMultiplier = 3.3f;
    public float cosMultiplier = 2.8f;
    public float timeMultiplier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TMP_Text>();

        #region Words Information
        wordIndexes = new List<int> { 0 };
        wordLengths = new List<int>();
        
        string s = textMesh.text;
        for(int index = s.IndexOf(' '); index > -1; index = s.IndexOf(' ', index + 1))
        {
            wordLengths.Add(index - wordIndexes[wordIndexes.Count - 1]);
            wordIndexes.Add(index + 1);
        }
        wordLengths.Add(s.Length - wordIndexes[wordIndexes.Count - 1]);
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;

        switch (animation)
        {
            case AnimationType.WOBBLE:
                Wobble();
                break;

            case AnimationType.CHARACTER_WOBBLE:
                CharacterWobble();
                break;

            case AnimationType.WORD_WOBBLE:
                WordWobble();
                break;

            default:
                break;
        }

        mesh.vertices = vertices;
        textMesh.canvasRenderer.SetMesh(mesh);
    }

    void Wobble()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 offset = Wobble(Time.time + i);

            vertices[i] = vertices[i] + offset;
        }
    }

    void CharacterWobble()
    {
        for (int i = 0; i < textMesh.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo c = textMesh.textInfo.characterInfo[i];
            int index = c.vertexIndex;

            Vector3 offset = Wobble(Time.time + i);
            vertices[index + 1] += offset;
            vertices[index + 2] += offset;
            vertices[index + 3] += offset;
        }
    }

    void WordWobble()
    {
        for (int w = 0; w < wordIndexes.Count; w++)
        {
            int wordIndex = wordIndexes[w];
            Vector3 offset = Wobble(Time.time + w);

            for (int i = 0; i < wordLengths[w]; i++)
            {
                TMP_CharacterInfo c = textMesh.textInfo.characterInfo[wordIndex + i];
                int index = c.vertexIndex;
                vertices[index + 1] += offset;
                vertices[index + 2] += offset;
                vertices[index + 3] += offset;
            }
        }
    }

    Vector2 Wobble(float time)
    {
        time *= timeMultiplier;
        return new Vector2(Mathf.Sin(time* sinMultiplier), Mathf.Cos(time* cosMultiplier));
    }
}
