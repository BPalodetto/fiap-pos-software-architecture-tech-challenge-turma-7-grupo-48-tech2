﻿namespace Domain.ValueObjects;

public struct Cpf
{
	public Cpf(string number)
	{
		if (!IsValidCpf(number: number))
		{
			throw new ArgumentException(
				message: "Invalid CPF",
				paramName: nameof(number)
			);
		}

		Number = number;
	}

	public string Number { get; }

	public string FormatedNumber => Convert.ToUInt64(value: Number)
		.ToString(format: @"000\.000\.000\-00");

	public static implicit operator Cpf(string number)
	{
		return new Cpf(number: number);
	}

	public static implicit operator string(Cpf number)
	{
		return number.FormatedNumber;
	}

	public static bool IsValidCpf(string number)
	{
		int[] multiplier1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
		int[] multiplier2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

		number = number.Trim()
			.Replace(
				oldValue: ".",
				newValue: ""
			)
			.Replace(
				oldValue: "-",
				newValue: ""
			);
		if (number.Length != 11)
			return false;

		for (var j = 0; j < 10; j++)
			if (j.ToString()
				    .PadLeft(
					    totalWidth: 11,
					    paddingChar: char.Parse(s: j.ToString())
				    ) ==
			    number)
				return false;

		var hasCpf = number.Substring(
			startIndex: 0,
			length: 9
		);
		var sum = 0;

		for (var i = 0; i < 9; i++)
			sum += int.Parse(
				       s: hasCpf[index: i]
					       .ToString()
			       ) *
			       multiplier1[i];

		var remainder = sum % 11;
		if (remainder < 2)
			remainder = 0;
		else
			remainder = 11 - remainder;

		var digit = remainder.ToString();
		hasCpf = hasCpf + digit;
		sum = 0;
		for (var i = 0; i < 10; i++)
			sum += int.Parse(
				       s: hasCpf[index: i]
					       .ToString()
			       ) *
			       multiplier2[i];

		remainder = sum % 11;
		if (remainder < 2)
			remainder = 0;
		else
			remainder = 11 - remainder;

		digit = digit + remainder;

		return number.EndsWith(value: digit);
	}
}