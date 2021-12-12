using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
	[SerializeField]
	private float maxHealth = 100;
	private float currentHealth;

	private void Start() {
		currentHealth = maxHealth;
	}

	public void GetDamaged(float damage) {
		currentHealth -= damage;
		if(currentHealth <= 0) {
			currentHealth = 0;
			//gameover
		}
	}
}
