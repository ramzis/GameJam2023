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

    private new void Awake()
    {
        base.Awake();
        if (textures == null || textures.Count < 1)
            textures = new List<Texture>() { GetDefaultTexture() };
    }

    private new void Start()
    {
        base.Start();
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

    public override void Interact()
    {
        SetNextMood();
    }
}
