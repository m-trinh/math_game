using System;
namespace WritePadXamarinSample
{
	public class RandomQuestions
	{

		private int firstNumber;
		private int secondNumber;
		private int thirdNumber;
		private int fourthNumber;
		private string firstOperand;
		private string secondOperand;
		private string thirdOperand;
		public string difficulty;
		public decimal solution;

		public RandomQuestions (string difficulty)
		{
			Random random = new Random (Guid.NewGuid ().GetHashCode ());

			firstNumber = random.Next (1, 10);
			secondNumber = random.Next (1, 10);
			thirdNumber = difficulty == "Easy" ? 0
							: random.Next (1, 16);
			fourthNumber = difficulty == "Easy" ? 0
							: difficulty == "Medium" ? 0
							: random.Next (1, 20);

			string [] operands = { "+", "-", "*" /*"/"*/ };
			firstOperand = operands [random.Next (0, operands.Length)];
			secondOperand = operands [random.Next (0, operands.Length)];
			thirdOperand = operands [random.Next (0, operands.Length)];
			if (difficulty == "Easy") {
				//The solution is the first operand the calculate given the operand and the second operator
				switch (firstOperand) {
				case "/":
					solution = FirstNumber / SecondNumber;
					break;
				case "*":
					solution = FirstNumber * SecondNumber;
					break;
				case "+":
					solution = FirstNumber + SecondNumber;
					break;
				case "-":
					solution = FirstNumber - SecondNumber;
					break;
				}
			} else if (difficulty == "Medium") {
				//Determine what the first operand is
				//Depending on the second operand, we will need to format the expression differently
				switch (FirstOperand) {
				case "/":
					if (SecondOperand == "/") {
						solution = FirstNumber / SecondNumber / ThirdNumber;
					} else if (SecondOperand == "*") {
						solution = FirstNumber / SecondNumber * ThirdNumber;
					} else if (SecondOperand == "+") {
						solution = FirstNumber / SecondNumber + ThirdNumber;
					} else {
						solution = FirstNumber / SecondNumber - ThirdNumber;
					}
					break;
				case "*":
					if (SecondOperand == "/") {
						solution = FirstNumber * SecondNumber / ThirdNumber;
					} else if (SecondOperand == "*") {
						solution = FirstNumber * SecondNumber * ThirdNumber;
					} else if (SecondOperand == "+") {
						solution = FirstNumber * SecondNumber + ThirdNumber;
					} else {
						solution = FirstNumber * SecondNumber - ThirdNumber;
					}
					break;
				case "+":
					if (SecondOperand == "/") {
						solution = FirstNumber + SecondNumber / ThirdNumber;
					} else if (SecondOperand == "*") {
						solution = FirstNumber + SecondNumber * ThirdNumber;
					} else if (SecondOperand == "+") {
						solution = FirstNumber + SecondNumber + ThirdNumber;
					} else {
						solution = FirstNumber + SecondNumber - ThirdNumber;
					}
					break;
				case "-":
					if (SecondOperand == "/") {
						solution = FirstNumber - SecondNumber / ThirdNumber;
					} else if (SecondOperand == "*") {
						solution = FirstNumber - SecondNumber * ThirdNumber;
					} else if (SecondOperand == "+") {
						solution = FirstNumber - SecondNumber + ThirdNumber;
					} else {
						solution = FirstNumber - SecondNumber - ThirdNumber;
					}
					break;
				}
			} else {
				//Determine what the first operand is
				//Depending on the second operand, we will need to format the expression differently
				//Hard difficulty also has a third operand, so we will have to determine that before we format the expression            	
				switch (FirstOperand) {
				case "/":
					if (SecondOperand == "/") {
						if (ThirdOperand == "/") {
							solution = FirstNumber / SecondNumber / ThirdNumber / FourthNumber;
						} else if (ThirdOperand == "*") {
							solution = FirstNumber / SecondNumber / ThirdNumber * FourthNumber;
						} else if (ThirdOperand == "+") {
							solution = FirstNumber / SecondNumber / ThirdNumber + FourthNumber;
						} else {
							solution = FirstNumber / SecondNumber / ThirdNumber - FourthNumber;
						}
					} else if (SecondOperand == "*") {
						if (ThirdOperand == "/") {
							solution = FirstNumber / SecondNumber * ThirdNumber / FourthNumber;
						} else if (ThirdOperand == "*") {
							solution = FirstNumber / SecondNumber * ThirdNumber * FourthNumber;
						} else if (ThirdOperand == "+") {
							solution = FirstNumber / SecondNumber * ThirdNumber + FourthNumber;
						} else {
							solution = FirstNumber / SecondNumber * ThirdNumber - FourthNumber;
						}
					} else if (SecondOperand == "+") {
						if (ThirdOperand == "/") {
							solution = (FirstNumber / SecondNumber) + (ThirdNumber / FourthNumber);
						} else if (ThirdOperand == "*") {
							solution = (FirstNumber / SecondNumber) + (ThirdNumber * FourthNumber);
						} else if (ThirdOperand == "+") {
							solution = (FirstNumber / SecondNumber) + ThirdNumber + FourthNumber;
						} else {
							solution = (FirstNumber / SecondNumber) + ThirdNumber - FourthNumber;
						}
					} else {
						if (ThirdOperand == "/") {
							solution = (FirstNumber / SecondNumber) - (ThirdNumber / FourthNumber);
						} else if (ThirdOperand == "*") {
							solution = (FirstNumber / SecondNumber) - (ThirdNumber * FourthNumber);
						} else if (ThirdOperand == "+") {
							solution = (FirstNumber / SecondNumber) - ThirdNumber + FourthNumber;
						} else {
							solution = (FirstNumber / SecondNumber) - ThirdNumber - FourthNumber;
						}
					}
					break;
				case "*":
					if (SecondOperand == "/") {
						if (ThirdOperand == "/") {
							solution = FirstNumber * SecondNumber / ThirdNumber / FourthNumber;
						} else if (ThirdOperand == "*") {
							solution = FirstNumber * SecondNumber / ThirdNumber * FourthNumber;
						} else if (ThirdOperand == "+") {
							solution = FirstNumber * SecondNumber / ThirdNumber + FourthNumber;
						} else {
							solution = FirstNumber * SecondNumber / ThirdNumber - FourthNumber;
						}
					} else if (SecondOperand == "*") {
						if (ThirdOperand == "/") {
							solution = FirstNumber * SecondNumber * ThirdNumber / FourthNumber;
						} else if (ThirdOperand == "*") {
							solution = FirstNumber * SecondNumber * ThirdNumber * FourthNumber;
						} else if (ThirdOperand == "+") {
							solution = FirstNumber * SecondNumber * ThirdNumber + FourthNumber;
						} else {
							solution = FirstNumber * SecondNumber * ThirdNumber - FourthNumber;
						}
					} else if (SecondOperand == "+") {
						if (ThirdOperand == "/") {
							solution = (FirstNumber * SecondNumber) + (ThirdNumber / FourthNumber);
						} else if (ThirdOperand == "*") {
							solution = (FirstNumber * SecondNumber) + (ThirdNumber * FourthNumber);
						} else if (ThirdOperand == "+") {
							solution = (FirstNumber * SecondNumber) + ThirdNumber + FourthNumber;
						} else {
							solution = (FirstNumber * SecondNumber) + ThirdNumber - FourthNumber;
						}
					} else {
						if (ThirdOperand == "/") {
							solution = (FirstNumber * SecondNumber) - (ThirdNumber / FourthNumber);
						} else if (ThirdOperand == "*") {
							solution = (FirstNumber * SecondNumber) - (ThirdNumber * FourthNumber);
						} else if (ThirdOperand == "+") {
							solution = (FirstNumber * SecondNumber) - ThirdNumber + FourthNumber;
						} else {
							solution = (FirstNumber * SecondNumber) - ThirdNumber - FourthNumber;
						}
					}
					break;
				case "+":
					if (SecondOperand == "/") {
						if (ThirdOperand == "/") {
							solution = FirstNumber + ((SecondNumber / ThirdNumber) / FourthNumber);
						} else if (ThirdOperand == "*") {
							solution = FirstNumber + ((SecondNumber / ThirdNumber) * FourthNumber);
						} else if (ThirdOperand == "+") {
							solution = FirstNumber + (SecondNumber / ThirdNumber) + FourthNumber;
						} else {
							solution = FirstNumber + (SecondNumber / ThirdNumber) - FourthNumber;
						}
					} else if (SecondOperand == "*") {
						if (ThirdOperand == "/") {
							solution = FirstNumber + ((SecondNumber * ThirdNumber) / FourthNumber);
						} else if (ThirdOperand == "*") {
							solution = FirstNumber + ((SecondNumber * ThirdNumber) * FourthNumber);
						} else if (ThirdOperand == "+") {
							solution = FirstNumber + (SecondNumber * ThirdNumber) + FourthNumber;
						} else {
							solution = FirstNumber + (SecondNumber * ThirdNumber) - FourthNumber;
						}
					} else if (SecondOperand == "+") {
						if (ThirdOperand == "/") {
							solution = FirstNumber + SecondNumber + (ThirdNumber / FourthNumber);
						} else if (ThirdOperand == "*") {
							solution = FirstNumber + SecondNumber + (ThirdNumber * FourthNumber);
						} else if (ThirdOperand == "+") {
							solution = FirstNumber + SecondNumber + ThirdNumber + FourthNumber;
						} else {
							solution = FirstNumber + SecondNumber + ThirdNumber - FourthNumber;
						}
					} else {
						if (ThirdOperand == "/") {
							solution = FirstNumber + SecondNumber - (ThirdNumber / FourthNumber);
						} else if (ThirdOperand == "*") {
							solution = FirstNumber + SecondNumber - (ThirdNumber * FourthNumber);
						} else if (ThirdOperand == "+") {
							solution = FirstNumber + SecondNumber - ThirdNumber + FourthNumber;
						} else {
							solution = FirstNumber + SecondNumber - ThirdNumber - FourthNumber;
						}
					}
					break;
				case "-":
					if (SecondOperand == "/") {
						if (ThirdOperand == "/") {
							solution = FirstNumber - ((SecondNumber / ThirdNumber) / FourthNumber);
						} else if (ThirdOperand == "*") {
							solution = FirstNumber - ((SecondNumber / ThirdNumber) * FourthNumber);
						} else if (ThirdOperand == "+") {
							solution = FirstNumber - (SecondNumber / ThirdNumber) + FourthNumber;
						} else {
							solution = FirstNumber - (SecondNumber / ThirdNumber) - FourthNumber;
						}
					} else if (SecondOperand == "*") {
						if (ThirdOperand == "/") {
							solution = FirstNumber - ((SecondNumber * ThirdNumber) / FourthNumber);
						} else if (ThirdOperand == "*") {
							solution = FirstNumber - ((SecondNumber * ThirdNumber) * FourthNumber);
						} else if (ThirdOperand == "+") {
							solution = FirstNumber - (SecondNumber * ThirdNumber) + FourthNumber;
						} else {
							solution = FirstNumber - (SecondNumber * ThirdNumber) - FourthNumber;
						}
					} else if (SecondOperand == "+") {
						if (ThirdOperand == "/") {
							solution = FirstNumber - SecondNumber + (ThirdNumber / FourthNumber);
						} else if (ThirdOperand == "*") {
							solution = FirstNumber - SecondNumber + (ThirdNumber * FourthNumber);
						} else if (ThirdOperand == "+") {
							solution = FirstNumber - SecondNumber + ThirdNumber + FourthNumber;
						} else {
							solution = FirstNumber - SecondNumber + ThirdNumber - FourthNumber;
						}
					} else {
						if (ThirdOperand == "/") {
							solution = FirstNumber - SecondNumber - (ThirdNumber / FourthNumber);
						} else if (ThirdOperand == "*") {
							solution = FirstNumber - SecondNumber - (ThirdNumber * FourthNumber);
						} else if (ThirdOperand == "+") {
							solution = FirstNumber - SecondNumber - ThirdNumber + FourthNumber;
						} else {
							solution = FirstNumber - SecondNumber - ThirdNumber - FourthNumber;
						}
					}
					break;

				}
			}
		}

		public int FirstNumber {
			get {
				return firstNumber;
			}
		}

		public int SecondNumber {
			get {
				return secondNumber;
			}
		}

		public int ThirdNumber {
			get {
				return thirdNumber;
			}
		}

		public int FourthNumber {
			get {
				return fourthNumber;
			}
		}

		public string FirstOperand {
			get {
				return firstOperand;
			}
		}

		public string SecondOperand {
			get {
				return secondOperand;
			}
		}

		public string ThirdOperand {
			get {
				return thirdOperand;
			}

		}

		public string Difficulty {
			get; set;
		}


		public decimal Solution {
			get {
				return solution;

			}
		}	}
}
