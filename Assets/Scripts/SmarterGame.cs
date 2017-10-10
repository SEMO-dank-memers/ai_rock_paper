using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmarterGame : MonoBehaviour {

	private enum TStrikes {Rock, Paper, Scissors };
	private int[] NAB = new int[9];
	private int[,] NCAB = new int[9,3];
	TStrikes[] AB = new TStrikes[2];
	TStrikes Response = new TStrikes();
	double ProbRock;
	double ProbPaper;
	double ProbScissors;
	TStrikes Prediction;
	int N;
	int NSuccess;
	enum State { ROCK, PAPER, SCISSORS, UNDEFINED };
	public Button btnRock, btnPaper, btnScissors;
	public Sprite spriteRock, spritePaper, spriteScissors;
	private SpriteRenderer spriteRenderer;
	bool hasPlayerInput = false;
	State playerState = new State();

	TStrikes ProcessMove(TStrikes move)
	{
		int i = 0;
		int j = 0;
		N++;
		if (move == Prediction) NSuccess++;
		if ((AB[0] == TStrikes.Rock) && (AB[1] == TStrikes.Rock)) i = 0;
		if ((AB[0] == TStrikes.Rock) && (AB[1] == TStrikes.Paper)) i = 1;
		if ((AB[0] == TStrikes.Rock) && (AB[1] == TStrikes.Scissors)) i = 2;
		if ((AB[0] == TStrikes.Paper) && (AB[1] == TStrikes.Rock)) i = 3;
		if ((AB[0] == TStrikes.Paper) && (AB[1] == TStrikes.Paper)) i = 4;
		if ((AB[0] == TStrikes.Paper) && (AB[1] == TStrikes.Scissors)) i = 5;
		if ((AB[0] == TStrikes.Scissors) && (AB[1] == TStrikes.Rock)) i = 6;
		if ((AB[0] == TStrikes.Scissors) && (AB[1] == TStrikes.Paper)) i = 7;
		if ((AB[0] == TStrikes.Scissors) && (AB[1] == TStrikes.Scissors)) i = 8;
		if (move == TStrikes.Rock) j = 0;
		if (move == TStrikes.Paper) j = 1;
		if (move == TStrikes.Scissors) j = 2;
		NAB[i]++;
		NCAB[i,j]++;
		AB[0] = AB[1];
		AB[1] = move;
		if ((AB[0] == TStrikes.Rock) && (AB[1] == TStrikes.Rock)) i = 0;
		if ((AB[0] == TStrikes.Rock) && (AB[1] == TStrikes.Paper)) i = 1;
		if ((AB[0] == TStrikes.Rock) && (AB[1] == TStrikes.Scissors)) i = 2;
		if ((AB[0] == TStrikes.Paper) && (AB[1] == TStrikes.Rock)) i = 3;
		if ((AB[0] == TStrikes.Paper) && (AB[1] == TStrikes.Paper)) i = 4;
		if ((AB[0] == TStrikes.Paper) && (AB[1] == TStrikes.Scissors)) i = 5;
		if ((AB[0] == TStrikes.Scissors) && (AB[1] == TStrikes.Rock)) i = 6;
		if ((AB[0] == TStrikes.Scissors) && (AB[1] == TStrikes.Paper)) i = 7;
		if ((AB[0] == TStrikes.Scissors) && (AB[1] == TStrikes.Scissors)) i = 8;
		ProbRock = (double)NCAB[i,0] / (double)NAB[i];
		ProbPaper = (double)NCAB[i,1] / (double)NAB[i];
		ProbScissors = (double)NCAB[i,2] / (double)NAB[i];
		if ((ProbRock > ProbPaper) &&
		    (ProbRock > ProbScissors)) {
			Prediction = TStrikes.Rock;
			return TStrikes.Rock;
		}
		if ((ProbPaper > ProbRock) &&
		    (ProbPaper > ProbScissors)) {
			Prediction = TStrikes.Paper;
			return TStrikes.Paper;
		}
		if ((ProbScissors > ProbRock) &&
		    (ProbScissors > ProbPaper)) {
			Prediction = TStrikes.Scissors;
			return TStrikes.Scissors;
		}
		Prediction = (TStrikes)UnityEngine.Random.Range(0,2); // Last resort
		return Prediction;
	}


	// Use this for initialization
	void Start () {
		for (int i = 0; i < 2; i++) {
			AB[i] = new TStrikes ();
		}
		spriteRenderer = GetComponent<SpriteRenderer>();
		if (spriteRenderer.sprite == null) spriteRenderer.sprite = spriteRock;
		playerState = State.UNDEFINED;
		btnRock.onClick.AddListener(RockyRoad);
		btnPaper.onClick.AddListener(PaperMoon);
		btnScissors.onClick.AddListener(DontRunWithThis);
	}

	// Update is called once per frame
	void Update(){
		if (hasPlayerInput) {
			if (playerState == State.ROCK) {
				Response = ProcessMove ((TStrikes)1);
				print (Response);
			}
			if (playerState == State.PAPER) {
				Response = ProcessMove ((TStrikes)2);
				print(Response);
			}	
			if (playerState == State.SCISSORS) {
				Response = ProcessMove ((TStrikes)0);
				print (Response);
			}
			if(Response == (TStrikes)0)
				spriteRenderer.sprite = spriteRock;
			if (Response == (TStrikes)1)
				spriteRenderer.sprite = spritePaper;
			if (Response == (TStrikes)2)
				spriteRenderer.sprite = spriteScissors;
		}
		hasPlayerInput = false;
	}

	void RockyRoad()
	{
		playerState = State.ROCK;
		hasPlayerInput = true;
	}

	void PaperMoon()
	{
		playerState = State.PAPER;
		hasPlayerInput = true;
	}

	void DontRunWithThis()
	{
		playerState = State.SCISSORS;
		hasPlayerInput = true;
	}

}