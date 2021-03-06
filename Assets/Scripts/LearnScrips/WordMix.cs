using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


public class WordMix : MonoBehaviour
{

	public Text wordLeft;
	public Text lvlNumber;

	public InputField solutionChild;

	private string mixedWord;
	private string solution;
	public Image imageColor;

	private int lvlCount = 0;
	private int lvlAmount = 10;
	private int wrongChoices = 0;
	private int randomNumber = 0;

	//json file
	public TextAsset textJson;
	public WordList list = new WordList();

	//button
	public Button testWord;
	public Button menu;

	void Start()
	{
		testWord.interactable = false;
		menu.onClick.AddListener(() => GoBack());
		lvlAmount = MenuPickLevelAdvanced.lvlAmmountStatic;
		testWord.onClick.AddListener(() => ButtonClicked());
		solutionChild.onValueChanged.AddListener(delegate {EnableButton(); });
		ReadJsonFile();
		PlayGame();

	}

	public void EnableButton(){
		if(String.IsNullOrEmpty(solutionChild.text)){
			testWord.interactable = false;
			solutionChild.interactable = true;
		}
		else{
			testWord.interactable = true;
		}
	}

	public void GoBack(){
		MenuPickLevelAdvanced.maxNumberStatic = 0;
		MenuPickLevelAdvanced.lvlAmmountStatic = 0;
		MenuPickLevelAdvanced.fourChoices = 0;
		SceneManager.LoadScene("MenuLearning");
	}


	public void PlayGame(){
		lvlCount++;
		if(lvlCount > lvlAmount){
			PlayerPrefs.SetInt("wrongAnswers", wrongChoices);
			SceneManager.LoadScene("LearnFinishScreen");
			Debug.Log("Game Vorbei \n" + "Anzahl Fehler: " + wrongChoices);
		}
		//change color to white
		imageColor.color = new Color32(255, 255, 255, 255);
		if(lvlCount <= lvlAmount)	lvlNumber.text = "Level: " + lvlCount + "/" + lvlAmount;
		GetWord();
		wordLeft.text = mixedWord;
	}

	public void GetWord(){
		randomNumber = GetRandomNumbers();
		Debug.Log("random Number: " + randomNumber);
		Debug.Log("lenght list: " + list.words.Length);
		solution = list.words.GetValue(randomNumber).ToString();
		solutionChild.characterLimit = solution.Length;
		mixedWord = mixWord(solution);
		Debug.Log(mixedWord);
		Debug.Log("Soltion: " + solution);
	}

	public string mixWord(string word){
		string finishedMixing = "";
		System.Random rnd = new System.Random();
		SortedList<int,char> list = new SortedList<int,char>();
		foreach(char c in word)
			list.Add(rnd.Next(), c);
		foreach(var x in list){
			finishedMixing += x.Value.ToString();
		}
		if(word.Equals(finishedMixing)){
			mixWord(word);
		}
		return finishedMixing;
	}

	public int GetRandomNumbers(){
		if (list.words.Length != 0) {
			return UnityEngine.Random.Range(0, list.words.Length);
		}
		else{
			return -1;
		}
	}

	public void ReadJsonFile(){
		list = JsonUtility.FromJson<WordList>(textJson.text);
	}


	public void ButtonClicked(){
		testWord.interactable = false;
		StartCoroutine(waiter(1));
	}

	IEnumerator waiter(int sec){
		solutionChild.interactable = false;
		if(lvlCount <= lvlAmount){
			if(solution.Equals(solutionChild.text)){
				//change color green
				imageColor.color = new Color32(37, 250, 53, 255);
				yield return new WaitForSeconds(sec);
				solutionChild.text = "";
				PlayGame();
			}
			else{
				//change color red
				imageColor.color = new Color32(251, 37, 37, 255);
				yield return new WaitForSeconds(sec);
				//change color to white
				imageColor.color = new Color32(255, 255, 255, 255);
				solutionChild.text = "";
				wrongChoices++;
			}
		}
	}
}

[System.Serializable]
public class WordList
{
	public string[] words;
}