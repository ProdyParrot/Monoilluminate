using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBehaviour : MonoBehaviour
{
    public GameLogic gameRef;
    public GameObject lightParticle;
    public GameObject lightParticleBig;
    public bool executing;
    public float timer;
    public float progress;
    public float timer2;
    public int directionX = 1;
    public int directionY = 0;
    public float speed = 0.1f;
    public int directionStep;
    public bool headingToOOB;
    public bool intenseLight;

    public PanelBehaviour currentPanel;
    public PanelBehaviour nextPanel;

    // Start is called before the first frame update
    void Start()
    {
        gameRef = GameObject.Find("GameEngine").GetComponent<GameLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (headingToOOB)
        {
            timer2 += Time.deltaTime;
            if (timer2 > 0.1625f)
            {
                QuotaFunction();
            }
        }
        if (executing)
        {
            progress += Time.deltaTime;
            if (progress > 0.5f)
            {
                // Execute panel piece logic, if any
                if (nextPanel)
                {
                    ParsePanel(nextPanel.currPiece);
                }
                else
                {
                    QuotaFunction();
                    return;
                }
                currentPanel = nextPanel;
                ProcessDirection();
                if (nextPanel == null)
                {
                    headingToOOB = true;
                }
                progress = 0.0f;
            }

            timer += Time.deltaTime;

            

            // Move
            float lightVelocity = speed * Time.deltaTime;
            Vector3 currPos = transform.localPosition;
            transform.localPosition += new Vector3(directionX * lightVelocity, directionY * lightVelocity, 0.0f);
        }
    }

    private void FixedUpdate()
    {
        if (executing)
        {
            if (!intenseLight)
            {
                gameRef.lightParticles.Add(Instantiate(lightParticle, transform.position, Quaternion.identity, transform.parent));
            }
            else
            {
                gameRef.lightParticles.Add(Instantiate(lightParticleBig, transform.position, Quaternion.identity, transform.parent));
            }
        }
    }

    public void ParsePanel(int id = 0)
    {
        switch (id)
        {
            case 0:
                // Nothing on the piece
                break;
            case 1:
                // Going into another arrow
                QuotaFunction();
                break;
            case 2:
                // Going into clockwise gear
                gameRef.PlaySound(2);
                ClockwiseDirection();
                break;
            case 3:
                // Going into anticlockwise gear
                gameRef.PlaySound(2);
                AntiClockwiseDirection();
                break;
            case 4:
                // Going into magnifier
                intenseLight = true;
                break;
            case 5:
                // Going into block
                if (!intenseLight)
                {
                    QuotaFunction();
                }
                else
                {
                    intenseLight = false;
                }
                break;
            case 6:
                // Going into goal
                gameRef.goalQuota++;
                QuotaFunction();
                break;
        }
    }

    public void QuotaFunction()
    {
        gameRef.CheckQuota();
        Destroy(gameObject);
    }

    public void ClockwiseDirection()
    {
        directionStep++;
        if (directionStep > 3)
        {
            directionStep = 0;
        }
    }

    public void AntiClockwiseDirection()
    {
        directionStep--;
        if (directionStep < 0)
        {
            directionStep = 3;
        }
    }

    public void ProcessDirection()
    {
        switch (directionStep)
        {
            case 0:
                directionX = 1; // Right
                directionY = 0;
                nextPanel = currentPanel.rightPanel;
                break;
            case 1:
                directionX = 0; // Down
                directionY = -1;
                nextPanel = currentPanel.downPanel;
                break;
            case 2:
                directionX = -1; // Left
                directionY = 0;
                nextPanel = currentPanel.leftPanel;
                break;
            case 3:
                directionX = 0; // Up
                directionY = 1;
                nextPanel = currentPanel.upPanel;
                break;
        }
    }
}
