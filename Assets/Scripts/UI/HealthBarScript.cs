using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour {

	private Image healthLine;

    private void Start() {
        healthLine = transform.GetChild( 0 ).gameObject.GetComponent<Image>();
    }

    public void UpdateBar( float currentHealth, float maxHealth ) {
    	healthLine.fillAmount = currentHealth / maxHealth;
    }
}
