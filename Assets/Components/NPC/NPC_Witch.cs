using System;
using System.Collections.Generic;
using UnityEngine;

public partial class NPC_Witch : NPC
{
    [Serializable]
    public enum State
    {
        HandingOutRecipe,
        WaitingForPotion,
        ReactingToPotion,
    }

    [Serializable]
    public enum Mood
    {
        Happy,
        Mid,
        Angry
    }

    [Serializable]
    public struct Data
    {
        public int recipe;
        public bool lastRecipeValid;
        public bool poisonReceived;
        public Mood mood;
        public State state;
    }

    #region Static Data

    [SerializeField]
    private List<RecipeData> recipes;
    [SerializeField]
    private RecipeData poisonRecipe;
    [SerializeField]
    private List<Texture> textures;

    #endregion

    #region Dynamic Data

    [SerializeField]
    private Data data;

    #endregion

    #region Lifecycle Methods

    private new void Awake()
    {
        base.Awake();
        InitStaticData();
        InitDynamicData();
    }

    private new void Start()
    {
        base.Start();
        RegisterEvents();
    }

    private void InitStaticData()
    {
        if (textures == null || textures.Count < 1)
            textures = new List<Texture>() { GetDefaultTexture() };
    }

    private void InitDynamicData()
    {
        data = new Data
        {
            recipe = 0,
            poisonReceived = false,
            lastRecipeValid = true,
            state = State.HandingOutRecipe,
            mood = Mood.Happy,
        };
    }

    private void RegisterEvents()
    {
        eventListeners.RegisterEvent(new ClickedEvent());
        eventListeners.RegisterEvent(new TextEvent());
    }

    #endregion

    #region Interface Implementations

    public override void Interact()
    {
        eventListeners.RaiseEvent(new ClickedEvent(this));
        switch(data.state)
        {
            case State.HandingOutRecipe:
            {
                eventListeners.RaiseEvent(
                    new TextEvent($"npc.witch.{(int)data.mood}.intro.{data.recipe}")
                );
                data.state = State.WaitingForPotion;
                break;
            }
        }
    }

    public override void ReceiveMessage(string message)
    {
        Debug.Log($"[NPC_Witch] Received: {message}");
        switch(message)
        {
            case "Get ready for Objective 1":
            {
                SetMood(Mood.Happy);
                break;
            }
        }   
    }

    public override void ReceiveItems(List<ItemData> items)
    {
        if (data.state != State.WaitingForPotion)
        {
            eventListeners.RaiseEvent(
                new TextEvent($"npc.witch.{(int)data.mood}.not_ready_to_receive")
            );
            return;
        }
        // TODO: reenable check when real items sent
        //if (items.Count < recipes[data.recipe].ingredients.Count)
        //{
        //    eventListeners.RaiseEvent(
        //        new TextEvent($"npc.witch.{(int)data.mood}.not_enough_ingredients")
        //    );
        //    return;
        //}

        EvaluatePotion(new HashSet<ItemData>(items));
        ReactToPotion();
    }

    #endregion

    #region Private Behaviours

    private void SetMood(Mood mood)
    {
        data.mood = mood;
        SetTexture(textures[(int)mood % textures.Count]);
    }

    private void SetNextMood()
    {
        SetMood((Mood)(((int)data.mood + 1) % textures.Count));
    }

    private void EvaluatePotion(HashSet<ItemData> providedIngredients)
    {
        var expectedIngredients = recipes[data.recipe].ingredients;

        if (data.recipe == 4)
        {
            data.poisonReceived = poisonRecipe.ingredients.TrueForAll((x) =>
                providedIngredients.Contains(x));
        }

        data.lastRecipeValid = expectedIngredients.TrueForAll((x) =>
            providedIngredients.Contains(x));

        // TODO: remove
        data.lastRecipeValid = true;
    }

    private void ReactToPotion()
    {
        data.state = State.ReactingToPotion;
        if(data.poisonReceived)
        {
            eventListeners.RaiseEvent(
                new TextEvent($"npc.witch.{(int)data.mood}.dead")
            );
            return;
        }
        else if(data.lastRecipeValid)
        {
            eventListeners.RaiseEvent(
                new TextEvent($"npc.witch.{(int)data.mood}.valid_recipe.{data.recipe}")
            );
        }
        else if(!data.lastRecipeValid)
        {
            SetNextMood();
            eventListeners.RaiseEvent(
               new TextEvent($"npc.witch.{(int)data.mood}.invalid_recipe.{data.recipe}")
            );
        }
        if(data.recipe < 4)
        {
            data.recipe++;
            data.state = State.HandingOutRecipe;
        }
    }

    #endregion
}
