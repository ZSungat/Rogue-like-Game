using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberController : MonoBehaviour
{
    public static DamageNumberController instance;

    private void Awake()
    {
        instance = this;
    }

    public DamageNumber NumberToSpawn;
    public Transform NumberCanvas;

    private List<DamageNumber> NumberPool = new List<DamageNumber>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnDamage(float damageAmount, Vector3 location)
    {
        int Rounded = Mathf.RoundToInt(damageAmount);

        DamageNumber NewDamage = GetFromPool();

        NewDamage.Setup(Rounded);
        NewDamage.gameObject.SetActive(true);

        NewDamage.transform.position = location;
    }

    public DamageNumber GetFromPool()
    {
        DamageNumber NumberToOutput = null;

        if (NumberPool.Count == 0)
        {
            NumberToOutput = Instantiate(NumberToSpawn, NumberCanvas);
        }
        else
        {
            NumberToOutput = NumberPool[0];
            NumberPool.RemoveAt(0);
        }

        return NumberToOutput;
    }

    public void PlaceInPool(DamageNumber NumberToPlace)
    {
        NumberToPlace.gameObject.SetActive(false);

        NumberPool.Add(NumberToPlace);
    }
}
