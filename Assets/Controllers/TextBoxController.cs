using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TextBoxController : MonoBehaviour
{
    public List<DialogData> dialogData;
    public event Action<bool> OnShowTextBox;
    public event Action<Author> OnSwitchSpeaker;

    public Queue<Line> TextQueue;
    public TMP_Text TextPrinter;

    Author lastAuthor = Author.Witch;
    private bool textBoxVisible;

    void Awake()
    {
        TextQueue = new Queue<Line>();
    }

    public void SayIntroDialog(int level)
    {
        StartText(dialogData[level].introLines);
    }

    public void SayFirstIngredientDialog(int level)
    {
        Debug.Log("SayFirstIngredientDialog");
        StartText(dialogData[level].firstIngredientLines);
    }

    public void StartText(List<Line> lines)
    {
        TextQueue.Clear();
        foreach (Line line in lines)
        {
            TextQueue.Enqueue(line);
        }

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

                if (textBoxVisible)
                {
                    OnShowTextBox?.Invoke(false);
                    textBoxVisible = false;
                }

                continue;
            }

            Line line = TextQueue.Dequeue();

            if (lastAuthor != line.Author)
            {
                lastAuthor = line.Author;

                if(textBoxVisible)
                {
                    OnShowTextBox?.Invoke(false);
                    textBoxVisible = false;
                    yield return new WaitForSecondsRealtime(1f);
                }

                OnSwitchSpeaker?.Invoke(line.Author);
            }

            if(!textBoxVisible)
            {
                OnShowTextBox?.Invoke(true);
                textBoxVisible = true;
                yield return new WaitForSecondsRealtime(1f);
            }

            TextPrinter.text = line.Text;

            for (int i = 1; i <= TextPrinter.text.Length; i++)
            {
                TextPrinter.maxVisibleCharacters = i;
                yield return new WaitForSecondsRealtime(0.02f);
            }

            yield return new WaitForSecondsRealtime(2.5f);
        }
    }
}
