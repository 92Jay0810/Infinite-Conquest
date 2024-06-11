using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Player : MonoBehaviour
{
    // resource
    Text wood_text;
    Text rock_text;
    Text iron_text;
    Text coin_text;
    Text food_text;
    Text research_text;
    RawImage current_generation_Image;
    [SerializeField] Sprite[] generations_Image;
    private int wood = 0;
    private int rock = 0;
    private int iron = 0;
    private int coin = 0;
    private int food = 0;
    private int research = 0;
    private int generation = 1;
    // create_menu
    Image button_menu;
    Button update_generation_button;
    Image build_menu;
    [SerializeField] Button[] build_buttons;
    Image train_menu;
    [SerializeField] Button[] train_buttons;
    private Vector3 lastCallMenuPosition;
    // reel_menu
    Button reel_botton;
    float reel_display_timer;
    Image mutiple_reel_menu;
    Text mutiple_reel_topic;
    Button[] mutiple_reel_buttons;
    Image single_reel_menu;
    Text single_reel_topic;
    Button[] single_reel_buttons;
    bool is_Multiple_choice = true;
    Image current_reel_menu;
    //reward
    float gainResource_buff;
    float attackResource_buff;
    float gainResearch_buff;
    Image gainResource_buff_image;
    Image attackResource_buff_image;
    Image gainResearch_buff_image;
    //photon
    private PhotonView Photonview;
    void Start()
    {
        // resource
        wood_text = transform.Find("Camera/resource_canva/wood/wood_int").GetComponent<Text>();
        rock_text = transform.Find("Camera/resource_canva/rock/rock_int").GetComponent<Text>();
        iron_text = transform.Find("Camera/resource_canva/iron/iron_int").GetComponent<Text>();
        coin_text = transform.Find("Camera/resource_canva/coin/coin_int").GetComponent<Text>();
        food_text = transform.Find("Camera/resource_canva/food/food_int").GetComponent<Text>();
        research_text = transform.Find("Camera/resource_canva/research/research_int").GetComponent<Text>();
        current_generation_Image = transform.Find("Camera/resource_canva/research/generation").GetComponent<RawImage>();
        // create_menu
        button_menu = transform.Find("Camera/create_canva/button_menu").GetComponent<Image>();
        update_generation_button = transform.Find("Camera/create_canva/button_menu/UpdateGenerationButton").GetComponent<Button>();
        build_menu = transform.Find("Camera/create_canva/build_menu").GetComponent<Image>();
        train_menu = transform.Find("Camera/create_canva/train_menu").GetComponent<Image>();
        // reel_menu
        single_reel_menu = transform.Find("Camera/reel_canva/single_reel").GetComponent<Image>();
        single_reel_topic = transform.Find("Camera/reel_canva/single_reel/topicframe/topictext").GetComponent<Text>();
        single_reel_buttons = new Button[2];
        single_reel_buttons[0] = transform.Find("Camera/reel_canva/single_reel/case1").GetComponent<Button>();
        single_reel_buttons[1] = transform.Find("Camera/reel_canva/single_reel/case2").GetComponent<Button>();
        mutiple_reel_menu = transform.Find("Camera/reel_canva/mutiple_reel").GetComponent<Image>();
        mutiple_reel_topic = transform.Find("Camera/reel_canva/mutiple_reel/topicframe/topictext").GetComponent<Text>();
        mutiple_reel_buttons = new Button[4];
        mutiple_reel_buttons[0] = transform.Find("Camera/reel_canva/mutiple_reel/case1").GetComponent<Button>();
        mutiple_reel_buttons[1] = transform.Find("Camera/reel_canva/mutiple_reel/case2").GetComponent<Button>();
        mutiple_reel_buttons[2] = transform.Find("Camera/reel_canva/mutiple_reel/case3").GetComponent<Button>();
        mutiple_reel_buttons[3] = transform.Find("Camera/reel_canva/mutiple_reel/case4").GetComponent<Button>();
        reel_botton = transform.Find("Camera/reel_canva/reel_button").GetComponent<Button>();
        InitReel();
        //reward
        gainResource_buff = 1.0f;
        attackResource_buff = 1.0f;
        gainResearch_buff = 1.0f;
        gainResource_buff_image = transform.Find("Camera/reel_canva/coll-nb").GetComponent<Image>();
        attackResource_buff_image = transform.Find("Camera/reel_canva/thief-nb").GetComponent<Image>();
        gainResearch_buff_image = transform.Find("Camera/reel_canva/tech-nb").GetComponent<Image>();
        //photon
        Photonview = this.gameObject.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (Photonview.IsMine)
        {
            if (Input.GetMouseButtonDown(1))
            {
                CreateMenuDetected();
            }
            //若 build_menu打開的話，就持續檢查是否達成升級世代的條件
            if (button_menu.gameObject.activeSelf)
            {
                UpdateGenerationButtonDetected();
            }
            //若 build_menu打開的話，就持續檢查是否達成條件
            if (build_menu.gameObject.activeSelf)
            {
                Build_menu_detect();
            }
            //若 train_menu打開的話，就持續檢查是否達成條件
            if (train_menu.gameObject.activeSelf)
            {
                Train_menu_detect();
            }
            //若 reel_menu沒打開的話，就持續檢查是否要開奏摺
            if (!current_reel_menu.gameObject.activeSelf)
            {
                DetectReelButton();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                ChangeWood(500, false, false);
                ChangeRock(500, false, false);
                ChangeIron(500, false, false);
                ChangeCoin(500, false, false);
                ChangeFood(500, false, false);
                ChangeResearch(500, false);
                InitReel();
                reel_display_timer = 0.0f;
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                Debug.Log("buff增加，gainResource20%");
                gainResource_buff = 1.2f;
                //可以在類似的code 新增顯示buff圖案
            }
        }
    }
    public void ChangeWood(int number, bool gain_buff, bool attack_gain_buff)
    {
        if (gain_buff)
        {
            wood += (int)(gainResource_buff * number);
            wood_text.text = wood.ToString();
        }
        else if (attack_gain_buff)
        {
            wood += (int)(attackResource_buff * number);
            wood_text.text = wood.ToString();
        }
        else
        {
            wood += number;
            wood_text.text = wood.ToString();
        }
    }
    public void ChangeRock(int number, bool gain_buff, bool attack_gain_buff)
    {
        if (gain_buff)
        {
            rock += (int)(gainResource_buff * number);
            rock_text.text = rock.ToString();
        }
        else if (attack_gain_buff)
        {
            rock += (int)(attackResource_buff * number);
            rock_text.text = rock.ToString();
        }
        else
        {
            rock += number;
            rock_text.text = rock.ToString();
        }
    }
    public void ChangeIron(int number, bool gain_buff, bool attack_gain_buff)
    {
        if (gain_buff)
        {
            iron += (int)(gainResource_buff * number);
            iron_text.text = iron.ToString();
        }
        else if (attack_gain_buff)
        {
            iron += (int)(attackResource_buff * number);
            iron_text.text = iron.ToString();
        }
        else
        {
            iron += number;
            iron_text.text = iron.ToString();
        }
    }
    public void ChangeCoin(int number, bool gain_buff, bool attack_gain_buff)
    {
        if (gain_buff)
        {
            coin += (int)(gainResource_buff * number);
            coin_text.text = coin.ToString();
        }
        else if (attack_gain_buff)
        {
            coin += (int)(attackResource_buff * number);
            coin_text.text = coin.ToString();
        }
        else
        {
            coin += number;
            coin_text.text = coin.ToString();
        }
    }
    public void ChangeFood(int number, bool gain_buff, bool attack_gain_buff)
    {
        if (gain_buff)
        {
            food += (int)(gainResource_buff * number);
            food_text.text = food.ToString();
        }
        else if (attack_gain_buff)
        {
            food += (int)(attackResource_buff * number);
            food_text.text = food.ToString();
        }
        else
        {
            food += number;
            food_text.text = food.ToString();
        }
    }
    public void ChangeResearch(int number, bool gain_buffer)
    {
        if (gain_buffer)
        {
            research += (int)(gainResearch_buff * number);
            research_text.text = research.ToString();
        }
        else
        {
            research += number;
            research_text.text = research.ToString();
        }
    }

    void CreateMenuDetected()
    {
        Vector3 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //不要hit到Default層的場景
        int charMask = LayerMask.GetMask("Default");
        RaycastHit2D hit = Physics2D.Raycast(inputPosition, Vector2.zero, 10f, charMask);
        if (!hit)
        {
            //hit場景，場景在layer3
            int layerMask = LayerMask.GetMask("ground");
            RaycastHit2D hitGround = Physics2D.Raycast(inputPosition, Vector2.zero, 10f, layerMask);
            if (hitGround.collider != null)
            {
                if (hitGround.collider.tag == "ground")
                {
                    //點擊到地板，就顯示按鈕
                    button_menu.gameObject.transform.position = new Vector3(inputPosition.x, inputPosition.y, 0f);
                    button_menu.gameObject.SetActive(!button_menu.gameObject.activeSelf);
                    //並更新開啟菜單位置
                    lastCallMenuPosition = new Vector3(inputPosition.x, inputPosition.y, 0f);
                }
            }
        }
    }
    public void BuildButton()
    {
        button_menu.gameObject.SetActive(false);
        build_menu.gameObject.SetActive(true);

    }
    private void Build_menu_detect()
    {
        //商會
        if (wood >= 100 && rock >= 100 && iron >= 150)
        {
            build_buttons[0].interactable = true;
        }
        else
        {
            build_buttons[0].interactable = false;
        }
        //礦場
        if (wood >= 100 && rock >= 100 && iron >= 100)
        {
            build_buttons[1].interactable = true;
        }
        else
        {
            build_buttons[1].interactable = false;
        }
    }
    public void CreateQuarry()
    {
        ChangeWood(-100, false, false);
        ChangeRock(-100, false, false);
        ChangeIron(-150, false, false);
        GameObject quarry= PhotonNetwork.Instantiate("quarry", lastCallMenuPosition, Quaternion.identity);
        Quarry quarry_component = quarry.GetComponent<Quarry>();
        quarry_component.player = this.GetComponent<Player>();
        build_menu.gameObject.SetActive(false);
    }
    public void CreateMines()
    {
        ChangeWood(-100, false, false);
        ChangeRock(-100, false, false);
        ChangeIron(-100, false, false);
        GameObject mines = PhotonNetwork.Instantiate("mines", lastCallMenuPosition, Quaternion.identity);
        Mines mines_component = mines.GetComponent<Mines>();
        mines_component.player = this.GetComponent<Player>();
        build_menu.gameObject.SetActive(false);
    }
    void UpdateGenerationButtonDetected()
    {
        bool update_generation = false;
        switch (generation)
        {
            case 1:
                if (food >= 500)
                {
                    update_generation = true;
                }
                break;
            case 2:
                if (coin >= 200 && food >= 800)
                {
                    update_generation = true;
                }
                break;
            case 3:
                if (coin >= 800 && food >= 1000)
                {
                    update_generation = true;
                }
                break;
            case 4:
                if (coin >= 1500 && food >= 1200)
                {
                    update_generation = true;
                }
                break;
            case 5:
                break;
            default:
                Debug.Log("時代按鈕有錯");
                break;
        }
        if (update_generation)
        {
            update_generation_button.interactable = true;
        }
        else
        {
            update_generation_button.interactable = false;
        }
    }
    public void UpdateGenerationButton()
    {
        switch (generation)
        {
            case 1:
                ChangeFood(-500, false, false);
                current_generation_Image.texture = generations_Image[1].texture;
                break;
            case 2:
                ChangeCoin(-200, false, false);
                ChangeFood(-800, false, false);
                current_generation_Image.texture = generations_Image[2].texture;
                break;
            case 3:
                ChangeCoin(-800, false, false);
                ChangeFood(-1000, false, false);
                current_generation_Image.texture = generations_Image[3].texture;
                break;
            case 4:
                ChangeCoin(-1500, false, false);
                ChangeFood(-1200, false, false);
                current_generation_Image.texture = generations_Image[4].texture;
                break;
            default:
                Debug.Log("時代有錯");
                break;
        }
        generation += 1;
        update_generation_button.interactable = false;
        button_menu.gameObject.SetActive(false);
    }
    public void TrainButton()
    {
        button_menu.gameObject.SetActive(false);
        train_menu.gameObject.SetActive(true);
    }
    private void Train_menu_detect()
    {
        //平民
        if (food >= 25)
        {
            train_buttons[0].interactable = true;
        }
        else
        {
            train_buttons[0].interactable = false;
        }
        //士兵
        if (food >= 30)
        {
            train_buttons[1].interactable = true;
        }
        else
        {
            train_buttons[1].interactable = false;
        }
        //農民
        if (food >= 35 && generation >= 2)
        {
            train_buttons[2].interactable = true;
        }
        else
        {
            train_buttons[2].interactable = false;
        }
        //魔法師
        if (coin >= 10 && food >= 50 && generation >= 3)
        {
            train_buttons[3].interactable = true;
        }
        else
        {
            train_buttons[3].interactable = false;
        }
    }
    public void CreateFarmer()
    {
        ChangeFood(-35, false, false);
        GameObject farmer = PhotonNetwork.Instantiate("farmer", lastCallMenuPosition, Quaternion.identity);
        Farmer farmer_component = farmer.GetComponent<Farmer>();
        farmer_component.player = this.GetComponent<Player>();
        train_menu.gameObject.SetActive(false);
    }
    public void CreateMagician()
    {
        ChangeFood(-50, false, false);
        ChangeCoin(-10, false, false);
        PhotonNetwork.Instantiate("magician", lastCallMenuPosition, Quaternion.identity);
        train_menu.gameObject.SetActive(false);
    }
    public void CreateSoldier()
    {
        ChangeFood(-30, false, false);
        PhotonNetwork.Instantiate("soldier", lastCallMenuPosition, Quaternion.identity);
        train_menu.gameObject.SetActive(false);
    }
    public void CreateVillager()
    {
        ChangeFood(-25, false, false);
        GameObject villager = PhotonNetwork.Instantiate("villager", lastCallMenuPosition, Quaternion.identity);
       villager villager_component = villager.GetComponent<villager>();
        villager_component.player = this.GetComponent<Player>();
        train_menu.gameObject.SetActive(false);
    }
    private void InitReel()
    {
        reel_display_timer = Random.Range(180f, 300f);
        //模擬查詢資料庫找到題目類型、題目難度、題目描述、正確答案
        is_Multiple_choice = (Random.Range(0, 2) == 1);
        int difficult = Random.Range(0, 3) + 1; //難度1~3
        if (is_Multiple_choice)
        {
            int rightAnwer = Random.Range(0, 4);
            current_reel_menu = mutiple_reel_menu;
            for (int i = 0; i < 4; i++)
            {
                mutiple_reel_buttons[i].onClick.RemoveAllListeners();
                mutiple_reel_buttons[i].onClick.AddListener(() => Click_WrongAnswer(difficult));
            }
            mutiple_reel_topic.text = " 難度 " + difficult.ToString() + " 正確答案:　" + (rightAnwer + 1).ToString();
            mutiple_reel_buttons[rightAnwer].onClick.RemoveAllListeners();
            mutiple_reel_buttons[rightAnwer].onClick.AddListener(() => Click_RightAnswer(difficult));
        }
        else
        {
            int rightAnwer = Random.Range(0, 2);
            current_reel_menu = single_reel_menu;
            for (int i = 0; i < 2; i++)
            {
                single_reel_buttons[i].onClick.RemoveAllListeners();
                single_reel_buttons[i].onClick.AddListener(() => Click_WrongAnswer(difficult));
            }
            single_reel_topic.text = " 難度 " + difficult.ToString() + " 正確答案:　" + (rightAnwer + 1).ToString();
            single_reel_buttons[rightAnwer].onClick.RemoveAllListeners();
            single_reel_buttons[rightAnwer].onClick.AddListener(() => Click_RightAnswer(difficult));
        }
    }
    private void DetectReelButton()
    {
        if (reel_display_timer > 0.0f)
        {
            reel_display_timer -= Time.deltaTime;
        }
        else
        {
            reel_botton.gameObject.SetActive(true);
        }

    }
    public void Reel_Button()
    {
        current_reel_menu.gameObject.SetActive(true);
        reel_botton.gameObject.SetActive(false);
    }
    public void Click_RightAnswer(int difficult)
    {
        Debug.Log("RightAnswer");
        current_reel_menu.gameObject.SetActive(false);
        int reward = Random.Range(0, 4);
        if (is_Multiple_choice)
        {
            if (difficult == 1)
            {
                switch (reward)
                {
                    case 0:
                        ChangeWood(150, false, false);
                        ChangeRock(150, false, false);
                        ChangeIron(150, false, false);
                        ChangeCoin(150, false, false);
                        ChangeFood(150, false, false);
                        break;
                    case 1:
                        gainResource_buff = 1.5f;
                        break;
                    case 2:
                        attackResource_buff = 1.03f;
                        break;
                    case 3:
                        gainResearch_buff = 2f;
                        break;
                }
            }
            if (difficult == 2)
            {
                switch (reward)
                {
                    case 0:
                        ChangeWood(250, false, false);
                        ChangeRock(250, false, false);
                        ChangeIron(250, false, false);
                        ChangeCoin(250, false, false);
                        ChangeFood(250, false, false);
                        break;
                    case 1:
                        gainResource_buff = 1.7f;
                        break;
                    case 2:
                        attackResource_buff = 1.05f;
                        break;
                    case 3:
                        gainResearch_buff = 2.5f;
                        break;
                }
            }
            if (difficult == 3)
            {
                switch (reward)
                {
                    case 0:
                        ChangeWood(350, false, false);
                        ChangeRock(350, false, false);
                        ChangeIron(350, false, false);
                        ChangeCoin(350, false, false);
                        ChangeFood(350, false, false);
                        break;
                    case 1:
                        gainResource_buff = 2f;
                        break;
                    case 2:
                        attackResource_buff = 1.07f;
                        break;
                    case 3:
                        gainResearch_buff = 3f;
                        break;
                }
            }
        }
        else
        {
            if (difficult == 1)
            {
                switch (reward)
                {
                    case 0:
                        ChangeWood(100, false, false);
                        ChangeRock(100, false, false);
                        ChangeIron(100, false, false);
                        ChangeCoin(100, false, false);
                        ChangeFood(100, false, false);
                        break;
                    case 1:
                        gainResource_buff = 1.3f;
                        break;
                    case 2:
                        attackResource_buff = 1.01f;
                        break;
                    case 3:
                        gainResearch_buff = 1.3f;
                        break;
                }
            }
            if (difficult == 2)
            {
                switch (reward)
                {
                    case 0:
                        ChangeWood(150, false, false);
                        ChangeRock(150, false, false);
                        ChangeIron(150, false, false);
                        ChangeCoin(150, false, false);
                        ChangeFood(150, false, false);
                        break;
                    case 1:
                        gainResource_buff = 1.5f;
                        break;
                    case 2:
                        attackResource_buff = 1.03f;
                        break;
                    case 3:
                        gainResearch_buff = 1.5f;
                        break;
                }
            }
            if (difficult == 3)
            {
                switch (reward)
                {
                    case 0:
                        ChangeWood(250, false, false);
                        ChangeRock(250, false, false);
                        ChangeIron(250, false, false);
                        ChangeCoin(250, false, false);
                        ChangeFood(250, false, false);
                        break;
                    case 1:
                        gainResource_buff = 1.7f;
                        break;
                    case 2:
                        attackResource_buff = 1.05f;
                        break;
                    case 3:
                        gainResearch_buff = 2f;
                        break;
                }
            }
        }
        Debug.Log("gainResource_buff  " + gainResource_buff + " attackResource_buff " + attackResource_buff + "gainResearch_buff " + gainResearch_buff);
        if (reward != 0)
        {
            Active_buff(reward);
        }
        InitReel();
    }
    public void Click_WrongAnswer(int difficult)
    {
        Debug.Log("WrongAnswer");
        current_reel_menu.gameObject.SetActive(false);
        int penalty = Random.Range(0, 4);
        if (is_Multiple_choice)
        {
            if (difficult == 1)
            {
                switch (penalty)
                {
                    case 0:
                        ChangeWood(-100, false, false);
                        ChangeRock(-100, false, false);
                        ChangeIron(-100, false, false);
                        ChangeCoin(-100, false, false);
                        ChangeFood(-100, false, false);
                        break;
                    case 1:
                        gainResource_buff = 0.5f;
                        break;
                    case 2:
                        attackResource_buff = 0.97f;
                        break;
                    case 3:
                        gainResearch_buff = 0.3f;
                        break;
                }
            }
            if (difficult == 2)
            {
                switch (penalty)
                {
                    case 0:
                        ChangeWood(-70, false, false);
                        ChangeRock(-70, false, false);
                        ChangeIron(-70, false, false);
                        ChangeCoin(-70, false, false);
                        ChangeFood(-70, false, false);
                        break;
                    case 1:
                        gainResource_buff = 0.7f;
                        break;
                    case 2:
                        attackResource_buff = 0.98f;
                        break;
                    case 3:
                        gainResearch_buff = 0.5f;
                        break;
                }
            }
            if (difficult == 3)
            {
                switch (penalty)
                {
                    case 0:
                        ChangeWood(-50, false, false);
                        ChangeRock(-50, false, false);
                        ChangeIron(-50, false, false);
                        ChangeCoin(-50, false, false);
                        ChangeFood(-50, false, false);
                        break;
                    case 1:
                        gainResource_buff = 0.9f;
                        break;
                    case 2:
                        attackResource_buff = 0.99f;
                        break;
                    case 3:
                        gainResearch_buff = 0.75f;
                        break;
                }
            }
        }
        else
        {
            if (difficult == 1)
            {
                switch (penalty)
                {
                    case 0:
                        ChangeWood(-80, false, false);
                        ChangeRock(-80, false, false);
                        ChangeIron(-80, false, false);
                        ChangeCoin(-80, false, false);
                        ChangeFood(-80, false, false);
                        break;
                    case 1:
                        gainResource_buff = 0.3f;
                        break;
                    case 2:
                        attackResource_buff = 0.95f;
                        break;
                    case 3:
                        gainResearch_buff = 0f;
                        break;
                }
            }
            if (difficult == 2)
            {
                switch (penalty)
                {
                    case 0:
                        ChangeWood(-50, false, false);
                        ChangeRock(-50, false, false);
                        ChangeIron(-50, false, false);
                        ChangeCoin(-50, false, false);
                        ChangeFood(-50, false, false);
                        break;
                    case 1:
                        gainResource_buff = 0.5f;
                        break;
                    case 2:
                        attackResource_buff = 0.97f;
                        break;
                    case 3:
                        gainResearch_buff = 0.5f;
                        break;
                }
            }
            if (difficult == 3)
            {
                switch (penalty)
                {
                    case 0:
                        ChangeWood(-30, false, false);
                        ChangeRock(-30, false, false);
                        ChangeIron(-30, false, false);
                        ChangeCoin(-30, false, false);
                        ChangeFood(-30, false, false);
                        break;
                    case 1:
                        gainResource_buff = 0.9f;
                        break;
                    case 2:
                        attackResource_buff = 0.99f;
                        break;
                    case 3:
                        gainResearch_buff = 0.7f;
                        break;
                }
            }

        }
        Debug.Log("gainResource_buff  " + gainResource_buff + " attackResource_buff " + attackResource_buff + "gainResearch_buff " + gainResearch_buff);
        //只有處罰0 沒有buff圖案
        if (penalty != 0)
        {
            Active_buff(penalty);
        }
        InitReel();
    }
    private void Active_buff(int change_buff_index)
    {
        float buffDuration = Random.Range(120f, 180f);
        switch (change_buff_index)
        {
            case 1:
                gainResource_buff_image.gameObject.SetActive(true);
                StartCoroutine(BuffCoroutine(gainResource_buff_image, buffDuration, change_buff_index));
                break;
            case 2:
                attackResource_buff_image.gameObject.SetActive(true);
                StartCoroutine(BuffCoroutine(attackResource_buff_image, buffDuration, change_buff_index));
                break;
            case 3:
                gainResearch_buff_image.gameObject.SetActive(true);
                StartCoroutine(BuffCoroutine(gainResearch_buff_image, buffDuration, change_buff_index));
                break;
        }
    }
    private IEnumerator BuffCoroutine(Image buffimage, float buffDuration, int buffer_index)
    {
        // 等待 buffDuration 秒
        yield return new WaitForSeconds(buffDuration);
        // 隱藏圖片
        buffimage.gameObject.SetActive(false);
        switch (buffer_index)
        {
            case 1:
                gainResource_buff = 1.0f;
                break;
            case 2:
                attackResource_buff = 1.0f;
                break;
            case 3:
                gainResearch_buff = 1.0f;
                break;
        }
    }
    public void Cross()
    {
        build_menu.gameObject.SetActive(false);
        train_menu.gameObject.SetActive(false);
        current_reel_menu.gameObject.SetActive(false);
    }
}
