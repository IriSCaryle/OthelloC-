using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PieceCounter : MonoBehaviour
{

    [SerializeField] Text text;
    [SerializeField] MainScript main;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        


    }

   public void CheckOthelloBoard()
    {
        int black=0;
        int white=0;
        int none= 0;

       (none,black,white) = main.CheckBoard_PieceNumber();


        text.text = "黒:" + black + "白:" + white;
    }



}
