using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceGiver : MonoBehaviour
{
    [Header("Refs")] 
    [SerializeField] Lever _lever;
    [SerializeField]private GroundButton _button;
    [SerializeField] private ResourceHoldingPlace _energy;
    [SerializeField] private ResourceHoldingPlace _ammo;
    [SerializeField] private GameObject energyPreFab;
    [SerializeField] private GameObject ammoPreFab;
    [SerializeField] private int amountEmptyCrates;
    [SerializeField] private TMP_Text cratesText;
    
    [Header("Attributes")]
    [SerializeField] private int id;
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

    // Update is called once per frame
    void Update()
    {
        ButtonPressCheck();        
    }

    private void ButtonPressCheck()
    {
        if (id != 0 && _button.buttonWasPressed)
        {
            if(id == 1)GiveResource(GObject.typeObjects.AmmoCrate);
            else if (id == 2)GiveResource(GObject.typeObjects.EnergyCell);
            _button.buttonWasPressed = false;
        }
        else
        {
            if (!_button.buttonWasPressed || _lever.status == 0) return;
            _button.buttonWasPressed = false;
        
            if (_lever.status == 1 && id == 0) GiveResource(GObject.typeObjects.AmmoCrate);
            if (_lever.status == -1 && id == 0) GiveResource(GObject.typeObjects.EnergyCell);
        }
        
        
    }

    private void GiveResource(GObject.typeObjects type)
    {
        if (amountEmptyCrates - 1 < 0 || noBoxSpawnLimit) return;
        amountEmptyCrates -= 1;
        if (type == GObject.typeObjects.AmmoCrate)
        {
            // if (_ammo.isLoaded)
            // {
            //     //TODO badstuff?
            //     return;
            // }
            _ammo.EjectShell();
            StartCoroutine(SpawnNewAmmo(1));
        }
        if (type == GObject.typeObjects.EnergyCell)
        {
            // if (_energy.isLoaded)
            // {
            //     //TODO badstuff?
            //     return;
            // }
            _energy.EjectShell();
            StartCoroutine(SpawnNewEnergy(1));
        }
        UpdateText();
    }

    private IEnumerator SpawnNewEnergy(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject energy = Instantiate(energyPreFab, _energy.resourcePlace.transform.position, Quaternion.identity);
        energy.GetComponentInChildren<GObject>().Start();
        _energy.PlaceResource(energy.GetComponentInChildren<GObject>());
    }

    private IEnumerator SpawnNewAmmo(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject ammo = Instantiate(ammoPreFab, _ammo.resourcePlace.transform.position, Quaternion.identity);
        ammo.GetComponentInChildren<GObject>().Start();
        _ammo.PlaceResource(ammo.GetComponentInChildren<GObject>());
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
