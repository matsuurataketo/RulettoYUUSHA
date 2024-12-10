using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameEfect : MonoBehaviour
{

    public GameObject particlePrefab; // �p�[�e�B�N���̃v���n�u�������ɐݒ肷��
    public Transform spawnPoint;     // �p�[�e�B�N���𐶐�����ʒu�i�C�Ӂj
    // Start is called before the first frame update
    public void PlayParticle()
    {
        Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f); // x����-90�x�ɉ�]
        GameObject particleInstance = Instantiate(particlePrefab, spawnPoint.position, rotation);

        // 2�b��Ƀp�[�e�B�N�����폜
        Destroy(particleInstance, 2f);
    }
}
