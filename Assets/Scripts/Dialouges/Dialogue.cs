
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    public Sentence[] sentences;
}
[System.Serializable]
public class Sentence
{
    [TextArea(3, 10)]
    public string sentence;
    public bool isTrigger;
}