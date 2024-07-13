using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]

public class Fruit : MonoBehaviour
{
    public float size;
    public FruitType willBecomeWhenMerged;
    public FruitType type;
    public Sprite fruitSprite;

    public bool isDropped = false;

    public int instanceID;


    private void Start()
    {
        transform.localScale *= size;
        instanceID = GetInstanceID();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fruit"))
        {
            // we will handle logic of merging and finishing here
            var fruit = collision.gameObject.GetComponent<Fruit>();

            if (fruit != null)
            {
                if (fruit.type == FruitType.BIGPineapple)
                {
                    GameManager.Instance.SetGameOverTrue();
                    Destroy(gameObject);
                    Destroy(collision.gameObject);
                    return;
                }

                if (fruit.type == type)
                {
                    //the fruit collided with the same fruit. Now we merge them

                    Vector2 average = (transform.localPosition + collision.transform.localPosition) / 2f;
                    if (instanceID < collision.gameObject.GetComponent<Fruit>().instanceID) return;
                    
                    //set the score
                    GameManager.Instance.IncrementScore(GameManager.Instance.scoreIncreaseValue);
                    Debug.Log("MERGED !!");

                    //Instantiate the new gameObject
                    var newFruit = Instantiate(FruitHandler.Instance.GetFruitRefByEnum(willBecomeWhenMerged), average, Quaternion.identity);
                    newFruit.AddComponent<Rigidbody2D>();

                    //destroy both the fruits
                    Destroy(gameObject);
                    Destroy(collision.gameObject);
                }
            }

        }

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
