using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject healthBarObject;
    private EntityInterface entity;
    public Image healthMeter;
    // Start is called before the first frame update
    void Start()
    {
        if (healthMeter.sprite != null)
            Debug.Log("Exists");
        
        entity = healthBarObject.GetComponent<EntityInterface>();
        healthMeter.fillMethod = Image.FillMethod.Horizontal;
        healthMeter.fillOrigin = 0;
        healthMeter.type = Image.Type.Filled;
        UpdateBar();
    }

    public void UpdateBar()
    {
        Debug.Log(entity.getHealth() / entity.getMaxHealth() + " ---> " + healthMeter.fillAmount);
        healthMeter.fillAmount = (entity.getHealth() / entity.getMaxHealth());      
    }
    
}
