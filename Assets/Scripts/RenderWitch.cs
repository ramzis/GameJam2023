using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderWitch : MonoBehaviour
{

    [SerializeField] Texture happy;
    [SerializeField] Texture mid;
    [SerializeField] Texture angy;

    [SerializeField] GameObject Quad;
    Renderer QuadMat;

    public enum State
    {
        Happy,
        Mid,
        Angry
    }
    // Start is called before the first frame update
    void Start()
    {
        QuadMat = Quad.GetComponent<Renderer>();
    }


    // Update is called once per frame
    public void SetMoodImg(int lives)
    {
        switch(lives)
        {
            case 3:
                QuadMat.material.mainTexture = happy;
            break;

            case 2:
                QuadMat.material.mainTexture = mid;
                break;

            case 1:
                QuadMat.material.mainTexture = angy;
                break;
        }
    }
}
