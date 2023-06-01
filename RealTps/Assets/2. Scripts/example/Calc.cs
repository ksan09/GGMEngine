using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calc : MonoBehaviour
{
    //오버로드
    //같은 기능을 하는 여러가지 버전
    //내부 동작은 비슷한데 굳이 이름을 쪼개서 혼란을 일으킬 필요가 없다.
    //함수 이름을 똑같이하되 입력과 출력을 조금씩 다르게 하면 자동으로 지정됨

    private void Start()
    {
        Debug.Log(Sum(1, 1));
        Debug.Log(Sum(-5,8,10));

        Debug.Log(Sum(1.3f, 1.6f));
        Debug.Log(Sum(1.3f, 1.6f, 3.14f));

    }

    public int Sum(int a, int b)
    {
        return a + b;
    }

    public int Sum(int a, int b, int c)
    {
        return a + b + c;
    }

    public float Sum(float a, float b)
    {
        return a + b;
    }

    public float Sum(float a, float b, float c)
    {
        return a + b + c;
    }

}
