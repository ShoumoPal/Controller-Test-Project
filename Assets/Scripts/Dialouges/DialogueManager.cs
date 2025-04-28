using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] RectTransform panel;
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] float timeBetweenSentences;
    [SerializeField] float textSpeed;
    [SerializeField] Button skipButton;

    private Queue<Sentence> sentences;
    private Queue<UnityEvent> dialogueEvents;
    private Tween tween;
    private bool skipRequested;

    private void Start()
    {
        sentences = new();
        dialogueEvents = new();
    }
    public void StartDialogue(Dialogue dialogue, List<UnityEvent> _dialogueEvents)
    {
        tween = panel.DOAnchorPos(new(0f, -300f), 1f).SetEase(Ease.OutBounce);
        panel.GetComponent<Image>().DOFade(0.45f, 1f).SetEase(Ease.Linear);
        sentences.Clear();

        foreach (var s in dialogue.sentences)
        {
            sentences.Enqueue(s);
        }
        foreach (var e in _dialogueEvents)
        {
            dialogueEvents.Enqueue(e);
        }
        StartCoroutine(DisplayNextSentenceCR());
    }

    private IEnumerator DisplayNextSentenceCR()
    {
        Sentence s;
        string sentence;
        float elapsedTime = 0f;

        yield return new WaitUntil(() => !tween.IsActive());

        do
        {
            skipButton.gameObject.SetActive(true);

            s = sentences.Dequeue();
            sentence = s.sentence;

            if (s.isTrigger)
            {
                dialogueEvents.Dequeue()?.Invoke();
            }

            textMesh.text = string.Empty;
            foreach(char c in sentence)
            {
                if (skipRequested)
                {
                    textMesh.text = sentence;
                    skipRequested = false;
                    break;
                }
                textMesh.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
            
            while(elapsedTime < timeBetweenSentences)
            {
                if (skipRequested)
                {
                    skipRequested = false;
                    break;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            elapsedTime = 0f;
        }
        while(sentences.Count > 0);

        EndDialogue();
    }

    private void EndDialogue()
    {
        Debug.Log("Dialogue ended");
        skipButton.gameObject.SetActive(false);
        panel.DOAnchorPos(new(0f, -900f), 1f).SetEase(Ease.Linear);
        panel.GetComponent<Image>().DOFade(0f, 1f).SetEase(Ease.Linear);
    }

    public void RequestSkip()
    {
        skipRequested = true;
    }
}
