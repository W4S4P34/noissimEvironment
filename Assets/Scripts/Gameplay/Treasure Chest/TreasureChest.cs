using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TreasureChest : MonoBehaviour
{
    public void SetListItem(List<Item> listItem)
    {
        this.listItem = listItem;
    }
    public static TreasureChest CreateTreasureChest(List<Item> listItem, int MAX_ITEM, Vector3 position)
    {
        GameObject treasureObject = Instantiate(GameAssets.i.pfTreasureChest, position, Quaternion.identity);
        TreasureChest treasureChest = treasureObject.GetComponent<TreasureChest>();
        // Random item here

        treasureChest.SetListItem(listItem);
        return treasureChest;
    }

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite closeSprite;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float approachRange;

    private List<Item> listItem;
    private bool isOpen = false;

    #region Monobehaviour Methods
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen)
        {
            var colliderObjects = Physics2D.OverlapCircleAll(transform.position, approachRange);
            foreach (var item in colliderObjects)
            {
                if (item.CompareTag("Player"))
                {
                    animator.SetBool("approach", true);
                    return;
                }
            }
            animator.SetBool("approach", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpen && collision.CompareTag("Player") && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isOpen = true;
            spriteRenderer.sprite = closeSprite;
            animator.SetBool("approach", false);
            // Drop out item here
            foreach(var item in listItem)
            {
                Instantiate(item.gameObject, transform.position, Quaternion.identity)?.GetComponent<Item>()?.DropOut();
            }
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, approachRange);
    }
    #endregion
}
