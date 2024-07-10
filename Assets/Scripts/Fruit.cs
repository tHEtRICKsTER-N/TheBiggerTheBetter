using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]

public class Fruit : MonoBehaviour
{
    public float size;
    public FruitType willBecomeWhenMerged;
    public Sprite fruitSprite;

    public bool isDropped = false;
    Vector2 mousePos;

    private void Start()
    {
        transform.localScale *= size;
    }

    private void Update()
    {
        if (!isDropped)
        {
            //we will try to find a location to drop it
            //fruit will follow the player's finger position

            if (Input.GetMouseButton(0))
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos = new Vector2(Mathf.Clamp(mousePos.x, -2.45f, 2.45f), 3.87f);
                transform.localPosition = mousePos;

            }
            if (Input.GetMouseButtonUp(0))
            {
                //that means the player has released the fruit, so it will fall

                this.AddComponent<Rigidbody2D>();
                isDropped = true;

                //we will update the list
                //we will make a dropper trail
            }
        }
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
