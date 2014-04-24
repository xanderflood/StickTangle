﻿using UnityEngine;
using System.Collections;

using SquareType = Grid.SquareType;
using Square = Grid.Square;

public class Magnet : Piece {

	public AudioClip magnet;
	public Material CBMat;

    protected override void Awake()
    {
        base.Awake();

		Color temp = new Color();
		
		temp.r = 1f;
		temp.g = 0f;
		temp.b = 0f;
		temp.a = 1f;
        this.renderer.material.color = temp;
    }

	// TODO: This is duplicated across all subclasses of Piece
	protected override void SetColorBlindMaterial() {
		renderer.material = CBMat;
	}

	private void Start() {
		if (LevelManager.modeling)
			return;

		grid.SetSquare(row, col, new Square(SquareType.Magnet));
		grid.magnetMap.Add(pos, this);
	}

	public bool IsMovingAway(int r, int c, int dr, int dc) {
		audio.clip = magnet;

		if (row == r) {
			if (dr == 0) {
//				if (!audio.isPlaying)
//					audio.Play();
				return true;
			}
		} else {
			Utils.Assert(col == c);
			if (dc == 0) {
//				if (!audio.isPlaying)
//					audio.Play();
				return true;
			}
		}
		return false;
	}
}
