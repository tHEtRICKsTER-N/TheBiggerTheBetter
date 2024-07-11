using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]

public class Fruit : MonoBehaviour
{
    public float size;
    public FruitType willBecomeWhenMerged;
    public Sprite fruitSprite;

    public bool isDropped = false;
  

    private void Start()
    {
        transform.localScale *= size;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // we will handle logic of merging and finishing here
    }

}

public enum FruitType
{
    None,
    Cherry,
    Strawberry,
    Berry,
    OrangeFruit,
    Tangerine,
    Apple,
    Pear,
    Guava,
    BIGBanana,
    BIGPineapple
}
