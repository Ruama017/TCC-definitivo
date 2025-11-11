using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSuper : MonoBehaviour
{
    [Header("Super state")]
    public bool isSuper = false;
    public float superTimeRemaining = 0f;
    public GameObject superVfx;

    [Header("Stomp / Bounce")]
    public float stompBounceForce = 8f;

    private Rigidbody2D rb;
    private Coroutine superCoroutine;
    private GameObject vfxInstance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ActivateSuper(float duration)
    {
        if (superCoroutine != null) StopCoroutine(superCoroutine);
        superCoroutine = StartCoroutine(SuperRoutine(duration));
    }

    private IEnumerator SuperRoutine(float duration)
    {
        isSuper = true;
        superTimeRemaining = duration;

        if (superVfx != null && vfxInstance == null)
            vfxInstance = Instantiate(superVfx, transform);

        while (superTimeRemaining > 0f)
        {
            superTimeRemaining -= Time.deltaTime;
            yield return null;
        }

        isSuper = false;
        if (vfxInstance != null) Destroy(vfxInstance);
        superCoroutine = null;
    }

    public void BounceOnStomp()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * stompBounceForce, ForceMode2D.Impulse);
    }
}
