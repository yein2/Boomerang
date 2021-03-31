using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager inst;


    // ----- 스코어 -----
    public int leftComboNum;// 콤보 횟수
    public int leftTargetscore;// 타겟파괴시 스코어
    public int leftContainScore; // 부메랑을 받기 직전까지 얻은 점수들을 담아놓을 변수
    
    public int rightComboNum;// 콤보 횟수
    public int rightTargetscore;// 타겟파괴시 스코어
    public int rightContainScore; // 부메랑을 받기 직전까지 얻은 점수들을 담아놓을 변수
    public int currentScore;  // 현재 스코어    
    public int excellentScore; // Excellent 클리어 점수를 담을 변수
    public int greatScore; // Great 클리어 점수를 담을 변수
    // Good은 나머지 점수



    private void Awake() {
        if(!inst)
               inst = this;
    // Left
    leftComboNum = 0; // 콤보 횟수 (왼손, 오른손)
    leftTargetscore = 500; // 타겟파괴시 스코어 (왼손, 오른손)
    leftContainScore = 0;  // 부메랑을 받기 직전까지 얻은 점수들을 담아놓을 변수 (왼손, 오른손)

    // Right
    rightComboNum = 0; // 콤보 횟수 (왼손, 오른손)
    rightTargetscore = 500; // 타겟파괴시 스코어 (왼손, 오른손)
    rightContainScore = 0;  // 부메랑을 받기 직전까지 얻은 점수들을 담아놓을 변수 (왼손, 오른손)

    // 공통 적용
    currentScore = 0; // 현재 스코어 (공통)
    excellentScore = 0; // 클리어 점수를 담을 변수 (공통)
    greatScore = 0; // Great 클리어 점수를 담을 변수 (공통)
    
    }

 


    // 함수
    

    // 부메랑이 날아가는 동안 얻은 점수들을 containScore변수에 담아주는 함수

    
    public void ContainScore(bool hand)
    {
        // 왼손
        if(!hand)
        {
            leftComboNum++; // 콤보 횟수 추가
            leftContainScore += leftTargetscore * leftComboNum; // 점수에 파괴해서 얻은 점수                             
        }        
        // 오른손
        if(hand)
        {
            rightComboNum++; // 콤보 횟수 추가
            rightContainScore += rightTargetscore * rightComboNum; // 점수에 파괴해서 얻은 점수                             
        }
    }

    // 부메랑 캐치 시에 점수 두배증가
    public void CatchBoomr(bool hand)
    {
        if(!hand)
        currentScore += leftContainScore; // 담고 있는 점수 2배증가 / 컨테이너에 있는 값을 current에 넣어줌
        if(hand)
        currentScore += rightContainScore; // 담고 있는 점수 2배증가
    }

    // 부메랑을 받을 시 또는 놓쳤을 때에 내 현재 스코어에 받아온 점수를 담음
    public void AddScore(bool hand)
    {
        if(!hand)
            //leftContainScore += leftTargetscore; // 컨테이너에 타겟 스코어를 더해줌 (캐치했을 때 사용됨)
            currentScore += leftTargetscore * leftComboNum; // 현재 스코어에 바로 타겟 스코어를 더해줌
        if(hand)
        {
            //leftContainScore += leftTargetscore; // 컨테이너에 타겟 스코어를 더해줌 (캐치했을 때 사용됨)
            currentScore += rightTargetscore * rightComboNum; // 현재 스코어에 바로 타겟 스코어를 더해줌
        }
    }

    // 안쓰일수도 있음
    public void ComboScore(bool hand)
    {
        if(!hand)
            leftComboNum++;
        if(hand)
            rightComboNum++;
         // 타겟 점수 증가
    }
    
    public void EndThrow(bool hand) // 던지기가 끝났을 때 (잡거나 / 놓치거나)
    {
        if(!hand)
        {
            leftTargetscore = 500; // 타겟점수 초기화
            leftComboNum = 0;      // 콤보 초기화
            leftContainScore = 0;  // 컨테이너 0으로 초기화
        }
        if(hand)
        {
            rightTargetscore = 500;
            rightComboNum = 0;
            rightContainScore = 0;        
        }
    }


    // 스테이지 시작하면 스코어 초기화
    public void ResetScore() 
    {
        leftContainScore = 0; // 왼쪽 오른쪽부메랑 컨테이너들 초기화
        rightContainScore = 0;        

        currentScore = 0;
        //excellentScore = 0;
        //greatScore = 0;         
    }

    // 해당 라운드 클리어 조건들
    // 엑설런트★★★ , 그레잇★★
    public void PresetClearScore(int targetCount)
    {
        excellentScore = (leftTargetscore * targetCount) * 3;
        greatScore = (leftTargetscore * targetCount) * 2;        
    }

    
}
