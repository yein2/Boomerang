using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    MeshRenderer meshTest;

    void Update()
    {
      if(Input.GetKeyDown(KeyCode.A))
      {
          StartCoroutine(DissolveShield());
      }
    }

    IEnumerator DissolveShield()    // 쉴드가 점점 사라지는 효과
    {
        float amount = -1f;
        while(amount <= 1)
        {
            amount += 0.1f;
            meshTest.material.SetFloat("Vector1_40E1A2BF", amount);
            //Debug.Log(meshTest.material.GetFloat("Vector1_40E1A2BF"));
            yield return new WaitForSeconds(0.05f); 
        }
        // 후처리
        yield break;
    }
}
