using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineTillingForBlock : MonoBehaviour
{
    [SerializeField]
    private Sprite vineSprite;
    [SerializeField]
    private GameObject block;
    [SerializeField]
    private Vector3 offsets;
    [SerializeField]
    private float scaleMulti;

    private SpriteRenderer[] sprites;
    private float oldLength = 1;


    void Start()
    {   
        UpdateTilling();
    }

    void Update()
    {
        if(oldLength != block.transform.localScale.y ){
            UpdateTilling();
        }
    }

    void UpdateTilling()
    {   
        oldLength = Mathf.Abs(block.transform.localScale.y);
        int num = (int) oldLength;

        // Destroy existing vine sprites if they exist
        if (sprites != null)
        {
            foreach (var sprite in sprites)
            {
                if (sprite != null)
                {
                    Destroy(sprite.gameObject);
                }
            }
        }

        // Initialize the array with the number of sprites based on the block's scale
        sprites = new SpriteRenderer[num];
        Vector3 scaledOffsets = new Vector3(offsets.x, offsets.y, offsets.z);
        // Create and position vine sprites
        for (int i = 0; i < num; i++)
        {
            GameObject vine = new GameObject("VineSprite_" + i);
            vine.transform.parent = block.transform;
            float inverseNum = 1f/num;
            float verticalPos = (i * inverseNum) - ((num - 1) * inverseNum/2);
            vine.transform.localPosition = new Vector3(0, verticalPos , 0) + scaledOffsets;
            vine.transform.localScale = new Vector3(scaleMulti, scaleMulti * inverseNum, scaleMulti);
            SpriteRenderer sr = vine.AddComponent<SpriteRenderer>();
            sr.sprite = vineSprite;

            sr.flipX = Random.value > 0.5f; 
            sr.flipY = Random.value > 0.5f;

            sprites[i] = sr;
        }
    }
}
