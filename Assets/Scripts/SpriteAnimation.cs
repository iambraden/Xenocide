using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    [Header("Sprite Animation")]
    public Sprite[] animationSprites;
    public float animationTime = 1.0f;

    private SpriteRenderer spriteRenderer;
    private int animationFrame;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        InvokeRepeating(nameof(animateSprite), this.animationTime, this.animationTime);
    }

    void animateSprite()    //only works for 2-frame animation
    {
        if(animationFrame == 0)
        {
            animationFrame = 1;
        }
        else
        {
            animationFrame = 0;
        }

        spriteRenderer.sprite = this.animationSprites[animationFrame];
    }
}
