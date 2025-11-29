using UnityEngine;

public class VFXAnimator : MonoBehaviour
{
    public Sprite[] frames;      // your 6 PNGs
    public float duration = 0.15f;
    public float xsize = 1;
    public float ysize = 1;


    private SpriteRenderer sr;
    private float timer = 0f;
    private int index = 0;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        timer = 0f;
        index = 0;

        if (frames.Length > 0)
            sr.sprite = frames[0];
    }

    private void Update()
    {
        if (frames.Length == 0) return;

        float frameTime = duration / frames.Length;
        timer += Time.deltaTime;

        while (timer >= frameTime)
        {
            timer -= frameTime;
            index++;

            if (index >= frames.Length)
            {
                Destroy(gameObject);  // animation finished
                return;
            }

            sr.sprite = frames[index];
        }
    }
}
