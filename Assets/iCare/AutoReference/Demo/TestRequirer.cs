using UnityEngine;

namespace iCare.AutoReference.Demo {
    public sealed class TestRequirer : MonoBehaviour {
        [Auto(nameof(TestItems.HelloItem))]
        [SerializeField] TestItemSo helloItem;
        
        [Auto(nameof(TestItems.ItemOne))]
        [SerializeField] TestItemSo itemOne;
        
        [Auto(nameof(TestItems.ItemTwo))]
        [SerializeField] TestItemSo itemTwo;

        [AutoList] [SerializeField] TestItemSo[] allItems;
        [AutoList] [SerializeField] TestItemSo[] allItemsArray;
    }
}