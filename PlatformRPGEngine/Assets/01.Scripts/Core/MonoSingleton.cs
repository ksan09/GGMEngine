using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    private static bool IsQuitting = false;

    public static T Instance
    {
        get
        {
            // ��Ȱ��ȭ �ƴٸ� ������ ����ΰ� ���� �����.
            if (IsQuitting)
            {
                _instance = null;
            }

            // instance�� NULL�϶� ���� �����Ѵ�.
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();

                if (_instance == null)
                {
                    Debug.LogError($"{typeof(T).Name} is not exits");
                }
                else
                {
                    IsQuitting = false; //���� �뵵��.
                }

            }
            return _instance;

        }
    }

    private void OnDisable()
    {
        // ��Ȱ��ȭ �ȴٸ� null�� ����
        IsQuitting = true;
        _instance = null;
    }
}