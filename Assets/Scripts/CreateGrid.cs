using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Square = Grid.Square;
using SquareType = Grid.SquareType;

public class CreateGrid : MonoBehaviour {
	public GameObject blockPrefab;
	private Material lineMaterial;
	
	private int dim = Grid.Dim;
	private GameObject board;
	private Grid g;

	private const int lineLayer = -1;
	
	void Start() {
		board = Utils.FindObject("Board");
		
		g = Utils.GetComponent<Grid>(board);
		
		lineMaterial = new Material( "Shader \"Lines/Colored Blended\" {" +
		                            "SubShader { Pass { " +
		                            "    Blend SrcAlpha OneMinusSrcAlpha " +
		                            "    ZWrite Off Cull Off Fog { Mode Off } " +
		                            "    BindChannels {" +
		                            "      Bind \"vertex\", vertex Bind \"color\", color }" +
		                            "} } }" );
		lineMaterial.hideFlags = HideFlags.HideAndDontSave;
		lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		
		CreateBorder();
	}
	
	private void CreateBorder() {
		// Bottom row
		CreateRow(dim, i => PlaceBlock(g, dim - 1, i));
		
		// Top row
		CreateRow(dim, i => PlaceBlock(g, 0, i));
		
		// Right column
		CreateRow(dim - 2, i => PlaceBlock(g, i + 1, 0));
		
		// Left column
		CreateRow(dim - 2, i => PlaceBlock(g, i + 1, dim - 1));
	}
	
	private void OnPostRender() {
		GL.PushMatrix();
		lineMaterial.SetPass(0);
		GL.Begin(GL.LINES);
		GL.Color(Color.grey);
		
		float dimF = dim;
		
		// Vertical lines
		for (int i = 1; i < dim; i++) {
			GL.Vertex3(i - dimF/2, -1 * dimF/2, lineLayer);
			GL.Vertex3(i - dimF/2, dimF/2, lineLayer);
		}
		
		// Horizontal lines
		for (int i = 1; i < dim; i++) {
			GL.Vertex3(-1 * dimF/2, i - dimF/2, lineLayer);
			GL.Vertex3(dimF/2, i - dimF/2, lineLayer);
		}
		
		GL.PopMatrix();
		GL.End();
	}
	
	private Vector3 PlaceBlock(Grid g, int row, int col) {
		return g.PosToCoord(row, col);
	}
	
	private void CreateRow(int length, Func<int, Vector3> placementFunc) {
		for (int i = 0; i < length; i++) {
			CreateBlock(placementFunc(i));
		}
	}

	private void CreateBlock(Vector3 position) {
		GameObject block = Instantiate(blockPrefab) as GameObject;
		position.z = -1;
		block.transform.position = position;
		block.transform.parent = board.transform;
	}
}
