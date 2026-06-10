using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class ComboBonusManager : MonoBehaviour
{
	public int bonusCombo;

	public int initialBonusCoin;

	public int additionalBonusCoin;

	private int curTargetCombo;

	private int curBonusCoin;

	private void OnGameStart()
	{
		curTargetCombo = bonusCombo;
		curBonusCoin = initialBonusCoin;
	}

	private void OnCombo(int comboIn)
	{
		if (comboIn == 0)
		{
			curTargetCombo = bonusCombo;
			curBonusCoin = initialBonusCoin;
		}
		else if (comboIn >= curTargetCombo)
		{
			MonoSingleton<EventManager>.instance.TriggerEvent(new OnComboBonusEvent(new ComboBonus(curTargetCombo, new Reward(Reward.Type.Coin, curBonusCoin))));
			curTargetCombo += bonusCombo;
			curBonusCoin += additionalBonusCoin;
		}
	}
}
