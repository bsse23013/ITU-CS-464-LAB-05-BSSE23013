using UnityEngine;

// Lecture 06, Part 3, extended with the red coin and the blue bonus ring.
// Put on an empty GameObject in the middle of the arena (position 0,0,0).
public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] Transform player;
    [SerializeField] float interval = 1.5f;
    [SerializeField] int maxCoins = 5;
    [SerializeField] float area = 7f;          // half-size of the play area

    [Header("Bonus ring (blue coins)")]
    [SerializeField] float bonusMinRadius = 1.5f;   // keeps coins off the player
    [SerializeField] float bonusMaxRadius = 5f;

    void Start() => InvokeRepeating(nameof(SpawnCoin), 1f, interval);

    // ---- Normal gold coins (lecture version) ----
    void SpawnCoin()
    {
        // Only gold coins are children of the Spawner, so red/blue never use up this cap.
        if (transform.childCount >= maxCoins) return;

        Vector3 pos = RandomPoint();
        if (Vector3.Distance(pos, player.position) < 2f) return;
        Make(pos, CoinType.Normal, transform);
    }

    // ---- TASK 1: exactly one red coin, called by the GameManager ----
    public void SpawnRedCoin()
    {
        for (int attempt = 0; attempt < 30; attempt++)
        {
            Vector3 pos = RandomPoint();
            if (Vector3.Distance(pos, player.position) < 2f) continue;
            Make(pos, CoinType.Red, null);
            return;
        }
    }

    // ---- TASK 2: N blue coins in a circle around the player ----
    public void SpawnBonusCoins(int count)
    {
        Vector3 center = player.position;
        float goldenAngle = 137.508f * Mathf.Deg2Rad;   // spreads points evenly, no overlaps

        for (int i = 0; i < count; i++)
        {
            float t = (i + 0.5f) / count;
            // sqrt of the lerped squares = even spread over the ring's AREA
            float r = Mathf.Sqrt(Mathf.Lerp(bonusMinRadius * bonusMinRadius,
                                            bonusMaxRadius * bonusMaxRadius, t));
            float a = i * goldenAngle;

            float x = Mathf.Clamp(center.x + Mathf.Cos(a) * r, -area, area);
            float z = Mathf.Clamp(center.z + Mathf.Sin(a) * r, -area, area);
            Make(new Vector3(x, 0.5f, z), CoinType.Blue, null);
        }
    }

    // ---- helpers ----
    Vector3 RandomPoint() =>
        new Vector3(Random.Range(-area, area), 0.5f, Random.Range(-area, area));

    void Make(Vector3 pos, CoinType type, Transform parent)
    {
        GameObject go = Instantiate(coinPrefab, pos, Quaternion.identity, parent);
        go.GetComponent<Coin>().Init(type);
    }
}