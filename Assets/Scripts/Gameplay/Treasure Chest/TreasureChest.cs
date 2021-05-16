using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

        System.Random r = new System.Random();

        List<double> temp = new List<double>(listItem.Count + 1) { 0 };

        for (int i = 0; i < listItem.Count; i++)
        {
            temp.Add(temp[temp.Count - 1] + listItem[i].dropItemRate);
        }

        int numberOfItems = r.Next(1, MAX_ITEM + 1);

        List<Item> newList = new List<Item>(numberOfItems);

        for(int i = 0; i < numberOfItems; i++) {
            double rand = r.NextDouble() * 100;

            for(int j = 0; j < listItem.Count; j++)
            {
                Debug.Log(temp[j] + "; " + temp[j + 1]);
                if(rand > temp[j] && rand < temp[j + 1])
                {
                    newList.Add(listItem[j]);
                    break;
                }
            }
        }

        treasureChest.SetListItem(newList);
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
