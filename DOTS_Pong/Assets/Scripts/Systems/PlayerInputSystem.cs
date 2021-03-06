﻿using System;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class PlayerInputSystem : JobComponentSystem
{
	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		Entities
		.WithoutBurst()
		.ForEach((ref PaddleMovementData moveData, in PaddleInputData inputData) =>
		{
			moveData.direction = 0;

			moveData.direction += Input.GetKey(inputData.upKey) ? 1 : 0;
			moveData.direction -= Input.GetKey(inputData.downKey) ? 1 : 0;

			BrainFlowData bfScript = GameManager.main.brainflowPrefab.GetComponent<BrainFlowData>();
			if (bfScript.isBoardNull())
            {
				return;
            }

			
			//double data = (BrainFlowData.ratios[6] + BrainFlowData.ratios[7]) /  2;
			//Debug.Log(data);
			double emg_thresh = .2d;
			double data = BrainFlowData.ratios[0];
			GameManager.main.updatePlayerData(0, string.Format("{0:N2}", data));
			moveData.direction += data > emg_thresh ? 1 : 0;

			double data2 = BrainFlowData.ratios[1];
			//Debug.Log(data2);
			GameManager.main.updatePlayerData(1, string.Format("{0:N2}", data2));
			moveData.direction += data2 > emg_thresh ? -1 : 0;
			

			/*
			double data3 = BrainFlowData.accelData[BrainFlowData.accelData.Length - 2];
			GameManager.main.updatePlayerAccelData(string.Format("{0:N2}", data3));
			int direction = data3 > 0 ? 1 : -1;
			moveData.direction += Math.Abs(data3) > .4f ? 1 * direction : 0;
			*/

		}).Run();

		return default;
	}
}
