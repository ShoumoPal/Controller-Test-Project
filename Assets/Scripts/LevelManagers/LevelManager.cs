using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	#region Singleton
	public static LevelManager Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    [SerializeField] LevelSphereTrigger levelTrigger;
    [SerializeField] float levelTime;

    public void EndCurrentLevel()
    {
        levelTrigger.EnableBall();
    }
}
