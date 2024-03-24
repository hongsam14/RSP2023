using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Result
{
    LOSE,
    WIN,
    DRAW,
    OUT
}

public class GameBehaviour : MonoBehaviour
{
    public GameObject zoomCamera;
    public GameObject mainCamera;
    
    SlowMotion slowMotion;
    
    // Start is called before the first frame update
    void Start()
    {
        slowMotion = GetComponent<SlowMotion>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Result Judge_rsp(uint my_finger, int other_finger)
    {
        switch (my_finger)
        {
            case 0: //rock
                if (other_finger == 0)
                    return Result.DRAW;
                if (other_finger == 2)
                    return Result.WIN;
                if (other_finger == 5)
                    return Result.LOSE;
                break;
            case 2: //sizzer
                if (other_finger == 0)
                    return Result.LOSE;
                if (other_finger == 2)
                    return Result.DRAW;
                if (other_finger == 5)
                    return Result.WIN;
                break;
            case 5: //paper
                if (other_finger == 0)
                    return Result.WIN;
                if (other_finger == 2)
                    return Result.LOSE;
                if (other_finger == 5)
                    return Result.DRAW;
                break;
            default:
                return Result.OUT;
        }
        return Result.OUT;
    }
}
