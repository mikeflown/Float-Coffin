using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RepairSystemButton : MonoBehaviour
{
    public string systemName;
    public Button repairButton;
    public Image progressBar;
    public GameObject warningIcon;
    private bool isRepairing = false;
    private void Start()
    {
        if (repairButton == null) repairButton = GetComponent<Button>();
        repairButton.onClick.AddListener(StartRepair);
        if (progressBar) progressBar.fillAmount = 1f;
        if (warningIcon) warningIcon.SetActive(false);
    }
    private void Update()
    {
        if (EnergyManager.Instance == null) return;
        bool isBroken = false;
        switch (systemName)
        {
            case "Oxygen": isBroken = EnergyManager.Instance.oxygenBroken; break;
            case "Engine": isBroken = EnergyManager.Instance.engineBroken; break;
            case "Radar": isBroken = EnergyManager.Instance.radarBroken; break;
            case "StrobeLeft": isBroken = EnergyManager.Instance.strobeLeftBroken; break;
            case "StrobeRight": isBroken = EnergyManager.Instance.strobeRightBroken; break;
        }
        if (warningIcon) warningIcon.SetActive(isBroken);
        if (isBroken)
        {
            if (!isRepairing && progressBar) progressBar.fillAmount = 0f;
        }
        else
        {
            if (progressBar) progressBar.fillAmount = 1f;
        }
        if (repairButton)
        {
            repairButton.interactable = isBroken && !isRepairing;
        }
    }
    private void StartRepair()
    {
        if (isRepairing || EnergyManager.Instance == null) return;
        int repairCost = 2;
        switch (systemName)
        {
            case "Oxygen": repairCost = EnergyManager.Instance.oxygenRepairCost; break;
            case "Engine": repairCost = EnergyManager.Instance.engineRepairCost; break;
            case "Radar": repairCost = EnergyManager.Instance.radarRepairCost; break;
            case "StrobeLeft": repairCost = EnergyManager.Instance.strobeLeftRepairCost; break;
            case "StrobeRight": repairCost = EnergyManager.Instance.strobeRightRepairCost; break;
        }
        if (EnergyManager.Instance.usedPower + repairCost > EnergyManager.Instance.maxPower)
        {
            Debug.Log("Недостаточно энергии для ремонта!");
            return;
        }
        EnergyManager.Instance.usedPower += repairCost;
        isRepairing = true;
        if (progressBar) progressBar.fillAmount = 0f;
        StartCoroutine(RepairProcess(repairCost));
    }
    private IEnumerator RepairProcess(int repairCost)
    {
        float repairTime = 8f;
        switch (systemName)
        {
            case "Oxygen": repairTime = EnergyManager.Instance.oxygenRepairTime; break;
            case "Engine": repairTime = EnergyManager.Instance.engineRepairTime; break;
            case "Radar": repairTime = EnergyManager.Instance.radarRepairTime; break;
            case "StrobeLeft": repairTime = EnergyManager.Instance.strobeLeftRepairTime; break;
            case "StrobeRight": repairTime = EnergyManager.Instance.strobeRightRepairTime; break;
        }
        float timer = 0f;
        while (timer < repairTime)
        {
            timer += Time.deltaTime;
            if (progressBar) progressBar.fillAmount = timer / repairTime;
            yield return null;
        }
        if (progressBar) progressBar.fillAmount = 1f;
        EnergyManager.Instance.usedPower -= repairCost;
        if (EnergyManager.Instance.usedPower < 0) EnergyManager.Instance.usedPower = 0;
        StartCoroutine(EnergyManager.Instance.RepairSystem(systemName));
        yield return new WaitForSeconds(0.2f);
        isRepairing = false;
    }
}