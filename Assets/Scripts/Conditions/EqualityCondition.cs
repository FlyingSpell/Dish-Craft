using System;

public class EqualityCondition: ICondition
{
	private int value;

	public EqualityCondition(int value)
	{
		this.value = value;
	}

	public bool IsTrue(int value)
	{
		return this.value == value;
	}

	public override String ToString()
	{
		return $"{value}";
	}
}

