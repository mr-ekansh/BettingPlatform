using UnityEngine;
using UnityEngine.UI;

public class BoxesDvT : MonoBehaviour
{
    private ChipDvT DvTManager;

    void Start()
    {
        DvTManager = GameObject.FindWithTag("DvTHandler").GetComponent<ChipDvT>();
    }

    public void AddChip()
    {
        DvTManager.AddandReset(this.gameObject.name, this.gameObject.transform);
    }
}
