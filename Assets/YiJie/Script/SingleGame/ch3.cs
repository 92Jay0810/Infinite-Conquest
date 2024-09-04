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
    private string playername = LoginAndRegister.LoggedInUsername ;
    //private string playername =""  ;

    //learning mode 
    [SerializeField] Image learningmode;
 
    Image learningmode_prefab;
    public TextAsset ch3LearnAsset;
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
        fs.SetScreenReference(1920, 1080);
        fs.SetupDialog();
        fs.SetVariable("playername", playername);
        fs.RegisterCommand("learningMode", learningMode);
       // fs.RegisterCommand("trainMode", trainMode);
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
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        progress = 2;
                        if (learningmode_prefab != null)
                        {
                            Destroy(learningmode_prefab.gameObject);
                        }
                    }
                    break;
                case 2:
                    fs.ReadTextFromResource("SingleMode/ch3/ch3_2");
                    progress = 3;
                    break;
                case 3:
                    fs.SetupButtonGroup();
                    fs.SetupButton("回去觀看學習模式.", () =>
                    {
                        gameEnd = false;
                        fs.RemoveButtonGroup();
                        progress = 4;
                    });
                    fs.SetupButton("進入戰鬥", () =>
                    {
                        gameEnd = false;
                        fs.RemoveButtonGroup();
                        progress = 6;
                    });
                    gameEnd = true;
                    break;
                case 4:
                    fs.ReadTextFromResource("SingleMode/ch2/learnmode");
                    progress = 5;
                    break;
                case 5:
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        progress = 3;
                        if (learningmode_prefab != null)
                        {
                            Destroy(learningmode_prefab.gameObject);
                        }
                    }
                    break;
                case 6:
                    fs.ReadTextFromResource("SingleMode/ch3/ch3_3");
                    progress = 7;
                    break;
                case 7:
                    //fs.ReadTextFromResource("SingleMode/ch2/trainmode");
                    progress = 8;
                    break;
                case 8:
                    /*
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        progress = 3;
                        if (trainmode_prefab != null)
                        {
                            Destroy(trainmode_prefab.gameObject);
                        }
                        answer_count = 0;
                        correct_answer_count = 0;
                    }*/
                    progress = 9;
                    break;
                case 9:
                    fs.ReadTextFromResource("SingleMode/ch3/ch3_4");
                    progress = 10;
                    break;

                case 10:
                    fs.SetupButtonGroup();
                    fs.SetupButton("回去觀看學習模式.", () =>
                    {
                        gameEnd = false;
                        fs.RemoveButtonGroup();
                        progress = 11;
                    });
                    fs.SetupButton("進入戰鬥", () =>
                    {
                        gameEnd = false;
                        fs.RemoveButtonGroup();
                        progress = 13;
                    });
                    gameEnd = true;
                    break;
                case 11:
                    fs.ReadTextFromResource("SingleMode/ch2/learnmode");
                    progress = 12;
                    break;
                case 12:
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        progress = 10;
                        if (learningmode_prefab != null)
                        {
                            Destroy(learningmode_prefab.gameObject);
                        }
                    }
                    break;
                case 13:
                    fs.ReadTextFromResource("SingleMode/ch3/ch3_5");
                    progress = 14;
                    break;
                case 14:
                    //fs.ReadTextFromResource("SingleMode/ch2/trainmode");
                    progress = 15;
                    break;
                case 15:
                    /*
                     if (Input.GetKeyDown(KeyCode.R))
                     {
                         progress = 3;
                         if (trainmode_prefab != null)
                         {
                             Destroy(trainmode_prefab.gameObject);
                         }
                         answer_count = 0;
                         correct_answer_count = 0;
                     }*/
                    progress = 16;
                    break;
                case 16:
                    fs.ReadTextFromResource("SingleMode/ch3/ch3_6");
                    progress = 17;
                    break;
                case 17:
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
        string content = ch3LearnAsset.text;
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

}
