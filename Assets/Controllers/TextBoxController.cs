using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TextBoxController : MonoBehaviour
{
    public event Action<bool> OnShowTextBox;

    public Queue<string> TextQueue;
    public TMP_Text TextPrinter;

    void Awake()
    {
        TextQueue = new Queue<string>();
        StartCoroutine(TypeText());
    }

    public void StartText(string[] newText)
    {
        OnShowTextBox?.Invoke(true);
        TextQueue.Clear();
        foreach (string text in newText)
        {
            TextQueue.Enqueue(text);
        }
        OnShowTextBox?.Invoke(true);

        StopAllCoroutines();
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        while (true)
        {
            if (TextQueue.Count < 1)
            {
                TextPrinter.text = "";
                yield return new WaitForSecondsRealtime(1f);
                OnShowTextBox?.Invoke(false);
                continue;
            }

            string str = TextQueue.Dequeue();
            TextPrinter.text = str;

            for (int i = 1; i <= TextPrinter.text.Length; i++)
            {
                TextPrinter.maxVisibleCharacters = i;
                yield return new WaitForSecondsRealtime(0.02f);
            }

            yield return new WaitForSecondsRealtime(2.5f);
        }
    }

    public void TestText()
    {
        StartText(new string[]{
            "Hello there, little piggy.",
            "My full time bartender is out on holidays for 5 days so I’m super thirsty.",
            "IF YOU KNOW WHAT I MEAN.",
            "Now you will be my servant!",
            "If you behave and make good cocktails - I will let you go… ",
            "If not - death awaits you, little pig!", }
        );
    }
}
