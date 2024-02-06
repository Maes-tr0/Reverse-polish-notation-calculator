using System;
using System.Collections.Generic;
using System.Linq;

public class InfixToRPNConverter
{
    private static int Precedence(char op)
    {
        switch (op)
        {
            case '+':
            case '-':
                return 1;
            case '*':
            case '/':
                return 2;
            default:
                return -1;
        }
    }

    private static bool IsOperator(char c)
    {
        return new[] { '+', '-', '*', '/' }.Contains(c);
    }

    public static string ConvertToRPN(string infix)
    {
        var result = new List<string>();
        var stack = new Stack<char>();

        for (int i = 0; i < infix.Length; i++)
        {
            char token = infix[i];

            if (char.IsWhiteSpace(token))
                continue;

            if (char.IsDigit(token) || token == '.')
            {
                string number = token.ToString();
                while (i + 1 < infix.Length && (char.IsDigit(infix[i + 1]) || infix[i + 1] == '.'))
                {
                    number += infix[++i];
                }
                result.Add(number);
            }
            else if (token == '(')
            {
                stack.Push(token);
            }
            else if (token == ')')
            {
                while (stack.Count > 0 && stack.Peek() != '(')
                {
                    result.Add(stack.Pop().ToString());
                }
                if (stack.Count > 0)
                    stack.Pop(); // Pop the '('
            }
            else if (token == '-' && (i == 0 || IsOperator(infix[i - 1]) || infix[i - 1] == '('))
            {
                // Handle unary minus
                result.Add("0");
                stack.Push('-');
            }
            else
            {
                while (stack.Count > 0 && Precedence(token) <= Precedence(stack.Peek()))
                {
                    result.Add(stack.Pop().ToString());
                }
                stack.Push(token);
            }
        }

        while (stack.Count > 0)
        {
            result.Add(stack.Pop().ToString());
        }

        return string.Join(" ", result);
    }
}

public class ReversePolishNotationCalculator
{
    public static double Evaluate(string expression)
    {
        var stack = new Stack<double>();
        string[] tokens = expression.Split(' ');

        foreach (string token in tokens)
        {
            if (double.TryParse(token, out double number))
            {
                stack.Push(number);
            }
            else if (stack.Count >= 2)
            {
                double rightOperand = stack.Pop();
                double leftOperand = stack.Pop();

                switch (token)
                {
                    case "+":
                        stack.Push(leftOperand + rightOperand);
                        break;
                    case "-":
                        stack.Push(leftOperand - rightOperand);
                        break;
                    case "*":
                        stack.Push(leftOperand * rightOperand);
                        break;
                    case "/":
                        stack.Push(leftOperand / rightOperand);
                        break;
                    default:
                        throw new ArgumentException($"Unsupported operator: {token}");
                }
            }
            else
            {
                throw new InvalidOperationException("Invalid expression");
            }
        }

        return stack.Pop();
    }
}

public class Program
{
    public static void Main()
    {
        string infixExpression = "12 + 2 * ((3 * 4) + (10 / 5))";
        string rpnExpression = InfixToRPNConverter.ConvertToRPN(infixExpression);
        double result = ReversePolishNotationCalculator.Evaluate(rpnExpression);

        Console.WriteLine($"Infix Expression: {infixExpression}");
        Console.WriteLine($"RPN Expression: {rpnExpression}");
        Console.WriteLine($"Result: {result}");
        Console.ReadLine();
    }
}
