using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LevelSphereTrigger : MonoBehaviour
{
    [SerializeField] VisualEffect sphereEffect;
    [SerializeField] float levelDuration;
    [SerializeField] DialogueTrigger endDialogue;

    private SunMomentScript sun;
    public bool startedLevel;
    private float currentLevelTime;

    private void Start()
    {
        sphereEffect.Stop();
        sun = FindObjectOfType<SunMomentScript>();
    }

    private IEnumerator CheckLevelEndCR()
    {
        while(currentLevelTime < levelDuration)
        {
            currentLevelTime += Time.deltaTime;
            yield return null;
        }
        EndLevel();
    }
    public void StartLevel()
    {
        startedLevel = true;
        DisableBall();
        sun.StartSunFX();
        SpawnHandler.Instance.StartAllSpawners();

        StartCoroutine(CheckLevelEndCR());
    }
    public void EndLevel()
    {
        if (endDialogue != null)
            endDialogue.gameObject.SetActive(true);
        startedLevel = false;
        DisableBall();
        sun.StartBackToNormalFX();
        SpawnHandler.Instance.StopAllSpawners();
    }

    public void DisableBall()
    {
        var collider = GetComponent<BoxCollider>();
        collider.enabled = false;
        sphereEffect.Stop();
    }

    public void EnableBall()
    {
        var collider = GetComponent<BoxCollider>();
        collider.enabled = true;
        sphereEffect.Play();
    }
}
