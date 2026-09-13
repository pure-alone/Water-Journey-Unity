using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class DewyApp : MonoBehaviour
{
    private const float GUIDED_LEARNING_Y = 500f;

    public enum Page { Home = 0, Scene1 = 1, Scene2 = 2, Scene3 = 3, Scene4 = 4, Scene5 = 5, Scene6 = 6, Credits = 7 }

    private Canvas canvas;
    private RectTransform currentRoot;
    private Button nextButton;
    private Button soundButton;
    private Text soundGlyph;
    private GameObject learningCard;
    private Text toastText;
    private Coroutine toastRoutine;
    private bool sceneComplete;

    public DewyAudio Audio { get; private set; }
    public Page CurrentPage { get; private set; }
    public RectTransform Stage { get; private set; }
    public float ScaleFactor => canvas == null ? 1f : canvas.scaleFactor;

    private void Start()
    {
        CreateInfrastructure();
        Audio = gameObject.AddComponent<DewyAudio>();
        ShowPage(Page.Home);
    }

    private void CreateInfrastructure()
    {
        GameObject canvasGo = new GameObject("DewyCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasGo.transform.SetParent(transform, false);
        canvas = canvasGo.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 0;

        CanvasScaler scaler = canvasGo.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(450f, 900f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        if (EventSystem.current == null)
        {
            GameObject es = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            es.transform.SetParent(transform, false);
        }
    }

    public void ShowPage(Page page)
    {
        CurrentPage = page;
        if (currentRoot != null)
        {
            currentRoot.gameObject.SetActive(false);
            Destroy(currentRoot.gameObject);
        }

        learningCard = null;
        currentRoot = DewyUI.MakeRect("Page_" + page, canvas.transform);
        DewyUI.Stretch(currentRoot);
        Image bg = currentRoot.gameObject.AddComponent<Image>();
        bg.color = DewyUI.Hex("#F4FBFF");

        DewyContent.PageCopy copy = GetCopy(page);
        BuildTopBar(copy.Brand);
        BuildStoryCopy(copy);
        Stage = DewyUI.Stage(currentRoot, DewyUI.Hex("#C9EFFD")).rectTransform;
        BuildNavigation(page);
        BuildToast();

        if (page >= Page.Scene1 && page <= Page.Scene6) SetSceneComplete(false);
        else SetSceneComplete(true);

        DewyPages.Build(this, page);
        Audio.PlayPageBgm(page);
        RefreshSoundButton();
    }

    private DewyContent.PageCopy GetCopy(Page page)
    {
        switch (page)
        {
            case Page.Home: return DewyContent.Home;
            case Page.Scene1: return DewyContent.Scene1;
            case Page.Scene2: return DewyContent.Scene2;
            case Page.Scene3: return DewyContent.Scene3;
            case Page.Scene4: return DewyContent.Scene4;
            case Page.Scene5: return DewyContent.Scene5;
            case Page.Scene6: return DewyContent.Scene6;
            default: return DewyContent.Credits;
        }
    }

    private void BuildTopBar(string brand)
    {
        DewyUI.Label("Brand", currentRoot, brand, 16, DewyUI.Ink, 22, 14, 320, 34, TextAnchor.MiddleLeft, FontStyle.Bold);
        soundButton = DewyUI.Button("SoundToggle", currentRoot, "♪", 382, 10, 46, 46, Color.white, DewyUI.Ink, 18);
        soundGlyph = soundButton.GetComponentInChildren<Text>();
        soundButton.onClick.AddListener(() =>
        {
            bool enabled = Audio.Toggle();
            RefreshSoundButton();
            Toast(enabled ? "Sound on" : "Sound off");
        });
    }

    private void RefreshSoundButton()
    {
        if (soundGlyph != null && Audio != null) soundGlyph.text = Audio.SoundEnabled ? "♪" : "×";
    }

    private void BuildStoryCopy(DewyContent.PageCopy copy)
    {
        DewyUI.Label("Kicker", currentRoot, copy.Kicker.ToUpperInvariant(), 10, DewyUI.BlueDeep, 22, 58, 406, 18, TextAnchor.MiddleLeft, FontStyle.Bold);
        DewyUI.Label("Title", currentRoot, copy.Title, 25, DewyUI.Ink, 22, 76, 406, 38, TextAnchor.MiddleLeft, FontStyle.Bold);
        DewyUI.Label("Body", currentRoot, copy.Body, 13, DewyUI.Muted, 22, 115, 406, 69, TextAnchor.UpperLeft, FontStyle.Normal);
    }

    private void BuildNavigation(Page page)
    {
        Image nav = DewyUI.Panel("Navigation", currentRoot, new Color(1,1,1,0.88f), 0, 808, 450, 92);
        Button back = DewyUI.Button("Back", nav.transform, "‹", 20, 20, 54, 54, Color.white, DewyUI.Ink, 28);
        back.gameObject.SetActive(page != Page.Home);
        if (page != Page.Home)
        {
            back.onClick.AddListener(() =>
            {
                Audio.PlaySfx("ui", .28f);
                ShowPage((Page)Mathf.Max(0, (int)CurrentPage - 1));
            });
        }

        float startX = 157f;
        for (int i=0;i<8;i++)
        {
            float s = i == (int)page ? 13f : 9f;
            Color c = i == (int)page ? DewyUI.BlueDeep : DewyUI.Hex("#D4E5EB");
            DewyUI.Circle("Dot" + i, nav.transform, c, startX + i*17f, 41f - (s-9f)/2f, s).raycastTarget = false;
        }

        string nextGlyph = page == Page.Scene6 ? "✓" : (page == Page.Credits ? "⌂" : "›");
        nextButton = DewyUI.Button("Next", nav.transform, nextGlyph, 376, 20, 54, 54, DewyUI.Ink, Color.white, page == Page.Scene6 ? 20 : 28);
        nextButton.onClick.AddListener(() =>
        {
            Audio.PlaySfx("ui", .28f);
            if (CurrentPage == Page.Credits) ShowPage(Page.Home);
            else ShowPage((Page)Mathf.Min(7, (int)CurrentPage + 1));
        });
    }

    private void BuildToast()
    {
        Image toast = DewyUI.Panel("Toast", currentRoot, DewyUI.Hex("#123A4D"), 80, 770, 290, 38, true);
        toastText = DewyUI.Label("ToastText", toast.transform, "", 12, Color.white, 8, 0, 274, 38, TextAnchor.MiddleCenter, FontStyle.Bold);
        toast.gameObject.SetActive(false);
    }

    public void SetSceneComplete(bool complete)
    {
        sceneComplete = complete;
        if (nextButton != null) nextButton.interactable = sceneComplete;
        if (learningCard != null) learningCard.SetActive(sceneComplete);
    }

    public void RegisterLearningCard(GameObject card)
    {
        learningCard = card;
        if (CurrentPage >= Page.Scene4 && CurrentPage <= Page.Scene6 && learningCard.transform is RectTransform rect)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -GUIDED_LEARNING_Y);
        }
        learningCard.SetActive(sceneComplete);
    }

    public void Toast(string message)
    {
        if (toastText == null) return;
        if (toastRoutine != null) StopCoroutine(toastRoutine);
        toastText.text = message;
        toastText.transform.parent.gameObject.SetActive(true);
        toastRoutine = StartCoroutine(HideToastLater());
    }

    private IEnumerator HideToastLater()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        if (toastText != null) toastText.transform.parent.gameObject.SetActive(false);
        toastRoutine = null;
    }
}
