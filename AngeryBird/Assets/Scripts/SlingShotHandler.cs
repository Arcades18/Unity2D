using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class SlingShotHandler : MonoBehaviour
{
    [Header("LineRender")]
    [SerializeField] private LineRenderer leftLineRenderer;
    [SerializeField] private LineRenderer rightLineRenderer;

    [Header("Transform")] 
    [SerializeField] private Transform leftStart;
    [SerializeField] private Transform rightStart;
    [SerializeField] private Transform centerpos;
    [SerializeField] private Transform idlePos;
    [SerializeField] private Transform elasticTransform;

    [Header("AngieeBird")]
    [SerializeField] private Angry angieBird;
    [SerializeField] private float angieeBirdOffset = 2f;

    [Header("SlingShotStats")]
    [SerializeField] private float maxDistance = 3.5f;
    [SerializeField] private float shotForce = 5f;
    [SerializeField] private float timeForNewBirdToSpawn = 2f;
    [SerializeField] private float ElasticDivider = 1.2f;
    [SerializeField] private AnimationCurve elasticCurve;

    [Header("Scripts")]
    [SerializeField] private SlingShotArea slingShotArea;
    [SerializeField] private CameraManager cameraManager;
    
    [Header("Sounds")]
    [SerializeField] private AudioClip elasticClip;
    [SerializeField] private AudioClip[] elasticReleasedClips;

    private AudioSource audioSource;
    
    private Vector2 slingShotPosition;
    private bool clickWithinArea;
    
    private Angry spawnedAngieeBird;
    
    private Vector2 direction;
    private Vector2 directionNormalize;

    private bool birdIsOnSlingShot;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        leftLineRenderer.enabled = false;
        rightLineRenderer.enabled = false;
        SpawnAndieeBird();
    }
    private void Update()
    {
        if(InputSystem.wasLeftButtonWasPressed && slingShotArea.IsWithSlingShotArea())
        {
            clickWithinArea = true;

            if (birdIsOnSlingShot)
            {
                SoundManager.instance.PlayClip(elasticClip , audioSource);
                cameraManager.SwitchToFollowCam(spawnedAngieeBird.transform);
            }
        }

        if (InputSystem.wasLeftButtonIsPressed && clickWithinArea && birdIsOnSlingShot)
        {
            DrawLines();
            positionAndRotation();
        }

        if (InputSystem.wasLeftButtonWasReleased && birdIsOnSlingShot)
        {
            if (GameManager.instance.hasEnoughShot())
            {

                clickWithinArea = false;
                spawnedAngieeBird.LauchBird(direction, shotForce);
                birdIsOnSlingShot = false;

                SoundManager.instance.PlayRandamClip(elasticReleasedClips , audioSource);

                GameManager.instance.usedShot();

                AnimateSlingShot();
                if (GameManager.instance.hasEnoughShot())
                {
                    StartCoroutine(spawnAngryBirdAfterTime());
                }
            }

        }
    }
    #region slingShot Function
    private void DrawLines()
    {
        

        Vector3 touchposition = Camera.main.ScreenToWorldPoint(InputSystem.mousePosition);

        slingShotPosition = centerpos.position + Vector3.ClampMagnitude(touchposition - centerpos.position, maxDistance);

        SetLines(slingShotPosition);

        direction = (Vector2)centerpos.position - slingShotPosition;
        directionNormalize = direction.normalized;
    }

    private void SetLines(Vector2 position)
    {
        if (!leftLineRenderer.enabled && !rightLineRenderer.enabled)
        {
            leftLineRenderer.enabled = true;
            rightLineRenderer.enabled = true;
        }
        leftLineRenderer.SetPosition(0, position);
        leftLineRenderer.SetPosition(1, leftStart.position);

        rightLineRenderer.SetPosition(0, position);
        rightLineRenderer.SetPosition(1 , rightStart.position);
    }
    #endregion

    #region Angiee Bird

    private void SpawnAndieeBird()
    {
        elasticTransform.DOComplete();
        SetLines(idlePos.position);
        Vector2 dir = (centerpos.position - idlePos.position).normalized;
        Vector2 spawnedPosition = (Vector2)idlePos.position + dir * angieeBirdOffset;

        spawnedAngieeBird = Instantiate(angieBird, spawnedPosition, Quaternion.identity);
        spawnedAngieeBird.transform.right = dir;

        birdIsOnSlingShot = true;
    }

    private void positionAndRotation()
    {
        spawnedAngieeBird.transform.position = slingShotPosition + directionNormalize * angieeBirdOffset;
        spawnedAngieeBird.transform.right = directionNormalize;
    }

    private IEnumerator spawnAngryBirdAfterTime()
    {
        yield return new WaitForSeconds(timeForNewBirdToSpawn);

        cameraManager.SwitchToIdleCam();

        SpawnAndieeBird();

    }

    #endregion

    #region Animate SlingShot

    private void AnimateSlingShot()
    {
        elasticTransform.position = leftLineRenderer.GetPosition(0);

        float Dist = Vector2.Distance(elasticTransform.position,centerpos.position);

        float time = Dist / ElasticDivider;

        elasticTransform.DOMove(centerpos.position,time).SetEase(elasticCurve);

        StartCoroutine(AnimateSlingShotLines(elasticTransform,time));
    }

    private IEnumerator AnimateSlingShotLines(Transform trans , float time)
    {
        float elapsedTime = 0f;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            SetLines(trans.position);

            yield return null;
        }
    }

    #endregion
}

