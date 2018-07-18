using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StroopTestController : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] internal bool Translate;
    [SerializeField] [Range(1, 3)] private int _section;
    [SerializeField] private int _promptsPerSection = 40;

    [Space(5)]
    [Header("UI")]
    [SerializeField] private Text _prompt;
    [SerializeField] private Text[] _buttons;

    private int _answeredPrompts;
    private Prompt[] _prompts;

    private Animator _promptAnimator;
    private int _incorrectAnimationHash;

    internal readonly string[] AvailableColors = 
    {
        "BLUE",
        "BROWN",
        "RED",
        "PURPLE",
        "GREY",
        "YELLOW",
        "FUCHSIA",
        "BLACK",
        "ORANGE",
        "GREEN",
    };

    private void Start()
    {
        _promptAnimator = _prompt.GetComponent<Animator>();
        _incorrectAnimationHash = Animator.StringToHash("Incorrect");

        InitializePrompts();
        LoadNewPrompt();
    }

    private void LoadNewPrompt()
    {
        _prompt.text = _prompts[_answeredPrompts].Text;

        string stringToParse = Translate 
            ? ToEnglish[_prompts[_answeredPrompts].TextColor.ToUpperInvariant()].ToLowerInvariant() 
            : _prompts[_answeredPrompts].TextColor;


        Color newColor = Color.black;

        // Hacky way to make a special case for bad colors 
        switch (stringToParse)
        {
            case "brown":
                ColorUtility.TryParseHtmlString("#7E481C", out newColor);
                break;
            case "fuchsia":
                ColorUtility.TryParseHtmlString("#FF6FFF", out newColor);
                break;
            default:
                ColorUtility.TryParseHtmlString(stringToParse, out newColor);
                break;
        }

        _prompt.color = newColor;

        int correctButton = Random.Range(0, _buttons.Length);
        string[] buttonColors = new string[_buttons.Length];

        _buttons[correctButton].text = _section == 1 ? _prompts[_answeredPrompts].Text : _prompts[_answeredPrompts].TextColor.ToUpperInvariant();
        buttonColors[correctButton] = _section == 1 ? _prompts[_answeredPrompts].Text : _prompts[_answeredPrompts].TextColor.ToUpperInvariant();

        for (int i = 0; i < _buttons.Length; i++)
        {
            if (i == correctButton) continue;
            string buttonColor;

            do
            {
                buttonColor = AvailableColors[Random.Range(0, AvailableColors.Length)];

                if (Translate)
                {
                    buttonColor = ToMalay[buttonColor];
                }
            }
            while (buttonColors.Contains(buttonColor));

            buttonColors[i] = buttonColor;
            _buttons[i].text = buttonColor;
        }
        
    }

    private void InitializePrompts()
    {
        switch (_section)
        {
            case 1:
                _prompts = GetRandomPrompts(ColorSetting.Black);
                break;
            case 2:
                _prompts = GetRandomPrompts(ColorSetting.Same);
                break;
            case 3:
                _prompts = GetRandomPrompts(ColorSetting.Random);
                break;
        }
    }

    public void CheckAnswer(Text text)
    {
        bool success = false;

        switch (_section)
        {
            case 1:
                success = text.text == _prompts[_answeredPrompts].Text;
                break;

            case 2:
            case 3:
                success = text.text == _prompts[_answeredPrompts].TextColor.ToUpperInvariant();
                break;
        }

        if (!success)
        {
            _promptAnimator.SetTrigger(_incorrectAnimationHash);
            return;
        }

        _answeredPrompts++;

        if (_answeredPrompts == _promptsPerSection)
        {
            SessionController.Instance.StopCountingTime();
            SceneController.Instance.LoadNextScene();
            return;
        }

        LoadNewPrompt();
    }


    private Prompt[] GetRandomPrompts(ColorSetting colorSetting)
    {
        Prompt[] prompts = new Prompt[_promptsPerSection];

        for (int i = 0; i < prompts.Length; i++)
        {
            string randomColor;

            do
            {
                randomColor = AvailableColors[Random.Range(0, AvailableColors.Length)];

                if (Translate)
                {
                    randomColor = ToMalay[randomColor];
                }
            }
            while(i > 0 && prompts[i -1].Text == randomColor);

            prompts[i] = new Prompt(randomColor, colorSetting, this);
        }

        return prompts;
    }

    internal readonly Dictionary<string, string> ToMalay = new Dictionary<string, string>()
    {
        { "BLUE", "BIRU" },
        { "BROWN", "COKELAT" },
        { "RED", "MERAH" },
        { "PURPLE", "UNGU" },
        { "GREY", "KELABU" },
        { "YELLOW", "KUNING" },
        { "FUCHSIA", "MERAH JAMBU" },
        { "BLACK", "HITAM" },
        { "ORANGE", "OREN" },
        { "GREEN", "HIJAU" }
    };

    internal readonly Dictionary<string, string> ToEnglish = new Dictionary<string, string>()
    {
        { "BIRU", "BLUE" },
        { "COKELAT", "BROWN" },
        { "MERAH", "RED" },
        { "UNGU", "PURPLE" },
        { "KELABU", "GREY" },
        { "KUNING", "YELLOW" },
        { "MERAH JAMBU", "FUCHSIA" },
        { "HITAM", "BLACK" },
        { "OREN", "ORANGE" },
        { "HIJAU", "GREEN" }
    };
}

public class Prompt
{
    public readonly string Text;        // uppercase, examples: "YELLOW", "BLACK"
    public readonly string TextColor;   // lowercase, examples: "yellow", "black"
    public readonly ColorSetting ColorSetting;

    public Prompt(string text, ColorSetting colorSetting, StroopTestController controller)
    {
        Text = text;
        ColorSetting = colorSetting;

        switch (ColorSetting)
        {
            case ColorSetting.Black:
                TextColor = controller.Translate ? "hitam" : "black";
                break;

            case ColorSetting.Same:
                TextColor = text.ToLowerInvariant();
                break;

            case ColorSetting.Random:
                string randomColor;
                do
                {
                    randomColor = controller.AvailableColors[Random.Range(0, controller.AvailableColors.Length)];

                    if (controller.Translate)
                    {
                        randomColor = controller.ToMalay[randomColor];
                    }
                }
                while (randomColor == text);
                TextColor = randomColor.ToLowerInvariant();
                break;
        }
    }
}

public enum ColorSetting
{
    None,
    Black,
    Same,
    Random
}
