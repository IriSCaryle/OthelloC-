using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBot : MonoBehaviour
{


    [SerializeField] MainScript main;

   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public int CheckTurn()
    {
        int AIturn = 0;
        switch ((int)main.PlayerTurn)
        {
            case 1:

                AIturn = 2;

                break;

            case 2:

                AIturn = 1;

                break;
        }


        return AIturn;
    }
}
