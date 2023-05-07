using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuiManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GuiManager instance;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Image healthBar_img;
    [SerializeField] private Image attack_img;
    [SerializeField] private Text attack_txt;

    private WaitForSeconds enemySpawnDelay;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    void Start()
    {
        enemySpawnDelay = new WaitForSeconds(3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthBar (float healthPercentage)
    {
        healthBar_img.fillAmount = healthPercentage;
    }

    public void GrayOutAttack (float AttackOpacity)
    {
        attack_img.color = new Color(attack_img.color.r, attack_img.color.g, attack_img.color.b, AttackOpacity);
    }

    
    
    public void ChangeText(string AttackText)
    {
        attack_txt.text = AttackText;
    }
}
