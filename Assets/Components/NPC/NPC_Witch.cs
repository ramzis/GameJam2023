using System.Collections.Generic;
using UnityEngine;

public class NPC_Witch : NPC
{
    public enum Mood
    {
        Happy,
        Mid,
        Angry
    }

    [SerializeField]
    private List<Texture> textures;

    private Mood currentMood;

    private void Start()
    {
        Init();
    }

    public override void Interact()
    {
        Debug.Log("[NPC_Witch]: Interacting");
        SetNextMood();
    }

    private void Init()
    {
        if (textures == null || textures.Count < 1)
            textures = new List<Texture>() { GetDefaultTexture() };

        SetMood(Mood.Happy);
    }

    private void SetMood(Mood mood)
    {
        currentMood = mood;
        SetTexture(textures[(int)mood % textures.Count]);
    }

    private void SetNextMood()
    {
        SetMood((Mood)(((int)currentMood + 1) % textures.Count));
    }
}
