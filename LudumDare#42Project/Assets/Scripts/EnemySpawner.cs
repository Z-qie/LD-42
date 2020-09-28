using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float powerRemaining;
    public Transform ground;
    public LayerMask obstacleMask;

    [System.Serializable]
    public class Wave
    {
        public EnemyController[] typesOfEmeny;
        public float waveThreshold;
        public float spawnRate;
        public float campingCheckRate = 2f;
        public float startingHealth;
        public float moveSpeed;

        public int hitsToKillPLayer;
    }

    public Wave[] waves;

    public Transform player;
    public int seed;

    private Wave currentWave;
    private Vector3 playerPositionOld;
    private int currentWaveNumber;

    private float nextSpawnTime;
    private float nextCampingCheckTime = 2f;
    private float campingThresholdDistance = 1f;
    private bool isCamping;
    private bool isDisabled;
    private System.Random prng;

    private void Start()
    {
        prng = new System.Random(seed);

        player.gameObject.GetComponent<LivingEntity>().OnDeath += OnPlayerDeath;
        playerPositionOld = player.position;
        IntoNextWave();
    }

    private void Update()
    {
        if (!isDisabled)
        {
            if (Time.time > nextCampingCheckTime)
            {
                nextCampingCheckTime = Time.time + currentWave.campingCheckRate;
                isCamping = Vector3.Distance(player.position, playerPositionOld) < campingThresholdDistance;
                playerPositionOld = player.position;
            }

            if (Time.time > nextSpawnTime)
            {
                nextSpawnTime = Time.time + currentWave.spawnRate;
                StartCoroutine(SpawnEnemy());
            }


            // first wavethreshold <100 to get player acquaint with game mechanism
            if (powerRemaining < currentWave.waveThreshold)
            {
                IntoNextWave();
            }
        }
    }

    private void OnPlayerDeath()
    {
        isDisabled = true;
    }



    private void IntoNextWave()
    {
        currentWave = waves[currentWaveNumber++];
    }


    IEnumerator SpawnEnemy()
    {
        //float flashDelayedTime = 0f;
        float spawnDelay = 1f;
        //float flashRate = 4f;
        Vector3 spawnPosition;

        if (isCamping)
        {
            spawnPosition = player.position; // TBC
        }
        else
        {
            spawnPosition = GetRandomPosition();

            //while (flashDelayedTime < spawnDelay)
            //{
            //    tileMaterial.color = Color.Lerp(originalColor, Color.red, Mathf.PingPong(flashDelayedTime * flashRate, 1));
            //    flashDelayedTime += Time.deltaTime;
            //    yield return null;
            //}
        }

        print("Generating Enemey");
        yield return new WaitForSeconds(spawnDelay);

        GenerateEnemeyPrefab(spawnPosition, currentWave.typesOfEmeny[prng.Next(0, currentWave.typesOfEmeny.Length)]);
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 spawnPosition;
        do
        {
            spawnPosition = new Vector3(((float)prng.NextDouble() - 0.5f) * ground.localScale.x / 2, 0.1f, ((float)prng.NextDouble() - 0.5f) * ground.localScale.z / 2);
        } while (Physics.CheckSphere(spawnPosition, 1f, obstacleMask));

        return spawnPosition;
    }

    private void GenerateEnemeyPrefab(Vector3 spawnPosition, EnemyController typeOfEmeny)
    {
        EnemyController newEnemy = Instantiate<EnemyController>(typeOfEmeny, spawnPosition, Quaternion.identity, transform);
        newEnemy.SetCharacristics(currentWave.startingHealth, currentWave.moveSpeed, currentWave.hitsToKillPLayer);
    }

}