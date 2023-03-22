using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private BaseItem _item;
    
    public BaseItem GetItem()
    {
        return _item;
    } 
}
