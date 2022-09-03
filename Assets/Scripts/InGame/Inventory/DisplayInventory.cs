using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    [SerializeField]
    private int numColumns = 10;
    [SerializeField]
    private int numRows = 1;

    private int itemSpriteSize = 40;

    private float width = 0f;
    private float height = 0f;
    private float xStart = 0f;
    private float yStart = 0f;
    private float slotSize = 0f;

    public float offset = 10f;
    public float spaceBetweenItem = 10f;

    public PlayerInventory playerInventory;
    public GameObject itemSlot;

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    private void Start() {
        StartCoroutine(SetStartPosition());
        CreateDisplay();
    }

    private void Update() {
        UpdateDisplay();
    }

    private IEnumerator SetStartPosition() {
        yield return new WaitForEndOfFrame();
        width = transform.GetComponent<RectTransform>().sizeDelta.x;
        height = transform.GetComponent<RectTransform>().sizeDelta.y;
        slotSize = (width - 2 * offset) / numColumns;
        xStart = -width / 2 + offset + slotSize / 2;
        yStart = -height / 2;
    }

    private void CreateDisplay() {
        for(int i=0; i < playerInventory.inventory.Count; i++) {
            var obj = Instantiate(itemSlot, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = playerInventory.inventory[i].amount.ToString("n0");
            itemsDisplayed.Add(playerInventory.inventory[i], obj);
        }
    }

    private void UpdateDisplay() {
        for(int i=0; i < playerInventory.inventory.Count; i++) {
            if(itemsDisplayed.ContainsKey(playerInventory.inventory[i])) {
                itemsDisplayed[playerInventory.inventory[i]].GetComponentInChildren<TextMeshProUGUI>().text = playerInventory.inventory[i].amount.ToString("n0");
            } else {
                var obj = Instantiate(itemSlot, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponent<Image>().sprite = playerInventory.inventory[i].item.Sprite;
                obj.GetComponentInChildren<TextMeshProUGUI>().text = playerInventory.inventory[i].amount.ToString("n0");
                itemsDisplayed.Add(playerInventory.inventory[i], obj);
            }
        }
    }

    public Vector3 GetPosition(int i) {
        return new Vector3(xStart + spaceBetweenItem * (i % numColumns), yStart + (-spaceBetweenItem * (i / numColumns)), 0f);
    }
}
