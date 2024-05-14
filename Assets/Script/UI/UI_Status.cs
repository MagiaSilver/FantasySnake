using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Status : MonoBehaviour
{
    Status_Data Status;
    [Header("Status Output")]
    [SerializeField]
    private TextMeshProUGUI _HP_Text;
    [SerializeField]
    private TextMeshProUGUI _LV_Text;
    [SerializeField]
    private TextMeshProUGUI _ATK_Text;
    [SerializeField]
    private Image HPbar;
    // Start is called before the first frame update
    public void SetStatus(Status_Data Status,int LV)
    {
        _HP_Text.text = string.Format("HP : {0}", Status.HP);
        _LV_Text.text = string.Format("LV : {0}", LV);
        if(_ATK_Text != null)_ATK_Text.text = string.Format("ATK : {0}", Status.Attack);
        HPbar.fillAmount = Status.HP / Status.Max_HP;
    }
}
