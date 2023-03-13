using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class SceneController : MonoBehaviour
{
    public const int gridRows = 3;
    public const int gridCols = 4;
    public const float offsetX = 2f;
    public const float offsetY = 2.5f;

    [SerializeField] MemoryCard originalCard;
    [SerializeField] Sprite[] images;
    [SerializeField] TMP_Text scoreLabel;
    [SerializeField] TMP_Text turnLabel;

    private MemoryCard firstRevealed;
    private MemoryCard secondRevealed;
    private int score = 0;
    private int turns = 0;

    public bool canReveal
    {
        get { return secondRevealed == null; }
    }


    // Start is called before the first frame update
    void Start()
    {

        Vector3 startPos = originalCard.transform.position;

        int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5};

        for (int i = 0; i < gridCols; i++)
        {
            for(int j = 0; j < gridRows; j++)
            {
                MemoryCard card;
                if( i ==0 && j ==0)
                {
                    card = originalCard;
                }else
                {
                    card = Instantiate(originalCard) as MemoryCard;
                }


                int index = j * gridCols + i;
                int id = numbers[index];
                card.SetCard(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = -(offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
            }
        }
            
    }

    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }

    public void CardRevealed(MemoryCard card)
    {
        if (firstRevealed == null)
        {
            firstRevealed = card;
        }
        else
        {
            secondRevealed = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {

        if (firstRevealed.Id == secondRevealed.Id)
        {
            score = score + 50;
            scoreLabel.text = $"Score: {score}";
            turns++;
            turnLabel.text = $"Turns: {turns}";
        }

        else
        {
            yield return new WaitForSeconds(.5f);

            firstRevealed.Unreveal();
            secondRevealed.Unreveal();
        }

        firstRevealed = null;
        secondRevealed = null;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
