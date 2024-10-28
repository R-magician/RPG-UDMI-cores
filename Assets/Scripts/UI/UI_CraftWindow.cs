//合成材料窗口

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    //生成武器
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialImages;

    //装备合成面板
    public void SetupCraftWindow(ItemDataEquipment _data)
    {
        //移除按钮事件
        craftButton.onClick.RemoveAllListeners();
        for (int i = 0; i < materialImages.Length; i++)
        {
            materialImages[i].color = Color.clear;
            materialImages[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        //所需要的材料列表
        for (int i = 0; i < _data.craftingMaterials.Count; i++)
        {
            if (_data.craftingMaterials.Count > materialImages.Length)
            {
                //材料比工艺多
            }

            materialImages[i].sprite = _data.craftingMaterials[i].data.icon;
            materialImages[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialImages[i].GetComponentInChildren<TextMeshProUGUI>();
            
            //所需材料数量
            materialSlotText.text = _data.craftingMaterials[i].stackSize.ToString();
            materialSlotText.color = Color.white;
        }

        itemIcon.sprite = _data.icon;
        itemName.text = _data.itemName;
        itemDescription.text = _data.GetDescription();
        
        craftButton.onClick.AddListener(() =>
        {
            //物品可以被合成
            Inventory.instance.CanCraft(_data, _data.craftingMaterials);
        });
    }
}
