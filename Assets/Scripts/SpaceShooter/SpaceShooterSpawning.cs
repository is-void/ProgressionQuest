using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpaceShooterSpawning : MonoBehaviour
{
    public SpaceShooterMovementSystem movementSystem;
    public TextMeshProUGUI waveText;
    public Transform phaseTransform;
    public float delayBeforeNextPhase;
    public GameObject[] phases;
    public GameObject currentPhase;
    public int index = 0;
    void Update()
    {
        if (currentPhase != null && currentPhase.transform.childCount == 0)
        {
            Debug.Log("Finished Phase");
            Destroy(currentPhase);
            StartCoroutine(SwitchPhase());
        }
    }
    public void BeginPhase()
    {
        waveText.enabled = false;
        currentPhase = phases[index];
        currentPhase = Instantiate(currentPhase, phaseTransform);
    }

    IEnumerator SwitchPhase()
    {
        if(index + 1 >= phases.Length)
        {
            yield return new WaitForSeconds(1.5f);
            movementSystem.Victory();
            yield break;
        }
        waveText.text = "Wave " + (index + 2);
        waveText.enabled = true;
        yield return new WaitForSeconds(delayBeforeNextPhase);
        index++;
        BeginPhase();
    }

}
