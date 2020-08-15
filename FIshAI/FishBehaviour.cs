using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    public enum FishState {Swimming, Idle, Hungry, Eating};

    public float aquariumLimitX;

    public float aquariumLimitY;

    public FishState fishState;

    private Animator anim;

    private SpriteRenderer spriteRenderer;

    public Vector3 targetPosition;

    public float baseSwimmingSpeed;

    private float swimmingSpeed;

    public bool isMovingRight;

    public float secondsToBlink;

    private AquariumController aquariumController;

    private FoodContainer foodTarget;

    public float eatingTime;

    private float elapsedBlinkTime;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        aquariumController = GameObject.FindGameObjectWithTag("AquariumController").GetComponent<AquariumController>();

        //Sort starting looking direction
        if (Random.Range(0, 2) == 0)
        {
            isMovingRight = true;
        }
        else
        {
            isMovingRight = false;
        }

        ChangeState(FishState.Swimming);

        swimmingSpeed = baseSwimmingSpeed * Random.Range(0.7f, 1.3f);

        secondsToBlink = Random.Range(10f, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        MainBehaviour();
    }

    private void MainBehaviour()
    {
        if(fishState == FishState.Swimming || fishState == FishState.Idle)
        {
            if (aquariumController.foodList.Count > 0)
            {
                ChangeState(FishState.Hungry);
            }
        }
        if (fishState == FishState.Swimming || fishState == FishState.Hungry)
        {
            elapsedBlinkTime += Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, swimmingSpeed * Time.deltaTime);

            //As the fish reachs its destinantion
            if (Vector3.Distance(transform.position, targetPosition) < 1)
            {
                //If its just swimming, randomize a new position to swim to
                if (fishState == FishState.Swimming)
                {
                    targetPosition = SortPosition();

                    isMovingRight = !isMovingRight;

                    targetPosition = SortPosition(isMovingRight);

                    LookAt(targetPosition);
                }
                //If its searching for food, start Eating
                else if (fishState == FishState.Hungry)
                {
                    ChangeState(FishState.Eating);
                }
                //If its already eating, searchs for the nex food location
                else if (fishState == FishState.Eating)
                {
                    GetClosestFoodLocation();
                }
            }

            //Timer for the Idle state of the fish
            if (fishState == FishState.Swimming && elapsedBlinkTime > secondsToBlink)
            {
                secondsToBlink = Random.Range(7f, 13f);
                ChangeState(FishState.Idle);
            }
        }
    }

    //Rotates the fish to look at a certain position
    private void LookAt(Vector3 position)
    {
        transform.right = position - transform.position;

        spriteRenderer.flipY = transform.position.x > position.x;
    }

    //Sort a random position inside the aquarium limits
    private Vector3 SortPosition()
    {
        Vector3 newPos;

        do
        {
            newPos = new Vector3(Random.Range(-aquariumLimitX, aquariumLimitX), Random.Range(-aquariumLimitY, aquariumLimitY), 0);

        } while (Vector3.Distance(transform.position, newPos) < 3);

        return newPos;
    }

    //Sort a random position inside the aquarium limits
    private Vector3 SortPosition(bool moveRight)
    {
        Vector3 newPos;

        if(moveRight)
        {
            newPos = new Vector3(aquariumLimitX, -2 + Random.Range(-aquariumLimitY, aquariumLimitY), 0);
        }
        else
        {
            newPos = new Vector3(-aquariumLimitX, -2 + Random.Range(-aquariumLimitY, aquariumLimitY), 0);
        }

        spriteRenderer.sortingOrder = Random.Range(-1, 2);

        return newPos;
    }

    private void ChangeState(FishState newState)
    {
        fishState = newState;

        switch (fishState)
        {
            case FishState.Swimming:
                targetPosition = SortPosition(isMovingRight); 
                LookAt(targetPosition);

                elapsedBlinkTime = 0;
                anim.SetBool("Eating", false);
                break;

            case FishState.Hungry:
                GetClosestFoodLocation();
                break;

            case FishState.Eating:
                anim.SetBool("Eating", true);
                StartCoroutine(EatingBehaviour());
                break;

            case FishState.Idle:
                anim.SetTrigger("Blink");
                break;
        }
    }

    //Searchs for the nearest food 
    private void GetClosestFoodLocation()
    {
        float distance = 100;

        if(aquariumController.foodList.Count == 0)
        {
            ChangeState(FishState.Swimming);
            return;
        }

        for (int i = 0; i < aquariumController.foodList.Count; i++)
        {
            float newDistance = Vector3.Distance(aquariumController.foodList[i].transform.position, transform.position);

            if(newDistance < distance)
            {
                foodTarget = aquariumController.foodList[i];
            }
        }

        targetPosition = foodTarget.transform.position;

        if(fishState != FishState.Eating)
        {
            LookAt(targetPosition);
        }
    }

    public void StoppedBlinkng()
    {
        ChangeState(FishState.Swimming);
    }

    private IEnumerator EatingBehaviour()
    {
        anim.SetBool("Eating", true);

        yield return new WaitForSeconds(eatingTime);

        if(foodTarget != null && foodTarget.RemoveRandomGrain())
        {
            ChangeState(FishState.Eating);
        }
        else
        {
            ChangeState(FishState.Swimming);
        }   
    }

    //If the fishe collides with any food, changes it's state to "Eating"
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "FishFood")
        {
            ChangeState(FishState.Eating);
        }
    }

    //If the food exists the fish collider, it will start searching for it again
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "FishFood")
        {
            ChangeState(FishState.Hungry);
        }
    }
}