using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public LayerMask whatIsProp;
    public ParticleSystem explosionParticle;

    public AudioSource explosionAudio;
    public float maxDamage = 100f;          //���� ������
    public float explosionForce = 1000f;    //���߷� �о�� ��
    public float lifeTime = 10f;
    public float explosionsRadius = 20f;    //���� ������

    private void Start() {
        Destroy(gameObject,lifeTime);
    }

    private void OnTriggerEnter(Collider other) {
        //���� �߽ɰ� �������� �ָ� �ش� ��ġ�� ������ ��� �ݶ��̴��� �迭�� ��ȯ,
        //whatIsProp�� ���� ���� ������ �ش� layer�� ��ȯ
        Collider[] colliders=Physics.OverlapSphere(transform.position, explosionsRadius, whatIsProp);

        for(int i = 0; i < colliders.Length; i++) {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionsRadius);
            Prop targetProp = colliders[i].GetComponent<Prop>();
            float damage = CalculaterDamage(colliders[i].transform.position);

            targetProp.TakeDamage(damage);
        }
        explosionParticle.transform.parent = null;
        explosionParticle.Play();
        explosionAudio.Play();        
        Destroy(explosionParticle.gameObject, explosionParticle.duration);
        Destroy(gameObject);
    }

    //�� �ۿ��� ������ �Ÿ� ���ϴ� ����- �־������� �������� ���� ���� ����->
    //�߽ɿ����� �Ÿ��� 3�̶�� 0.7�� �������� ��->
    //�����ڸ������� �Ÿ��� 3�̶�� 0.3���� ����.
    private float CalculaterDamage(Vector3 targetPosition) {
        //������ prop������ �Ÿ�->x
        Vector3 explosionToTarget = targetPosition - transform.position;
        //������ �Ÿ��� ��ȯ�ϴ� manitude ���.
        float distance = explosionToTarget.magnitude;
        //�� ������ - x 
        float edgeToCenterDistance =explosionsRadius-distance;
        float percentage = edgeToCenterDistance / explosionsRadius;
        float damage = maxDamage * percentage;
        damage = Mathf.Max(0, damage);
        return damage;
    }
    private void OnDestroy() {
        GameManager.Instance.OnBallDestroy();
    }
}
