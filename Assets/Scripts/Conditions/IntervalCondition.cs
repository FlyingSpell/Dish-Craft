using System;

public class IntervalCondition : ICondition
{
	private int leftValue;
	private int rightValue;

	public IntervalCondition(int leftValue, int rightValue)
	{
		this.leftValue = leftValue;
		this.rightValue = rightValue;
	}

	public bool IsTrue(int value)
	{
		return (leftValue <= value) && (value <= rightValue);
	}

	public override String ToString()
	{
		return $"[{leftValue}, {rightValue}]";
	}
}
