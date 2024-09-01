using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower;
using UnityEngine.UI;
using OpenAI;

public class  ch3 : MonoBehaviour
{
    FlowerSystem fs;
    private int progress = 0;
    private bool gameEnd = false;
    // private string playername = LoginAndRegister.LoggedInUsername ;
    private string playername =""  ;

    //learning mode 
    [SerializeField] Image learningmode;
 
    Image learningmode_prefab;
    public TextAsset ch2LearnAsset;
    private List<Knowledge> knowledgePoints;
    private int currentPageIndex = 0;

    //Open AI
    private OpenAIApi openai = new OpenAIApi("");
    private string prompt = "指令：你現在是理化老師，用簡單的語言向國中生解釋這些內容，並舉例說明這個概念和公式是如何應用的。讓他們能夠容易理解並能夠在日常生活中找到相關例子。";
    private List<ChatMessage> messages = new List<ChatMessage>();

    //train mode
    [SerializeField] Image trainmode;
    string Checkanswer_string="";
    void Start()
    {
        fs = FlowerManager.Instance.CreateFlowerSystem("default", true);
        fs.SetupDialog();
        fs.SetVariable("playername", playername);
        fs.RegisterCommand("learningMode", learningMode);
        fs.RegisterCommand("trainMode", trainMode);
    }


    void Update()
    {
        if (fs.isCompleted && !gameEnd)
        {
            switch (progress)
            {
                case 0:
                    fs.ReadTextFromResource("SingleMode/ch3/ch3_1");
                    progress = 1;
                    break;
                case 1:
                    fs.SetTextList(new List<string> { "結束[w]" });
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fs.Next();
        }
    }
   
    public class Knowledge
    {
        public KnowledgeType Type;
        public string Content;
    }

    public enum KnowledgeType
    {
        Text,
        Image
    }
    private void learningMode(List<string> properties)
    {
        //先找對話的canva
        GameObject canvas = GameObject.Find("DefaultDialogPrefab(Clone)");
        // 在 Canvas 的子物件位置創建prefab
        learningmode_prefab = Instantiate(learningmode, canvas.transform);
        learningmode_prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        //在learningMode圖片下，尋找按鈕及文字
        Button nextButton = learningmode_prefab.transform.Find("next").GetComponent<Button>();
        Button previousButton = learningmode_prefab.transform.Find("previous").GetComponent<Button>();
        Text showText = learningmode_prefab.transform.Find("QuestionScrollView/Viewport/Content/showText").GetComponent<Text>();
        Image learning_Image = learningmode_prefab.transform.Find("learning_Image").GetComponent<Image>();
        Button promptcallButton = learningmode_prefab.transform.Find("promptcall").GetComponent<Button>();
        GameObject promptPanel = learningmode_prefab.transform.Find("PromptPanel").gameObject;
        Button promptButton = learningmode_prefab.transform.Find("PromptPanel/prompt").GetComponent<Button>();
        Text promptText = learningmode_prefab.transform.Find("PromptPanel/Scroll View/Viewport/Content/promptText").GetComponent<Text>();

        //讀取檔案
        string content = ch2LearnAsset.text;
        knowledgePoints = new List<Knowledge>();
        // 分頁
        string[] parts = content.Split(new[] { "[knowledge]" }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string part in parts)
        {
            Knowledge knowledge = new Knowledge();
                if (part.Contains("[text]"))
                {
                    knowledge.Type = KnowledgeType.Text;
                    knowledge.Content = part.Substring("[text]".Length+2).Trim(); ;
                }
                else if (part.Contains("[image]"))
                {
                    knowledge.Type = KnowledgeType.Image;
                // +2為了去除換行
                knowledge.Content = part.Substring("[image]".Length+2).Trim();
            }
            knowledgePoints.Add(knowledge);
        }
        //初始化
        DisplayCurrentPage(showText,learning_Image);
        nextButton.onClick.AddListener(() =>
        {
            if (currentPageIndex < knowledgePoints.Count - 1)
            {
                currentPageIndex++;
                DisplayCurrentPage(showText, learning_Image);
                previousButton.interactable = currentPageIndex > 0;
                nextButton.interactable = currentPageIndex < knowledgePoints.Count - 1;
            }
        });
        previousButton.onClick.AddListener(() =>
        {
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                DisplayCurrentPage(showText, learning_Image);
                previousButton.interactable = currentPageIndex > 0;
                nextButton.interactable = currentPageIndex < knowledgePoints.Count - 1;
            }
        });
        promptcallButton.onClick.AddListener(() =>
        {
            bool isActive = promptPanel.gameObject.activeSelf;
            promptPanel.gameObject.SetActive(!isActive);
        });
        // call OpenAI
        promptButton.onClick.AddListener(async () =>
        {
            promptText.text = "";
            messages.Clear();
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = prompt + "\n" +"資料： "+ showText.text,
            };
            messages.Add(newMessage);
            Debug.Log("傳送chat內容："+newMessage.Content);
            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-4o",
                Messages = messages
            });
            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var Response_message = completionResponse.Choices[0].Message;
                Response_message.Content = Response_message.Content.Trim();
                promptText.text = Response_message.Content;
                Debug.Log("chat回覆：" + Response_message.Content);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }
        });
        void DisplayCurrentPage(Text contentText , Image contentImage)
        {
            if (knowledgePoints.Count > 0)
            {
                Knowledge currentKnowledge = knowledgePoints[currentPageIndex];
                promptText.text = "";
                if (currentKnowledge.Type == KnowledgeType.Text)
                {
                    contentText.text = currentKnowledge.Content;
                    contentText.gameObject.SetActive(true);
                    promptButton.gameObject.SetActive(true);
                    contentImage.gameObject.SetActive(false);
                }
                else if (currentKnowledge.Type == KnowledgeType.Image)
                {
                    contentImage.sprite = Resources.Load<Sprite>(currentKnowledge.Content);
                    contentImage.gameObject.SetActive(true);
                    contentText.gameObject.SetActive(false);
                    promptButton.gameObject.SetActive(false);
                }
            }
        }
    }

    private void trainMode(List<string> properties)
    {
        //先找對話的canva
        GameObject canvas = GameObject.Find("DefaultDialogPrefab(Clone)");
        // 在 Canvas 的子物件位置創建prefab
        learningmode_prefab = Instantiate(trainmode, canvas.transform);
        learningmode_prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        //在learningMode圖片下，尋找按鈕及文字
        GameObject questionScroll = learningmode_prefab.transform.Find("question/questionScroll").gameObject;
        Text questionText = learningmode_prefab.transform.Find("question/questionScroll/Viewport/Content/questionText").GetComponent<Text>();
        Button Solution_Question_Button = learningmode_prefab.transform.Find("Solution_Question").GetComponent<Button>();
        GameObject SloutionScroll = learningmode_prefab.transform.Find("question/SolutionScroll").gameObject;
        Text SloutionText = learningmode_prefab.transform.Find("question/SolutionScroll/Viewport/Content/SloutionText").GetComponent<Text>();
        GameObject option= learningmode_prefab.transform.Find("option").gameObject;
        Button option_1 =learningmode_prefab.transform.Find("option/Choice/1/Button").GetComponent<Button>();
        Button option_2 = learningmode_prefab.transform.Find("option/Choice/2/Button").GetComponent<Button>();
        Button option_3= learningmode_prefab.transform.Find("option/Choice/3/Button").GetComponent<Button>();
        Button option_4 = learningmode_prefab.transform.Find("option/Choice/4/Button").GetComponent<Button>();
        Button Next_Button = learningmode_prefab.transform.Find("Next").GetComponent<Button>();
        Button Check_Button = learningmode_prefab.transform.Find("Check").GetComponent<Button>();
        Text answerText = learningmode_prefab.transform.Find("answerText").GetComponent<Text>();
        Button promptcallButton = learningmode_prefab.transform.Find("promptcall").GetComponent<Button>();
        Image promptPanel = learningmode_prefab.transform.Find("Panel").GetComponent<Image>();
        Button promptButton = learningmode_prefab.transform.Find("Panel/prompt").GetComponent<Button>();
        Text promptText = learningmode_prefab.transform.Find("Panel/promptTextscroll/Viewport/Content/promptText").GetComponent<Text>();

        // 處理選擇邏輯
        Button[] optionButtons = { option_1, option_2, option_3, option_4 };
        foreach (Button Choice_option in optionButtons)
        {
            Choice_option.onClick.AddListener(() =>
            {
                // 重製顏色
                foreach (Button btn in optionButtons)
                {
                    btn.GetComponent<Image>().color = Color.white;
                }
                // 改變選中按鈕顏色
                Choice_option.GetComponent<Image>().color = Color.green;

                answerText.text = "你选择了: " + Choice_option.GetComponentInChildren<Text>().text;
                Checkanswer_string = Choice_option.GetComponentInChildren<Text>().text;
            });
        }
        initQuestion(Check_Button, Next_Button, option, optionButtons, Solution_Question_Button , answerText);

       
        promptcallButton.onClick.AddListener(() =>
        {
            bool isActive = promptPanel.gameObject.activeSelf;
            promptPanel.gameObject.SetActive(!isActive);
        });
        Check_Button.onClick.AddListener(() =>{
            Check_Button.gameObject.SetActive(false);
            Next_Button.gameObject.SetActive(true);
            option.gameObject.SetActive(false);
            answerText.text = "你的答案是" + Checkanswer_string + "解答為" + (Random.Range(0, 4) + 1).ToString();
            Solution_Question_Button.interactable = true;
        });

        Solution_Question_Button.onClick.AddListener(() =>
        {
            questionScroll.SetActive(!questionScroll.activeSelf);
            SloutionScroll.SetActive(!SloutionScroll.activeSelf);
        });
        Next_Button.onClick.AddListener( () =>initQuestion(Check_Button, Next_Button, option, optionButtons, Solution_Question_Button ,answerText));


        // call OpenAI
        /*promptButton.onClick.AddListener(async () =>
        {
            promptText.text = "";
            messages.Clear();
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = prompt + "\n" + "資料： " ,
            };
            messages.Add(newMessage);
            Debug.Log("傳送chat內容：" + newMessage.Content);
            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-4o",
                Messages = messages
            });
            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var Response_message = completionResponse.Choices[0].Message;
                Response_message.Content = Response_message.Content.Trim();
                promptText.text = Response_message.Content;
                Debug.Log("chat回覆：" + Response_message.Content);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }
        });*/

    }
    private void initQuestion(Button check_button,Button next_button, GameObject option, Button[] optionButtons, Button Solution_Question_Button ,Text answerText)
    {
        check_button.gameObject.SetActive(true);
        next_button.gameObject.SetActive(false);
        option.SetActive(true);
        //重製所有按鈕為白色
        foreach (Button Choice_option in optionButtons)
        {
            Choice_option.GetComponent<Image>().color = Color.white;
        }
        Solution_Question_Button.interactable = false;
        answerText.text = "請回答題目後，按下確認顯示答案";
    }
}
