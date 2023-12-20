using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] Stage[] stage;
    [SerializeField] TitleUI titleUI;
    [SerializeField] InGameUI inGameUI;
    [SerializeField] WaitBoard waitBoard;
    [SerializeField] ScoreBoard scoreBoard;

    [SerializeField] CardTouchEvent touchEvent;
    [SerializeField] Transform floatingPoint;
    [SerializeField] Transform particlePoint;

    private int currStage = 0;

    private Coroutine timerRoutine;
    private Stack<Card> revertStack;
    private Queue<bool> matchingQueue;
    private float comboMult = 0.8f;
    private int gainScoreStage = 0;

    private int prevHighScore;
    private int prevHighStage;
    private StageHistory[] prevHistory;

    void Awake()
    {
        revertStack = new Stack<Card>();
        matchingQueue = new Queue<bool>();
        GameManager.Data.History = new StageHistory[stage.Length];

        if (GameManager.Stage == null)
        {
            GameManager.Stage = this;
        }

        gameObject.SetActive(false);

        inGameUI.backBtn.onClick.AddListener(() => BackTitle());
        inGameUI.revertBtn.onClick.AddListener(() => Revert());
        inGameUI.resetBtn.onClick.AddListener(() => ResetStage());
    }

    public void StageStart()
    {
        gameObject.SetActive(true);
        inGameUI.gameObject.SetActive(true);

        prevHighScore = GameManager.Data.HighScore;
        prevHighStage = GameManager.Data.HighStage;
        prevHistory = new StageHistory[GameManager.Data.History.Length];

        for (int i = 0; i < GameManager.Data.History.Length; i++)
        {
            prevHistory[i].BestScore = GameManager.Data.History[i].BestScore;
            prevHistory[i].BestTime = GameManager.Data.History[i].BestTime;
        }

        StartCoroutine(WaitTimer());

        GameManager.Sound.PlayMusic("InGame");
    }

    public void UpdateCard(Card card)
    {
        Card otherFlipCard = stage[currStage].cardDeck.GetFlippedOtherCard(card);

        if (otherFlipCard == null)
        {
            revertStack.Push(card);
            return;
        }

        if (card.cardData.Suit == otherFlipCard.cardData.Suit &&
            card.cardData.Rank == otherFlipCard.cardData.Rank)
        {
            matchingQueue.Enqueue(true);

            card.isCompleted = true;
            otherFlipCard.isCompleted = true;
            revertStack.Clear();

            // 점수 추가 
            CalculateScore();
            GameManager.Sound.PlaySFX("Match");

            if (CheckGameState())
            {
                // 스테이지 클리어
                SuccessEffect();
                RecordHistory();
                CalculateTimeBonus();

                if (currStage == stage.Length - 1)
                {
                    EndGame(true);
                }
                else
                {
                    ChangeNextStage();
                }
            }
        }
        else
        {
            StartCoroutine(card.FlipCard(false));
            StartCoroutine(otherFlipCard.FlipCard(false));

            revertStack.Clear();
            matchingQueue.Clear();
        }
    }

    private void CalculateScore()
    {
        int additionalScore = (int)(stage[currStage].bonus * (matchingQueue.Count - 1) * comboMult);
        GameManager.Data.CurrScore += stage[currStage].bonus + additionalScore;
        gainScoreStage += stage[currStage].bonus + additionalScore;
        inGameUI.SetScore(GameManager.Data.CurrScore);

        if (matchingQueue.Count > 1)
        {
            GameManager.UI.SetFloating(floatingPoint.gameObject, $"{matchingQueue.Count - 1} Combo");
        }
    }

    private void CalculateTimeBonus()
    {
        int timeBonus = 0;

        if ((stage[currStage].orgTimer / 2) < stage[currStage].timer)
        {
            timeBonus = stage[currStage].bonus * 2;
            GameManager.Data.CurrScore += timeBonus;
        }

        waitBoard.SetNotice($"Time Bonus : {timeBonus}\nCurrent Point : {GameManager.Data.CurrScore}");
    }

    private bool CheckGameState()
    {
        foreach (Card card in stage[currStage].cardDeck.instanceCards)
        {
            if (!card.isCompleted)
            {
                return false;
            }
        }

        return true;
    }

    private void ChangeNextStage()
    {
        StopCoroutine(timerRoutine);

        StartCoroutine(StageChangeRoutine());
    }

    IEnumerator StageChangeRoutine()
    {
        yield return new WaitForSeconds(1.5f);

        stage[currStage].gameObject.SetActive(false);
        currStage++;
        gainScoreStage = 0;
        revertStack.Clear();

        StartCoroutine(WaitTimer());
    }

    IEnumerator StageTimer()
    {
        while (stage[currStage].timer > 0)
        {
            inGameUI.SetTime((int)stage[currStage].timer);
            stage[currStage].timer -= Time.deltaTime;
            yield return null;
        }

        EndGame(false);
    }

    IEnumerator WaitTimer()
    {
        touchEvent.enabled = false;
        waitBoard.gameObject.SetActive(true);
        inGameUI.gameObject.SetActive(false);

        float time = 5f;

        while (time > 0)
        {
            waitBoard.SetTimer((int)time);
            time -= Time.deltaTime;
            yield return null;
        }

        touchEvent.enabled = true;
        waitBoard.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(true);
        
        inGameUI.SetStage(currStage + 1);
        inGameUI.SetScore(GameManager.Data.CurrScore);

        stage[currStage].gameObject.SetActive(true);
        stage[currStage].cardDeck.CreateDeck();

        timerRoutine = StartCoroutine(StageTimer());
    }

    private void EndGame(bool clear)
    {
        scoreBoard.gameObject.SetActive(true);

        bool updateScore = GameManager.Data.CurrScore > GameManager.Data.HighScore;
        bool updateStage = currStage > GameManager.Data.HighStage;

        if (updateScore)
        {
            GameManager.Data.HighScore = GameManager.Data.CurrScore;
        }

        if (updateStage)
        {
            GameManager.Data.HighStage = currStage;
        }

        GameManager.Sound.StopMusic();

        if (clear)
        {
            GameManager.Sound.PlaySFX("Victory");
        }
        else
        {
            GameManager.Sound.PlaySFX("Fail");
        }

        RecordHistory();
        scoreBoard.ShowBoard(clear, updateScore, currStage);
        AfterProcess();
    }

    private void AfterProcess()
    {
        gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(false);
        stage[currStage].gameObject.SetActive(false);

        GameManager.Data.CurrScore = 0;
        gainScoreStage = 0;
        currStage = 0;
        revertStack.Clear();
        waitBoard.SetNotice("");

        inGameUI.SetScore(GameManager.Data.CurrScore);
    }

    private void BackTitle()
    {
        AfterProcess();
        titleUI.gameObject.SetActive(true);

        GameManager.Data.HighScore = prevHighScore;
        GameManager.Data.HighStage = prevHighStage;

        for (int i = 0; i < GameManager.Data.History.Length; i++)
        {
            GameManager.Data.History[i].BestScore = prevHistory[i].BestScore;
            GameManager.Data.History[i].BestTime = prevHistory[i].BestTime;
        }

        GameManager.Sound.PlaySFX("BtnClick");
    }

    private void Revert()
    {
        if (revertStack.Count <= 0)
        {
            return;
        }

        Card card = revertStack.Pop();

        if (card.isCompleted)
        {
            card.isCompleted = false;
        }

        StartCoroutine(card.FlipCard(false));

        GameManager.Sound.PlaySFX("BtnClick");
    }

    private void ResetStage()
    {
        StopCoroutine(timerRoutine);
        GameManager.Data.CurrScore -= gainScoreStage;
        
        stage[currStage].gameObject.SetActive(false);
        gainScoreStage = 0;
        revertStack.Clear();

        StartCoroutine(WaitTimer());

        GameManager.Sound.PlaySFX("BtnClick");
    }

    private void RecordHistory()
    {
        if (GameManager.Data.History[currStage].BestScore < gainScoreStage)
        {
            GameManager.Data.History[currStage].BestScore = gainScoreStage;
        }

        if (GameManager.Data.History[currStage].BestTime < (int)stage[currStage].timer)
        {
            GameManager.Data.History[currStage].BestTime = (int)stage[currStage].timer;
        }
    }
    
    private void SuccessEffect()
    {
        StartCoroutine(CreateSuccessParticle());
        GameManager.Sound.PlaySFX("Success");
    }

    private IEnumerator CreateSuccessParticle()
    {
        for (int i = 0; i < 3; i++)
        {
            float randomX = UnityEngine.Random.Range(-1f, 1f);
            float randomZ = UnityEngine.Random.Range(-1f, 1f);
            Vector3 position = new Vector3(particlePoint.position.x + randomX, particlePoint.position.y, particlePoint.position.z + randomZ);

            GameObject particle = Instantiate(Resources.Load<GameObject>("Particle/Victory"), position, Quaternion.identity, particlePoint);
            Destroy(particle.gameObject, 2f);
            yield return new WaitForSeconds(0.4f);
        }
    }
}