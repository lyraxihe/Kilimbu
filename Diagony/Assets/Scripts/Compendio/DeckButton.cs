using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckButton : MonoBehaviour
{

    [SerializeField] GameObject ScrollAreaCompendio;
    [SerializeField] Sprite VolverIcon;
    [SerializeField] Sprite VolverIconHighlight;
    [SerializeField] Sprite VolverIconPressed;
    [SerializeField] Sprite CartasIcon;
    [SerializeField] Sprite CartasIconHighlight;
    [SerializeField] Sprite CartasIconPressed;
    [SerializeField] Sprite CartasIconDisabled;
    [SerializeField] GameObject Map;
    [SerializeField] GameObject PauseButton;
    private SpriteState deckButtonState;
    private Button button;
    //[SerializeField] TMP_Text ButtonText;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        deckButtonState = button.spriteState;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {

        if (!ScrollAreaCompendio.activeSelf)
        {
            //ButtonText.text = "Volver";
            gameObject.GetComponent<Image>().sprite = VolverIcon;
            deckButtonState.pressedSprite = VolverIconPressed;
            deckButtonState.highlightedSprite = VolverIconHighlight;
            button.spriteState = deckButtonState;

            Map.SetActive(false);
            PauseButton.SetActive(false);
        }
        else
        {
            //ButtonText.text = "Mis Cartas";
            gameObject.GetComponent<Image>().sprite = CartasIcon;
            deckButtonState.disabledSprite = CartasIconDisabled;
            deckButtonState.pressedSprite = CartasIconPressed;
            deckButtonState.highlightedSprite = CartasIconHighlight;
            button.spriteState = deckButtonState;
            Map.SetActive(true);
            PauseButton.SetActive(true);
        }

        ScrollAreaCompendio.SetActive(!ScrollAreaCompendio.activeSelf);


    }

}
