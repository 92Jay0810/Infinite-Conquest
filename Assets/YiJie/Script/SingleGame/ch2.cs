using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Flower;
using UnityEngine.UI;
using OpenAI;
using MySql.Data.MySqlClient;

public class  ch2 : MonoBehaviour
{
    FlowerSystem fs;
    private int progress = 0;
    private bool gameEnd = false;
    private string playername = LoginAndRegister.LoggedInUsername;
    private int playerid= LoginAndRegister.LoggedInUserID;

    //learning mode 
    [SerializeField] Image learningmode; 
    Image learningmode_prefab;
    public TextAsset ch2LearnAsset;
    private List<Knowledge> knowledgePoints;
    private int currentPageIndex = 0;

    //Open AI
    private OpenAIApi openai = new OpenAIApi("");
    private string learn_prompt = "指令：\n 你現在是理化老師，用簡單的語言向國中生解釋這些內容，並舉例說明這個概念和公式是如何應用的。讓他們能夠容易理解並能夠在日常生活中找到相關例子。";
    private string train_prompt = "指令：\n 你現在是理化老師，請根據下列的題目內容（以及選項，如果有）生成一段簡短的指導，幫助使用者理解如何解答這道題目。對於選擇題，請引導用戶如何審查選項並做出最佳選擇；對於是非題，請引導用戶如何判斷陳述的正確性；對於問答題，請指導用戶如何組織並表達他們的答案。";
    private string judge_prompt = "指令：\n 請根據下列題目和使用者的回答，進行評價並返回如下格式的字串：'評價：高'、'評價：中'、或 '評價：低'。評價應基於回答的完整性、準確性和深度來決定，以國中生程度來判定。";
    private string detail_prompt = "指令：\n 你現在是理化老師，請根據下列題目、選項（若有的話）和詳解，為用戶生成一個詳細的解釋。解釋應該幫助用戶理解題目的重點，為什麼正確答案是正確的，並說明其他選項為什麼不正確，讓用戶在下一次遇到類似問題時能夠更好地解答。";
    private List<ChatMessage> messages = new List<ChatMessage>();

    //train mode
    [SerializeField] Image trainmode;
    Image trainmode_prefab;
    string Checkanswer_string="";
    const int max_answer_count = 5;
    int answer_count = 0;
    int correct_answer_count = 0;
    [SerializeField] Image train_result;
    Image train_result_prefab;

    //train mode DB
    /*
    string server = "localhost";
    string database = "questionnare"; // 資料庫名稱
    string user = "root";
    string password = "123";*/
    string server = "34.80.93.104";
    string database = "questionnare"; // 資料庫名稱
    string user = "ss";
    string password = "worinidaya";
    private MySqlConnection connection;

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
                    fs.ReadTextFromResource("SingleMode/ch2/ch2_1");
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
                    fs.ReadTextFromResource("SingleMode/ch2/ch2_2");
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
                    fs.ReadTextFromResource("SingleMode/ch2/trainmode");
                    progress = 7;
                    break;
                case 7:
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        progress = 3;
                        if (trainmode_prefab != null)
                        {
                            Destroy(trainmode_prefab.gameObject);
                        }
                        answer_count = 0;
                        correct_answer_count = 0;
                    }
                    break;
                case 8:
                    fs.ReadTextFromResource("SingleMode/ch2/ch2_3");
                    progress = 9;
                    break;
                case 9:
                    fs.SetupButtonGroup();
                    fs.SetupButton("回去觀看學習模式.", () =>
                    {
                        gameEnd = false;
                        fs.RemoveButtonGroup();
                        progress = 10;
                    });
                    fs.SetupButton("進入最終戰鬥", () =>
                    {
                        gameEnd = false;
                        fs.RemoveButtonGroup();
                        progress = 12;
                    });
                    gameEnd = true;
                    break;
                case 10:
                    fs.ReadTextFromResource("SingleMode/ch2/learnmode");
                    progress = 11;
                    break;
                case 11:
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        progress = 9;
                        if (learningmode_prefab != null)
                        {
                            Destroy(learningmode_prefab.gameObject);
                        }
                    }
                    break;
                case 12:
                    fs.ReadTextFromResource("SingleMode/ch2/checkmode");
                    progress = 13;
                    break;
                case 13:
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        progress = 9;
                        if (trainmode_prefab != null)
                        {
                            Destroy(trainmode_prefab.gameObject);
                        }
                        answer_count = 0;
                        correct_answer_count = 0;
                    }
                    break;
                case 14:
                    fs.ReadTextFromResource("SingleMode/ch2/ch2_4");
                    progress = 15;
                    break;
                case 15:
                    fs.SetTextList(new List<string> { "結束，進入下一章[w]" });
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
                Content = learn_prompt + "\n" +"資料： "+ showText.text,
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
        //先初始化DB並確認是否有連接
        if (initDB())
        {
            //先找對話的canva
            GameObject canvas = GameObject.Find("DefaultDialogPrefab(Clone)");
            // 在 Canvas 的子物件位置創建prefab
            trainmode_prefab = Instantiate(trainmode, canvas.transform);
            trainmode_prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            //在learningMode圖片下，尋找按鈕及文字
            GameObject question = trainmode_prefab.transform.Find("question").gameObject;
            GameObject questionScroll = trainmode_prefab.transform.Find("question/questionScroll").gameObject;
            Text questionText = trainmode_prefab.transform.Find("question/questionScroll/Viewport/Content/questionText").GetComponent<Text>();
            Button Solution_Question_Button = trainmode_prefab.transform.Find("Solution_Question").GetComponent<Button>();
            GameObject SolutionScroll = trainmode_prefab.transform.Find("question/SolutionScroll").gameObject;
            Text SolutionText = trainmode_prefab.transform.Find("question/SolutionScroll/Viewport/Content/SloutionText").GetComponent<Text>();
            GameObject option = trainmode_prefab.transform.Find("option").gameObject;
            GameObject choice = trainmode_prefab.transform.Find("option/Choice").gameObject;
            Button Choice_1 = trainmode_prefab.transform.Find("option/Choice/1/Button").GetComponent<Button>();
            Button Choice_2 = trainmode_prefab.transform.Find("option/Choice/2/Button").GetComponent<Button>();
            Button Choice_3 = trainmode_prefab.transform.Find("option/Choice/3/Button").GetComponent<Button>();
            Button Choice_4 = trainmode_prefab.transform.Find("option/Choice/4/Button").GetComponent<Button>();
            GameObject TrueFalse = trainmode_prefab.transform.Find("option/TrueFalse").gameObject;
            Button TrueFalse_true = trainmode_prefab.transform.Find("option/TrueFalse/true").GetComponent<Button>();
            Button TrueFalse_false = trainmode_prefab.transform.Find("option/TrueFalse/false").GetComponent<Button>();
            InputField ask = trainmode_prefab.transform.Find("option/ask").GetComponent<InputField>();
            Button Next_Button = trainmode_prefab.transform.Find("Next").GetComponent<Button>();
            Button Check_Button = trainmode_prefab.transform.Find("Check").GetComponent<Button>();
            Text answerText = trainmode_prefab.transform.Find("answerScroll/Viewport/Content/answerText").GetComponent<Text>();
            Button promptcallButton = trainmode_prefab.transform.Find("promptcall").GetComponent<Button>();
            Image promptPanel = trainmode_prefab.transform.Find("Panel").GetComponent<Image>();
            Button promptButton = trainmode_prefab.transform.Find("Panel/prompt").GetComponent<Button>();
            Text promptText = trainmode_prefab.transform.Find("Panel/promptTextscroll/Viewport/Content/promptText").GetComponent<Text>();
            promptText.text = "";
            // 處理選擇邏輯
            Button[] optionButtons = { Choice_1, Choice_2, Choice_3, Choice_4 };
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

            TrueFalse_true.onClick.AddListener(() =>
            {
                TrueFalse_true.GetComponent<Image>().color = Color.green;
                TrueFalse_false.GetComponent<Image>().color = Color.white;
                answerText.text = "你选择了: True";
                Checkanswer_string = "true";

            });
            TrueFalse_false.onClick.AddListener(() =>
            {
                TrueFalse_true.GetComponent<Image>().color = Color.white;
                TrueFalse_false.GetComponent<Image>().color = Color.green;
                answerText.text = "你选择了: false";
                Checkanswer_string = "false";

            });
            ask.onEndEdit.AddListener(inputText =>
            {
                answerText.text = "你的回答: " + inputText;
                Checkanswer_string = inputText;
            });
            initQuestion(Check_Button, Next_Button, question, option, choice, TrueFalse, ask, Solution_Question_Button, answerText, questionText);
            //pass by txt file， true  repersent train_mode false represent check_mode，check_mode no hint 
            if (properties[0] == "true")
            {
                promptcallButton.onClick.AddListener(() =>
                   {
                    
                        promptcallButton.interactable = true;
                       bool isActive = promptPanel.gameObject.activeSelf;
                       promptPanel.gameObject.SetActive(!isActive);
                   });
            }
            else
            {
                promptcallButton.interactable = false;
            }
                Solution_Question_Button.onClick.AddListener(() =>
            {
                questionScroll.SetActive(!questionScroll.activeSelf);
                SolutionScroll.SetActive(!SolutionScroll.activeSelf);
            });
            Next_Button.onClick.AddListener(() =>
            {
                if (answer_count < max_answer_count)
                {
                    promptText.text = "";
                    initQuestion(Check_Button, Next_Button, question, option, choice, TrueFalse, ask, Solution_Question_Button, answerText, questionText);
                }
                else
                {
                    DisplayResult(properties[0]);
                }
            });

            // call OpenAI
        promptButton.onClick.AddListener(async () =>
        {
            promptText.text = "";
            messages.Clear();
            string QuestionTypeString = "";
            string OptionString = "選項：\n";
            if (choice.activeSelf)
            {
                QuestionTypeString = "選擇題";
                Text option_1 = choice.transform.Find("1/Viewport/Content/questionText").GetComponent<Text>();
                Text option_2 = choice.transform.Find("2/Viewport/Content/questionText").GetComponent<Text>();
                Text option_3 = choice.transform.Find("3/Viewport/Content/questionText").GetComponent<Text>();
                Text option_4 = choice.transform.Find("4/Viewport/Content/questionText").GetComponent<Text>();
                OptionString += "(1)"+option_1.text + "\n (2) " + option_2.text + "\n (3) " + option_3.text + "\n (4) " + option_4.text;

            }
            else if (TrueFalse.activeSelf)
            {
                QuestionTypeString = "是非題";
                OptionString += " 是 \n 否 " ;
            }
            else
            {
                QuestionTypeString = "問答題";
            }
            //如果題目詳解切換按鈕可以互動，表示已經回答完題目，則進行題目和詳解的詳細說明與解答
            string MessageContent=" ";
            if (Solution_Question_Button.interactable)
            {
                MessageContent = detail_prompt +"\n 題目類型："+QuestionTypeString +"\n 題目：\n " + questionText.text+"\n"+OptionString+" \n 詳解：\n"+SolutionText.text;
            }
            else
            {
                MessageContent = train_prompt + "\n 題目類型：" + QuestionTypeString + "\n 題目：\n " + questionText.text + "\n" + OptionString;
            }
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = MessageContent,
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
        });

        }
        else
        {
            Debug.Log("開啟資料庫失敗，請按下R退出");
            //先找對話的canva
            GameObject canvas = GameObject.Find("DefaultDialogPrefab(Clone)");
            // 在 Canvas 的子物件位置創建prefab
            // 創建一個新的 GameObject 來持有 Text 組件
            GameObject textObject = new GameObject("DisplayText");
            textObject.transform.SetParent(canvas.transform, false);
            // 添加 Text 組件
            Text textComponent = textObject.AddComponent<Text>();
            textComponent.text = "開啟資料庫失敗，請按下R退出";
            // 設置文本的一些基本屬性
            textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textComponent.fontSize = 24;
            textComponent.color = Color.black;
            // 設置 RectTransform
            RectTransform rectTransform = textComponent.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(200, 50); // 設置文本區域的寬度和高度
            // 設置文本在父物件中的對齊方式
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }
        
    }
    private void initQuestion(Button check_button,Button next_button, GameObject question, GameObject option,GameObject choice, GameObject TrueFalse, InputField askField, Button Solution_Question_Button ,Text answerText, Text questionText)
    {
        // 打开数据库连接
        if (connection.State != System.Data.ConnectionState.Open)
        {
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to open the database connection: " + ex.Message);
                return;
            }
        }
        check_button.gameObject.SetActive(true);
        next_button.gameObject.SetActive(false);
        Solution_Question_Button.interactable = false;
        GameObject questionScroll = question.transform.Find("questionScroll").gameObject;
        GameObject SolutionScroll = question.transform.Find("SolutionScroll").gameObject;
        questionScroll.SetActive(true);
        SolutionScroll.SetActive(false);
        Text Solutiontext = SolutionScroll.transform.Find("Viewport/Content/SloutionText").GetComponent<Text>();
        answerText.text = "請回答題目後，按下確認顯示答案";
        Checkanswer_string = "";
        // Randomly select question type
        int questionType = UnityEngine.Random.Range(0, 3); // 0: Multiple Choice, 1: True/False, 2: Short Answer
        option.SetActive(true);
        Debug.Log(questionType);
        switch (questionType)
        {
            case 0: // Multiple Choice
                Button Choice_1 = choice.transform.Find("1/Button").GetComponent<Button>();
                Button Choice_2 = choice.transform.Find("2/Button").GetComponent<Button>();
                Button Choice_3 = choice.transform.Find("3/Button").GetComponent<Button>();
                Button Choice_4 = choice.transform.Find("4/Button").GetComponent<Button>();
                Button[] optionButtons = { Choice_1, Choice_2, Choice_3, Choice_4 };
                foreach (Button btn in optionButtons)
                {
                    btn.gameObject.SetActive(true);
                    btn.GetComponent<Image>().color = Color.white; // Reset button color
                }
                choice.SetActive(true);
                TrueFalse.SetActive(false);
                askField.gameObject.SetActive(false);
              
                //隨機 取得選擇題題目並且章節為2 的題目
                MySqlCommand command0 = new MySqlCommand("SELECT * FROM questionnare.questions WHERE QuestionType = 0 AND ChapterID = 2 ORDER BY RAND() LIMIT 1", connection);
                MySqlDataReader reader0 = command0.ExecuteReader();
                string QuestionID ="" ;
                string tempAnswerOptionID_choice = "";
                if (reader0.Read()) // 使用 if 因為只會返回一條記錄
                {
                    string  random_question_descript_choice = reader0["Description"].ToString();
                    Debug.Log(random_question_descript_choice);
                    questionText.text = random_question_descript_choice;
                    QuestionID = reader0["QuestionID"].ToString();
                    tempAnswerOptionID_choice = reader0["AnswerOptionID"].ToString();
                    Solutiontext.text= reader0["Explanation"].ToString();
                }
                if (reader0 != null && !reader0.IsClosed)
                {
                    reader0.Close();
                }
                check_button.onClick.RemoveAllListeners(); // Clear previous listeners
                    check_button.onClick.AddListener(() =>
                    {
                        check_button.gameObject.SetActive(false);
                        next_button.gameObject.SetActive(true);
                        option.gameObject.SetActive(false);
                        answer_count++;
                        Debug.Log(" answer_count" + answer_count);
                        if (Checkanswer_string == tempAnswerOptionID_choice)
                        {
                            correct_answer_count++;
                            Debug.Log("  correct_answer_count" + correct_answer_count);
                            answerText.text = " 你的答案是    " + Checkanswer_string + " 解答為 " + tempAnswerOptionID_choice+" 正確! ";
                        }
                        else
                        {
                            answerText.text = " 你的答案是    " + Checkanswer_string + " 解答為 " + tempAnswerOptionID_choice+" 錯誤 ";
                        }
                        Solution_Question_Button.interactable = true;
                     });
                Text option_1 = choice.transform.Find("1/Viewport/Content/questionText").GetComponent<Text>();
                Text option_2 = choice.transform.Find("2/Viewport/Content/questionText").GetComponent<Text>();
                Text option_3 = choice.transform.Find("3/Viewport/Content/questionText").GetComponent<Text>();
                Text option_4 = choice.transform.Find("4/Viewport/Content/questionText").GetComponent<Text>();
                // Replace @QuestionID with the actual QuestionID
                MySqlCommand commandOption = new MySqlCommand("SELECT * FROM questionnare.options WHERE QuestionID = @QuestionID ORDER BY OptionID", connection);
                commandOption.Parameters.AddWithValue("@QuestionID", QuestionID);
                MySqlDataReader readerOption = commandOption.ExecuteReader();
                if (readerOption.HasRows)
                {
                    while (readerOption.Read())
                    {
                        int optionID = Convert.ToInt32(readerOption["OptionID"]);
                        string optionText = readerOption["Description"].ToString();
                        switch (optionID)
                        {
                            case 1:
                                option_1.text = optionText;
                                break;
                            case 2:
                                option_2.text = optionText;
                                break;
                            case 3:
                                option_3.text = optionText;
                                break;
                            case 4:
                                option_4.text = optionText;
                                break;
                        }
                    }
                }
                readerOption.Close();
                break;

            case 1: //    True_False
                TrueFalse.SetActive(true);
                Button true_button = TrueFalse.transform.Find("true").GetComponent<Button>();
                true_button.gameObject.SetActive(true);
                true_button.GetComponent<Image>().color = Color.white; // Reset button color
                Button false_button = TrueFalse.transform.Find("false").GetComponent<Button>();
                false_button.gameObject.SetActive(true);
                false_button.GetComponent<Image>().color = Color.white; // Reset button color

                choice.SetActive(false);
                askField.gameObject.SetActive(false);

                //隨機 取得是非題並且章節為2 的題目
                MySqlCommand command = new MySqlCommand("SELECT * FROM questionnare.questions WHERE QuestionType = 1 AND ChapterID = 2 ORDER BY RAND() LIMIT 1", connection);
                MySqlDataReader reader = command.ExecuteReader();
                string tempAnswerOptionID = "";
                if (reader.Read()) // 使用 if 因為只會返回一條記錄
                {
                    string random_question_descript = reader["Description"].ToString();
                    Debug.Log(random_question_descript);
                    questionText.text = random_question_descript;
                    tempAnswerOptionID =  reader["AnswerOptionID"].ToString()=="1" ? "true"  : "false" ;
                    Solutiontext.text = reader["Explanation"].ToString();
                }
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
                check_button.onClick.RemoveAllListeners(); // Clear previous listeners
                    check_button.onClick.AddListener(() =>
                    {
                        check_button.gameObject.SetActive(false);
                        next_button.gameObject.SetActive(true);
                        option.gameObject.SetActive(false);
                        answer_count++;
                        Debug.Log(" answer_count" + answer_count);
                        if (Checkanswer_string == tempAnswerOptionID)
                        {
                            correct_answer_count++;
                            Debug.Log("  correct_answer_count" + correct_answer_count);
                            answerText.text = " 你的答案是    " + Checkanswer_string + " 解答為 " + tempAnswerOptionID + " 正確! ";
                        }
                        else
                        {
                            answerText.text = " 你的答案是    " + Checkanswer_string + " 解答為 " + tempAnswerOptionID + " 錯誤 ";
                        }
                        Solution_Question_Button.interactable = true;
                    });
                break;

            case 2: // Short Answer
                askField.text = ""; // Clear the input field
                choice.SetActive(false);
                TrueFalse.SetActive(false);
                askField.gameObject.SetActive(true);
                //隨機 取得是非題並且章節為2 的題目
                MySqlCommand command1 = new MySqlCommand("SELECT * FROM questionnare.questions WHERE QuestionType = 2 AND ChapterID = 2 ORDER BY RAND() LIMIT 1", connection);
                MySqlDataReader reader1 = command1.ExecuteReader();
                if (reader1.Read()) // 使用 if 因為只會返回一條記錄
                {
                    string random_question_descript_ask = reader1["Description"].ToString();
                    Debug.Log(random_question_descript_ask);
                    questionText.text = random_question_descript_ask;
                    Solutiontext.text = reader1["Explanation"].ToString();
                }
                if (reader1 != null && !reader1.IsClosed)
                {
                    reader1.Close();
                }
                check_button.onClick.RemoveAllListeners(); // Clear previous listeners
                check_button.onClick.AddListener( async () =>
                    {
                        check_button.gameObject.SetActive(false);
                        next_button.gameObject.SetActive(true);
                        option.gameObject.SetActive(false);
                        answer_count++;
                        Debug.Log(" answer_count" + answer_count);
                        answerText.text = "你的答案是" + Checkanswer_string + "解答評價中，正在尋求專家評價";
                        messages.Clear();
                        var newMessage = new ChatMessage()
                        {
                            Role = "user",
                            Content =judge_prompt+"\n 題目：\n"+questionText.text+ "\n使用者回答：\n"+Checkanswer_string,
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
                            Debug.Log("chat回覆：" + Response_message.Content);
                            string judge_string = "";
                            if (Response_message.Content== "評價：高"|| Response_message.Content == "評價：中")
                            {
                                judge_string = "通過!";
                                correct_answer_count++;
                                Debug.Log("  correct_answer_count" + correct_answer_count);
                            }
                            else
                            {
                                judge_string = "不通過!";
                            }
                            answerText.text = " 專家回答：" + Response_message.Content + " " + judge_string+ " 你的答案是    " + Checkanswer_string ;
                        
                        }
                        else
                        {
                            Debug.LogWarning("No text was generated from this prompt.");
                        }
                        Solution_Question_Button.interactable = true;
                    });
                break;
        }
        // 在操作完成后关闭数据库连接
        if (connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();
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
            Debug.LogError("Failed to connect to MySQL database: " + ex.Message);
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

    void DisplayResult(String trainORcheck)
    {
        if (trainmode_prefab != null)
        {
            Destroy(trainmode_prefab.gameObject);
        }
        Debug.Log("display result");
        //這裡根據正確率決定跳入哪裡，透過按鈕改變prograss變數
        //先找對話的canva
        GameObject canvas = GameObject.Find("DefaultDialogPrefab(Clone)");
        // 在 Canvas 的子物件位置創建prefab
        train_result_prefab= Instantiate(train_result, canvas.transform);
        train_result_prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        //在learningMode圖片下，尋找按鈕及文字
        Text corrent_count_text = train_result_prefab.transform.Find("corrent_count_text").GetComponent<Text>();
        Button Pass_Button = train_result_prefab.transform.Find("Pass").GetComponent<Button>();
        Button Retry_Button = train_result_prefab.transform.Find("Retry").GetComponent<Button>();
        corrent_count_text.text = correct_answer_count.ToString();
        if (correct_answer_count >= answer_count * 0.6)
        {
            Pass_Button.gameObject.SetActive(true);
            Pass_Button.onClick.AddListener(() =>
            {
                if (train_result_prefab != null)
                {
                    Destroy(train_result_prefab.gameObject);
                }
                if (trainORcheck == "true")
                {
                    progress = 8;
                }
                else
                {
                    progress = 14;
                }
            });
            Retry_Button.gameObject.SetActive(false);
        }
        else
        {
            Pass_Button.gameObject.SetActive(false);
            Retry_Button.gameObject.SetActive(true);
            Retry_Button.onClick.AddListener(() =>
            {
                if (train_result_prefab != null)
                {
                    Destroy(train_result_prefab.gameObject);
                }
                if (trainORcheck == "true")
                {
                    progress = 3;
                }
                else
                {
                    progress = 9;
                }
            });
        }
        answer_count = 0;
        correct_answer_count = 0;
    }

    //每回答一題，就insert一次，還未測試
    private void insertOrUpdateUserAnswerAnalysis(int userID, int chapterID, bool isCorrect)
    {
        // 打开数据库连接
        if (connection.State != System.Data.ConnectionState.Open)
        {
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to open the database connection: " + ex.Message);
                return;
            }
        }

        try
        {
            int incrementCorrectAnswerCount = isCorrect ? 1 : 0;
            string query = @"
            INSERT INTO UserAnswerAnalysis (UserID, ChapterID, TotalAnswerCount, CorrectAnswerCount) 
            VALUES (@UserID, @ChapterID, 1, @CorrectAnswerCount)
            ON DUPLICATE KEY UPDATE 
            TotalAnswerCount = TotalAnswerCount + 1,
            CorrectAnswerCount = CorrectAnswerCount + @CorrectAnswerCount";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", userID);
            command.Parameters.AddWithValue("@ChapterID", chapterID);
            command.Parameters.AddWithValue("@CorrectAnswerCount", incrementCorrectAnswerCount);

            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to insert/update user answer analysis: " + ex.Message);
        }
        finally
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
