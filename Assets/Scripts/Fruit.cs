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
    [SerializeField] private Color _fruitColor;
    private GameObject _newFruit;


    private void Start()
    {
        transform.localScale *= size;
        instanceID = GetInstanceID();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isDropped)
        {
            if (collision.gameObject.CompareTag("Finish"))
            {
                //some fruit is above the line
                GameManager.Instance.StartBufferTime();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isDropped)
        {
            if (collision.CompareTag("Finish"))
            {
                GameManager.Instance.StopBufferTime();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fruit"))
        {

            // we will handle logic of merging and finishing here
            var fruit = collision.gameObject.GetComponent<Fruit>();

            if (fruit != null)
            {
                if (fruit.type == FruitType.BIGPineapple && this.type == FruitType.BIGPineapple)
                {
                    GameManager.Instance.SetGameWinTrue();
                    Destroy(gameObject);
                    Destroy(collision.gameObject);
                    return;
                }


                if (fruit.type == type)
                {
                    //the fruit collided with the same fruit. Now we merge them

                    //we will play merge sound
                    SoundManager.Instance.sfxAudioSource.PlayOneShot(SoundManager.Instance.mergeSound);

                    Vector2 average = (transform.localPosition + collision.transform.localPosition) / 2f;
                    if (instanceID < collision.gameObject.GetComponent<Fruit>().instanceID) return;

                    //set the score
                    GameManager.Instance.IncrementScore(GameManager.Instance.scoreIncreaseValue);
                    Debug.Log("MERGED !!");

                    //Instantiate the new gameObject
                    _newFruit = Instantiate(FruitHandler.Instance.GetFruitRefByEnum(willBecomeWhenMerged), average, Quaternion.identity);
                    _newFruit.AddComponent<Rigidbody2D>();

                    //destroy both the fruits
                    Destroy(gameObject);
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (_newFruit != null)
        {
            var effect = Instantiate(FruitHandler.Instance.spawnEffect, _newFruit.transform.localPosition, Quaternion.identity);
            effect.GetComponent<SpriteRenderer>().material.color = _fruitColor;
            Destroy(effect.gameObject, 0.75f);
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
