using System.Collections.Generic;
using System.Collections;
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

    private bool busy;

    public bool Busy()
    {
        return busy;
    }

    void Awake()
    {
        TextQueue = new Queue<Line>();
    }

    public IEnumerator SayIntroDialog(int level)
    {
        yield return new WaitWhile(() => busy);
        StartText(dialogData[level].introLines);
    }

    public IEnumerator SayFirstIngredientDialog(int level)
    {
        yield return new WaitWhile(() => busy);
            StartText(dialogData[level].firstIngredientLines);
    }

    public IEnumerator SayCorrectIngredientDialog(int level)
    {
        yield return new WaitWhile(() => busy);
        StartText(dialogData[level].correctIngredientLines);
    }

    public IEnumerator SayWrongIngredientDialog(int level)
    {
        yield return new WaitWhile(() => busy);
        StartText(dialogData[level].wrongIngredientLines);
    }

    private void StartText(List<Line> lines)
    {
        busy = true;

        TextQueue.Clear();
        foreach (Line line in lines) TextQueue.Enqueue(line);

        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        while (true)
        {
            // If all text is read, exit
            if (TextQueue.Count < 1)
            {
                TextPrinter.text = "";

                yield return HideBox();

                break;
            }

            // Read by line
            Line line = TextQueue.Dequeue();

            // Switch images for authors
            if (lastAuthor != line.Author)
            {
                lastAuthor = line.Author;

                yield return HideBox();

                OnSwitchSpeaker?.Invoke(line.Author);
            }

            yield return ShowBox();

            TextPrinter.text = line.Text;

            for (int i = 1; i <= TextPrinter.text.Length; i++)
            {
                TextPrinter.maxVisibleCharacters = i;
                yield return new WaitForSecondsRealtime(0.02f);
            }

            yield return new WaitForSecondsRealtime(2.5f);
        }

        busy = false;
    }

    private IEnumerator HideBox()
    {
        if (textBoxVisible)
        {
            OnShowTextBox?.Invoke(false);
            textBoxVisible = false;
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    private IEnumerator ShowBox()
    {
        if (!textBoxVisible)
        {
            OnShowTextBox?.Invoke(true);
            textBoxVisible = true;
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}


