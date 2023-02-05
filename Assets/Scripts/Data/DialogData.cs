using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog Data", menuName = "Data/Dialog", order = 1)]
public class DialogData : ScriptableObject
{
    [Header("Metadata")]
    public string title = "Dialog";

    [SerializeField]
    List<Line> introLines;

    [SerializeField]
    List<Line> firstIngredientLines;

    [SerializeField]
    List<Line> correctIngredientLines;

    [SerializeField]
    List<Line> wrongIngredientLines;

    [SerializeField]
    List<Line> outroLines;
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