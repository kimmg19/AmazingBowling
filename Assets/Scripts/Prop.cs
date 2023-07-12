using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public int score = 5;
    public ParticleSystem explosionParticle;
    public float hp = 10f;

    public void TakeDamage(float damage) {
        hp -= damage;

        if (hp <= 0) {
            ParticleSystem instance =Instantiate(explosionParticle,transform.position,transform.rotation);
            AudioSource explosionAudio = instance.GetComponent<AudioSource>();
            explosionAudio.Play();
            GameManager.Instance.AddScore(score);
            Destroy(instance.gameObject,instance.duration);
            gameObject.SetActive(false);    //Prop 이 굉장히 많기 때문에 파괴 안하고 비활성만.
          
        }
    }
}
