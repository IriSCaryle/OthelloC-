using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBot : MonoBehaviour
{
    [SerializeField] Othello othello;
    [SerializeField] MainScript main;

    int[,] NowOthelloBoard = new int[8, 8];
    int AIturn = 0;


    int[,] OThelloEvaluation = new int[8, 8]
   {
        {100,20,80,80,80,80,20,100},
        {20,30,30,30,30,30,30,20},
        {80,60,70,55,55,70,60,80},
        {80,60,55,50,50,55,60,80},
        {80,60,55,50,50,55,60,80},
        {80,60,70,55,55,70,60,80},
        {20,30,30,30,30,30,30,20},
        {100,20,80,80,80,80,20,100}
   };
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


    public void AITurn()
    {
        bool isput = false;

        List<List<int>> CanChangePieces = new List<List<int>>();

        int change_piece_num = 0;

        NowOthelloBoard = main.OthelloBoard;//現在のオセロの盤面を持ってくる

        for (int v = 0; v < NowOthelloBoard.GetLength(0); v++)
        {
            for (int h = 0; h < NowOthelloBoard.GetLength(1); h++)
            {
                Debug.Log("AI:" + v + "," + h + "を検索中");
                isput = main.SerchChengePieces(v, h);
                if (isput)
                {
                    Debug.Log("AI:置ける場所が検出されました" + v + "," + h +"List" +change_piece_num+"行目に配置します");
                    CanChangePieces[change_piece_num].Add(v);//out of range のエラー・・・なんで・・・
                    CanChangePieces[change_piece_num].Add(h);
                    change_piece_num++;
                }
                else
                {
                    main.SerchCanceled();
                }
            }
        }

        Debug.Log("置ける場所の評価をします");

        Debug.Log("CanChangePieces Lenhth:" + CanChangePieces.Count);

        int max = 0;
        int vertical =0;
        int horizontal = 0;

        for (int i = 0; i < CanChangePieces.Count; i++)
        {
            if(OThelloEvaluation[CanChangePieces[i][0], CanChangePieces[i][1]] > max)
            {
                max = OThelloEvaluation[CanChangePieces[i][0], CanChangePieces[i][1]];

                vertical = CanChangePieces[i][0];

                horizontal = CanChangePieces[i][1];
            }
        }

        Debug.Log("最も評価の高い座標は" + vertical +"," + horizontal +"でした");
        Debug.Log("配置します");

        othello = main.OthelloScriptsArrow[vertical, horizontal];

        othello.OnEnter();
        othello.OnClick();

    }
}
