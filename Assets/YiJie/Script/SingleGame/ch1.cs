using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Flower;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using OpenAI;
using MySql.Data.MySqlClient;

public class ch1 : MonoBehaviour
{
    FlowerSystem fs;
    private int progress = 0;
    private bool gameEnd = false;
    [SerializeField] GameObject openingText_1;
    [SerializeField] GameObject openingText_2;
    private string playername = LoginAndRegister.LoggedInUsername;
    private int playerid = LoginAndRegister.LoggedInUserID;

    //learning mode variable
    [SerializeField] Image learningmode;
    Image learningmode_prefab;
    public TextAsset ch1LearnAsset;
    private List<Knowledge> knowledgePoints;
    private int currentPageIndex = 0;

    //Open AI
    private OpenAIApi openai = new OpenAIApi("");
    private string prompt = "指令：你現在是理化老師，用簡單的語言向國中生解釋這些內容，並舉例說明這個概念和公式是如何應用的。讓他們能夠容易理解並能夠在日常生活中找到相關例子。";
    private List<ChatMessage> messages = new List<ChatMessage>();
    //彩蛋 如果都一次願意，出現隱藏文字
    int not_allow1 = 0;
    int not_allow2 = 0;

    //DB
    string server = "34.80.93.104";
    string database = "questionnare"; // 資料庫名稱
    string user = "ss";
    string password = "worinidaya";
    private MySqlConnection connection;
    [SerializeField] GameObject conntectfail;
    void Start()
    {
        fs = FlowerManager.Instance.CreateFlowerSystem("default", true);
        fs.SetScreenReference(1920, 1080);
        fs.SetupDialog();
        //UI Stage沒什麼用，在3D遊戲比較有用，顯示圖片在canva中
        //fs.SetupUIStage("default", "DefaultUIStagePrefab", 10);
        fs.RegisterCommand("CreateOpeningText_1", CreateOpeningText_1);
        fs.RegisterCommand("CreateOpeningText_2", CreateOpeningText_2);
        fs.SetVariable("playername", playername);
        fs.RegisterCommand("learningMode", learningMode);
    }


    void Update()
    {
        if (fs.isCompleted && !gameEnd)
        {
            switch (progress)
            {
                case 0:
                    fs.ReadTextFromResource("SingleMode/ch1/opening_1");
                    progress = 1;
                    break;
                case 1:
                    fs.SetupButtonGroup();
                    fs.SetupButton("同意.", () =>
                    {
                        gameEnd = false;
                        fs.RemoveButtonGroup();
                        fs.SetTextList(new List<string> { "那你先去訓練場找教官吧。[w]" });
                        progress = 2;
                    });
                    fs.SetupButton("不同意", () =>
                    {
                        gameEnd = false;
                        fs.RemoveButtonGroup();
                        fs.SetTextList(new List<string> { "真的不願意嗎?[w]" });
                    });
                    gameEnd = true;
                    break;
                case 2:
                    fs.ReadTextFromResource("SingleMode/ch1/opening_2");
                    progress = 3;
                    break;
                case 3:
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        progress = 4;
                        if (learningmode_prefab != null)
                        {
                            Destroy(learningmode_prefab.gameObject);
                        }
                    }
                    break;
                case 4:
                    fs.ReadTextFromResource("SingleMode/ch1/opening_3");
                    progress = 5;
                    break;
                case 5:
                    fs.SetupButtonGroup();
                    fs.SetupButton("要知道.", () =>
                    {
                        gameEnd = false;
                        fs.RemoveButtonGroup();
                        progress = 6;
                    });
                    fs.SetupButton("不要知道", () =>
                    {
                        not_allow1++;
                        gameEnd = false;
                        fs.RemoveButtonGroup();
                        fs.SetTextList(new List<string> { "[#playername]:「那還是算了吧」[w] " +
                            "冴衙:「真的不願意嗎?」[w]" });
                    });
                    gameEnd = true;
                    break;
                case 6:
                    fs.ReadTextFromResource("SingleMode/ch1/opening_4");
                    progress = 7;
                    break;
                case 7:
                    fs.SetupButtonGroup();
                    fs.SetupButton("願意.", () =>
                    {
                        gameEnd = false;
                        fs.RemoveButtonGroup();
                        fs.SetTextList(new List<string> { "[#playername] :「願意」[w]"+
                            "冴衙:「好，我相信我不會看錯人的，你一定能夠在帝國學院的野心中成功守住我們學院的。」[w]" });
                        if (not_allow1 == 0 && not_allow2 == 0)
                        {
                            fs.SetTextList(new List<string> { "[#playername] :「從我選擇了解這件事情開始，我就注定要成為這名戰士了，更何況身為這所學校的學生，能為學校做出貢獻就是我的光榮。」[w]"+
                            "冴衙:「說得好，我相信我不會看錯人的，你一定能夠在帝國學院的野心中成功守住我們學院的。」[w] "});
                        }
                        progress = 8;
                    });
                    fs.SetupButton("不願意", () =>
                    {
                        not_allow2++;
                        gameEnd = false;
                        fs.RemoveButtonGroup();
                        fs.SetTextList(new List<string> { "[#playername]:「那還是算了吧」[w] " +
                            "冴衙:「真的不願意嗎?」[w]" });
                    });
                    gameEnd = true;
                    break;
                case 8:
                    fs.ReadTextFromResource("SingleMode/ch1/opening_5");
                    progress = 9;
                    break;
                case 9:
                    fs.SetTextList(new List<string> { "序章結束進入第一章[w]" });
                    if (initDB())
                    {
                        UpdateCurrentChapter(playerid, 2);
                        SceneManager.LoadScene("ch2Scene");
                    }
                    else
                    {
                        //先找對話的canva
                        GameObject canvas = GameObject.Find("DefaultDialogPrefab(Clone)");
                        // 在 Canvas 的子物件位置創建prefab
                        GameObject conntectfailInstance = Instantiate(conntectfail, canvas.transform);
                        // 設置 RectTransform
                        RectTransform rectTransform = conntectfailInstance.GetComponent<RectTransform>();
                        rectTransform.anchoredPosition = new Vector2(0, 0);
                        Button conntectfailbutton = conntectfailInstance.transform.Find("return").GetComponent<Button>();
                        conntectfailbutton.onClick.AddListener(()=> {
                            gameEnd = false;
                        });
                    }
                    gameEnd = true;
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fs.Next();
        }
    }
    private void CreateOpeningText_1(List<string> properties)
    {
        //先找對話的canva
        GameObject canvas = GameObject.Find("DefaultDialogPrefab(Clone)");
        // 在 Canvas 的子物件位置創建文字 prefab
        GameObject spawnedText = Instantiate(openingText_1, canvas.transform);
        // 設置初始位置
        spawnedText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -400);
    }
    private void CreateOpeningText_2(List<string> properties)
    {
        //先找對話的canva
        GameObject canvas = GameObject.Find("DefaultDialogPrefab(Clone)");
        // 在 Canvas 的子物件位置創建文字 prefab
        GameObject spawnedText = Instantiate(openingText_2, canvas.transform);
        // 設置初始位置
        spawnedText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -450);
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
        string content = ch1LearnAsset.text;
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


    private bool initDB()
    {
        string connectionString = $"Server={server};Database={database};User ID={user};Password={password};Pooling=false;";
        try
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
            return true;  // 連接成功，返回 true
        }
        catch (Exception ex)
        {
            Debug.LogError("開啟資料庫失敗: " + ex.Message);
            return false;  // 連接失敗，返回 false
        }
        finally
        {
            if (connection != null)
            {
                connection.Close();
            }
        }
    }

    private void UpdateCurrentChapter(int userID, int chapterID)
    {
        if (userID == 0)
        {
            return;
        }
        string updateQuery = @"
        UPDATE users 
        SET currentChapter = @ChapterID 
        WHERE id = @UserID AND currentChapter < @ChapterID ";

        using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
        {
            command.Parameters.AddWithValue("@ChapterID", chapterID);
            command.Parameters.AddWithValue("@UserID", userID);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                Debug.Log("Updated currentChapter to " + chapterID);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to update currentChapter: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }

}
