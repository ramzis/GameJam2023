using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog Data", menuName = "Data/Dialog", order = 1)]
public class DialogData : ScriptableObject
{
    [Header("Metadata")]
    public string title = "Dialog";

    public List<Line> introLines;

    public List<Line> firstIngredientLines;

    public List<Line> correctIngredientLines;

    public List<Line> wrongIngredientLines;

    public List<Line> outroLines;
}

[Serializable]
public struct Line
{
    [SerializeField]
    public Author Author;
    [SerializeField]
    public string Text;
}

[Serializable]
public enum Author
{
    Witch,
    Rabbit
}