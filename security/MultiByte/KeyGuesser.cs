namespace security
{
    using System;
    using System.Text;

    public class KeyGuesser
    {
        private const int MaxLength = 64;

        public int GetProbableKeyLength(string text)
        {
            var ics = new double[MaxLength];
            for (var i = 1; i <= MaxLength; i++)
            {
                var offsetText = GetOffsetText(text, i);
                var ic = Utils.CalculateIC(offsetText);
                ics[i - 1] = ic;
            }
            for (var i = 0; i < ics.Length; i++)
            {
                var ic = ics[i];
                var next = i > 0 ? ics[i - 1] : ics[i + 1];
                var diff = ic - next;
                if (diff > 0.1)
                {
                    return i + 1;
                }
            }
            return -1;
        }

        private string GetOffsetText(string text, int offset)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < text.Length; i += offset)
            {
                builder.Append(text[i]);
            }
            return builder.ToString();
        }
    }
}
