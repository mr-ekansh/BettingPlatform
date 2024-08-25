using UnityEngine;

public class BoxesRoulette : MonoBehaviour
{
    private ChipPlaceRoulette ChipAPI;

    void Start()
    {
        ChipAPI = GameObject.FindWithTag("RouletteAPIHandler").GetComponent<ChipPlaceRoulette>();
    }

    public void AddChip()
    {
        ChipAPI.AddandReset(this.gameObject.name, this.gameObject.transform);
    }

}

