using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
	private enum TStrikes {Rock, Paper, Scissors, Unknown};
	public int Response;

	struct TWorkingMemory {
		public TStrikes strikeA; //previous strike
		public TStrikes strikeB; //previous strike
		public TStrikes strikeC; //predicted strike
	}

	class TRule{
		public TRule(){
			matched = false;
			weight = 0;
		}

		public void SetRule(TStrikes A, TStrikes B, TStrikes C){
			antecedentA = A;
			antecedentB = B;
			consequentC = C;
		}

		public TStrikes antecedentA; // user input
		public TStrikes antecedentB; // user input
		public TStrikes consequentC; // prediction
		public bool matched;
		public int weight;
	}

	const int NUM_RULES = 27;
	TWorkingMemory WorkingMemory = new TWorkingMemory();
	private TRule[] Rules = new TRule[NUM_RULES];
	int PreviousRuleFired = -1;
	TStrikes Prediction;
	TStrikes RandomPrediction = (TStrikes)0;
	int N=0;
	int NSuccess=0;
	int NRandomSuccess=0;

	void Start (){
		for (int i = 0; i < 27; i++) {
			Rules [i] = new TRule ();
		}
		Rules[0].SetRule(TStrikes.Rock, TStrikes.Rock, TStrikes.Rock);
		Rules[1].SetRule(TStrikes.Rock, TStrikes.Rock, TStrikes.Paper);
		Rules[2].SetRule(TStrikes.Rock, TStrikes.Rock, TStrikes.Scissors);
		Rules[3].SetRule(TStrikes.Rock, TStrikes.Paper, TStrikes.Rock);
		Rules[4].SetRule(TStrikes.Rock, TStrikes.Paper, TStrikes.Paper);
		Rules[5].SetRule(TStrikes.Rock, TStrikes.Paper, TStrikes.Scissors);
		Rules[6].SetRule(TStrikes.Rock, TStrikes.Scissors, TStrikes.Rock);
		Rules[7].SetRule(TStrikes.Rock, TStrikes.Scissors, TStrikes.Paper);
		Rules[8].SetRule(TStrikes.Rock, TStrikes.Scissors, TStrikes.Scissors);
		Rules[9].SetRule(TStrikes.Paper, TStrikes.Rock, TStrikes.Rock);
		Rules[10].SetRule(TStrikes.Paper, TStrikes.Rock, TStrikes.Paper);
		Rules[11].SetRule(TStrikes.Paper, TStrikes.Rock, TStrikes.Scissors);
		Rules[12].SetRule(TStrikes.Paper, TStrikes.Paper, TStrikes.Rock);
		Rules[13].SetRule(TStrikes.Paper, TStrikes.Paper, TStrikes.Paper);
		Rules[14].SetRule(TStrikes.Paper, TStrikes.Paper, TStrikes.Scissors);
		Rules[15].SetRule(TStrikes.Paper, TStrikes.Scissors, TStrikes.Rock);
		Rules[16].SetRule(TStrikes.Paper, TStrikes.Scissors, TStrikes.Paper);
		Rules[17].SetRule(TStrikes.Paper, TStrikes.Scissors, TStrikes.Scissors);
		Rules[18].SetRule(TStrikes.Scissors, TStrikes.Rock, TStrikes.Rock);
		Rules[19].SetRule(TStrikes.Scissors, TStrikes.Rock, TStrikes.Paper);
		Rules[20].SetRule(TStrikes.Scissors, TStrikes.Rock, TStrikes.Scissors);
		Rules[21].SetRule(TStrikes.Scissors, TStrikes.Paper, TStrikes.Rock);
		Rules[22].SetRule(TStrikes.Scissors, TStrikes.Paper, TStrikes.Paper);
		Rules[23].SetRule(TStrikes.Scissors, TStrikes.Paper, TStrikes.Scissors);
		Rules[24].SetRule(TStrikes.Scissors, TStrikes.Scissors, TStrikes.Rock);
		Rules[25].SetRule(TStrikes.Scissors, TStrikes.Scissors, TStrikes.Paper);
		Rules[26].SetRule(TStrikes.Scissors, TStrikes.Scissors, TStrikes.Scissors);
		WorkingMemory.strikeA = TStrikes.Unknown;
		WorkingMemory.strikeB = TStrikes.Unknown;
		WorkingMemory.strikeC = TStrikes.Unknown;
	}

	TStrikes ProcessMove(TStrikes move){
		int i;
		int RuleToFire = -1;

		// Part 1:
		if(WorkingMemory.strikeA == TStrikes.Unknown)
		{
			WorkingMemory.strikeA = move;
			return TStrikes.Unknown;
		}
		if(WorkingMemory.strikeB == TStrikes.Unknown)
		{
			WorkingMemory.strikeB = move;
			return TStrikes.Unknown;
		}

		// Part 2:
		// Process previous prediction first
		// Tally and adjust weights
		N++;
		if(move == WorkingMemory.strikeC)
		{
			NSuccess++;
			print (PreviousRuleFired);
			if(PreviousRuleFired != -1)
				Rules[PreviousRuleFired].weight++;
		} else {
			if(PreviousRuleFired != -1)
				Rules[PreviousRuleFired].weight--;
			// Backward chain to increment the rule that
			// should have been fired:
			for(i=0; i<NUM_RULES; i++)
			{
				if(Rules[i].matched && (Rules[i].consequentC == move))
				{
					Rules[i].weight++;
					break;
				}
			}
		}
		if(move == RandomPrediction)
			NRandomSuccess++;
		// Roll back
		WorkingMemory.strikeA = WorkingMemory.strikeB;
		WorkingMemory.strikeB = move;

		// Part 3:
		// Now make new prediction
		for(i=0; i<NUM_RULES; i++)
		{
			if(Rules[i].antecedentA == WorkingMemory.strikeA &&
				Rules[i].antecedentB == WorkingMemory.strikeB)
				Rules[i].matched = true;
			else
				Rules[i].matched = false;
		}
		// Pick the matched rule with the highest weight...
		RuleToFire = -1;
		for(i=0; i<NUM_RULES; i++)
		{
			if(Rules[i].matched)
			{
				if(RuleToFire == -1)
					RuleToFire = i;
				else if(Rules[i].weight > Rules[RuleToFire].weight)
					RuleToFire = i;
			}
		}

		// Fire the rule
		if(RuleToFire != -1) {
			WorkingMemory.strikeC = Rules[RuleToFire].consequentC;
			PreviousRuleFired = RuleToFire;
		} else {
			WorkingMemory.strikeC = TStrikes.Unknown;
			PreviousRuleFired = -1;
		}
		return WorkingMemory.strikeC;
	}

	void Update(){
		if (Input.GetKeyDown ("r")) {
			ProcessMove((TStrikes)1);
			Response = (int)WorkingMemory.strikeC;
			RandomPrediction = (TStrikes)Random.Range (0, 2);
		}
		if (Input.GetKeyDown ("p")) {
			ProcessMove((TStrikes)2);
			Response = (int)WorkingMemory.strikeC;
			RandomPrediction = (TStrikes)Random.Range (0, 2);
		}	
		if (Input.GetKeyDown ("s")) {
			ProcessMove((TStrikes)0);
			Response = (int)WorkingMemory.strikeC;
			RandomPrediction = (TStrikes)Random.Range (0, 2);
		}				
	}
}
