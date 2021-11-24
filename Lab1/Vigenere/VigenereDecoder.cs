namespace security.Vigenere
{
    using System.Text;

    public class VigenereDecoder
    {
        public string Decode(string text, VigenereKey key)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                var k = key[i % key.Length];
                var ci = Utils.GetIndex(c);
                var ki = Utils.GetIndex(k);
                var index = (ci - ki + Utils.NumberOfLetters) % Utils.NumberOfLetters;
                builder.Append(Utils.EnglishAlphabet[index]);
            }
            return builder.ToString();
        }
    }
}
