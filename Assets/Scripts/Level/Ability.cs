using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ability : MonoBehaviour
{
   
    private List<Ability> a;
    [SerializeField] private GameObject player;
    [SerializeField]private PlayerController playerController;

    public bool canDoubleJump;
    public bool isInmune;    
    public bool canFloat;    
    public enum AbilityType
    {
        Inmunidad,
        SaltoDoble,
        Desintoxicacion,
        VidaExtra,
        Paracaidas        
    }
    public AbilityType abilityType;
 
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        a = new List<Ability>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))
        {
           if(a.Count == 0) a = collision.GetComponent<PlayerController>().abilities;
           
            foreach (Ability ability in a)
            {
                if (ability.abilityType == abilityType)
                {
                   collision.GetComponent<PlayerController>().TakeAbility(ability);
                    print(ability);
                }
            }

        }

    }

    public void AbilityDie()
    {
        //gameObject.SetActive(false);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject);
        
    }

    public void NewAbility()
    {
        //sustanceType = SustanceType.Cannabis;
        switch (abilityType)
        {
            case AbilityType.Inmunidad:
                print("Inmunidad");
                isInmune = true;    
                break;
            case AbilityType.SaltoDoble:
                print("SaltoDoble");
                canDoubleJump = true;
                break;
            case AbilityType.Desintoxicacion:
                print("Desintoxicacion");
                break;
            case AbilityType.VidaExtra:
                print("VidaExtra");
                break;
            case AbilityType.Paracaidas:
                print("Paracaidas");
                canFloat = true;
                break;  
        }
    }
}
