using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker {

	//public static Color RED = new Color32(255, 119, 119, 255);
	public static Color RED = new Color32(255, 78, 78, 255);
	public static Color ORANGE = new Color32(255, 209, 117, 255);
	//public static Color YELLOW = new Color32(252, 255, 119, 255);
	public static Color YELLOW = new Color32(253, 246, 87, 255);
	//public static Color GREEN = new Color32(134, 255, 144, 255);
	public static Color GREEN = new Color32(86, 255, 86, 255);
	//public static Color BLUE = new Color32(133, 139, 250, 255);
	public static Color BLUE = new Color32(105, 112, 255, 255);
	//public static Color PURPLE = new Color32(255,119 ,255, 255);
	//public static Color WHITE = new Color32(0,0,0, 255);
	//public static Color GREY = new Color32(116,116 ,116, 255);

	//public static Color[] COPT = {RED, ORANGE, YELLOW, GREEN, BLUE, PURPLE, WHITE, GREY};

	public static Color BBLUE = new Color32(186, 239, 238, 255);
	public static Color LBLUE = new Color32(78, 174, 217, 255);
	public static Color DBLUE = new Color32(38, 68, 123, 255);

	public static Color BGREY = new Color32(229, 229, 229, 255);
	public static Color LGREY = new Color32(192, 192, 192, 255);
	public static Color DGREY = new Color32(60, 60, 60, 255);

	public static Color BCAR = new Color32(234, 230, 190, 255);
	public static Color LCAR = new Color32(223, 194, 138, 255);
	public static Color DCAR = new Color32(135, 67, 63, 255);

	public static Color BCHOC = new Color32(244, 214, 167, 255);
	public static Color LCHOC = new Color32(161, 128, 114, 255);
	public static Color DCHOC = new Color32(101, 68, 68, 255);

	public static Color BGRN = new Color32(215, 243, 196, 255);
	public static Color LGRN = new Color32(139, 190, 87, 255);
	public static Color DGRN = new Color32(57, 96, 58, 255);

	public static Color BBRY = new Color32(255, 246, 206, 255);
	public static Color LBRY = new Color32(233, 131, 165, 255);
	public static Color DBRY = new Color32(58, 47, 99, 255);

	public static readonly Dictionary<string, Color32[]> COL_DICT = new Dictionary<string, Color32[]> 
	{
		{"WINTER", new Color32[] {BBLUE, LBLUE, DBLUE}},
		{"MONO", new Color32[] {BGREY, LGREY, DGREY}},
		{"CARAMEL", new Color32[] {BCAR, LCAR, DCAR}},
		{"CHOCO", new Color32[] {BCHOC, LCHOC, DCHOC}},
		{"SPRING", new Color32[] {BGRN, LGRN, DGRN}},
		{"BERRY", new Color32[] {BBRY, LBRY, DBRY}}
	};
}
