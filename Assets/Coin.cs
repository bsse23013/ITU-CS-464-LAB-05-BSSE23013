using UnityEngine;

public enum CoinType { Normal, Red, Blue }

// Lecture 06, Parts 2 + 3, extended: ONE prefab, three kinds of coin.
// The Spawner calls Init() to make a coin Normal (gold), Red or Blue.
public class Coin : MonoBehaviour
{
    [SerializeField] int value = 1;
    [SerializeField] float spinSpeed = 90f;
    [SerializeField] CoinType type = CoinType.Normal;

    void Awake() => ApplyColor();

    // Called by the Spawner right after Instantiate.
    public void Init(CoinType newType)
    {
        type = newType;
        ApplyColor();
    }

    void Update() => transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);

    void ApplyColor()
    {
        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend == null) return;

        Color c = type switch
        {
            CoinType.Red  => Color.red,
            CoinType.Blue => new Color(0.1f, 0.4f, 1f),
            _             => new Color(1f, 0.84f, 0f)   // gold
        };

        Material m = rend.material;                       // this coin's own copy
        if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", c);  // URP / HDRP
        if (m.HasProperty("_Color"))     m.SetColor("_Color", c);      // Built-in
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{type} coin touched by {other.name}");   // proof it fired
        if (other.CompareTag("Player"))
        {
            switch (type)   // TASK: each coin type reports something different
            {
                case CoinType.Normal: GameManager.Instance.AddScore(value);       break;
                case CoinType.Red:    GameManager.Instance.OnRedCoinCollected();   break;
                case CoinType.Blue:   GameManager.Instance.OnBlueCoinCollected();  break;
            }
            Destroy(gameObject);
        }
    }
}