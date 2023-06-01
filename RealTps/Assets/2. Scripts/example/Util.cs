using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    private void Start()
    {
        Container<string> container = new Container<string>();
        container.messages = new string[3];

        container.messages[0] = "Hello";
        container.messages[1] = "Wolrd";
        container.messages[3] = "Generic";

        for(int i = 0; i < container.messages.Length; i++)
        {
            Debug.Log(container.messages[i]);
        }


        Container<int> container2 = new Container<int>();
        container2.messages = new int[3];

        container2.messages[0] = 0;
        container2.messages[1] = 10;
        container2.messages[3] = 100;

        for (int i = 0; i < container.messages.Length; i++)
        {
            Debug.Log(container.messages[i]);
        }

    }


    // Start is called before the first frame update
    /*void Start()
    {
        Print<int>(30);
        Print<string>("Hello World");

    }

    //public void PrintInt(int inputMessage)
    //{
    //    Debug.Log(inputMessage);
    //}

    //public void PrintString(string inputMessage)
    //{
    //    Debug.Log(inputMessage);
    //}

    //�����ε� �� ����ϴ� ��� �Ʒ�ó�� �� �� �ִ�.


    public void Print<T>(T inputMessage)
    {
        Debug.Log(inputMessage);
    }*/
}

//Ŭ���������� ��� ����
public class Container<T>
{
    public T[] messages;
}