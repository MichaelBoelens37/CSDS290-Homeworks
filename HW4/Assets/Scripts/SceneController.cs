using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.U2D;
using System.Linq;

public class SceneController : MonoBehaviour {
	int gridRows = 2;
	int gridCols = 3;
    float cardScale = 1.0f;
    float offsetX = 3.0f;
	float offsetY = 2.5f;

	[SerializeField] private MemoryCard originalCard;
	[SerializeField] private Sprite[] images;
	[SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private TMP_Dropdown gridDropdown;
    [SerializeField] private GameObject poofEffectPrefab;
    [SerializeField] private GameObject gameOverImage;
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private GameObject endGameButton;


    private MemoryCard _firstRevealed;
	private MemoryCard _secondRevealed;
	private int _score = 0;

	public bool canReveal {
		get {return _secondRevealed == null;}
	}

	// Use this for initialization
	void Start() {
        var ddL = gridDropdown.GetComponent<TMP_Dropdown>();
        ddL.onValueChanged.AddListener(delegate { gridDropdownChanged(ddL); });
        ddL.value = 0;
        InitializeGrid();
        startGameButton.GetComponent<Button>().onClick.AddListener(OnStartGameButtonClick);
        endGameButton.GetComponent<Button>().onClick.AddListener(OnEndGameButtonClick);
    }

    void gridDropdownChanged(TMP_Dropdown change)
    {
        Debug.Log("DD index changed to: " + change.value.ToString());
        _score = 0;
        scoreLabel.text = "Score: " + _score;
        if (change.value.ToString().Equals("0"))
        {
            gridRows = 2;
            gridCols = 3;
            cardScale = 1.1f;
            offsetX = 3.0f;
            offsetY = 3.0f;
        } else if (change.value.ToString().Equals("1"))
        {
            gridRows = 2;
            gridCols = 4;
            cardScale = 1.0f;
            offsetX = 2.0f;
            offsetY = 3.0f;
        } else if (change.value.ToString().Equals("2"))
        {
            gridRows = 2;
            gridCols = 5;
            cardScale = 0.9f;
            offsetX = 1.5f;
            offsetY = 2.5f;
        } else if (change.value.ToString().Equals("3"))
        {
            gridRows = 3;
            gridCols = 4;
            cardScale = 0.75f;
            offsetX = 2f;
            offsetY = 1.7f;
        } else if (change.value.ToString().Equals("4"))
        {
            gridRows = 4;
            gridCols = 4;
            cardScale = 0.6f;
            offsetX = 2f;
            offsetY = 1.25f;
        }
        else if (change.value.ToString().Equals("5"))
        {
            gridRows = 4;
            gridCols = 5;
            cardScale = 0.6f;
            offsetX = 1.5f;
            offsetY = 1.25f;
        }
        InitializeGrid();
    }

    void InitializeGrid()
	{
        //clear Grid
        MemoryCard[] previousCards = GameObject.FindObjectsOfType<MemoryCard>();
        foreach (MemoryCard card in previousCards)
        {
            if (card.gameObject != originalCard.gameObject)
            {
                Destroy(card.gameObject);
            }
        }
        gameOverImage.SetActive(false);
        startGameButton.SetActive(false);
        endGameButton.SetActive(false);

        Vector3 startPos = originalCard.transform.position;

        // create random shuffled list of cards
        int totalCards = gridRows * gridCols;
        int[] numbers = new int[totalCards];
        int[] temp = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        temp = ShuffleArray(temp);
        for (int i = 0; i < totalCards / 2; i++)
        {
       
            numbers[i * 2] = temp[i];
            numbers[i * 2 + 1] = temp[i];
        }
        numbers = ShuffleArray(numbers);

        // place cards in a grid
        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                MemoryCard card;

                // use the original for the first grid space
                if (i == 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MemoryCard;
                }
                
                // next card in the list for each grid space
                int index = j * gridCols + i;
                int id = numbers[index];
                card.SetCard(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = -(offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, 0f);
                card.transform.localScale = new Vector3(cardScale, cardScale, 1.0f);
                card.Unreveal();

            }
        }
    }
	// Knuth shuffle algorithm
	private int[] ShuffleArray(int[] numbers) {
		int[] newArray = numbers.Clone() as int[];
		for (int i = 0; i < newArray.Length; i++ ) {
			int tmp = newArray[i];
			int r = Random.Range(i, newArray.Length);
			newArray[i] = newArray[r];
			newArray[r] = tmp;
		}
		return newArray;
	}

	public void CardRevealed(MemoryCard card) {
		if (_firstRevealed == null) {
			_firstRevealed = card;
		} else {
			_secondRevealed = card;
			StartCoroutine(CheckMatch());
		}
	}
	
	private IEnumerator CheckMatch() {

		// increment score if the cards match
		if (_firstRevealed.id == _secondRevealed.id) {
			_score++;
			scoreLabel.text = "Score: " + _score;
            
			//make them quiver
            _firstRevealed.transform.position = new Vector3(_firstRevealed.transform.position.x + 0.1f, _firstRevealed.transform.position.y, _firstRevealed.transform.position.z);
            _secondRevealed.transform.position = new Vector3(_secondRevealed.transform.position.x + 0.1f, _secondRevealed.transform.position.y, _secondRevealed.transform.position.z);
            yield return new WaitForSeconds(.05f);
            _firstRevealed.transform.position = new Vector3(_firstRevealed.transform.position.x - 0.2f, _firstRevealed.transform.position.y, _firstRevealed.transform.position.z);
            _secondRevealed.transform.position = new Vector3(_secondRevealed.transform.position.x - 0.2f, _secondRevealed.transform.position.y, _secondRevealed.transform.position.z);
            yield return new WaitForSeconds(.05f);
            _firstRevealed.transform.position = new Vector3(_firstRevealed.transform.position.x + 0.2f, _firstRevealed.transform.position.y, _firstRevealed.transform.position.z);
            _secondRevealed.transform.position = new Vector3(_secondRevealed.transform.position.x + 0.2f, _secondRevealed.transform.position.y, _secondRevealed.transform.position.z);
            yield return new WaitForSeconds(.05f);
            _firstRevealed.transform.position = new Vector3(_firstRevealed.transform.position.x - 0.2f, _firstRevealed.transform.position.y, _firstRevealed.transform.position.z);
            _secondRevealed.transform.position = new Vector3(_secondRevealed.transform.position.x - 0.2f, _secondRevealed.transform.position.y, _secondRevealed.transform.position.z);
            yield return new WaitForSeconds(.05f);
            _firstRevealed.transform.position = new Vector3(_firstRevealed.transform.position.x + 0.2f, _firstRevealed.transform.position.y, _firstRevealed.transform.position.z);
            _secondRevealed.transform.position = new Vector3(_secondRevealed.transform.position.x + 0.2f, _secondRevealed.transform.position.y, _secondRevealed.transform.position.z);
            yield return new WaitForSeconds(.05f);
            _firstRevealed.transform.position = new Vector3(_firstRevealed.transform.position.x - 0.2f, _firstRevealed.transform.position.y, _firstRevealed.transform.position.z);
            _secondRevealed.transform.position = new Vector3(_secondRevealed.transform.position.x - 0.2f, _secondRevealed.transform.position.y, _secondRevealed.transform.position.z);
            yield return new WaitForSeconds(.05f);
            _firstRevealed.transform.position = new Vector3(_firstRevealed.transform.position.x + 0.2f, _firstRevealed.transform.position.y, _firstRevealed.transform.position.z);
            _secondRevealed.transform.position = new Vector3(_secondRevealed.transform.position.x + 0.2f, _secondRevealed.transform.position.y, _secondRevealed.transform.position.z);
            yield return new WaitForSeconds(.05f);
            _firstRevealed.transform.position = new Vector3(_firstRevealed.transform.position.x - 0.2f, _firstRevealed.transform.position.y, _firstRevealed.transform.position.z);
            _secondRevealed.transform.position = new Vector3(_secondRevealed.transform.position.x - 0.2f, _secondRevealed.transform.position.y, _secondRevealed.transform.position.z);
            yield return new WaitForSeconds(.05f);
            _firstRevealed.transform.position = new Vector3(_firstRevealed.transform.position.x + 0.2f, _firstRevealed.transform.position.y, _firstRevealed.transform.position.z);
            _secondRevealed.transform.position = new Vector3(_secondRevealed.transform.position.x + 0.2f, _secondRevealed.transform.position.y, _secondRevealed.transform.position.z);
            yield return new WaitForSeconds(.05f);
            _firstRevealed.transform.position = new Vector3(_firstRevealed.transform.position.x - 0.1f, _firstRevealed.transform.position.y, _firstRevealed.transform.position.z);
            _secondRevealed.transform.position = new Vector3(_secondRevealed.transform.position.x - 0.1f, _secondRevealed.transform.position.y, _secondRevealed.transform.position.z);
            yield return new WaitForSeconds(.3f);


            // Make poof effect
            Instantiate(poofEffectPrefab, _firstRevealed.transform.position, Quaternion.identity);
            Instantiate(poofEffectPrefab, _secondRevealed.transform.position, Quaternion.identity);

            // Move cards behind  scene
            _firstRevealed.transform.position = new Vector3(_firstRevealed.transform.position.x, _firstRevealed.transform.position.y, 10f);
            _secondRevealed.transform.position = new Vector3(_secondRevealed.transform.position.x, _secondRevealed.transform.position.y, 10f);

            // Add a short delay before resetting the card positions
            yield return new WaitForSeconds(0.25f);

            if (_score >= (gridCols * gridRows)/2)
            {
                gameOverImage.SetActive(true); 
                yield return new WaitForSeconds(3.5f);
                gameOverImage.SetActive(false);
                startGameButton.SetActive(true);
                endGameButton.SetActive(true);
            }

            
        }

		// otherwise turn them back over after .5s pause
		else {
			yield return new WaitForSeconds(.5f);

			_firstRevealed.Unreveal();
			_secondRevealed.Unreveal();
		}
		
		_firstRevealed = null;
		_secondRevealed = null;
	}

    public void OnEndGameButtonClick()
    {
        ExitGame();
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void OnStartGameButtonClick()
    {
        Restart();
    }
    public void Restart() {

		//Application.LoadLevel("Scene"); /* obsolete since Unity 2017 */

		SceneManager.LoadScene("Scene");
	}
}
