using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject Player;
	public GameObject EnemyPrefab;

	private GameObject[] currentEnemiesSpawned;
	private List<SpawnPoint> lSpawnPoints;

	public int MAX_ENEMIES = 4;

	float timer = -1;
	int currentEnemyCount = 0;
	int count = 0;
	float SpawnDelay = 2.0f;
	float DeactivateAITime = 1.0f;
	bool InitEnemies = false;

	private CharacterHealth playerHealth;
	private CharacterControl playerCC;

	void Start()
	{
		timer = SpawnDelay;
		currentEnemyCount = MAX_ENEMIES;

		currentEnemiesSpawned = new GameObject[currentEnemyCount];

		playerHealth = Player.GetComponent<CharacterHealth> ();
		playerCC = Player.GetComponent<CharacterControl> ();

		StartCoroutine (DelayEnemySpawn ());
	}

	IEnumerator DelayEnemySpawn()
	{
		yield return new WaitForSeconds (0.5f);
		var SpawnPoints = FindObjectsOfType<SpawnPoint> ();
		lSpawnPoints = new List<SpawnPoint> ();
		foreach (SpawnPoint sp in SpawnPoints) {
			if (sp.IsOcuppied)
				continue;
			else
				lSpawnPoints.Add (sp);
		}
			
		SetupEnemy (lSpawnPoints);
	}

	void SetupEnemy(List<SpawnPoint> SpawnPoints)
	{
		int Length = SpawnPoints.Count;
		for (int i = 0; i < currentEnemyCount; i++) {
			SpawnPoint mSpawnPointRef = SpawnPoints [Random.Range (0, Length)];
			Transform SpawnPos = mSpawnPointRef.transform;

			Debug.Log (SpawnPos.name);
			currentEnemiesSpawned [i] = Instantiate (EnemyPrefab , SpawnPos.position , SpawnPos.rotation);
			currentEnemiesSpawned [i].transform.Rotate (Vector3.up, 180.0f);
			currentEnemiesSpawned [i].SetActive (false);
		}

		InitEnemies = true;
	}

	void Update()
	{
		SpawnTimer ();
		CheckEnemyStatus ();
	}

	void CheckEnemyStatus()
	{
		if (!InitEnemies)
			return;

		for(int i = 0; i<currentEnemyCount; i++){

			var EnemyHealth = currentEnemiesSpawned[i].GetComponent<CharacterHealth> ();
			var EnemyCC = currentEnemiesSpawned[i].GetComponent<CharacterControl> ();

			if (playerHealth.currentHealth < 0.0f) {
				Debug.Log ("===Player is Dead!!");
				EnemyCC.Stop ();
			}

			if (EnemyHealth.currentHealth < 0.0f) {
				//currentEnemiesSpawned [i].SetActive (false);
				DeactivateAITime -= Time.deltaTime;
				if (DeactivateAITime < 0) {
					currentEnemiesSpawned [i].SetActive (false);
					DeactivateAITime = 0.5f;
				}
			}

			if (EnemyHealth.IsAlive) {
				playerCC.IsEngaging = true;
			} else {
				playerCC.IsEngaging = false;
			}
		}
	}

	void SpawnTimer()
	{
		timer -= Time.deltaTime;
		if (timer <= 0) {

			if (count == currentEnemyCount) {
				//count = 0;
				return;
			}
			else {
				currentEnemiesSpawned [count].SetActive (true);
				bool IsAlive = currentEnemiesSpawned [count].GetComponent<CharacterHealth> ().IsAlive;

				//currentEnemiesSpawned [count].GetComponent<CharacterControl> ().SetStoppingDistance (Random.Range (2,4));
				if (IsAlive)
					return;

				count++;
				timer = 1.0f + DeactivateAITime;
			}
		}
	}
}
