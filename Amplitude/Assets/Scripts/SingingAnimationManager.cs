using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingingAnimationManager : MonoBehaviour
{
    public ListenerComponent mic;
    public SpriteRenderer playerSpriteRenderer;

    public Sprite sprite_none;
    public float volumeThreshold_low;
    public Sprite sprite_low;
    public float volumeThreshold_mid;
    public Sprite sprite_mid;
    public float volumeThreshold_high;
    public Sprite sprite_high;

    private bool volume_none = true;
    private bool volume_low = false;
    private bool volume_mid = false;
    private bool volume_high = false;

    // Start is called before the first frame update
    void Start()
    {
        mic.OnStartSilence.AddListener(SetToNone);
    }

    void SetToNone()
    {
        playerSpriteRenderer.sprite = sprite_none;
        volume_none = true;
        volume_low = false;
        volume_mid = false;
        volume_high = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (mic.volumeSmoothed > volumeThreshold_low && mic.volumeSmoothed < volumeThreshold_mid)
        {
            if (!volume_low)
            {
                playerSpriteRenderer.sprite = sprite_low;
                volume_none = false;
                volume_low = true;
                volume_mid = false;
                volume_high = false;
            }
        }
        else if (mic.volumeSmoothed > volumeThreshold_mid && mic.volumeSmoothed < volumeThreshold_high)
        {
            if (!volume_mid)
            {
                playerSpriteRenderer.sprite = sprite_mid;
                volume_none = false;
                volume_low = false;
                volume_mid = true;
                volume_high = false;
            }
        }
        else if (mic.volumeSmoothed > volumeThreshold_high)
        {
            if (!volume_high)
            {
                playerSpriteRenderer.sprite = sprite_high;
                volume_none = false;
                volume_low = false;
                volume_mid = false;
                volume_high = true;
            }
        }
    }
}
