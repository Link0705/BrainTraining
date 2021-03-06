using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MathOperations : MonoBehaviour
{

	public Text TextNumberLeft;
	public Text TextNumberRight;
	public Text operatorSymbole;
	public Text showLevelNumber;

	public InputField solutionNumber;

	public Button checkButton;
	public Button menu;

	public Image imageColor;

	private int leftNumber;
	private int rightNumber;
	private int solution;
	private int maxRandomNumber = 10;
	private int minRandomNumber = 0;
	private int lvlNumber = 10;
	private int countLvl = 0;
	//add = 1, sub = 2, mult = 3, div = 4
	private int gameMode = 3;
	private int wrongSolution = 0;

	// Start is called before the first frame update
	void Start()
	{
		menu.onClick.AddListener(() => GoBack());
		lvlNumber = MenuPickLevelAdvanced.lvlAmmountStatic;
		gameMode = MenuPickLevelAdvanced.fourChoices;
		checkButton.onClick.AddListener(() => buttonClick());
		checkButton.interactable = false;
		solutionNumber.onValueChanged.AddListener(delegate {EnableButton(); });
		PlayGame(gameMode);
	}


	public void EnableButton(){
		if(string.IsNullOrEmpty(solutionNumber.text)){
			checkButton.interactable = false;
			solutionNumber.interactable = true;
		}
		else{
			checkButton.interactable = true;
		}
	}

	public void GoBack(){
		MenuPickLevelAdvanced.maxNumberStatic = 0;
		MenuPickLevelAdvanced.lvlAmmountStatic = 0;
		MenuPickLevelAdvanced.fourChoices = 0;
		SceneManager.LoadScene("MenuLearning");
	}

	public void PlayGame(int gameMode){
		countLvl++;
		if(countLvl > lvlNumber){
			PlayerPrefs.SetInt("wrongAnswers", wrongSolution);
			SceneManager.LoadScene("LearnFinishScreen");
			Debug.Log("Game Vorbei \n" + "Anzahl Fehler: " + wrongSolution);
		}
		//change color to white
		imageColor.color = new Color32(255, 255, 255, 255);

		if(countLvl <= lvlNumber)	showLevelNumber.text = "Level: " + countLvl + "/" + lvlNumber;
		solutionNumber.text = "";
		if(gameMode == 4){
			operatorSymbole.text = "/";
			minRandomNumber = 1;
			leftNumber = RandomNumbers(minRandomNumber, maxRandomNumber);
			rightNumber = RandomNumbers(minRandomNumber, maxRandomNumber);
			solution = leftNumber * rightNumber;
			SetNumbersLeftAndRight(solution, rightNumber);
			return;
		}
		else if(gameMode == 2){
			minRandomNumber = 0;
			leftNumber = RandomNumbers(minRandomNumber, maxRandomNumber);
			rightNumber = RandomNumbers(minRandomNumber, maxRandomNumber);
			if(leftNumber < rightNumber){
				Debug.Log(" leftNumber: " + leftNumber + " rightNumber: " + rightNumber + "zweite If");
				SetNumbersLeftAndRight(rightNumber, leftNumber);
				solution = rightNumber - leftNumber;
			}
			else{
				Debug.Log(" leftNumber: " + leftNumber + " rightNumber: " + rightNumber + "zweite If");
				SetNumbersLeftAndRight(leftNumber, rightNumber);
				solution = leftNumber - rightNumber;
			}
			operatorSymbole.text = "-";
			return;
		}
		else{
			minRandomNumber = 0;
			leftNumber = RandomNumbers(minRandomNumber, maxRandomNumber);
			rightNumber = RandomNumbers(minRandomNumber, maxRandomNumber);
			SetNumbersLeftAndRight(leftNumber, rightNumber);
			if(gameMode == 1){
				operatorSymbole.text = "+";
				solution = leftNumber + rightNumber;
			}
			else{
				operatorSymbole.text = "*";
				solution = leftNumber * rightNumber;
			}
			return;
		}
	}

	public int RandomNumbers(int minRandomNumber, int maxRandomNumber){
		return UnityEngine.Random.Range(minRandomNumber, maxRandomNumber);
	}


	public void SetNumbersLeftAndRight(int left, int right){
			TextNumberLeft.text = left.ToString();
			TextNumberRight.text = right.ToString();
	}

	public void buttonClick(){
		checkButton.interactable = false;
		if(gameMode == 4){
			StartCoroutine(waiterDiv(1));
		}
		else{
			StartCoroutine(waiter(1));
		}
	}

	IEnumerator waiter(int sec){
		solutionNumber.interactable = false;
		if(countLvl <= lvlNumber){
			if(int.Parse(solutionNumber.text) == solution){
				//change color green
				imageColor.color = new Color32(37, 250, 53, 255);
				yield return new WaitForSeconds(sec);
				PlayGame(gameMode);
			}
			else{
				wrongSolution++;
				//change color red
				imageColor.color = new Color32(251, 37, 37, 255);
				yield return new WaitForSeconds(sec);
				//change color to white
				imageColor.color = new Color32(255, 255, 255, 255);
				solutionNumber.text = "";
			}
		}
	}

	IEnumerator waiterDiv(int sec){
		solutionNumber.interactable = false;
		if(countLvl <= lvlNumber){
			if(int.Parse(solutionNumber.text) == leftNumber){
				//change color green
				imageColor.color = new Color32(37, 250, 53, 255);
				yield return new WaitForSeconds(sec);
				PlayGame(gameMode);
			}
			else{
				wrongSolution++;
				//change color red
				imageColor.color = new Color32(251, 37, 37, 255);
				yield return new WaitForSeconds(sec);
				//change color to white
				imageColor.color = new Color32(255, 255, 255, 255);
				solutionNumber.text = "";
			}
		}
	}
}
