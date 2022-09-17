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

    private float itemSlotSize = 50f;
    private float width = 0f;
    private float height = 0f;
    private float xStart = 0f;
    private float yStart = 0f;
    private float slotSize = 0f;

    public GameObject itemSlot;
    public float offset = 10f;
    public float intervalBetweenItem = 5f;

    public InventoryObject inventoryObject;

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
        calcItemSlotSize();
    }

    private void calcItemSlotSize() {
        itemSlotSize = (height - (offset * 2f) - intervalBetweenItem * (numRows - 1)) / numRows;
    }

    private void CreateDisplay() {
        for(int i=0; i < inventoryObject.inventory.itemList.Count; i++) {
            InventorySlot slot = inventoryObject.inventory.itemList[i];

            var obj = Instantiate(itemSlot, Vector3.zero, Quaternion.identity, transform);
            obj.transform.GetComponent<Image>().sprite = inventoryObject.database.itemDict[slot.item.id].Sprite;
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(itemSlotSize, itemSlotSize);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            itemsDisplayed.Add(slot, obj);
        }
    }

    private void UpdateDisplay() {
        for(int i=0; i < inventoryObject.inventory.itemList.Count; i++) {
            InventorySlot slot = inventoryObject.inventory.itemList[i];

            if(itemsDisplayed.ContainsKey(slot)) {
                itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            } else {
                var obj = Instantiate(itemSlot, Vector3.zero, Quaternion.identity, transform);
                obj.transform.GetComponent<Image>().sprite = inventoryObject.database.itemDict[slot.item.id].Sprite;
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponent<RectTransform>().sizeDelta = new Vector2(itemSlotSize, itemSlotSize);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
                itemsDisplayed.Add(slot, obj);
            }
        }
    }

    public Vector3 GetPosition(int i) {
        return new Vector3(xStart + (intervalBetweenItem + itemSlotSize) * (i % numColumns), yStart + -(intervalBetweenItem + itemSlotSize) * (i / numColumns), 0f);
    }
}
