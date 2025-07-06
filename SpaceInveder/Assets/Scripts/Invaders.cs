using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Invaders : MonoBehaviour
{
    public Invader[] prefab;
    public int rows = 5;
    public int columns = 10;
    public AnimationCurve speed;
    public Projectile missilePrefab;
    public float missileRate = 1.0f; 
    private Vector3 _direction = Vector3.right;
    public int amountKilled {get; private set;}
    public int amountAlive => this.totalKilled - this.amountKilled; 
    public int totalKilled => this.rows * this.columns;
    public float percentKilled => (float)this.amountKilled/(float)this.totalKilled;
    private void Awake()
    {
        for(int row = 0; row < this.rows; row++)
        {
            float width = 2.0f * (this.columns - 1);
            float heigth = 2.0f * (this.rows - 1);
            Vector2 centering = new Vector2(-width / 2, -heigth / 2);
            Vector3 rowPosition = new Vector3(centering.x,centering.y + (row * 2.0f), 0.0f);
            for(int cols = 0;cols < this.columns; cols++)
            {
               Invader invader = Instantiate(prefab[row], this.transform);
                invader.killed += invaderKilled;
                Vector3 position = rowPosition;
                position.x += cols * 2.0f; 
               invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(missileAttack),this.missileRate,this.missileRate);
    }

    private void Update()
    {
        this.transform.position += _direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach(Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }
            if (_direction == Vector3.right && invader.position.x >= (rightEdge.x - 1))
            {
                AdvanceRow();
            }
            else if(_direction == Vector3.left && invader.position.x <= (leftEdge.x + 1))
            {
                AdvanceRow();
            }
        }
    }

    private void AdvanceRow()
    {
        _direction.x *= -1.0f;
        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    private void missileAttack()
    {
        foreach(Transform invader in transform)
        {
            if(!invader.gameObject.activeInHierarchy)
            {
                continue;
            }
            if(Random.value < (1.0f / (float)amountAlive))
            {
                Instantiate(this.missilePrefab,invader.position,Quaternion.identity);
                break;
            }
        }
    }

    private void invaderKilled()
    {
        this.amountKilled++;

        if(this.amountKilled >= this.totalKilled)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
