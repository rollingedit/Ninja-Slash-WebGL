using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ComboEndLabel : BaseLabel
{
	public UILabel uiLabel;

	public TweenScale tweenScale;

	private int comboBonus;

	public override void Initialize()
	{
		comboBonus = 0;
		UpdateLabel();
	}

	public override void UpdateLabel()
	{
		if (comboBonus == 0)
		{
			uiLabel.text = string.Empty;
		}
		else
		{
			uiLabel.text = string.Concat((object)"+", (object)comboBonus);
		}
	}

	private void OnComboEnd(int score)
	{
		((MonoBehaviour)this).StopAllCoroutines();
		comboBonus = score;
		tweenScale.Reset();
		tweenScale.Play(true);
		UpdateLabel();
		((MonoBehaviour)this).StartCoroutine("FlushLabel");
	}

	[DebuggerHidden]
	private global::System.Collections.IEnumerator FlushLabel()
	{
		yield return (object)new WaitForSeconds(3f);
		Initialize();
	}
}
