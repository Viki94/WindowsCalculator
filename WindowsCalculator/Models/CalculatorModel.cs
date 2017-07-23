using System;

namespace WindowsCalculator.Models
{
    public class CalculatorModel
    {
        public string OnScreenDisplay { get; set; }
        public decimal HiddenNumber { get; set; }
        public string Operation { get; set; }
        public string CalculationHistory { get; set; }
        public decimal MemoryStore { get; set; }
        private bool ClearDisplayOnNext { get; set; }
        private bool HasDecimalPoint { get; set; }

        public CalculatorModel()
        {
            OnScreenDisplay = "0";
            CalculationHistory = string.Empty;
            MemoryStore = 0.0M;
            ClearDisplayOnNext = true;
            HasDecimalPoint = false;
        }

        public void ProcessNumericButton(int numPressed)
        {
            if (ClearDisplayOnNext)
            {
                ClearDisplayOnNext = false;
                OnScreenDisplay = numPressed.ToString();
            }
            else
            {
                if (double.Parse(OnScreenDisplay) == 0 && numPressed != 0)
                {
                    OnScreenDisplay = numPressed.ToString();
                }
                else if (OnScreenDisplay.Length < 16)
                {
                    OnScreenDisplay += numPressed.ToString();
                }
            }

            CalculationHistory += numPressed.ToString();
            SessionBagModel.Current.calcInstance = this;
        }

        public void ProcessOperationButton(string opPressed)
        {
            CalculationHistory = string.Empty;
            if (opPressed != "=")
            {
                CalculationHistory += OnScreenDisplay;
            }

            switch (opPressed)
            {
                case "MC":
                    MemoryStore = 0;
                    OnScreenDisplay = MemoryStore.ToString();
                    break;
                case "MR":
                    Operation = opPressed;
                    ProcessEqualsButton();
                    break;
                case "M+":
                    Operation = opPressed;
                    ProcessEqualsButton();
                    break;
                case "M-":
                    Operation = opPressed;
                    ProcessEqualsButton();
                    break;

                case "=":
                    ProcessEqualsButton();
                    break;
                case "C":
                    Clear();
                    break;
                case "CE":
                    ClearEntry();
                    break;
                case "\u232b":
                    if (OnScreenDisplay.Length > 1)
                    {
                        OnScreenDisplay = OnScreenDisplay.Remove(OnScreenDisplay.Length - 1, 1);
                    }
                    else
                    {
                        OnScreenDisplay = "0";
                    }
                    break;
                case ",":
                    AddToCalculationHistory(opPressed);
                    ProcessDecimalPoint();
                    break;

                case "*":
                    AddToCalculationHistory(opPressed);
                    ProcessDualOperandMathOperator(opPressed);
                    break;
                case "/":
                    AddToCalculationHistory(opPressed);
                    ProcessDualOperandMathOperator(opPressed);
                    break;
                case "+":
                    AddToCalculationHistory(opPressed);
                    ProcessDualOperandMathOperator(opPressed);
                    break;
                case "-":
                    AddToCalculationHistory(opPressed);
                    ProcessDualOperandMathOperator(opPressed);
                    break;

                case "\u00b1":
                    OnScreenDisplay = (-double.Parse(OnScreenDisplay)).ToString();
                    CalculationHistory = string.Empty;
                    CalculationHistory += OnScreenDisplay;
                    break;
                case "%":
                    OnScreenDisplay = (double.Parse(OnScreenDisplay) / 100 * double.Parse(OnScreenDisplay)).ToString();
                    CalculationHistory = string.Empty;
                    CalculationHistory += OnScreenDisplay;
                    break;
                case "\u221a":
                    OnScreenDisplay = ((decimal)Math.Sqrt(double.Parse(OnScreenDisplay))).ToString();
                    AddToCalculationHistory(opPressed);
                    break;
                case "^2":
                    OnScreenDisplay = ((decimal)Math.Pow(double.Parse(OnScreenDisplay), 2)).ToString();
                    AddToCalculationHistory(opPressed);
                    break;
                case "1/x":
                    if (OnScreenDisplay != "0")
                    {
                        OnScreenDisplay = ((decimal)(1 / double.Parse(OnScreenDisplay))).ToString();
                    }
                    else
                    {
                        OnScreenDisplay = "Няма деление на нула";
                    }
                    break;
            }
        }

        private void AddToCalculationHistory(string opPressed)
        {
            if (CalculationHistory.Length > 0)
            {
                Operation = opPressed;
                if (Operation == "1/x")
                {
                    CalculationHistory += ("1 / " + HiddenNumber);
                }
                else if (Operation == "\u221a")
                {
                    CalculationHistory = CalculationHistory.Remove(CalculationHistory.Length - 1, 1);
                    CalculationHistory += "\u221a (" + ((decimal)Math.Pow(double.Parse(OnScreenDisplay), 2)).ToString() + ")";
                }
                else
                {
                    CalculationHistory += " " + Operation + " ";
                }
            }
        }

        private void ProcessEqualsButton()
        {
            switch (Operation)
            {
                case "*":
                    OnScreenDisplay = (HiddenNumber * decimal.Parse(OnScreenDisplay)).ToString();
                    break;
                case "/":
                    if (OnScreenDisplay != "0")
                    {
                        OnScreenDisplay = (HiddenNumber / decimal.Parse(OnScreenDisplay)).ToString();
                    }
                    else
                    {
                        OnScreenDisplay = "Няма деление на нула";
                    }
                    break;
                case "+":
                    OnScreenDisplay = (HiddenNumber + decimal.Parse(OnScreenDisplay)).ToString();
                    break;
                case "-":
                    OnScreenDisplay = (HiddenNumber - decimal.Parse(OnScreenDisplay)).ToString();
                    break;
                case "MR":
                    OnScreenDisplay = MemoryStore.ToString();
                    break;
                case "M+":
                    MemoryStore = MemoryStore + decimal.Parse(OnScreenDisplay);
                    break;
                case "M-":
                    MemoryStore = MemoryStore - decimal.Parse(OnScreenDisplay);
                    break;
            }

            //Operation = null;
            ClearDisplayOnNext = true;
            SessionBagModel.Current.calcInstance = this;
        }

        private void ProcessDualOperandMathOperator(string op)
        {
            Operation = op;
            ClearDisplayOnNext = true;
            HiddenNumber = decimal.Parse(OnScreenDisplay);
            SessionBagModel.Current.calcInstance = this;
        }

        private void ProcessDecimalPoint()
        {
            if (ClearDisplayOnNext)
            {
                OnScreenDisplay = "0,";
                ClearDisplayOnNext = false;
            }
            else if (OnScreenDisplay.IndexOf(',') == -1)
            {
                OnScreenDisplay += ",";
            }

            SessionBagModel.Current.calcInstance = this;
        }

        private void Clear()
        {
            OnScreenDisplay = "0";
            HiddenNumber = 0;
            ClearDisplayOnNext = true;
            CalculationHistory = string.Empty;
            Operation = null;
            SessionBagModel.Current.calcInstance = this;
        }

        private void ClearEntry()
        {
            OnScreenDisplay = "0";
            SessionBagModel.Current.calcInstance = this;
        }
    }
}