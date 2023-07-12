using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public LayerMask whatIsProp;
    public ParticleSystem explosionParticle;

    public AudioSource explosionAudio;
    public float maxDamage = 100f;          //폭발 데미지
    public float explosionForce = 1000f;    //폭발로 밀어내는 힘
    public float lifeTime = 10f;
    public float explosionsRadius = 20f;    //폭발 반지름

    private void Start() {
        Destroy(gameObject,lifeTime);
    }

    private void OnTriggerEnter(Collider other) {
        //구의 중심과 반지름을 주면 해당 위치에 들어오는 모든 콜라이더를 배열로 반환,
        //whatIsProp과 같은 조건 들어오면 해당 layer만 반환
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

    //원 밖에서 안으로 거리 구하는 이유- 멀어질수록 데미지가 적게 들어가기 때문->
    //중심에서의 거리가 3이라면 0.7의 데미지가 들어감->
    //가장자리에서의 거리가 3이라면 0.3으로 계산됨.
    private float CalculaterDamage(Vector3 targetPosition) {
        //원에서 prop까지의 거리->x
        Vector3 explosionToTarget = targetPosition - transform.position;
        //벡터의 거리를 반환하는 manitude 기능.
        float distance = explosionToTarget.magnitude;
        //원 반지름 - x 
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
