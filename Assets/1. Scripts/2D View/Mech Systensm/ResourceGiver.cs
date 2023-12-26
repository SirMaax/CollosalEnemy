using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceGiver : MechSystem
{
    [Header("Attributes")] 
    [SerializeField] private bool _isUsingButton;
    [SerializeField] private int id;
    [SerializeField] private bool noBoxSpawnLimit;
    [SerializeField] private float _respawnTime;
    
    [Header("Refs")] [SerializeField] Lever _lever;
    [SerializeField] private ResourceConsole _energy;
    [SerializeField] private ResourceConsole _ammo;
    [SerializeField] private GameObject energyPreFab;
    [SerializeField] private GameObject ammoPreFab;
    [SerializeField] private int amountEmptyCrates;
    [SerializeField] private TMP_Text cratesText;


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

    public void Update()
    {
        if(_energy != null && !_energy.isLoaded)StartCoroutine(SpawnNewEnergy(_respawnTime));
        else if(_ammo != null && !_ammo.isLoaded)StartCoroutine(SpawnNewAmmo(_respawnTime));
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
        GameObject energy = Instantiate(energyPreFab, _energy.resourcePlace.transform.position, Quaternion.identity);
        energy.GetComponentInChildren<Object>().Start();
        _energy.PlaceResource(energy.GetComponentInChildren<Resource>());
        energy.GetComponentInChildren<Object>().IncreaseOpacityOverTime(time);
        _energy.canTakeResource = false;
        yield return new WaitForSeconds(time);
        _energy.canTakeResource = true;
    }

    private IEnumerator SpawnNewAmmo(float time)
    {
        
        GameObject ammo = Instantiate(ammoPreFab, _ammo.resourcePlace.transform.position, Quaternion.identity);
        ammo.GetComponentInChildren<Object>().Start();
        _ammo.PlaceResource(ammo.GetComponentInChildren<Resource>());
        ammo.GetComponentInChildren<Object>().IncreaseOpacityOverTime(time);
        _ammo.canTakeResource = false;
        yield return new WaitForSeconds(time);
        _ammo.canTakeResource = true;

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