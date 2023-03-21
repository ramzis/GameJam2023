using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class TextBoxController : MonoBehaviour, IListen<NPCController>
{
    public List<DialogData> dialogData;
    public event Action<bool> OnShowTextBox;
    public event Action<Author> OnSwitchSpeaker;

    public Queue<List<Line>> TextQueue;
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
        TextQueue = new Queue<List<Line>>();
    }

    private List<Line> lines;
    private void LateUpdate()
    {
        if(!busy)
        {
            if (TextQueue.TryDequeue(out lines))
            {
                busy = true;
                StartCoroutine(SayLines(lines));
            }
            else if(textBoxVisible)
            {
                TextPrinter.text = "";
                StartCoroutine(HideBox());
            }
        }
    }

    public void SayIntroDialog(int level)
    {
        lock(TextQueue)
        {
            TextQueue.Enqueue(dialogData[level].introLines);
        }
    }

    public void SayFirstIngredientDialog(int level)
    {
        lock (TextQueue)
        {
            TextQueue.Enqueue(dialogData[level].firstIngredientLines);
        }
    }

    public void SayCorrectIngredientDialog(int level)
    {
        lock (TextQueue)
        {
            TextQueue.Enqueue(dialogData[level].correctIngredientLines);
        }
    }

    public void SayWrongIngredientDialog(int level)
    {
        lock (TextQueue)
        {
            TextQueue.Enqueue(dialogData[level].wrongIngredientLines);
        }
    }

    private IEnumerator SayLines(List<Line> lines)
    {
        foreach(var line in lines)
        {
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

    public (List<int>, Action<dynamic>) HandleEvent(NPCController component, IEvent<NPCController> @event)
    {
        return @event switch
        {
            NPCController.Event_Say e => (
                new List<int>() { 100 },
                (payload) =>
                {
                    string[] tokens = payload.Split(".");
                    var character = tokens[1];
                    var message = tokens[3];
                    switch(message)
                    {
                        case "intro":
                        {
                            var id = tokens[4];
                            SayIntroDialog(int.Parse(id));
                            break;
                        }
                        case "valid_recipe":
                        {
                            var id = tokens[4];
                            SayCorrectIngredientDialog(int.Parse(id));
                            break;
                        }
                        case "invalid_recipe":
                        {
                            var id = tokens[4];
                            SayWrongIngredientDialog(int.Parse(id));
                            break;
                        }
                    }

                }
            ),
            _ => (null, null)
        };
    }
}
