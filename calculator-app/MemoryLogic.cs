using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calculator_app
{
    public class MemoryLogic
    {
        private double _memoryValue = 0;
        private List<double> _memoryStack = new List<double>();

        public bool HasMemory => _memoryValue != 0 || _memoryStack.Count > 0;

        // Memory Clear (MC)
        public void ClearMemory()
        {
            _memoryValue = 0;
            _memoryStack.Clear();
        }

        // Memory Add (M+)
        public void AddToMemory(double value)
        {
            _memoryValue += value;

            // Update the top of the stack if it exists
            if (_memoryStack.Count > 0)
            {
                _memoryStack[0] = _memoryValue;
            }
            else
            {
                _memoryStack.Add(_memoryValue);
            }
        }

        // Memory Subtract (M-)
        public void SubtractFromMemory(double value)
        {
            _memoryValue -= value;

            // Update the top of the stack if it exists
            if (_memoryStack.Count > 0)
            {
                _memoryStack[0] = _memoryValue;
            }
            else
            {
                _memoryStack.Add(_memoryValue);
            }
        }

        // Memory Store (MS)
        public void StoreInMemory(double value)
        {
            _memoryValue = value;
            _memoryStack.Insert(0, value); // Add to the top of the stack
        }

        // Memory Recall (MR)
        public double RecallMemory()
        {
            return _memoryValue;
        }

        // Memory List (M>)
        public List<double> GetMemoryStack()
        {
            return new List<double>(_memoryStack); // Return a copy to prevent modification
        }
    }
}