using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceGiver : MechSystem
{
    [Header("Refs")] [SerializeField] Lever _lever;
    [SerializeField] private ResourceConsole _energy;
    [SerializeField] private ResourceConsole _ammo;
    [SerializeField] private GameObject energyPreFab;
    [SerializeField] private GameObject ammoPreFab;
    [SerializeField] private int amountEmptyCrates;
    [SerializeField] private TMP_Text cratesText;

    [Header("Attributes")] [SerializeField]
    private int id;

    [SerializeField] private bool noBoxSpawnLimit;

    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
        if (id == 0)
        {
            StartCoroutine(SpawnNewAmmo(0));
            StartCoroutine(SpawnNewEnergy(0));
        }
        else if (id == 1)
        {
            StartCoroutine(SpawnNewAmmo(0));
        }
        else if (id == 2)
        {
            StartCoroutine(SpawnNewEnergy(0));
        }
    }
    
    public override void Trigger(int whichMethod = -1)
    {
        CheckLever();
    }

    private void CheckLever()
    {
        if (id != 0)
        {
            if (id == 1) GiveResource(Object.typeObjects.AmmoCrate);
            else if (id == 2) GiveResource(Object.typeObjects.EnergyCell);
        }
        else
        {
            if (_lever.status == 0) return;
            if (_lever.status == 1 && id == 0) GiveResource(Object.typeObjects.AmmoCrate);
            else if (_lever.status == -1 && id == 0) GiveResource(Object.typeObjects.EnergyCell);
        }
    }

    private void GiveResource(Object.typeObjects type)
    {
        if (!noBoxSpawnLimit)
        {
            if (amountEmptyCrates - 1 < 0) return;
            amountEmptyCrates -= 1;
        }

        if (type == Object.typeObjects.AmmoCrate)
        {
            _ammo.EjectObject();
            StartCoroutine(SpawnNewAmmo(1));
        }

        if (type == Object.typeObjects.EnergyCell)
        {
            _energy.EjectObject();
            StartCoroutine(SpawnNewEnergy(1));
        }

        UpdateText();
    }

    private IEnumerator SpawnNewEnergy(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject energy = Instantiate(energyPreFab, _energy.resourcePlace.transform.position, Quaternion.identity);
        energy.GetComponentInChildren<Object>().Start();
        _energy.PlaceResource(energy.GetComponentInChildren<Resource>());
    }

    private IEnumerator SpawnNewAmmo(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject ammo = Instantiate(ammoPreFab, _ammo.resourcePlace.transform.position, Quaternion.identity);
        ammo.GetComponentInChildren<Object>().Start();
        _ammo.PlaceResource(ammo.GetComponentInChildren<Resource>());
    }

    public void IncreaseNumberEmptyCrates()
    {
        amountEmptyCrates += 1;
        UpdateText();
    }

    private void UpdateText()
    {
        if (cratesText.IsUnityNull()) return;
        cratesText.SetText("Crates: " + amountEmptyCrates.ToString());
    }
}