using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public bool isIntro;
    public bool autoPlay;
    public Text dialogueText;
    public List<Dialogue> dialogueLines;
    private float currentDelay;

    Game game;

    void Start()
    {
        if (dialogueText == null)
            dialogueText = GetComponentInChildren<Text>();
        game = Game.game;
        if (isIntro)
        {
            if (game.skipIntro)
                gameObject.SetActive(false);
        }
        if (autoPlay && gameObject.activeInHierarchy)
            StartCoroutine(PlayDialogue());
    }

    void Update()
    {
        // maintain rotation
        transform.eulerAngles = Vector3.zero;
        // give buffer
        if (transform.position.x < -8)
            transform.position += new Vector3(3, 0, 0);
        if (transform.position.x > 8)
            transform.position -= new Vector3(3, 0, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentDelay = 0;
        }
    }

    IEnumerator PlayDialogue_old()
    {
        foreach (Dialogue line in dialogueLines)
        {
            dialogueText.transform.SetLocalPositionAndRotation(
                line.position, Quaternion.Euler(line.rotation));
            dialogueText.text = line.text;
            yield return new WaitForSeconds(line.delay);
        }
        gameObject.SetActive(false);
        game.currentState = GameState.RUNNING;
    }

    IEnumerator PlayDialogue()
    {
        foreach (Dialogue line in dialogueLines)
        {
            dialogueText.transform.SetLocalPositionAndRotation(
                line.position, Quaternion.Euler(line.rotation));
            dialogueText.text = line.text;
            // yield return new WaitForSeconds(line.delay);
            currentDelay = line.delay;
            while (currentDelay > 0)
            {
                Debug.Log(currentDelay);
                currentDelay -= Time.deltaTime;
                yield return null;
            }
        }
        gameObject.SetActive(false);
        game.currentState = GameState.RUNNING;
    }


    public void PlayDialogue(int dialogueId)
    {
        StartCoroutine(PlayDialogueById(dialogueId));
    }

    IEnumerator PlayDialogueById(int dialogueId)
    {
        if (dialogueId >= dialogueLines.Count)
            yield break;

        Dialogue line = dialogueLines[dialogueId];
        dialogueText.transform.SetLocalPositionAndRotation(
            line.position, Quaternion.Euler(line.rotation));
        dialogueText.text = line.text;
        yield return new WaitForSeconds(line.delay);
        dialogueText.text = "";
    }
}

[Serializable]
public struct Dialogue
{
    public string text;
    public float delay;
    public Vector3 position;
    public Vector3 rotation;
}