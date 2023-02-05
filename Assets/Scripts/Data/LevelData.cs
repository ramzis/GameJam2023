using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Data/Level", order = 1)]
public class LevelData : ScriptableObject
{
    [Header("Metadata")]
    public string title = "Level";

    public DialogData dialogs;
}