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
    public void SetMoodImg(State state)
    {
        switch(state)
        {
            case State.Happy:
                QuadMat.material.mainTexture = happy;
            break;

            case State.Mid:
                QuadMat.material.mainTexture = mid;
                break;

            case State.Angry:
                QuadMat.material.mainTexture = angy;
                break;
        }
    }
}
