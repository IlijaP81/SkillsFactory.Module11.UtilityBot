namespace UtilityBot.Services
{
    internal class Calculations
    {
        /// <summary>
        /// Returns number of symbols in string
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public int CalculateNumberOfSymbols(string userInput)
        {
            int stringLenght = 0;
            if (string.IsNullOrEmpty(userInput)) return 0;
            stringLenght = userInput.Length;
            return stringLenght;
        }

        /// <summary>
        /// Returns sum of digits in string excluding non-digit symbols
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public int CalculateSum(string userInput)
        {
            if (string.IsNullOrEmpty(userInput)) return 0;
            int sum = 0;
            string[] fragments = userInput.Split(new char[] { ' ' });
            foreach (string fragment in fragments)
            { 
                if (int.TryParse(fragment, out int result))
                {
                    sum += result; 
                }
            }
            return sum;
        }
    }
}
